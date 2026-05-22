using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeAPotion : MonoBehaviour
{
    //製藥水材料碰到機器
    public Image Stopwatch; //原色圖片
    public Sprite[] StopwatchUISprites;

    public Image StopwatchOutside; //外框圖片
    public Sprite[] StopwatchUIOutsideSprites;

    public float StopwatchTimer;
    public float ScriptStopwatchTimer;
    bool isStopwatch;

    //隨機判斷要不要產生怪物 但 新手教學關卡要產生怪物
    bool isProduceMonster;
    public GameObject Monster;
    GameObject MonsterPrefab;
    public Transform ProducePos;

    //製藥水機的耐力值
    public float MachineDurability;
    float MachineDurability_Script;
    public float DeductMachineDurability;
    public Image MachineDurabilityBar;
    public Sprite[] MachineDurabilityBarSprite;

    public Image MachineDurabilityBarOustside;   //耐久外框圖片
    public Sprite[] MachineDurabilityBarSpriteOustside;

    public GameObject[] Potions;
    public int SelectPotionID;

    public Transform needle; // 指針物件（需拖曳到 Inspector）
    public float maxRotation = -360f; // 旋轉範圍（滿格時的角度）

    public Animator MachineAni;
    float SaveMachineDurability;
    bool isRun;

    //作為生成使用 
    public GameObject[] PotionsPrefabs;         // 多項可生成物件
    public Transform PotionsPop;         // 生成點
    public int maxPotions = 5;           // 生成上限   //還是5包刮生成點
    private GameObject CurrentPotions;    // 該生成點當前物件
    private bool canSpawnPotions = false;  //是否可生成
    public static List<GameObject> allSpawnedPotions = new List<GameObject>(); // 全域已生成物件紀錄

    //機器耐久值恢復
    public GameObject FixMachineDurability;  //機器耐久維修物
    bool MachineDurabilityFix = false;  //不可修
    public GameObject FixMachineShow; //機器維修會顯示在機器上的圖
    private bool isFixMachineShow = false;
    private Coroutine repairCoroutine; // 協程參考，避免重複啟動
    private bool touchingFixMachine;  //耐久維修物是否碰撞中
    //生成液體的物件
    public GameObject[] MakeLiquidItem;
    private GameObject CurrentChoseLiquidItem;
    private bool Teach = false;  //教學面板開過
    private bool SfxUse = false;
    //確保動畫播完才可以修
    private bool isRed;
    private bool isYellow;
    private bool tisBlue;
    private bool tisGreen;
    private bool MachineDurabilityEmpty = false; //耐久歸零
    private bool isWorkSfx = false;

    private int buyCount;  //計算商店現在買幾次
    void Start()
    {
        ScriptStopwatchTimer = StopwatchTimer;
        MachineDurability_Script = MachineDurability;
        //讀取商店現在買幾次
        int ShopID = 1;
        buyCount = PlayerPrefs.GetInt("BuyCount_" + ShopID.ToString(), 0);
        allSpawnedPotions.Clear();
        Debug.Log("目前數量：" + allSpawnedPotions.Count);
    }
    void OnEnable()    //檢查滑鼠放開事件
    {
        DraggableReturn2D.OnReleased += OnItemReleased;
    }

    void OnDisable()   //取消滑鼠放開事件
    {
        DraggableReturn2D.OnReleased -= OnItemReleased;
    }
    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Red") || coll.gameObject.CompareTag("Yellow") || coll.gameObject.CompareTag("Blue") || coll.gameObject.CompareTag("Green"))
        {
            CurrentChoseLiquidItem = coll.gameObject;
        }
        if (coll.gameObject == FixMachineDurability)
        {
            touchingFixMachine = true;
        }
    }
    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Red") || coll.gameObject.CompareTag("Yellow") || coll.gameObject.CompareTag("Blue") || coll.gameObject.CompareTag("Green"))
        {
            CurrentChoseLiquidItem = null;
        }
        if (coll.gameObject == FixMachineDurability)
        {
            touchingFixMachine = false;
        }

    }
    private void OnItemReleased(DraggableReturn2D Item)
    {
        if (CurrentChoseLiquidItem != null)
        {
            if (Application.loadedLevelName == "TeachGame")
            {
                if (Item.tag == "Red")
                {
                    Reset();
                    SelectPotionID = 0;
                    //AudioManager.Instance.PlaySfx(1);             //音效
                    MachineAni.speed = 1;                         //動
                    MachineAni.SetTrigger("idelTOmoveRR");
                    ProduceMonster();
                    isRed = false;
                }
                if (Item.tag == "Yellow")
                {
                    Reset();
                    SelectPotionID = 1;
                    // AudioManager.Instance.PlaySfx(1);             //音效
                    MachineAni.speed = 1;                         //動
                    MachineAni.SetTrigger("idelTOmoveYY");
                    ProduceMonster();
                    isYellow = false;
                }
                if (Item.tag == "Blue")
                {
                    Reset();
                    SelectPotionID = 2;
                    //AudioManager.Instance.PlaySfx(1);             //音效
                    MachineAni.speed = 1;                         //動
                    MachineAni.SetTrigger("idelTOmoveBB");
                    ProduceMonster();
                    tisBlue = false;
                }
                if (Item.tag == "Green")
                {
                    Reset();
                    SelectPotionID = 3;
                    //AudioManager.Instance.PlaySfx(1);             //音效
                    MachineAni.speed = 1;                         //動
                    MachineAni.SetTrigger("idelTOmoveGG");
                    ProduceMonster();
                    tisGreen = false;
                }
            }

            if (Application.loadedLevelName == "FirstGame")  //第一關使用  碰撞觸發生成與計時器與怪物
            {
                if (Item.tag == "Red")
                {

                    Reset();
                    //SpawnObject(0);
                    canSpawnPotions = true;
                    // AudioManager.Instance.PlaySfx(1);             //音效
                    MachineAni.speed = 1;                         //動
                    MachineAni.SetTrigger("idelTOmoveRR");
                    ProduceMonster();
                    isRed = false;
                }
                if (Item.tag == "Yellow")
                {
                    Reset();
                    canSpawnPotions = true;
                    //AudioManager.Instance.PlaySfx(1);             //音效
                    MachineAni.speed = 1;                         //動
                    MachineAni.SetTrigger("idelTOmoveYY");
                    ProduceMonster();
                    isYellow = false;
                }

                if (Item.tag == "Blue")
                {
                    Reset();
                    canSpawnPotions = true;
                    //AudioManager.Instance.PlaySfx(1);             //音效
                    MachineAni.speed = 1;
                    MachineAni.SetTrigger("idelTOmoveBB");
                    ProduceMonster();
                    tisBlue = false;
                }
                if (Item.tag == "Green")
                {
                    Reset();
                    canSpawnPotions = true;
                    //AudioManager.Instance.PlaySfx(1);             //音效
                    MachineAni.speed = 1;                         //動
                    MachineAni.SetTrigger("idelTOmoveGG");
                    ProduceMonster();
                    tisGreen = false;
                }
            }
        }
        if (touchingFixMachine && !MachineDurabilityFix)
        {
            AudioManager.Instance.PlaySfx(21);             //音效
            //if (isRun && MachineDurability_Script < MachineDurability&& (isRed == false || isYellow == false || tisBlue == false || tisGreen == false))
            if (isRun && MachineDurability_Script < MachineDurability)
            {
                MachineAni.speed = 1;                         //動
                MachineDurabilityFix = false;
                isFixMachineShow = false;
                // FindObjectOfType<FixMachineDurabilityChangeImage>().ChangeOrigin();  //換回去
                FixMachineShow.SetActive(false);
                if (repairCoroutine != null)
                {
                    StopCoroutine(repairCoroutine);
                    repairCoroutine = null;
                }
            }
            else if (!isRun && MachineDurability_Script < MachineDurability)
            {
                MachineAni.speed = 0;                         //不動
                MachineDurabilityFix = true;
                isFixMachineShow = true;
                FixMachineShow.SetActive(true); //機器維修會顯示在機器上的圖
                //FindObjectOfType<FixMachineDurabilityChangeImage>().ChangePicture();  //換回去
                repairCoroutine = StartCoroutine(FixDurabilityOverTime());
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Application.loadedLevelName != "TeachGame")  //其他
        {
            if (allSpawnedPotions.Count >= maxPotions)
            {
                LockMakeLiquidItem();
                // MachineDurabilityEmpty = false;
            }
            else if (MachineDurabilityEmpty == true)
            {
                LockMakeLiquidItem();
            }
            else if (MachineDurabilityEmpty == false)
            {
                UnlockMakeLiquidItem();
            }
            else
            {
                UnlockMakeLiquidItem();
            }
        }
        AnimatorStateInfo stateInfo = MachineAni.GetCurrentAnimatorStateInfo(0);
        var clipInfo = MachineAni.GetCurrentAnimatorClipInfo(0);
        if (stateInfo.IsName("blue work") || stateInfo.IsName("red work") || stateInfo.IsName("green work") || stateInfo.IsName("yellow work"))
        {
            if (isStopwatch && ScriptStopwatchTimer > 0)                                   //時鐘沒有加速
            {
                //  MachineAni.speed = 1;                         //動
                float normalizedTime = Mathf.Min(stateInfo.normalizedTime, 1f);
                Stopwatch.gameObject.SetActive(true);
                // ScriptStopwatchTimer -= Time.deltaTime;
                // Stopwatch.transform.GetChild(1).GetComponent<Image>().fillAmount = ScriptStopwatchTimer / StopwatchTimer;
                // float fillAmount = ScriptStopwatchTimer / StopwatchTimer;
                Stopwatch.transform.GetChild(1).GetComponent<Image>().fillAmount = 1f - normalizedTime;
                //float zRotation = fillAmount * maxRotation; // 比例轉角度，例如 1.0 * -360 = -360°
                float zRotation = (1f - normalizedTime) * maxRotation;
                needle.localEulerAngles = new Vector3(0, 0, -zRotation);
                //if (ScriptStopwatchTimer / StopwatchTimer > 0.5f)
                if (normalizedTime < 0.5f)
                {
                    Stopwatch.transform.GetChild(1).GetComponent<Image>().sprite = StopwatchUISprites[0];
                    StopwatchOutside.sprite = StopwatchUIOutsideSprites[0];

                }
                else
                {
                    Stopwatch.transform.GetChild(1).GetComponent<Image>().sprite = StopwatchUISprites[1];
                    StopwatchOutside.sprite = StopwatchUIOutsideSprites[1];
                }
                //if (Stopwatch.transform.GetChild(1).GetComponent<Image>().fillAmount == 0)
                if (normalizedTime >= 0.99f)
                {
                    if (Application.loadedLevelName == "TeachGame")
                    {
                        Potions[SelectPotionID].SetActive(true);
                        if (SfxUse == false)
                        {
                            AudioManager.Instance.PlaySfx(2);             //音效
                            SfxUse = true;
                        }

                        Stopwatch.gameObject.SetActive(false);
                    }
                    Stopwatch.gameObject.SetActive(false);
                    isRun = false;
                    if (Teach) return; //不重開
                    if (Application.loadedLevelName == "TeachGame")
                    {
                        FindObjectOfType<TeachGM>().OpenTeach8();
                        Teach = true;
                    }
                }
            }
            // if (isRun)
            {
                if (isRun)
                {
                    MachineDurabilityFix = false;   //不可維修
                    isFixMachineShow = false;
                    // FindObjectOfType<FixMachineDurabilityChangeImage>().ChangeOrigin();  //換回去
                    FixMachineShow.SetActive(false);
                }
                // AnimatorStateInfo stateInfo = MachineAni.GetCurrentAnimatorStateInfo(0);
                // var clipInfo = MachineAni.GetCurrentAnimatorClipInfo(0);
                //  if (stateInfo.IsName("blue work") || stateInfo.IsName("red work") || stateInfo.IsName("green work") || stateInfo.IsName("yellow work"))
                {
                    if (Application.loadedLevelName != "TeachGame")
                    {
                        if (clipInfo.Length > 0)
                        {
                            float originalLength = clipInfo[0].clip.length;// 取得動畫的秒數
                            float targetDuration = Mathf.Max(originalLength - (buyCount * 2.0f)); //每買一次就快2秒
                            MachineAni.speed = originalLength / targetDuration;
                        }
                    }
                    if (isWorkSfx == false)
                    {
                        AudioManager.Instance.PlaySfx(3);             //音效
                        isWorkSfx = true;
                    }

                    if (stateInfo.normalizedTime >= 0.99f)
                    {

                        MachineDurability_Script = SaveMachineDurability;
                        DeductDurability();
                        //  isRun = false;


                    }
                    else
                    {
                        float animationLength = stateInfo.length; // 動畫總秒數
                        float normalizedTime = stateInfo.normalizedTime; // 播放進度（1.0代表播放完1次）
                        float currentTimeInSeconds = animationLength * Mathf.Min(normalizedTime, 1f);

                        SaveMachineDurability = MachineDurability_Script - currentTimeInSeconds;
                        DeductDurability();

                    }
                }
            }
        }
        if (Application.loadedLevelName != "TeachGame")   //第一關生成使用 配合動畫生成
        {
            AnimatorStateInfo stateInfo2 = MachineAni.GetCurrentAnimatorStateInfo(0);
            if (canSpawnPotions == true)
            {
                if (stateInfo2.IsName("red work") && stateInfo2.normalizedTime > 0.99f)
                {
                    SpawnObject(0);
                    isWorkSfx = false;
                    //canSpawnPotions = false;
                    // Stopwatch.gameObject.SetActive(false);
                    // MachineAni.SetBool("idelTOmoveR",true);

                }
                if (stateInfo2.IsName("yellow work") && stateInfo2.normalizedTime > 0.99f)
                {
                    SpawnObject(1);
                    isWorkSfx = false;
                    //canSpawnPotions = false;
                }
                if (stateInfo2.IsName("blue work") && stateInfo2.normalizedTime > 0.99f)
                {
                    SpawnObject(2);
                    isWorkSfx = false;
                    //canSpawnPotions = false;
                }
                if (stateInfo2.IsName("green work") && stateInfo2.normalizedTime > 0.99f)
                {
                    SpawnObject(3);
                    isWorkSfx = false;
                    //canSpawnPotions = false;
                }
                if (stateInfo2.IsName("red work") && stateInfo2.normalizedTime > 0.01f || stateInfo2.IsName("yellow work") && stateInfo2.normalizedTime > 0.01f || stateInfo2.IsName("blue work") && stateInfo2.normalizedTime > 0.01f || stateInfo2.IsName("green work") && stateInfo2.normalizedTime > 0.01f)
                {
                    isRed = false;
                    isYellow = false;
                    tisBlue = false;
                    tisGreen = false;
                    LockMakeLiquidItem();
                }
            }
            AnimatorStateInfo stateInfo3 = MachineAni.GetCurrentAnimatorStateInfo(0);
            if (stateInfo3.IsName("red work") && stateInfo2.normalizedTime > 0.99f)
            {
                MachineAni.speed = 1;
                isRed = true;
                UnlockMakeLiquidItem();
            }
            if (stateInfo3.IsName("yellow work") && stateInfo2.normalizedTime > 0.99f)
            {
                MachineAni.speed = 1;
                isYellow = true;
                UnlockMakeLiquidItem();
            }
            if (stateInfo3.IsName("blue work") && stateInfo2.normalizedTime > 0.99f)
            {
                MachineAni.speed = 1;
                tisBlue = true;
                UnlockMakeLiquidItem();
            }
            if (stateInfo3.IsName("green work") && stateInfo2.normalizedTime > 0.99f)
            {
                MachineAni.speed = 1;
                tisGreen = true;
                UnlockMakeLiquidItem();
            }
        }
    }

    //private void OnCollisionEnter2D(Collision2D hit)
    // {
    //   if (Application.loadedLevelName == "TeachGame")
    // {
    //   if (hit.collider.tag == "Red")
    //  {
    //  Reset();
    //  SelectPotionID = 0;
    //ProduceMonster();
    //}
    //if (hit.collider.tag == "Yellow")
    //{
    //  Reset();
    //SelectPotionID = 1;
    //ProduceMonster();

    //            }
    //          if (hit.collider.tag == "Blue")
    //        {
    //          Reset();
    //        SelectPotionID = 2;
    //      ProduceMonster();
    //
    //}
    //           if (hit.collider.tag == "Green")
    //         {
    //           Reset();
    //         SelectPotionID = 3;
    //       ProduceMonster();
    // }
    //}

    //       if (Application.loadedLevelName == "FirstGame")  //第一關使用  碰撞觸發生成與計時器與怪物
    //     {
    //       if (hit.collider.tag == "Red")
    //     {
    //       Reset();
    //     canSpawnPotions = true;
    //   ProduceMonster();
    //}
    //if (hit.collider.tag == "Yellow")
    //           {
    //             Reset();
    //           canSpawnPotions = true;
    //         ProduceMonster();
    //   }

    // if (hit.collider.tag == "Blue")
    // {
    //   Reset();
    // canSpawnPotions = true;
    // ProduceMonster();
    //}
    //if (hit.collider.tag == "Green")
    // {
    //   Reset();
    // canSpawnPotions = true;
    // ProduceMonster();
    //}


    //if (hit.gameObject == FixMachineDurability)  //觸發機器耐久恢復
    //{

    //  if (isRun)
    // {
    //     MachineDurabilityFix = false;   //不可維修
    //  }
    // else if (!MachineDurabilityFix ||!isRun)
    // {
    //    MachineDurabilityFix = true;
    //   repairCoroutine = StartCoroutine(FixDurabilityOverTime());
    // }
    // }

    //}
    //}

    // private void OnCollisionStay2D(Collision2D hit)  //觸發機器耐久恢復
    //{
    // if (hit.gameObject == FixMachineDurability)
    // {
    // if (isRun)
    // {
    //  MachineDurabilityFix = false;
    // isFixMachineShow = false;
    // FindObjectOfType<FixMachineDurabilityChangeImage>().ChangeOrigin();  //換回去
    // FixMachineShow.SetActive(false);
    //if (repairCoroutine != null)
    //{
    //  StopCoroutine(repairCoroutine);
    //  repairCoroutine = null;
    //}
    //}
    //else if (!MachineDurabilityFix)
    //{
    // MachineDurabilityFix = true;
    // isFixMachineShow = true;
    // FixMachineShow.SetActive(true); //機器維修會顯示在機器上的圖
    //FindObjectOfType<FixMachineDurabilityChangeImage>().ChangePicture();  //換回去
    //repairCoroutine = StartCoroutine(FixDurabilityOverTime());
    // }
    //}
    //}

    private IEnumerator FixDurabilityOverTime()   // 每秒恢復10%耐久
    {
        MachineDurability_Script = SaveMachineDurability;
        while (MachineDurabilityFix)
        {
            float repairAmount = MachineDurability * 0.005f;
            // float repairAmount = MachineDurability * 0.1f;
            MachineDurability_Script += repairAmount;

            // if (MachineDurability_Script > MachineDurability)
            //  MachineDurability_Script = MachineDurability;

            if (MachineDurability_Script > MachineDurability)
            {
                MachineDurability_Script = MachineDurability;

                if (MachineDurability_Script >= MachineDurability)  //回滿關起來
                {
                    MachineDurabilityEmpty = false;
                    MachineAni.speed = 1;                         //動
                    MachineDurabilityFix = false; //停止修
                    isFixMachineShow = false;
                    FixMachineShow.SetActive(false);
                    // FindObjectOfType<FixMachineDurabilityChangeImage>().ChangeOrigin();  //換回去
                }
            }

            //  同步更新 SaveMachineDurability
            SaveMachineDurability = MachineDurability_Script;

            // 更新顯示條
            DeductDurability();

            yield return new WaitForSeconds(1f);
        }
    }
    //private void OnCollisionExit2D(Collision2D hit)  //停止恢復
    // {
    //  if (hit.gameObject == FixMachineDurability)
    //  {
    //   MachineDurabilityFix = false;
    //   if (repairCoroutine != null)
    //  {
    //     StopCoroutine(repairCoroutine);
    //    repairCoroutine = null;
    //}
    // }
    // }

    public void SpawnObject(int prefabIndex) // 場景上含生成點最多可以有4個物件，若達生成上限，直接停止   
    {
        if (allSpawnedPotions.Count >= maxPotions)
        {
            Debug.Log(" 已達生成上限！");
            return;
        }
        if (prefabIndex == 0)  //紅
        {
            CurrentPotions = Instantiate(PotionsPrefabs[prefabIndex], PotionsPop.position, PotionsPop.rotation) as GameObject;
            allSpawnedPotions.Add(CurrentPotions); Debug.Log($" 生成物件：{PotionsPrefabs[prefabIndex].name} (目前總數：{allSpawnedPotions.Count})");
            DestroyPrefabButton.Add(CurrentPotions);
            AudioManager.Instance.PlaySfx(2);             //音效
            canSpawnPotions = false;
        }

        if (prefabIndex == 1)//黃
        {
            CurrentPotions = Instantiate(PotionsPrefabs[prefabIndex], PotionsPop.position, PotionsPop.rotation) as GameObject;
            allSpawnedPotions.Add(CurrentPotions); Debug.Log($" 生成物件：{PotionsPrefabs[prefabIndex].name} (目前總數：{allSpawnedPotions.Count})");
            DestroyPrefabButton.Add(CurrentPotions);
            AudioManager.Instance.PlaySfx(2);             //音效
            canSpawnPotions = false;
        }

        if (prefabIndex == 2) //藍
        {
            CurrentPotions = Instantiate(PotionsPrefabs[prefabIndex], PotionsPop.position, PotionsPop.rotation) as GameObject;
            allSpawnedPotions.Add(CurrentPotions); Debug.Log($" 生成物件：{PotionsPrefabs[prefabIndex].name} (目前總數：{allSpawnedPotions.Count})");
            DestroyPrefabButton.Add(CurrentPotions);
            AudioManager.Instance.PlaySfx(2);             //音效
            canSpawnPotions = false;
        }

        if (prefabIndex == 3) //綠
        {
            CurrentPotions = Instantiate(PotionsPrefabs[prefabIndex], PotionsPop.position, PotionsPop.rotation) as GameObject;
            allSpawnedPotions.Add(CurrentPotions); Debug.Log($" 生成物件：{PotionsPrefabs[prefabIndex].name} (目前總數：{allSpawnedPotions.Count})");
            DestroyPrefabButton.Add(CurrentPotions);
            AudioManager.Instance.PlaySfx(2);             //音效
            canSpawnPotions = false;
        }

    }

    public static void RemoveSpawnedObject(GameObject obj)  //刪除生成物件 恢復上限、場景數量
    {
        if (allSpawnedPotions.Contains(obj))
        {
            allSpawnedPotions.Remove(obj);
            Debug.Log($" 移除物件：{obj.name} (目前剩餘：{allSpawnedPotions.Count})");
        }
    }

    //製作藥水判斷要不要產生怪物
    void ProduceMonster()
    {
        isRun = true;
        if (Application.loadedLevelName == "TeachGame")
        {
            MonsterPrefab = Instantiate(Monster, ProducePos.position, Monster.transform.rotation) as GameObject;
            MonsterPrefab.GetComponent<MonsterGM>().InitTarget("blender");
        }
        else
        {
            //Random.Range(0, 2) 回傳整數 0 或 1,等於 0 回傳 true，否則為 false。
            isProduceMonster = Random.Range(0, 2) == 0;
            if (isProduceMonster && !MonsterPrefab)
            {
                MonsterPrefab = Instantiate(Monster, ProducePos.position, Monster.transform.rotation) as GameObject;
                MonsterPrefab.GetComponent<MonsterGM>().InitTarget("blender");
            }

        }
    }
    //怪物攻擊機台扣的耐力值
    public void ProduceMachineDurability()
    {
        SaveMachineDurability = MachineDurability_Script - DeductMachineDurability;
        MachineDurability_Script = SaveMachineDurability;
        DeductDurability();
        GameObject.FindWithTag("Monster").GetComponent<MonsterGM>().MonsterAni.SetTrigger("Win");

    }
    public void Reset()
    {
        isStopwatch = true;
        ScriptStopwatchTimer = StopwatchTimer;
        for (int i = 0; i < Potions.Length; i++)
        {
            Potions[i].SetActive(false);
        }

    }

    void DeductDurability()
    {
        MachineDurabilityBar.fillAmount = SaveMachineDurability / MachineDurability;

        if (SaveMachineDurability / MachineDurability > 0.5f)
        {
            MachineDurabilityBar.sprite = MachineDurabilityBarSprite[0];
            MachineDurabilityBarOustside.sprite = MachineDurabilityBarSpriteOustside[0];  //耐久值外框原色
            UnlockMakeLiquidItem();
            MachineDurabilityEmpty = false;
        }
        else if (SaveMachineDurability / MachineDurability <= 0f)      //耐久值歸零不能使用                                   
        {
            Debug.Log("耐久值歸零");
            MachineDurabilityEmpty = true;
            MachineDurabilityBar.sprite = MachineDurabilityBarSprite[1];
            MachineDurabilityBarOustside.sprite = MachineDurabilityBarSpriteOustside[1];  //耐久值外框變色
            LockMakeLiquidItem();
        }
        else if (SaveMachineDurability / MachineDurability < 0.5f)
        {
            MachineDurabilityBar.sprite = MachineDurabilityBarSprite[1];
            MachineDurabilityBarOustside.sprite = MachineDurabilityBarSpriteOustside[1];  //耐久值外框變色
            MachineDurabilityEmpty = false;
        }
        else if (SaveMachineDurability / MachineDurability > 0f)      //耐久值不為零                          
        {
            Debug.Log("耐久值不為零");
            MachineDurabilityEmpty = false;
            UnlockMakeLiquidItem();
        }
        //  else
        // {
        //    MachineDurabilityBar.sprite = MachineDurabilityBarSprite[1];
        //   MachineDurabilityBarOustside.sprite = MachineDurabilityBarSpriteOustside[1];  //耐久值外框變色
        // }
        //  if (MachineDurabilityBar.fillAmount <= 0)
        //   {
        // MachineDurabilityBar.sprite = MachineDurabilityBarSprite[1];
        // MachineDurabilityBarOustside.sprite = MachineDurabilityBarSpriteOustside[1];  //耐久值外框變色
        // LockMakeLiquidItem();
        //}
    }

    private void LockMakeLiquidItem()
    {
        for (int i = 0; i < MakeLiquidItem.Length; i++)
        {
            MakeLiquidItem[i].GetComponent<DraggableReturn2D>();
            MakeLiquidItem[i].GetComponent<DraggableReturn2D>().enabled = false;
        }
    }

    private void UnlockMakeLiquidItem()
    {
        for (int i = 0; i < MakeLiquidItem.Length; i++)
        {
            MakeLiquidItem[i].GetComponent<DraggableReturn2D>();
            MakeLiquidItem[i].GetComponent<DraggableReturn2D>().enabled = true;
        }
    }
}
