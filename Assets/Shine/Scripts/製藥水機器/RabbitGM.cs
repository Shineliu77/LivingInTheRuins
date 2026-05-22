using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RabbitGM : MonoBehaviour
{
    //製藥水材料碰到機器
    public Image Stopwatch; //原色圖片
    public Sprite[] StopwatchUISprites;

    public Image StopwatchOutside; //外框圖片
    public Sprite[] StopwatchUIOutsideSprites;

    public float StopwatchTimer;
    public float ScriptStopwatchTimer;
    bool isStopwatch;

    //隨機判斷要不要產生怪物 
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

    // public GameObject[] Potions;
    // public int SelectPotionID;
    private bool pendingTake = false;

    public Transform needle; // 指針物件（需拖曳到 Inspector）
    public float maxRotation = -360f; // 旋轉範圍（滿格時的角度）
    public bool ShowbeforeTeach4RFix = false;
    public Animator MachineAni;
    float SaveMachineDurability;
    bool isRun;
    bool Show = false;

    //生成物件
    public GameObject[] ObjectPrefabs;         // 多項可生成物件
    public Transform ObjectPop;         // 生成點
    public int maxObjects = 5;           // 生成上限
    public GameObject CurrentObject;    // 該生成點當前物件
    public static List<GameObject> allSpawnedObjects = new List<GameObject>(); // 全域已生成物件紀錄

    private bool canSpawn = false;
    public bool SpawnOK = false;
    //機器耐久值恢復
    public GameObject FixMachineDurability;  //機器耐久維修物
    private bool touchingFixMachine;  //耐久維修物是否碰撞中
    bool MachineDurabilityFix = false;  //不可修
    public GameObject FixMachineShow; //機器維修會顯示在機器上的圖
    private bool isFixMachineShow = false;
    private Coroutine repairCoroutine; // 協程參考，避免重複啟動
                                       // public System.Action OnWaitFinished;
    private bool waitEventSent = false;
    public Button[] RabbitButton;   //兔子按鈕
    public bool isLockButton = false;
    //確保動畫換回idel才可以修
    private bool isIdel;
    //確保動畫播完才可以修
    private bool takeCircle;
    private bool takeSquare;
    private bool takeTriangle;
    private bool MachineDurabilityEmpty = false; //耐久歸零
    private bool isworkSfx = false;

    private int buyCount;  //計算商店現在買幾次
    void Start()
    {
        ScriptStopwatchTimer = StopwatchTimer;
        MachineDurability_Script = MachineDurability;
        //讀取商店現在買幾次
        int ShopID = 2;
        buyCount = PlayerPrefs.GetInt("BuyCount_" + ShopID.ToString(), 0);
        allSpawnedObjects.Clear();
        Debug.Log("目前數量：" + allSpawnedObjects.Count);
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
        if (coll.gameObject == FixMachineDurability)
        {
            touchingFixMachine = true;

        }
    }
    void OnCollisionExit2D(Collision2D coll)  //結束碰撞
    {

        if (coll.gameObject == FixMachineDurability)
        {
            touchingFixMachine = false;

        }
    }
    // Update is called once per frame
    void Update() //無效
    {
        if (Application.loadedLevelName == "TeachGame")
        {

            if (FindObjectOfType<TeachGM>().TeachGMLockRabbitButton == true)  //新手關如果沒鎖的話
            {
                if (allSpawnedObjects.Count >= maxObjects)
                {
                    LockRabbitButtons();
                }
                if (MachineDurabilityEmpty == true)
                {
                    LockRabbitButtons();
                }
                else if (MachineDurabilityEmpty == false)
                {
                    UnlockRabbitButtons();
                }
                else
                {
                    UnlockRabbitButtons();
                }
            }

        }
        else if (Application.loadedLevelName != "TeachGame")  //其他
        {
            // if (allSpawnedObjects.Count >= maxObjects)
            // {
            //   LockRabbitButtons();
            // }
            if (MachineDurabilityEmpty == true)
            {
                LockRabbitButtons();
            }
            else if (MachineDurabilityEmpty == false)
            {
                UnlockRabbitButtons();
            }
            //else
            //{
            // UnlockRabbitButtons();
            // }
        }
        //   if (isStopwatch && ScriptStopwatchTimer > 0)
        //  {
        // Stopwatch.gameObject.SetActive(true);
        // ScriptStopwatchTimer -= Time.deltaTime;
        // Stopwatch.transform.GetChild(1).GetComponent<Image>().fillAmount = ScriptStopwatchTimer / StopwatchTimer;
        // float fillAmount = ScriptStopwatchTimer / StopwatchTimer;
        //  float zRotation = fillAmount * maxRotation; // 比例轉角度，例如 1.0 * -360 = -360°
        // needle.localEulerAngles = new Vector3(0, 0, -zRotation);
        //   if (ScriptStopwatchTimer / StopwatchTimer > 0.5f)
        //  {
        // Stopwatch.transform.GetChild(1).GetComponent<Image>().sprite = StopwatchUISprites[0];
        //    StopwatchOutside.sprite = StopwatchUIOutsideSprites[0];

        //  }
        // else
        //  {
        //    Stopwatch.transform.GetChild(1).GetComponent<Image>().sprite = StopwatchUISprites[1];
        //     StopwatchOutside.sprite = StopwatchUIOutsideSprites[1];
        // }
        //   if (Stopwatch.transform.GetChild(1).GetComponent<Image>().fillAmount == 0)
        // {
        // Potions[SelectPotionID].SetActive(true);
        //  Stopwatch.gameObject.SetActive(false);
        //   isRun = false;
        // if (Application.loadedLevelName == "TeachGame")
        // {
        // FindObjectOfType<TeachGM>().OpenTeach8();
        //}
        // }
        //}
        if (isRun)
        {
            AnimatorStateInfo stateInfo = MachineAni.GetCurrentAnimatorStateInfo(0);
            var clipInfo = MachineAni.GetCurrentAnimatorClipInfo(0);

            if (stateInfo.IsName("work circle") || stateInfo.IsName("work square") || stateInfo.IsName("work triangle"))
            {
                if (Application.loadedLevelName != "TeachGame")
                {
                    if (clipInfo.Length > 0)
                    {
                        float originalLength = clipInfo[0].clip.length;// 取得動畫的秒數
                        float targetDuration = Mathf.Max(originalLength - (buyCount * 2.0f));  //每買一次就快2秒
                        MachineAni.speed = originalLength / targetDuration;
                    }
                }
                {  //倒數計時
                    float normalizedTime = Mathf.Min(stateInfo.normalizedTime, 1f);
                    Stopwatch.gameObject.SetActive(true);
                    if (isworkSfx == false)
                    {
                        AudioManager.Instance.PlaySfx(7);             //音效
                        isworkSfx = true;
                    }


                    if (Application.loadedLevelName == "TeachGame") //如果再新手交關
                    {
                        if (!FindObjectOfType<TeachGM>().teachThree4)
                        {
                            FindObjectOfType<TeachGM>().OpenBTeachThree4();
                            // FindObjectOfType<TeachGM>().teachThree4 = true;
                        }
                    }

                    Stopwatch.transform.GetChild(1).GetComponent<Image>().fillAmount = 1f - normalizedTime;
                    float zRotation = (1f - normalizedTime) * maxRotation;
                    needle.localEulerAngles = new Vector3(0, 0, -zRotation);
                    // if (ScriptStopwatchTimer / StopwatchTimer > 0.5f)
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
                    if (normalizedTime >= 0.98f)
                    {
                        isworkSfx = false;
                        Stopwatch.gameObject.SetActive(false);
                        isRun = false;
                        MachineDurabilityFix = true; // 動畫結束後允許修復
                        isFixMachineShow = true;
                        FixMachineShow.SetActive(true); //機器維修會顯示在機器上的圖
                                                        // FindObjectOfType<FixMachineDurabilityChangeImage>().ChangePicture();  //換回去
                    }
                    MachineDurabilityFix = false;   //不可維修
                    isFixMachineShow = false;
                    //FindObjectOfType<FixMachineDurabilityChangeImage>().ChangeOrigin();  //換回去
                    FixMachineShow.SetActive(false);
                }  //倒數計時


                if (stateInfo.normalizedTime >= 0.98f)
                {

                    MachineDurability_Script = SaveMachineDurability;
                    DeductDurability();                                                              //ttt

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
        //if ( Application.loadedLevelName == "FirstGame")   //第一關生成使用 配合動畫生成
        // {
        AnimatorStateInfo stateInfo2 = MachineAni.GetCurrentAnimatorStateInfo(0);
        if (canSpawn == true)
        {
            if (stateInfo2.IsName("work circle") && stateInfo2.normalizedTime > 0.98f)  //生成圓
            {
                Debug.Log("開始work circle");
                SpawnObject(0);
                canSpawn = false;
                //  Stopwatch.gameObject.SetActive(false);
                //SpawnOK = true;

                //恢復速度
                MachineAni.speed = 1;
            }

            AnimatorStateInfo stateInfo3 = MachineAni.GetCurrentAnimatorStateInfo(0);  //生成方
            if (stateInfo3.IsName("work square") && stateInfo3.normalizedTime > 0.98f)
            {
                Debug.Log("開始work square");
                SpawnObject(1);
                canSpawn = false;
                // Stopwatch.gameObject.SetActive(false);
                //SpawnOK = true;
                //恢復速度
                MachineAni.speed = 1;
            }

            AnimatorStateInfo stateInfo4 = MachineAni.GetCurrentAnimatorStateInfo(0);  //生成角

            if (stateInfo4.IsName("work triangle") && stateInfo4.normalizedTime > 0.98f)
            {
                Debug.Log("開始work triangle");
                SpawnObject(2);
                canSpawn = false;
                //恢復速度
                MachineAni.speed = 1;
                //  Stopwatch.gameObject.SetActive(false);
                // SpawnOK = true;
            }
        }
        // }
        // AnimatorStateInfo stateInfo5 = MachineAni.GetCurrentAnimatorStateInfo(0);  //非手持等待  不能切動畫
        //if (stateInfo5.IsName("wait circle") || stateInfo5.IsName("wait square") || stateInfo5.IsName("wait triangle") && stateInfo5.normalizedTime > 0.99f)  //但會接著吻合條件動畫會直接跳
        //if (stateInfo5.normalizedTime > 0.99f)  //但會接著吻合條件動畫會直接跳
        //{
        // if(stateInfo5.IsName("take away circle") || stateInfo5.IsName("take away triangle") || stateInfo5.IsName("take away square"))  //直接刪無法導向
        //  if(stateInfo5.IsName("wait circle") && stateInfo5.normalizedTime > 0.99f || stateInfo5.IsName("wait square")|| stateInfo5.IsName("wait triangle") )
        //   {
        //   SpawnOK = true;
        // }
        // }
        AnimatorStateInfo stateInfo6 = MachineAni.GetCurrentAnimatorStateInfo(0);       //鎖住按鈕       
                                                                                        //if ((stateInfo6.IsName("work circle") && stateInfo6.normalizedTime > 0.01f )||( stateInfo6.IsName("work square") && stateInfo6.normalizedTime > 0.01f )|| (stateInfo6.IsName("work triangle") && stateInfo6.normalizedTime > 0.01f) || (stateInfo6.IsName("wait circle") && stateInfo6.normalizedTime > 0.01f) || (stateInfo6.IsName("wait square") && stateInfo6.normalizedTime > 0.01f) || (stateInfo6.IsName("wait triangle") && stateInfo6.normalizedTime > 0.01f) || (stateInfo6.IsName("take away circle") && stateInfo6.normalizedTime > 0.01f) || (stateInfo6.IsName("take away square") && stateInfo6.normalizedTime > 0.01f) || (stateInfo6.IsName("take away triangle") && stateInfo6.normalizedTime > 0.01f))
        if (isLockButton == true)
        {
            LockRabbitButtons();
        }
        if ((stateInfo6.IsName("work circle") && stateInfo6.normalizedTime > 0.01f) || (stateInfo6.IsName("work square") && stateInfo6.normalizedTime > 0.01f) || (stateInfo6.IsName("work triangle") && stateInfo6.normalizedTime > 0.01f) || (stateInfo6.IsName("wait circle") && stateInfo6.normalizedTime > 0.01f) || (stateInfo6.IsName("wait square") && stateInfo6.normalizedTime > 0.01f) || (stateInfo6.IsName("wait triangle") && stateInfo6.normalizedTime > 0.01f))
        {
            isLockButton = true;
            isIdel = false;
            takeCircle = false;
            takeSquare = false;
            takeTriangle = false;
            LockRabbitButtons();
        }
        //if (stateInfo6.IsName("take away circle") && stateInfo6.normalizedTime > 0.99f)
        //  {
        //    takeCircle = true;
        // }
        // if (stateInfo6.IsName("take away square") && stateInfo6.normalizedTime > 0.99f)
        // {
        //    takeSquare = true;
        // }
        //if ((stateInfo6.IsName("take away triangle") && stateInfo6.normalizedTime > 0.99f) || (stateInfo6.IsName("take away square") && stateInfo6.normalizedTime > 0.99f) || (stateInfo6.IsName("take away triangle") && stateInfo6.normalizedTime > 0.99f))
        if ((stateInfo6.IsName("take away circle") && stateInfo6.normalizedTime > 0.99f) || (stateInfo6.IsName("take away square") && stateInfo6.normalizedTime > 0.99f) || (stateInfo6.IsName("take away triangle") && stateInfo6.normalizedTime > 0.99f))
        {
            takeCircle = true;
            takeSquare = true;
            takeTriangle = true;
            isLockButton = false;
        }
        AnimatorStateInfo stateInfo7 = MachineAni.GetCurrentAnimatorStateInfo(0);       //解鎖按鈕   
        //if (allSpawnedObjects.Count <= maxObjects&&stateInfo7.IsName("idel") &&!MachineDurabilityEmpty)
        if ((allSpawnedObjects.Count <= maxObjects) && stateInfo7.IsName("take away circle") || stateInfo7.IsName("take away square") || stateInfo7.IsName("take away triangle") && !MachineDurabilityEmpty)
        {
            isIdel = true;
            isLockButton = false;
            UnlockRabbitButtons();
        }
        //if ((stateInfo7.IsName("idel") && MachineDurabilityEmpty == true) || (allSpawnedObjects.Count >= maxObjects))
        if ((stateInfo7.IsName("take away circle") || stateInfo7.IsName("take away square") || stateInfo7.IsName("take away triangle") && MachineDurabilityEmpty == true) || (allSpawnedObjects.Count >= maxObjects))
        {
            isIdel = true;
            isLockButton = false;
            LockRabbitButtons();
        }
    }
    public enum WorkType
    {
        None,
        Circle,
        Square,
        Triangle
    }
    public void RequestTake()
    {
        pendingTake = true;
    }

    public void OnWaitStateEntered()
    {
        if (!pendingTake) return;

        pendingTake = false;

        AnimatorStateInfo state = MachineAni.GetCurrentAnimatorStateInfo(0);

        ResetAllTakeTriggers();

        if (state.IsName("wait circle"))
            MachineAni.SetTrigger("takecircle");
        else if (state.IsName("wait square"))
            MachineAni.SetTrigger("takeSquare");
        else if (state.IsName("wait triangle"))
            MachineAni.SetTrigger("takeTriangle");
    }
    public WorkType currentWork = WorkType.None;

    public void RabbitCircle()   //如果按到哪個按鈕觸法哪個按鈕得生成
    {
        isLockButton = true;

        isIdel = false;
        takeCircle = false;
        AudioManager.Instance.PlaySfx(8);             //音效
        MachineAni.speed = 1;                         //動
        Reset();
        //如果觸發按鈕的話
        canSpawn = true;
        currentWork = WorkType.Circle;
        MachineAni.SetTrigger("IdleToWalkCircle");
        ProduceMonster();
    }
    public void RabbitSquaare()   //如果按到哪個按鈕觸法哪個按鈕得生成
    {
        isLockButton = true;
        isIdel = false;
        takeSquare = false;
        AudioManager.Instance.PlaySfx(8);             //音效
        MachineAni.speed = 1;                         //動
        Reset();
        currentWork = WorkType.Square;
        canSpawn = true;
        MachineAni.SetTrigger("IdleToSquare");
        ProduceMonster();
    }
    public void RabbittTriangle()   //如果按到哪個按鈕觸法哪個按鈕得生成
    {
        isLockButton = true;
        isIdel = false;
        takeTriangle = false;
        AudioManager.Instance.PlaySfx(8);             //音效
        MachineAni.speed = 1;                         //動
        Reset();
        currentWork = WorkType.Triangle;
        canSpawn = true;
        MachineAni.SetTrigger("IdleToTriangle");
        ProduceMonster();
    }

    void ResetAllTakeTriggers()
    {
        MachineAni.ResetTrigger("takecircle");
        MachineAni.ResetTrigger("takeSquare");
        MachineAni.ResetTrigger("takeTriangle");
    }

    public void PlayTakeAnimation()
    {
        AnimatorStateInfo state = MachineAni.GetCurrentAnimatorStateInfo(0);

        ResetAllTakeTriggers();

        if (state.IsName("wait circle"))
        {
            MachineAni.SetTrigger("takecircle");
        }
        else if (state.IsName("wait square"))
        {
            MachineAni.SetTrigger("takeSquare");
        }
        else if (state.IsName("wait triangle"))
        {
            MachineAni.SetTrigger("takeTriangle");
        }
    }

    //private void OnCollisionStay2D(Collision2D hit)  //觸發機器耐久恢復
    private void OnItemReleased(DraggableReturn2D hit)  //觸發機器耐久恢復
    {
        // if (hit.gameObject == FixMachineDurability)
        if (touchingFixMachine && !MachineDurabilityFix)
        {
            AudioManager.Instance.PlaySfx(21);             //音效
            if (isRun && !isIdel && (takeCircle == false || takeSquare == false || takeTriangle == false))
            {
                MachineAni.speed = 1;                         //動
                MachineDurabilityFix = false; //停止修
                isFixMachineShow = false;
                FixMachineShow.SetActive(false);
                // FindObjectOfType<FixMachineDurabilityChangeImage>().ChangeOrigin();
                if (repairCoroutine != null)
                {
                    StopCoroutine(repairCoroutine);
                    repairCoroutine = null;
                }
            }
            else if (!isRun && isIdel == true && (takeCircle == true || takeSquare == true || takeTriangle == true))
            {
                MachineAni.speed = 0;                         //不動
                MachineDurabilityFix = true;
                isFixMachineShow = true;
                FixMachineShow.SetActive(true); //機器維修會顯示在機器上的圖
                //FindObjectOfType<FixMachineDurabilityChangeImage>().ChangePicture();  //換回去
                repairCoroutine = StartCoroutine(FixDurabilityOverTime());
                isIdel = false;
                takeCircle = false;
                takeSquare = false;
                takeTriangle = false;
            }
        }
    }

    private IEnumerator FixDurabilityOverTime()   // 每秒恢復0.5%耐久
    {
        while (MachineDurabilityFix)
        {
            float repairAmount = MachineDurability * 0.005f;
            //float repairAmount = MachineDurability * 0.1f;
            MachineDurability_Script += repairAmount;

            //if (MachineDurability_Script > MachineDurability)
            //  MachineDurability_Script = MachineDurability;
            if (MachineDurability_Script > MachineDurability)
            {
                MachineDurability_Script = MachineDurability;
                if (MachineDurability_Script >= MachineDurability)  //回滿關起來
                {
                    MachineDurabilityEmpty = false;
                    MachineDurabilityFix = false; //停止修
                    isFixMachineShow = false;
                    FixMachineShow.SetActive(false);
                    MachineAni.speed = 1;                         //動
                    //FindObjectOfType<FixMachineDurabilityChangeImage>().ChangeOrigin();  //換回去
                }

            }
            //  同步更新 SaveMachineDurability
            SaveMachineDurability = MachineDurability_Script;

            // 更新顯示條
            DeductDurability();

            yield return new WaitForSeconds(1f);
        }
    }
    // private void OnCollisionExit2D(Collision2D hit)  //停止恢復
    // {
    // if (hit.gameObject == FixMachineDurability)
    // {
    //    MachineDurabilityFix = false;
    //  if (repairCoroutine != null)
    // {
    //    StopCoroutine(repairCoroutine);
    //    repairCoroutine = null;
    //}
    // }
    // }
    public void SpawnObject(int prefabIndex) // 場景上含生成點最多可以有4個物件，若達生成上限，直接停止   //之後要補生成點有物件不可生成(等場景儲存位置有了後)
    {
        if (allSpawnedObjects.Count >= maxObjects)
        {
            Debug.Log(" 已達生成上限！");
            return;
        }
        if (prefabIndex == 0)  //圓形
        {
            CurrentObject = Instantiate(ObjectPrefabs[prefabIndex], ObjectPop.position, ObjectPop.rotation) as GameObject;
            allSpawnedObjects.Add(CurrentObject); Debug.Log($" 生成物件：{ObjectPrefabs[prefabIndex].name} (目前總數：{allSpawnedObjects.Count})");
            DestroyPrefabButton.Add(CurrentObject);
            AudioManager.Instance.PlaySfx(6);             //音效
            canSpawn = false;

            if (Application.loadedLevelName == "TeachGame")   //f確保不提前跑出
            {
                if (!FindObjectOfType<TeachGM>().TeachThree5.activeSelf && FindObjectOfType<TeachGM>().teacheachThree5 == true && FindObjectOfType<TeachGM>().beforeTeach4RFix == false)
                {
                    FindObjectOfType<TeachGM>().OpenBeforeTeach4RFix();
                    FindObjectOfType<TeachGM>().beforeTeach4RFix = true;

                    var teach = FindObjectOfType<TeachGM>();  //禁止拖曳
                    if (teach != null)
                    {
                        teach.lockRegisteredObject();
                    }
                    else
                    {
                        // fallback（保險）
                        CurrentObject.GetComponent<BoxCollider2D>().enabled = false;
                    }  //不可拖曳
                }
                else
                {
                    ShowbeforeTeach4RFix = true;
                }
            }
        }

        if (prefabIndex == 1)//方形
        {
            CurrentObject = Instantiate(ObjectPrefabs[prefabIndex], ObjectPop.position, ObjectPop.rotation) as GameObject;
            allSpawnedObjects.Add(CurrentObject); Debug.Log($" 生成物件：{ObjectPrefabs[prefabIndex].name} (目前總數：{allSpawnedObjects.Count})");
            DestroyPrefabButton.Add(CurrentObject);
            AudioManager.Instance.PlaySfx(6);             //音效
            canSpawn = false;
        }

        if (prefabIndex == 2) //三角
        {
            CurrentObject = Instantiate(ObjectPrefabs[prefabIndex], ObjectPop.position, ObjectPop.rotation) as GameObject;
            allSpawnedObjects.Add(CurrentObject); Debug.Log($" 生成物件：{ObjectPrefabs[prefabIndex].name} (目前總數：{allSpawnedObjects.Count})");
            DestroyPrefabButton.Add(CurrentObject);
            AudioManager.Instance.PlaySfx(6);             //音效
            canSpawn = false;
        }

    }

    public static void RemoveSpawnedObject(GameObject obj)  //刪除生成物件 恢復上限、場景數量
    {
        if (allSpawnedObjects.Contains(obj))
        {
            allSpawnedObjects.Remove(obj);
            Debug.Log($" 移除物件：{obj.name} (目前剩餘：{allSpawnedObjects.Count})");
        }

    }

    //判斷要不要產生怪物
    void ProduceMonster()
    {
        isRun = true;
        //if (Application.loadedLevelName == "TeachGame")
        // {
        //  MonsterPrefab = Instantiate(Monster, ProducePos.position, Monster.transform.rotation) as GameObject;
        // }
        //else
        if (Application.loadedLevelName != "TeachGame")
        {
            //Random.Range(0, 2) 回傳整數 0 或 1,等於 0 回傳 true，否則為 false。
            isProduceMonster = Random.Range(0, 2) == 0;                //暫時停用MonsterGM有問題                      
            if (isProduceMonster && !MonsterPrefab)
            {
                MonsterPrefab = Instantiate(Monster, ProducePos.position, Monster.transform.rotation) as GameObject;
                MonsterPrefab.GetComponent<MonsterGM>().InitTarget("rabbit");
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
        SaveMachineDurability = MachineDurability_Script - DeductMachineDurability;
        MachineDurability_Script = SaveMachineDurability;
        DeductDurability();
        GameObject.FindWithTag("Monster").GetComponent<MonsterGM>().MonsterAni.SetTrigger("Win");


    }
    public void Reset()
    {
        isStopwatch = true;
        ScriptStopwatchTimer = StopwatchTimer;
        //for (int i = 0; i < Potions.Length; i++)
        //  {
        //      Potions[i].SetActive(false);
        //}
    }

    void DeductDurability()
    {
        MachineDurabilityBar.fillAmount = SaveMachineDurability / MachineDurability;

        if (SaveMachineDurability / MachineDurability > 0.5f)
        {
            MachineDurabilityBar.sprite = MachineDurabilityBarSprite[0];
            MachineDurabilityBarOustside.sprite = MachineDurabilityBarSpriteOustside[0];  //耐久值外框原色
            MachineDurabilityEmpty = false;
        }
        if (SaveMachineDurability / MachineDurability < 0.5f)
        {
            MachineDurabilityBar.sprite = MachineDurabilityBarSprite[1];
            MachineDurabilityBarOustside.sprite = MachineDurabilityBarSpriteOustside[1];  //耐久值外框變色
            MachineDurabilityEmpty = false;
        }
        if (SaveMachineDurability / MachineDurability <= 0f)      //耐久值歸零不能使用                                              //無法回血 鎖住按鈕                              
        {
            Debug.Log("耐久值歸零");
            MachineDurabilityBar.sprite = MachineDurabilityBarSprite[1];
            MachineDurabilityBarOustside.sprite = MachineDurabilityBarSpriteOustside[1];  //耐久值外框變色
            MachineDurabilityEmpty = true;
            Debug.Log("耐久值歸零2");
            LockRabbitButtons();
        }
        else if (SaveMachineDurability / MachineDurability > 0f)      //耐久值不為零                          
        {
            Debug.Log("耐久值不為零");
            MachineDurabilityEmpty = false;
            UnlockRabbitButtons();
        }
        else
        {
            MachineDurabilityBar.sprite = MachineDurabilityBarSprite[1];
            MachineDurabilityBarOustside.sprite = MachineDurabilityBarSpriteOustside[1];  //耐久值外框變色
        }
        // if (MachineDurabilityBar.fillAmount == 0)
    }


    public void Takecircle()
    {
        MachineAni.SetTrigger("takecircle");
    }

    public void Takesquare()
    {
        MachineAni.SetTrigger("takeSquare");
    }
    public void Taketriangle()
    {
        MachineAni.SetTrigger("takeTriangle");
    }


    //場景物件數達上限鎖住與未達解鎖
    private void LockRabbitButtons()
    {
        for (int i = 0; i < RabbitButton.Length; i++)
        {
            RabbitButton[i].interactable = false;
        }
    }

    private void UnlockRabbitButtons()
    {
        for (int i = 0; i < RabbitButton.Length; i++)
        {
            RabbitButton[i].interactable = true;
        }
    }
}

