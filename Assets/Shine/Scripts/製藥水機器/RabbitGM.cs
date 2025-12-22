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
    bool MachineDurabilityFix = false;  //不可修
    public GameObject FixMachineShow; //機器維修會顯示在機器上的圖
    private bool isFixMachineShow = false;
    private Coroutine repairCoroutine; // 協程參考，避免重複啟動
                                       // public System.Action OnWaitFinished;
    private bool waitEventSent = false;
    public Button[] RabbitButton;   //兔子按鈕
    void Start()
    {
        ScriptStopwatchTimer = StopwatchTimer;
        MachineDurability_Script = MachineDurability;

    }

    // Update is called once per frame
    void Update() //無效
    {
        if (Application.loadedLevelName == "TeachGame")
        {

            // if (FindObjectOfType<TeachGM>().RabbitButton.interactable == true)  //新手關如果沒鎖的話

            if (FindObjectOfType<TeachGM>().TeachGMLockRabbitButton == true)  //新手關如果沒鎖的話
            {
                if (allSpawnedObjects.Count >= maxObjects)
                {
                    LockRabbitButtons();
                }
                else
                {
                    UnlockRabbitButtons();
                }
            }

        }
        else if (Application.loadedLevelName != "TeachGame")  //其他
        {
            if (allSpawnedObjects.Count >= maxObjects)
            {
                LockRabbitButtons();
            }
            else
            {
                UnlockRabbitButtons();
            }
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
            if (stateInfo.IsName("work circle") || stateInfo.IsName("work square") || stateInfo.IsName("work triangle"))
            {
                {  //倒數計時
                    float normalizedTime = Mathf.Min(stateInfo.normalizedTime, 1f);
                    Stopwatch.gameObject.SetActive(true);


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
                        Stopwatch.gameObject.SetActive(false);
                        isRun = false;
                        MachineDurabilityFix = true; // 動畫結束後允許修復
                        isFixMachineShow = true;
                        FixMachineShow.SetActive(true); //機器維修會顯示在機器上的圖
                        FindObjectOfType<FixMachineDurabilityChangeImage>().ChangePicture();  //換回去
                    }
                    MachineDurabilityFix = false;   //不可維修
                    isFixMachineShow = false;
                    FindObjectOfType<FixMachineDurabilityChangeImage>().ChangeOrigin();  //換回去
                    FixMachineShow.SetActive(false);
                }  //倒數計時


                if (stateInfo.normalizedTime >= 0.98f)
                {

                    MachineDurability_Script = SaveMachineDurability;

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
            }

            AnimatorStateInfo stateInfo3 = MachineAni.GetCurrentAnimatorStateInfo(0);  //生成方
            if (stateInfo3.IsName("work square") && stateInfo3.normalizedTime > 0.98f)
            {
                Debug.Log("開始work square");
                SpawnObject(1);
                canSpawn = false;
                // Stopwatch.gameObject.SetActive(false);
                //SpawnOK = true;
            }

            AnimatorStateInfo stateInfo4 = MachineAni.GetCurrentAnimatorStateInfo(0);  //生成角

            if (stateInfo4.IsName("work triangle") && stateInfo4.normalizedTime > 0.98f)
            {
                Debug.Log("開始work triangle");
                SpawnObject(2);
                canSpawn = false;
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

        Reset();
        //如果觸發按鈕的話
        canSpawn = true;
        currentWork = WorkType.Circle;
        MachineAni.SetTrigger("IdleToWalkCircle");
        ProduceMonster();
    }
    public void RabbitSquaare()   //如果按到哪個按鈕觸法哪個按鈕得生成
    {
        Reset();
        currentWork = WorkType.Square;
        canSpawn = true;
        MachineAni.SetTrigger("IdleToSquare");
        ProduceMonster();
    }
    public void RabbittTriangle()   //如果按到哪個按鈕觸法哪個按鈕得生成
    {
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

    private void OnCollisionStay2D(Collision2D hit)  //觸發機器耐久恢復
    {
        if (hit.gameObject == FixMachineDurability)
        {
            if (isRun)
            {

                MachineDurabilityFix = false; //停止修
                isFixMachineShow = false;
                FixMachineShow.SetActive(false);
                FindObjectOfType<FixMachineDurabilityChangeImage>().ChangeOrigin();
                if (repairCoroutine != null)
                {
                    StopCoroutine(repairCoroutine);
                    repairCoroutine = null;
                }
            }
            else if (!MachineDurabilityFix)
            {
                MachineDurabilityFix = true;
                isFixMachineShow = true;
                FixMachineShow.SetActive(true); //機器維修會顯示在機器上的圖
                FindObjectOfType<FixMachineDurabilityChangeImage>().ChangePicture();  //換回去
                repairCoroutine = StartCoroutine(FixDurabilityOverTime());
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
                    MachineDurabilityFix = false; //停止修
                    isFixMachineShow = false;
                    FixMachineShow.SetActive(false);
                    FindObjectOfType<FixMachineDurabilityChangeImage>().ChangeOrigin();  //換回去
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
            canSpawn = false;
        }

        if (prefabIndex == 2) //三角
        {
            CurrentObject = Instantiate(ObjectPrefabs[prefabIndex], ObjectPop.position, ObjectPop.rotation) as GameObject;
            allSpawnedObjects.Add(CurrentObject); Debug.Log($" 生成物件：{ObjectPrefabs[prefabIndex].name} (目前總數：{allSpawnedObjects.Count})");
            DestroyPrefabButton.Add(CurrentObject);
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
        if (Application.loadedLevelName == "TeachGame")
        {
            MonsterPrefab = Instantiate(Monster, ProducePos.position, Monster.transform.rotation) as GameObject;
        }
        else
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
        }
        else
        {
            MachineDurabilityBar.sprite = MachineDurabilityBarSprite[1];
            MachineDurabilityBarOustside.sprite = MachineDurabilityBarSpriteOustside[1];  //耐久值外框變色
        }
        if (MachineDurabilityBar.fillAmount == 0)
        {

        }
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

