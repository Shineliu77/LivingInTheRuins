using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrokeProgressGM : MonoBehaviour
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

    //機器耐久值恢復
    public GameObject FixMachineDurability;  //機器耐久維修物
    public GameObject FixMachineShow; //機器維修會顯示在機器上的圖
    private bool isFixMachineShow = false;
    bool MachineDurabilityFix = false;  //不可修
    private Coroutine repairCoroutine; // 協程參考，避免重複啟動
    private bool touchingFixMachine;  //耐久維修物是否碰撞中
    private bool isFixMachineDurability = true;  //耐久值不是0
    //外駔件
    private bool touchingFixedItem;
    private GameObject currentFixedItem;


    //隨機判斷要不要產生怪物 但 新手教學關卡要產生怪物
    bool isProduceMonster;
    public GameObject Monster;
    GameObject MonsterPrefab;
    public Transform ProducePos;
    public float DeductMachineDurability;//扣除機器耐久
    bool isRun;
    private bool iswork = false;//一次只能使用一個

    void Start()
    {
        MachineDurability_Script = MachineDurability;
        SaveRemainingValue = MachineUIBar.fillAmount;

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
        if (coll.gameObject.CompareTag("fixeditem"))
        {
            touchingFixedItem = true;
            currentFixedItem = coll.gameObject;
        }

        if (coll.gameObject == FixMachineDurability)
        {
            touchingFixMachine = true;
            // MachineDurabilityFix = true;
        }
    }

    void OnCollisionExit2D(Collision2D coll)  //結束碰撞
    {
        if (coll.gameObject == currentFixedItem)
        {
            touchingFixedItem = false;
            currentFixedItem = null;
        }

        if (coll.gameObject == FixMachineDurability)
        {
            touchingFixMachine = false;
            //MachineDurabilityFix = false;
        }
    }

    //  滑鼠放開事件
    void OnItemReleased(DraggableReturn2D item)
    {
        // 放開時碰到 fixeditem  與  耐久值不是0
        if (touchingFixedItem && currentFixedItem != null && isFixMachineDurability == true && iswork == false)
        {
            MachineAni.speed = 1;                         //動
            Debug.Log("放開滑鼠：fixeditem");
            AudioManager.Instance.PlaySfx(1);             //音效
            currentFixedItem.SetActive(false);
            MachineAni.SetTrigger("IdleToWalk");
            ProduceMonster();
            iswork = true;
            touchingFixedItem = false;
            currentFixedItem = null;
            return;
        }

        // 放開時碰到修理元件
        if (touchingFixMachine && !MachineDurabilityFix)
        {
            AudioManager.Instance.PlaySfx(2);             //音效
            if (MachineDurability_Script < MachineDurability)
            {
                Debug.Log("放開滑鼠：FixMachine");

                MachineAni.speed = 0;                         //不動
                MachineDurabilityFix = true;
                isFixMachineShow = true;
                FixMachineShow.SetActive(true);//機器維修會顯示在機器上的圖
                                               //FindObjectOfType<FixMachineDurabilityChangeImage>().ChangePicture(); //換回去
                repairCoroutine = StartCoroutine(FixDurabilityOverTime());

                if (Application.loadedLevelName == "TeachGame" && !FindObjectOfType<TeachGM>().teachTwo3) //新手教學關使用
                {
                    {
                        FindObjectOfType<TeachGM>().OpenTeachTwo3();
                    }
                }
            }
        }
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = MachineAni.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("work"))
        {
            if (stateInfo.normalizedTime >= 0.99f && GameObject.FindGameObjectsWithTag("fixeditemOpen").Length <= 0)
            {
                if (Application.loadedLevelName == "TeachGame")
                {
                    FindObjectOfType<TeachGM>().ProduceIteamOpen();
                    AudioManager.Instance.PlaySfx(3);             //音效
                }

                //第一關使用
                else if (Application.loadedLevelName == "FirstGame")
                {
                    FindObjectOfType<FirstGame>().ProduceIteamOpen();
                    AudioManager.Instance.PlaySfx(3);             //音效
                }

                Placement.enabled = true;
                MachineUI.gameObject.SetActive(false);
                MachineDurability_Script = SaveRemainingValue;
            }
            if (stateInfo.normalizedTime < 0.99f)
            {
                MachineUI.gameObject.SetActive(true);
                MachineDurabilityFix = false;    //不可維修
                isFixMachineShow = false;
                //FindObjectOfType<FixMachineDurabilityChangeImage>().ChangeOrigin();  //換回去
                FixMachineShow.SetActive(false);
                float animationLength = stateInfo.length; // 動畫總秒數
                float normalizedTime = stateInfo.normalizedTime; // 播放進度（1.0代表播放完1次）
                float currentTimeInSeconds = animationLength * Mathf.Min(normalizedTime, 1f);

                SaveRemainingValue = MachineDurability_Script - currentTimeInSeconds;
                //    SaveRemainingValue = MachineDurability_Script;      好像要用個存的                                                               //ttt
                MachineUIBar.fillAmount = SaveRemainingValue / MachineDurability;

                if (MachineUIBar.fillAmount > 0.5f)
                {
                    MachineUIBar.sprite = MachineUIBarSprites[0];
                    MachineUIBarOutside.sprite = MachineUIBarSpritesOutside[0]; //耐久值外框原色
                    isFixMachineDurability = true;
                }
                else if (MachineUIBar.fillAmount <= 0f)                          //耐久值歸零不能使用  
                {
                    MachineUIBar.sprite = MachineUIBarSprites[1];
                    MachineUIBarOutside.sprite = MachineUIBarSpritesOutside[1]; //耐久值外框變色
                    isFixMachineDurability = false;
                }
                else
                {
                    MachineUIBar.sprite = MachineUIBarSprites[1];
                    MachineUIBarOutside.sprite = MachineUIBarSpritesOutside[1]; //耐久值外框變色
                    isFixMachineDurability = true;

                }
                MachineUI.transform.GetChild(1).GetComponent<Image>().fillAmount = 1f - (currentTimeInSeconds / animationLength);
                float fillAmount = 1f - (currentTimeInSeconds / animationLength);
                float zRotation = fillAmount * maxRotation; // 比例轉角度，例如 1.0 * -360 = -360°
                needle.localEulerAngles = new Vector3(0, 0, -zRotation);
                if (stateInfo.normalizedTime < 0.5f)
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
            if (stateInfo.normalizedTime > 0.99f)
            {

                iswork = false;

            }
        }
    }

    // 碰撞進入
    //  private void OnCollisionEnter2D(Collision2D coll)
    //{//tag的fixiem物品碰撞
    // if (coll.gameObject.CompareTag("fixeditem"))
    // {

    // coll.gameObject.SetActive(false);
    // MachineAni.SetTrigger("IdleToWalk");
    // ProduceMonster();
    // }

    // if (coll.gameObject == FixMachineDurability)
    //  {
    // if (!MachineDurabilityFix)
    //  {
    //  MachineDurabilityFix = true;
    //  isFixMachineShow = true;
    // FixMachineShow.SetActive(true); //機器維修會顯示在機器上的圖
    //  FindObjectOfType<FixMachineDurabilityChangeImage>().ChangePicture();  //換回去
    //   repairCoroutine = StartCoroutine(FixDurabilityOverTime());
    // }

    // if (Application.loadedLevelName == "TeachGame" && FindObjectOfType<TeachGM>().teachTwo3 == false)  //新手教學關使用
    // {
    //   FindObjectOfType<TeachGM>().OpenTeachTwo3();

    // }
    // }
    //  }

    private IEnumerator FixDurabilityOverTime()   // 每秒恢復10%耐久
    {
        MachineDurability_Script = SaveRemainingValue;
        while (MachineDurabilityFix)
        {
            float repairAmount = MachineDurability * 0.005f;
            //float repairAmount = MachineDurability * 0.1f;
            MachineDurability_Script += repairAmount;

            if (MachineDurability_Script > MachineDurability)
            {
                MachineDurability_Script = MachineDurability;
                if (MachineDurability_Script >= MachineDurability)  //回滿關起來
                {
                    MachineDurabilityFix = false; //停止修
                    isFixMachineShow = false;
                    MachineAni.speed = 1;         //動
                    FixMachineShow.SetActive(false);
                    //FindObjectOfType<FixMachineDurabilityChangeImage>().ChangeOrigin();  //換回去
                }

            }

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


    //判斷要不要產生怪物
    void ProduceMonster()
    {
        isRun = true;
        //  if (Application.loadedLevelName == "TeachGame")
        // {
        // MonsterPrefab = Instantiate(Monster, ProducePos.position, Monster.transform.rotation) as GameObject;
        //MonsterPrefab.GetComponent<MonsterGM>().InitTarget("opener0320");
        // }
        // else
        if (Application.loadedLevelName != "TeachGame")
        {
            //Random.Range(0, 2) 回傳整數 0 或 1,等於 0 回傳 true，否則為 false。
            isProduceMonster = Random.Range(0, 2) == 0;                //暫時停用MonsterGM有問題                      
            if (isProduceMonster && !MonsterPrefab)
            {
                MonsterPrefab = Instantiate(Monster, ProducePos.position, Monster.transform.rotation) as GameObject;
                MonsterPrefab.GetComponent<MonsterGM>().InitTarget("opener0320");
                MonsterPrefab.transform.localScale = new Vector3(
                     -MonsterPrefab.transform.localScale.x,   // 反轉 X 軸
                     MonsterPrefab.transform.localScale.y,
                     MonsterPrefab.transform.localScale.z
                 );
            }
        }
    }
    //怪物攻擊機台扣的耐力值
    public void ProduceMachineDurability()
    {
        SaveRemainingValue = MachineDurability_Script - DeductMachineDurability;
        MachineDurability_Script = SaveRemainingValue;
        MachineUIBar.fillAmount = SaveRemainingValue / MachineDurability;
        GameObject.FindWithTag("Monster").GetComponent<MonsterGM>().MonsterAni.SetTrigger("Win");
    }
}
