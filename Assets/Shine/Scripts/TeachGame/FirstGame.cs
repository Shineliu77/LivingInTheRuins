using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstGame : MonoBehaviour
{
    #region 關卡時間與人數上限
    [Header("關卡時間（秒）")]
    [Tooltip("可在 Inspector 自行設定一關的秒數")]
    public float levelDurationSeconds;
    public float RemainingLevelTime => Mathf.Max(0f, levelEndTime - Time.time);
    public Image MImage00, MImage0, SIMage00, SIMage0;
    public Sprite[] TimeSprite;
    [Header("同時在場顧客上限")]
    [Range(1, 10)]
    public int maxConcurrentCustomers = 3;  // 需求：固定 3（可保留彈性）
    #endregion

    #region 顧客生成
    [Header("顧客 Prefab")]
    public GameObject[] Customer;

    [Header("顧客站位（請放 3 個 Transform）")]
    public Transform[] customerSpawnPoints; // 三個站位（請在 Inspector 指定）
    public Transform StartPos;
    private readonly List<GameObject> activeCustomers = new List<GameObject>(); // 場上顧客
    private bool[] seatOccupied;           // 對應站位是否被占用
    private float levelEndTime;            // 本關結束時間點（Time.time + levelDurationSeconds）
    public int CustomerNumber = 0;         // 你原本在用的編號（總進場數）
    #endregion

    #region 產生組件（沿用你的欄位）
    [Header("組件")]
    public GameObject Iteam;
    public List<GameObject> IteamPrefab;
    private bool touchingopener = false;

    [Header("打開的組件")]
    public GameObject IteamOpen;
    private GameObject IteamOpenPrefab;

    [Header("打開組件生成位置")]
    public Transform IteamOpenProduce;
    // 製藥元件(試管)
    public Collider2D[] MakeAPotionIteams;
    #endregion
    private GameObject IteamGO;
    private GameObject customerGO;
    public Vector3[] originalPosition;
    public GameObject ScorePanel;  //分數面板
    public GameObject Door; //鐵門
    public GameObject ClickClose; //點擊換場
    public Transform MiddletargetPosition; //中間停頓點
    public Transform FinaltargetPosition; //最終停頓點
    public float MiddledoorSpeed; //中間速度
    public float FinaldoorSpeed;//最後速度
    public bool iscloseDoor = false;//關鐵門
    void Start()
    {
        MakeAPotionIteams[0].enabled = true;
        MakeAPotionIteams[1].enabled = true;
        MakeAPotionIteams[2].enabled = true;
        MakeAPotionIteams[3].enabled = true;

        // 初始化時間與座位占用狀態
        levelEndTime = Time.time + levelDurationSeconds;

        if (levelDurationSeconds > 0)     //結束遊戲顯示分數
        {
            Invoke("LevelTimesUP", levelDurationSeconds);
        }

        if (customerSpawnPoints == null || customerSpawnPoints.Length < 3)
        {
            Debug.LogWarning("[FirstGame] 請在 Inspector 指定 3 個顧客站位（customerSpawnPoints）");
        }
        seatOccupied = new bool[customerSpawnPoints != null ? customerSpawnPoints.Length : 0];

        // 開場先把可用位置補滿到上限
        TryFillCustomerSlots();
        // AudioManager.Instance.PlaySfx(9);             //音效
    }
    private void LevelTimesUP() //關卡時間到
    {
        if (!iscloseDoor)
        {
            StartCoroutine(DOORClosed());
            iscloseDoor = true;
            Debug.Log("關門2");
        }
    }
    void Update()
    {
        float remain = RemainingLevelTime;
        // 文字：MM:SS
        if (MImage0 != null)
        {
            int m00 = Mathf.FloorToInt(remain / 60) / 10;
            int m0 = Mathf.FloorToInt(remain / 60) % 10;
            int s00 = Mathf.FloorToInt(remain % 60) / 10;
            int s0 = Mathf.FloorToInt(remain % 60) % 10;
            MImage00.sprite = TimeSprite[m00];
            MImage0.sprite = TimeSprite[m0];
            SIMage00.sprite = TimeSprite[s00];
            SIMage0.sprite = TimeSprite[s0];

        }

        // 安全開關（避免 Null 例外）
        // for (int i = 0; i < customerSpawnPoints.Length; i++)
        // {
        // if (IteamPrefab[i] != null)
        // {
        // var col = IteamPrefab[i].GetComponent<BoxCollider2D>();
        // if (col) col.enabled = true;

        // touchingopener = true;
        //}
        //}
        //  if (GameObject.FindWithTag("fixeditemOpen"))
        //  {
        // var col = IteamOpenPrefab.GetComponent<BoxCollider2D>();
        //if (col) col.enabled = true;
        //  }
        GameObject[] allOpenItems = GameObject.FindGameObjectsWithTag("fixeditemOpen");
        foreach (GameObject item in allOpenItems)
        {
            var col = item.GetComponent<BoxCollider2D>();
            if (col != null && !col.enabled)
            {
                col.enabled = true;
                // Debug.Log(item.name + " 的碰撞器已動態啟用");
            }
        }
        GameObject[] allItems = GameObject.FindGameObjectsWithTag("fixeditem");
        foreach (GameObject order in allItems)
        {
            var col = order.GetComponent<BoxCollider2D>();
            if (col != null && !col.enabled)
            {
                col.enabled = true;
            }
        }
        //Debug.Log("關門1");
        // 時間到：停止再補客；等場上顧客清空後結束/換場
        if (Time.time >= levelEndTime && iscloseDoor == false)         //好像會被NotifyCustomerFinished(GameObject customerGO)引響        
        {
            Debug.Log("關門1");
            // 不再補新顧客，等待當前顧客處理完離場
            //  if (activeCustomers.Count == 0)
            // {
            // 這裡你可依規劃：直接結束關卡、計分、或換場
            //GoLevel2();       // 你的原本流程（若有條件達成才換）
            // 或者直接：GoOtherScene();
            StartCoroutine(DOORClosed());
            iscloseDoor = true;
            Debug.Log("關門2");
            // }
            //  return;
        }

        // 若尚未到時間、且現場顧客數低於上限，嘗試補位
        if (activeCustomers.Count < Mathf.Min(maxConcurrentCustomers, (customerSpawnPoints?.Length ?? 0)))
        {
            TryFillCustomerSlots();

        }

        //if (CustomerNumber == 1)
        //  {
        //  if (GameObject.FindWithTag("fixeditemOpen") && !GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled)              //IteamOpenOnTable也有debug
        //  {
        //    GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled = false;
        //   if (IteamOpenPrefab) IteamOpenPrefab.name = "fixeditemOpenFinished1";
        //  Debug.Log($"{IteamOpenPrefab.name} ");
        //  }
        //}
        // if (CustomerNumber == 2)
        //  {
        //   if (GameObject.FindWithTag("fixeditemOpen") && !GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled)
        //   {
        //  GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled = false;
        //  if (IteamOpenPrefab) IteamOpenPrefab.name = "fixeditemOpenFinished2";
        //  Debug.Log($"{IteamOpenPrefab.name} ");
        //  }
        // }
        // if (CustomerNumber == 3)
        // {
        //  if (GameObject.FindWithTag("fixeditemOpen") && !GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled)
        //  {
        //  GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled = false;
        //  if (IteamOpenPrefab) IteamOpenPrefab.name = "fixeditemOpenFinished3";
        //   Debug.Log($"{IteamOpenPrefab.name} ");
        //  }
        // }
        // if (CustomerNumber == 4)
        // {
        // if (GameObject.FindWithTag("fixeditemOpen") && !GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled)
        //  {
        //   GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled = false;
        //   if (IteamOpenPrefab) IteamOpenPrefab.name = "fixeditemOpenFinished4";
        //   Debug.Log($"{IteamOpenPrefab.name} ");
        // }
        //}

        //GameObject[] openItems = GameObject.FindGameObjectsWithTag("fixeditemOpen");

        //  foreach (GameObject item in openItems)
        //  {
        //  var drag = item.GetComponent<DraggableReturn2D>();
        //物件是否被玩家放下
        //  if (drag != null && !drag.enabled)
        //  {
        //      // 抓取 ID
        //  var setScript = item.GetComponent<SetIteamOpenObj>();
        //  if (setScript != null)
        // {
        //根據座位 ID 改名 (ID 是 0,1,2，所以名稱會是 Finished1, 2, 3)
        //    item.name = "fixeditemOpenFinished" + (setScript.ID + 1);

        // 重要：改完名後把 Tag 換掉，防止這一幀重複跑進來處理同一個物件
        //  item.tag = "Untagged";

        //  Debug.Log($"物件已就位，根據座位 ID {setScript.ID} 改名為: {item.name}");
        // }
        // item.transform.parent = null;
        // }
        // }
    }
    public void ConfirmItemForCustomer(GameObject item)
    {
        var setScript = item.GetComponent<SetIteamOpenObj>();

        // 【關鍵】檢查是否已經修好
        if (setScript != null && setScript.Rename == true)
        {
            item.transform.SetParent(null);
            item.name = "fixeditemOpenFinished" + (setScript.ID + 1);
            item.tag = "Untagged";
            Debug.Log("零件已修好，准許交件。");
        }
        else
        {
            Debug.Log("零件還是壞的，客人拒絕收件！");
            // 不做改名動作，這樣 CustomerGM 的名字比對就會失敗
        }
    }
    /// <summary>
    /// 依序把空位補到 maxConcurrentCustomers（僅在時間內）
    /// </summary>
    private void TryFillCustomerSlots()
    {
        if (customerSpawnPoints == null || customerSpawnPoints.Length == 0) return;
        if (Time.time >= levelEndTime) return;

        int target = Mathf.Min(maxConcurrentCustomers, customerSpawnPoints.Length);
        for (int i = 0; i < customerSpawnPoints.Length; i++)
        {
            if (activeCustomers.Count >= target) break;
            if (!seatOccupied[i])
            {
                SpawnCustomerAt(i);
            }
        }
    }

    /// <summary>
    /// 在指定站位生成顧客，並綁定回報事件
    /// </summary>
    // void OnEnable()    //檢查滑鼠放開事件
    //{
    //    DraggableReturn2D.OnReleased += OnItemReleased;
    //}

    //void OnDisable()   //取消滑鼠放開事件
    //{
    //  DraggableReturn2D.OnReleased -= OnItemReleased;
    // }


    private void SpawnCustomerAt(int seatIndex)
    {

        if (Customer == null || customerSpawnPoints == null || seatIndex < 0 || seatIndex >= customerSpawnPoints.Length) return;
        int RandomCustomerIndex = Random.Range(0, Customer.Length);
        GameObject selectedCustomerPrefab = Customer[RandomCustomerIndex];
        Transform spawn = customerSpawnPoints[seatIndex];
        GameObject customerGO = Instantiate(selectedCustomerPrefab, StartPos.position, StartPos.rotation);
        customerGO.transform.localPosition = StartPos.position;
        // customerGO.GetComponentInChildren<CustomerGM>().PosName = "顧客定位點" + (CustomerNumber + 1);
        customerGO.GetComponentInChildren<CustomerGM>().PosName = "顧客定位點" + (seatIndex + 1);                //因為有客人需要校正座標變子物件   //改用座位遞補離開的座位
        customerGO.GetComponentInChildren<CustomerGM>().ID = CustomerNumber;
        //GameObject IteamGO = Instantiate(Iteam, customerSpawnPoints[CustomerNumber].position, Iteam.transform.rotation) as GameObject;
        //IteamGO.transform.parent = customerGO.transform;
        //IteamGO.transform.localPosition = Vector3.zero;


        AudioManager.Instance.PlaySfx(9);             //音效
        //IteamPrefab.Add(IteamGO);
        seatOccupied[seatIndex] = true;
        activeCustomers.Add(customerGO);
        CustomerNumber++;

        // 可選：讓顧客面向指定方向
        // customerGO.transform.rotation = spawn.rotation;

        // 讓顧客知道自己的座位（若顧客需要移動/歸位可用）
        var marker = customerGO.GetComponentInChildren<FirstGameCustomerMarker>();
        if (marker == null) marker = customerGO.AddComponent<FirstGameCustomerMarker>();
        marker.SeatIndex = seatIndex;
        marker.Owner = this;
        Debug.Log($"座位 {seatIndex} 已補人，目前控制編號設定為: {CustomerNumber}");

    }
    public void SpawnIteamGO(Transform parentCustomer, int seatIndex)
    {
        if (Iteam == null) return;

        // 使用傳進來的 seatIndex 確保位置正確
        GameObject IteamGO = Instantiate(Iteam, customerSpawnPoints[seatIndex].position, Iteam.transform.rotation);

        var set = IteamGO.GetComponent<SetIteamOpenObj>();
        if (set == null) set = IteamGO.AddComponent<SetIteamOpenObj>();
        set.ID = seatIndex; // 綁定座位

        // 將父物件設為傳進來的那個顧客
        IteamGO.transform.SetParent(parentCustomer);

        // 歸零座標，讓它完美對準顧客
        IteamGO.transform.localPosition = Vector3.zero;

        IteamPrefab.Add(IteamGO);
    }
    //void OnItemReleased(DraggableReturn2D item)
    // {
    // if (touchingopener)
    //  {
    //     IteamGO.transform.parent = customerGO.transform;
    //    IteamGO.transform.localPosition = Vector3.zero;
    // }
    //}
    /// <summary>
    /// 由顧客在「事件處理完成、要離場」時呼叫此方法
    /// 你可以在顧客腳本處理完畢後呼叫：FindObjectOfType<FirstGame>().NotifyCustomerFinished(gameObject);
    /// 或讓顧客上的 FirstGameCustomerMarker 在 OnDestroy 時自動通報
    /// </summary>
    public void NotifyCustomerFinished(GameObject customerGO)
    {
        if (customerGO == null) return;
        //AudioManager.Instance.PlaySfx(9);             //音效
        GameObject rootCustomer = customerGO.transform.root.gameObject;
        // 釋放座位
        var marker = customerGO.GetComponentInChildren<FirstGameCustomerMarker>();
        // if (marker != null && marker.SeatIndex >= 0 && marker.SeatIndex < seatOccupied.Length)
        if (marker != null && marker.SeatIndex >= 0)
        {
            if (activeCustomers.Contains(customerGO))
            {
                activeCustomers.Remove(customerGO);

                seatOccupied[marker.SeatIndex] = false;
                // CustomerNumber = marker.SeatIndex;  //填補移除的客人編號
            }
        }
        //    activeCustomers.Remove(customerGO);
        //   Destroy(customerGO);

        // 顧客離場（若外部已 Destroy，這裡就不再重複）
        //if (customerGO != null) Destroy(customerGO);

        // 如果尚未到時間，離場後立刻補位；若已到時間，不再補新客
        if (Time.time < levelEndTime)
        {
            TryFillCustomerSlots();
        }
        // else if (Time.time >=levelEndTime&& iscloseDoor == false)
        // if (!iscloseDoor)
        // {
        // 若時間已到且清空，走收尾流程
        //if (activeCustomers.Count == 0)

        //  {
        // GoLevel2();  // 或 GoOtherScene();
        //}
    }
    public void ProduceIteam(int ID)
    {

        // IteamPrefab.Add(Instantiate(Iteam, customerSpawnPoints[ID].position, Iteam.transform.rotation) as GameObject);
        GameObject newIteam = Instantiate(Iteam, customerSpawnPoints[ID].position, Iteam.transform.rotation);

        // 關鍵：給予身分證，這樣 CountdownFillEmptyUse 才認得它
        var set = newIteam.GetComponent<SetIteamOpenObj>();
        if (set == null) set = newIteam.AddComponent<SetIteamOpenObj>();
        set.ID = ID;

        IteamPrefab.Add(newIteam);

    }

    public void ClearIteamPrefab()
    {
        IteamPrefab = null;

    }

    public void ProduceIteamOpen(int seatID)
    {
        if (IteamOpen != null && IteamOpenProduce != null)
        {
            IteamOpenPrefab = Instantiate(IteamOpen, IteamOpenProduce.position, IteamOpenProduce.rotation);
            var set = IteamOpenPrefab.GetComponent<SetIteamOpenObj>();
            if (set != null)
            {
                set.ID = seatID;
                set.ProcessorID = 0;
            }
            GameObject.FindGameObjectWithTag("brokePCB").GetComponent<DraggableReturn2D>().enabled = false;
        }
    }

    public void CountdownFillEmptyUse(int seatID)
    {
        // 1. 刪除所有 Tag 為 "fixeditemOpen" 且 ID 匹配的物件 (機器吐出的零件)
        GameObject[] allOpenItems = GameObject.FindGameObjectsWithTag("fixeditemOpen");
        foreach (GameObject obj in allOpenItems)
        {
            var set = obj.GetComponent<SetIteamOpenObj>();
            if (set != null && set.ID == seatID)
            {
                Destroy(obj);
            }
        }

        // 2. 刪除所有已經修好 (變更過名稱) 但還沒交件的物件
        // 這裡我們掃描所有包含 "fixeditemOpenFinished" 名稱的物件
        GameObject[] allFinishedItems = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allFinishedItems)
        {
            if (obj.name.Contains("fixeditemOpenFinished") && obj.name.EndsWith((seatID + 1).ToString()))
            {
                Destroy(obj);
            }
        }

        // 3. 清理掛在客人身上的原始組件 (IteamPrefab)
        //  for (int i = IteamPrefab.Count - 1; i >= 0; i--)
        // {
        //    if (IteamPrefab[i] != null)
        //  {
        //  var marker = IteamPrefab[i].GetComponentInParent<FirstGameCustomerMarker>();
        //  if (marker != null && marker.SeatIndex == seatID)
        // {
        //    Destroy(IteamPrefab[i]);
        //    IteamPrefab.RemoveAt(i);
        //}
        //  }
        //}
        SetIteamOpenObj[] allRemainingItems = GameObject.FindObjectsOfType<SetIteamOpenObj>();
        foreach (SetIteamOpenObj itemScript in allRemainingItems)
        {
            if (itemScript.ID == seatID)
            {
                GameObject objToDestroy = itemScript.gameObject;

                // 如果它是某人的子物件，解除父子關係再刪除，確保清理乾淨
                if (objToDestroy.transform.parent != null)
                {
                    objToDestroy.transform.SetParent(null);
                }

                // 同步清理 List，避免殘留 Missing Reference
                if (IteamPrefab != null && IteamPrefab.Contains(objToDestroy))
                {
                    IteamPrefab.Remove(objToDestroy);
                }

                Destroy(objToDestroy);
                Debug.Log($"已成功刪除座位 {seatID} 客人身上的子物件零件。");
            }
        }
        Debug.Log($"座位 {seatID} 的客人耐心歸零，相關物件已全數清理。");
    }
    public void OpenTeach10()
    {
        Time.timeScale = 0;
        var klarraAnimeScript = FindObjectOfType<KlarraAnime>();
        if (klarraAnimeScript != null)
        {
            klarraAnimeScript.HangUpPhone();
        }
    }

    // private void GoLevel2()
    // {
    //  ScoreGM scoreManager = FindObjectOfType<ScoreGM>();
    //  if (scoreManager != null && scoreManager.TotalScore == 50)
    // {
    //   GoOtherScene();
    //}
    // }
    private IEnumerator DOORClosed()  //關掉Teach9
    {
        // yield return new WaitUntil(() => !Teach10.activeSelf);
        Time.timeScale = 1;
        yield return StartCoroutine(CloseDoor());
    }

    private IEnumerator CloseDoor()
    {
        Door.SetActive(true);
        AudioManager.Instance.PlaySfx(4);                                              //音效
        // 門從目前位置往中間
        while (Vector3.Distance(Door.transform.position, MiddletargetPosition.position) > 0.01f)
        {
            Door.transform.position = Vector3.MoveTowards(Door.transform.position, MiddletargetPosition.position, MiddledoorSpeed * Time.deltaTime);
            yield return null;
        }
        Door.transform.position = MiddletargetPosition.position;

        yield return new WaitForSeconds(0.5f);  //停一下再移動

        ScorePanel.SetActive(true);//開分數
        while (Vector3.Distance(Door.transform.position, FinaltargetPosition.position) > 0.01f)
        {
            AudioManager.Instance.PlaySfx(3);                                              //音效
            Door.transform.position = Vector3.MoveTowards(Door.transform.position, FinaltargetPosition.position, FinaldoorSpeed * Time.deltaTime
        );
            yield return null;
        }

        // 保證最後位置精準
        Door.transform.position = FinaltargetPosition.position;

        ClickClose.SetActive(true);   //點擊使用

    }
    public void ClickGoOtherScence()//點擊才到下個場地
    {
        AudioManager.Instance.PlaySfx(5);                                              //音效
        if (Door.transform.position == FinaltargetPosition.position)
        {
            GoOtherScene();
        }
    }

    private void GoOtherScene()
    {
        // if (SceneManager.GetActiveScene().name == "FirstGame" && PlayerPrefs.GetInt("TutorialUnlocked", 1) == 1)
        //{
        //  int currentUnlocked = PlayerPrefs.GetInt("UnLockLevelIndex", 1);
        // PlayerPrefs.SetInt("UnLockLevelIndex", currentUnlocked + 1); // 解鎖下一關
        // PlayerPrefs.SetInt("TutorialUnlocked", 2);
        PlayerPrefs.Save();
        SceneManager.LoadScene("lobby");
        //}
    }
}
