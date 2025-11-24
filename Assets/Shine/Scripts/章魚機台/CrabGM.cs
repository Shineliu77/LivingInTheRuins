using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    void Start()
    {
        MachineDurability_Script = MachineDurability;
        SaveRemainingValue = MachineUIBar.fillAmount;

    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = MachineAni.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("get in"))
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
            if (stateInfo.normalizedTime < 0.99f)
            {
                MachineUI.gameObject.SetActive(true);
                MachineDurabilityFix = false;    //不可維修
                float animationLength = stateInfo.length; // 動畫總秒數
                float normalizedTime = stateInfo.normalizedTime; // 播放進度（1.0代表播放完1次）
                float currentTimeInSeconds = animationLength * Mathf.Min(normalizedTime, 1f);

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
        }
        if (canSpawnPCB && CurrentPCB == null)   //生成PCB
        {

            AnimatorStateInfo stateInfo2 = MachineAni.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("hold") && stateInfo.normalizedTime > 0.25f)
            {
                CurrentPCB = Instantiate(PCBPop, PCBPopPlace.position, PCBPopPlace.rotation);
                canSpawnPCB = false;
                if (Application.loadedLevelName == "TeachGame")
                {
                    FindObjectOfType<TeachGM>().OpenTeachEightThree();
                }
            }
        }
    }

    // 碰撞進入
    private void OnCollisionEnter2D(Collision2D coll) //碰撞觸發動畫
    {
        if (coll.gameObject.CompareTag("brokePCB"))
        {
            Debug.Log("碰到電路板");
            ProduceMonster();
            Destroy(coll.gameObject);
            canSpawnPCB = true;
            MachineAni.SetTrigger("IdleToWalk");

        }
    }
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
        if (Application.loadedLevelName == "TeachGame")
        {
            //   MonsterPrefab = Instantiate(Monster, ProducePos.position, Monster.transform.rotation) as GameObject;
            //  MonsterPrefab.GetComponent<MonsterGM>().InitTarget("crab");
        }
        else
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

    private void OnCollisionStay2D(Collision2D hit)  //觸發機器耐久恢復
    {
        if (hit.gameObject == FixMachineDurability)
        {
            //  if (isRun)
            // {
            //   MachineDurabilityFix = false;
            //  if (repairCoroutine != null)
            //   {
            //    StopCoroutine(repairCoroutine);
            //   repairCoroutine = null;
            //}
            // }
            // else if (!MachineDurabilityFix)
            if (!MachineDurabilityFix)
            {
                MachineDurabilityFix = true;
                repairCoroutine = StartCoroutine(FixDurabilityOverTime());
            }
        }
    }

    private IEnumerator FixDurabilityOverTime()   // 每秒恢復10%耐久
    {
        while (MachineDurabilityFix)
        {
            float repairAmount = MachineDurability * 0.005f;
            // float repairAmount = MachineDurability * 0.1f;
            MachineDurability_Script += repairAmount;

            if (MachineDurability_Script > MachineDurability)
                MachineDurability_Script = MachineDurability;
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
