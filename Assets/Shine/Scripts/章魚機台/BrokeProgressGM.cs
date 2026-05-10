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
    public bool iswork = false;//一次只能使用一個  工作不可修
    private bool inTimeOnlyOne = false;//一次只能生成一個
    private bool isworkSfx = false;
    private int savedSeatID = -1; // 用來存儲抓到currentFixedItem的 ID
    public int BrokeProgresssavedSeatID = -1; // 用來存儲抓到currentFixedItem的 ID
    private int buyCount;  //計算商店現在買幾次
    void Start()
    {
        MachineDurability_Script = MachineDurability;
        SaveRemainingValue = MachineUIBar.fillAmount;
        //讀取商店現在買幾次
        int FindShopID = 0;  //是第幾個商品
        buyCount = PlayerPrefs.GetInt("BuyCount_" + FindShopID.ToString(), 0);
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

            var marker = item.gameObject.GetComponentInParent<FirstGameCustomerMarker>();
            if (marker != null)
            {
                savedSeatID = marker.SeatIndex;
            }
            int idToUse = savedSeatID;
            BrokeProgresssavedSeatID = idToUse; // 存外組件打開的編號
            Debug.Log("準備生成，使用座位 ID: " + idToUse + savedSeatID);
            currentFixedItem.SetActive(false);

            Debug.Log("放開滑鼠：fixeditem");
            // AudioManager.Instance.PlaySfx(1);             //音效
            // currentFixedItem.SetActive(false);
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
            AudioManager.Instance.PlaySfx(21);             //音效
            if (MachineDurability_Script < MachineDurability && iswork == false)
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
        var clipInfo = MachineAni.GetCurrentAnimatorClipInfo(0);
        if (stateInfo.IsName("work"))
        {
            if (Application.loadedLevelName != "TeachGame")
            {

                if (clipInfo.Length > 0)
                {

                    float originalLength = clipInfo[0].clip.length;  // 取得動畫的秒數

                    //每買一次就快2秒
                    //int FindShopID = 0;
                    //int buyCount = PlayerPrefs.GetInt("BuyCount_" + FindShopID.ToString(), 0);
                    float targetDuration = Mathf.Max(originalLength - (buyCount * 2.0f));
                    MachineAni.speed = originalLength / targetDuration;
                }
            }

            //if (stateInfo.normalizedTime >= 0.99f && GameObject.FindGameObjectsWithTag("fixeditemOpen").Length <= 0)
            if (stateInfo.normalizedTime >= 0.99f && inTimeOnlyOne == false)
            {
                if (Application.loadedLevelName == "TeachGame")
                {
                    FindObjectOfType<TeachGM>().ProduceIteamOpen();
                    AudioManager.Instance.PlaySfx(0);             //音效
                }

                //第一關使用  //看商店買幾次
                else if (Application.loadedLevelName != "TeachGame")
                {
                    if (savedSeatID != -1)
                    {
                        int idToUse = savedSeatID;
                        BrokeProgresssavedSeatID = idToUse; // 存入外組件打開編號
                                                            // int idToUse = (savedSeatID != -1) ? savedSeatID : 0;

                        Debug.Log("生成，使用座位 ID: " + idToUse + savedSeatID);

                        FindObjectOfType<FirstGame>().ProduceIteamOpen(idToUse);
                        AudioManager.Instance.PlaySfx(0);             //音效
                                                                      //inTimeOnlyOne = true;
                        savedSeatID = -1; // 重置
                        BrokeProgresssavedSeatID = -1;
                        Debug.Log("生成完成，清空座位 ID: " + idToUse + savedSeatID + BrokeProgresssavedSeatID);
                    }
                }
                inTimeOnlyOne = true;
                Placement.enabled = true;
                MachineUI.gameObject.SetActive(false);
                MachineDurability_Script = SaveRemainingValue;
            }
            if (stateInfo.normalizedTime < 0.99f)
            {
                if (isworkSfx == false)
                {
                    AudioManager.Instance.PlaySfx(1);
                    isworkSfx = true;
                }

                inTimeOnlyOne = false;
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

                // if (MachineUIBar.fillAmount > 0.5f)
                // {
                //   MachineUIBar.sprite = MachineUIBarSprites[0];
                //  MachineUIBarOutside.sprite = MachineUIBarSpritesOutside[0]; //耐久值外框原色
                // isFixMachineDurability = true;
                //}
                // else if (MachineUIBar.fillAmount <= 0f)                          //耐久值歸零不能使用  
                //{
                //  MachineUIBar.sprite = MachineUIBarSprites[1];
                //  MachineUIBarOutside.sprite = MachineUIBarSpritesOutside[1]; //耐久值外框變色
                // isFixMachineDurability = false;
                //}
                //else if (MachineUIBar.fillAmount <= 0.5f)
                //{
                //  MachineUIBar.sprite = MachineUIBarSprites[1];
                //   MachineUIBarOutside.sprite = MachineUIBarSpritesOutside[1]; //耐久值外框變色
                //  isFixMachineDurability = true;

                //}
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
            if (stateInfo.normalizedTime > 0.99f)    //恢復速度
            {
                isworkSfx = false;
                iswork = false;
                MachineAni.speed = 1;

            }
        }

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
        else if (MachineUIBar.fillAmount <= 0.5f)
        {
            MachineUIBar.sprite = MachineUIBarSprites[1];
            MachineUIBarOutside.sprite = MachineUIBarSpritesOutside[1]; //耐久值外框變色
            isFixMachineDurability = true;

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