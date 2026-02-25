using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class CrabGM : MonoBehaviour
{
    public float MachineDurability;
    float MachineDurability_Script;
    public Animator MachineAni;
    public Image MachineUIBar;
    public Sprite[] MachineUIBarSprites;

    public Image MachineUIBarOutside; //耐久值外框
    public Sprite[] MachineUIBarSpritesOutside;

    public Collider2D Placement;
    public Image MachineUI;
    public Sprite[] MachineUISprites;

    public Image MachineUIOutside; //圓形計時器外框
    public Sprite[] MachineUISpritesOutside;
    public float SaveRemainingValue;

    public Transform needle; // 指針物件（需拖曳到 Inspector）
    public float maxRotation = -360f; // 旋轉範圍（滿格時的角度）

    public GameObject PCBPop;  //電路板生成
    public Transform PCBPopPlace;  //電路板生成
    GameObject CurrentPCB;
    private bool canSpawnPCB = false;

    //隨機判斷要不要產生怪物 但 新手教學關卡要產生怪物
    bool isProduceMonster;
    public GameObject Monster;
    GameObject MonsterPrefab;
    public Transform ProducePos;
    public float DeductMachineDurability;//扣除機器耐久
    bool isRun;

    //機器耐久值恢復
    public GameObject FixMachineDurability;  //機器耐久維修物
    bool MachineDurabilityFix = false;  //不可修
    public GameObject FixMachineShow; //機器維修會顯示在機器上的圖
    private bool isFixMachineShow = false;
    private Coroutine repairCoroutine; // 協程參考，避免重複啟動
    private bool touchingFixMachine; //耐久維修物是否碰撞中
    private bool iswork = false;//一次只能使用一個
    //電路板
    private bool touchingbrokePCB;
    private GameObject currentbrokePCB;

    //動畫
    float getInLength;
    float workLength;
    float totalLength;
    void Start()
    {
        MachineDurability_Script = MachineDurability;
        SaveRemainingValue = MachineUIBar.fillAmount;

        Animator animator = GetComponent<Animator>();

        getInLength = animator.runtimeAnimatorController
            .animationClips.First(c => c.name == "get in").length;

        workLength = animator.runtimeAnimatorController
            .animationClips.First(c => c.name == "work").length;

        totalLength = getInLength + workLength;

    }
    void OnEnable()    //檢查滑鼠放開事件
    {
        DraggableReturn2D.OnReleased += OnItemReleased;
    }

    void OnDisable()   //取消滑鼠放開事件
    {
        DraggableReturn2D.OnReleased -= OnItemReleased;
    }

    void OnCollisionStay2D(Collision2D coll)  //碰撞
    {
        if (coll.gameObject.CompareTag("brokePCB"))
        {
            touchingbrokePCB = true;
            currentbrokePCB = coll.gameObject;
        }

        if (coll.gameObject == FixMachineDurability)
        {
            touchingFixMachine = true;
        }
    }

    void OnCollisionExit2D(Collision2D coll)  //結束碰撞
    {
        if (coll.gameObject == currentbrokePCB)
        {
            touchingbrokePCB = false;
            currentbrokePCB = null;
        }

        if (coll.gameObject == FixMachineDurability)
        {
            touchingFixMachine = false;
        }
    }

    //  滑鼠放開事件
    void OnItemReleased(DraggableReturn2D item)
    {
        //if (!touchingbrokePCB) return;
        //放開時碰到電路板
        if (touchingbrokePCB && currentbrokePCB != null && iswork == false)
        {
            AudioManager.Instance.PlaySfx(1);             //音效
            MachineAni.speed = 1;                         //動
            //Destroy(item.gameObject); //這兩都ok
            canSpawnPCB = true;
            iswork = true;
            MachineAni.SetTrigger("IdleToWalk");
            ProduceMonster();
            Destroy(currentbrokePCB);
            touchingbrokePCB = false;
            currentbrokePCB = null;
            return;
        }
        // 放開時碰到修理元件
        if (touchingFixMachine && !MachineDurabilityFix)
        {
            AudioManager.Instance.PlaySfx(2);             //音效
            if (MachineDurability_Script < MachineDurability)
            {
                MachineAni.speed = 0;                         //不動
                                                              //Destroy(item.gameObject); //這兩都ok
                MachineDurabilityFix = true;
                isFixMachineShow = true;
                FixMachineShow.SetActive(true); //機器維修會顯示在機器上的圖
                                                //FindObjectOfType<FixMachineDurabilityChangeImage>().ChangePicture();  //換回去
                repairCoroutine = StartCoroutine(FixDurabilityOverTime());
                return;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = MachineAni.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("get in") || stateInfo.IsName("work"))
        {

            if (stateInfo.normalizedTime >= 0.99f)
            {
                // if (Application.loadedLevelName == "TeachGame")
                // {
                //   FindObjectOfType<TeachGM>().ProduceIteamOpen();
                // }

                MachineUI.gameObject.SetActive(false);
                MachineDurability_Script = SaveRemainingValue;

            }
            if (stateInfo.IsName("get in") || stateInfo.IsName("work"))
            {
                if (stateInfo.normalizedTime < 0.99f)
                {
                    MachineAni.speed = 1;
                    //if(stateInfo.IsName("work")){ MachineUI.gameObject.SetActive(true); }
                    MachineUI.gameObject.SetActive(true);
                    MachineDurabilityFix = false;    //不可維修
                    //FindObjectOfType<FixMachineDurabilityChangeImage>().ChangeOrigin();  //換回去
                    FixMachineShow.SetActive(false);
                    //float animationLength = stateInfo.length; // 動畫總秒數
                    //float normalizedTime = stateInfo.normalizedTime; // 播放進度（1.0代表播放完1次）
                    // float currentTimeInSeconds = animationLength * Mathf.Min(normalizedTime, 1f);

                    float currentTimeInSeconds = 0f;

                    if (stateInfo.IsName("get in"))
                    {
                        currentTimeInSeconds =
                            getInLength * Mathf.Clamp01(stateInfo.normalizedTime);
                    }
                    else if (stateInfo.IsName("work"))
                    {
                        currentTimeInSeconds =
                            getInLength +
                            (workLength * Mathf.Clamp01(stateInfo.normalizedTime));
                    }


                    SaveRemainingValue = MachineDurability_Script - currentTimeInSeconds;

                    MachineUIBar.fillAmount = SaveRemainingValue / MachineDurability;
                    if (MachineUIBar.fillAmount > 0.5f)
                    {
                        MachineUIBar.sprite = MachineUIBarSprites[0];
                        MachineUIBarOutside.sprite = MachineUIBarSpritesOutside[0]; //耐久值外框原色
                    }
                    else
                    {
                        MachineUIBar.sprite = MachineUIBarSprites[1];
                        MachineUIBarOutside.sprite = MachineUIBarSpritesOutside[1]; //耐久值外框變色
                    }
                    //MachineUI.transform.GetChild(1).GetComponent<Image>().fillAmount = 1f - (currentTimeInSeconds / animationLength);
                    // float fillAmount = 1f - (currentTimeInSeconds / animationLength);
                    MachineUI.transform.GetChild(1).GetComponent<Image>().fillAmount = 1f - (currentTimeInSeconds / totalLength);
                    float fillAmount = 1f - (currentTimeInSeconds / totalLength);
                    float zRotation = fillAmount * maxRotation; // 比例轉角度，例如 1.0 * -360 = -360°
                    needle.localEulerAngles = new Vector3(0, 0, -zRotation);
                    // if (stateInfo.normalizedTime < 0.5f)
                    if (1f - (currentTimeInSeconds / totalLength) > 0.5f)
                    {
                        MachineUI.transform.GetChild(1).GetComponent<Image>().sprite = MachineUISprites[0];
                        MachineUIOutside.sprite = MachineUISpritesOutside[0]; //圓形計時器外框原色
                    }
                    else
                    {
                        MachineUI.transform.GetChild(1).GetComponent<Image>().sprite = MachineUISprites[1];
                        MachineUIOutside.sprite = MachineUISpritesOutside[1]; //圓形計時器外框變色
                    }
                }

            }
        }
        if (canSpawnPCB && CurrentPCB == null)   //生成PCB
        {

            AnimatorStateInfo stateInfo2 = MachineAni.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("pull out") && stateInfo2.normalizedTime > 0.6f)
            {
                AudioManager.Instance.PlaySfx(3);                                                        //音效
                CurrentPCB = Instantiate(PCBPop, PCBPopPlace.position, PCBPopPlace.rotation);
                canSpawnPCB = false;
                if (Application.loadedLevelName == "TeachGame")
                {
                    FindObjectOfType<TeachGM>().OpenTeachEightThree();
                    GameObject.FindWithTag("PCB").GetComponent<DraggableReturn2D>().enabled = false;
                }
            }
        }
        AnimatorStateInfo stateInfo3 = MachineAni.GetCurrentAnimatorStateInfo(0);
        if ((stateInfo3.IsName("pull out") && stateInfo3.normalizedTime > 0.01f) || (stateInfo.IsName("hold") && stateInfo3.normalizedTime > 0.01f || stateInfo.IsName("take out") && stateInfo3.normalizedTime > 0.01f))
        {
            MachineAni.speed = 1;
            MachineDurabilityFix = false;    //不可維修
            FixMachineShow.SetActive(false);
        }
        if (stateInfo.IsName("take out") && stateInfo3.normalizedTime > 0.99f)
        {
            iswork = false;
        }
    }

    // 碰撞進入
    // private void OnCollisionEnter2D(Collision2D coll) //碰撞觸發動畫
    // {
    // if (coll.gameObject.CompareTag("brokePCB"))
    // {
    //  Debug.Log("碰到電路板");
    // ProduceMonster();
    //  Destroy(coll.gameObject);
    // canSpawnPCB = true;
    // MachineAni.SetTrigger("IdleToWalk");

    // }
    // }
    public void HoldPCB() //撥放持續拿PCB動畫
    {
        MachineAni.SetBool("hold", true);
    }
    public void TakePCB()
    {
        MachineAni.SetTrigger("takeout");
    }

    //判斷要不要產生怪物
    void ProduceMonster()
    {
        isRun = true;
        // if (Application.loadedLevelName == "TeachGame")
        // {
        //   MonsterPrefab = Instantiate(Monster, ProducePos.position, Monster.transform.rotation) as GameObject;
        //  MonsterPrefab.GetComponent<MonsterGM>().InitTarget("crab");
        //}
        //else
        if (Application.loadedLevelName != "TeachGame")
        {
            //Random.Range(0, 2) 回傳整數 0 或 1,等於 0 回傳 true，否則為 false。
            isProduceMonster = Random.Range(0, 2) == 0;                //暫時停用MonsterGM有問題                      
            if (isProduceMonster && !MonsterPrefab)
            {
                MonsterPrefab = Instantiate(Monster, ProducePos.position, Monster.transform.rotation) as GameObject;
                MonsterPrefab.GetComponent<MonsterGM>().InitTarget("crab");

            }
        }
    }

    //private void OnCollisionEnter2D(Collision2D coll) //觸發機器耐久恢復

    // {
    // if (coll.gameObject == FixMachineDurability)
    // {
    // if (isRun)
    // {
    // MachineDurabilityFix = false;
    //if (repairCoroutine != null)
    // {
    //  StopCoroutine(repairCoroutine);
    // repairCoroutine = null;
    // }
    // }
    // else if (!MachineDurabilityFix)
    //  if (!MachineDurabilityFix)
    // {
    // MachineDurabilityFix = true;
    // isFixMachineShow = true;
    // FixMachineShow.SetActive(true); //機器維修會顯示在機器上的圖
    // FindObjectOfType<FixMachineDurabilityChangeImage>().ChangePicture();  //換回去
    // repairCoroutine = StartCoroutine(FixDurabilityOverTime());
    //}
    //}
    //}

    private IEnumerator FixDurabilityOverTime()   // 每秒恢復10%耐久
    {
        MachineDurability_Script = SaveRemainingValue;
        while (MachineDurabilityFix)
        {
            float repairAmount = MachineDurability * 0.005f;
            // float repairAmount = MachineDurability * 0.1f;
            MachineDurability_Script += repairAmount;

            if (MachineDurability_Script > MachineDurability)
                if (MachineDurability_Script >= MachineDurability)  //回滿關起來
                {
                    MachineDurabilityFix = false; //停止修
                    isFixMachineShow = false;
                    MachineAni.speed = 1;         //動
                    FixMachineShow.SetActive(false);
                    // FindObjectOfType<FixMachineDurabilityChangeImage>().ChangeOrigin();  //換回去
                };
            SaveRemainingValue = MachineDurability_Script;
            MachineUIBar.fillAmount = SaveRemainingValue / MachineDurability;
            yield return new WaitForSeconds(1f);
        }
    }
    // private void OnCollisionExit2D(Collision2D hit)  //停止恢復
    // {
    //     if (hit.gameObject == FixMachineDurability)
    //  {
    //    MachineDurabilityFix = false;
    //    if (repairCoroutine != null)
    //  {
    //      StopCoroutine(repairCoroutine);
    //     repairCoroutine = null;
    // }
    //}
    // }
    //怪物攻擊機台扣的耐力值
    public void ProduceMachineDurability()
    {

        SaveRemainingValue = MachineDurability_Script - DeductMachineDurability;
        MachineDurability_Script = SaveRemainingValue;
        MachineUIBar.fillAmount = SaveRemainingValue / MachineDurability;
        GameObject.FindWithTag("Monster").GetComponent<MonsterGM>().MonsterAni.SetTrigger("Win");


    }
}
