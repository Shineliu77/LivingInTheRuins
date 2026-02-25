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
    public GameObject Customer;

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
    void Start()
    {
        MakeAPotionIteams[0].enabled = true;
        MakeAPotionIteams[1].enabled = true;
        MakeAPotionIteams[2].enabled = true;
        MakeAPotionIteams[3].enabled = true;

        // 初始化時間與座位占用狀態
        levelEndTime = Time.time + levelDurationSeconds;

        if (customerSpawnPoints == null || customerSpawnPoints.Length < 3)
        {
            Debug.LogWarning("[FirstGame] 請在 Inspector 指定 3 個顧客站位（customerSpawnPoints）");
        }
        seatOccupied = new bool[customerSpawnPoints != null ? customerSpawnPoints.Length : 0];

        // 開場先把可用位置補滿到上限
        TryFillCustomerSlots();
        AudioManager.Instance.PlaySfx(4);             //音效
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
        for (int i = 0; i < customerSpawnPoints.Length; i++)
        {
            if (IteamPrefab[i] != null)
            {
                var col = IteamPrefab[i].GetComponent<BoxCollider2D>();
                if (col) col.enabled = true;

                // touchingopener = true;
            }
        }
        if (IteamOpenPrefab != null)
        //if (IteamOpenPrefab != null || FindObjectOfType<IteamOpenOnTable>().hasitem == false)
        {
            var col = IteamOpenPrefab.GetComponent<BoxCollider2D>();

            if (col) col.enabled = true;

        }
        //if (GameObject.FindWithTag("fixeditemOpen"))
        // {
        //  var col = IteamOpenPrefab.GetComponent<BoxCollider2D>();
        // if (col) col.enabled = true;
        //}

        // 時間到：停止再補客；等場上顧客清空後結束/換場
        if (Time.time >= levelEndTime)
        {
            // 不再補新顧客，等待當前顧客處理完離場
            if (activeCustomers.Count == 0)
            {
                // 這裡你可依規劃：直接結束關卡、計分、或換場
                GoLevel2();       // 你的原本流程（若有條件達成才換）
                // 或者直接：GoOtherScene();
            }
            return;
        }

        // 若尚未到時間、且現場顧客數低於上限，嘗試補位
        if (activeCustomers.Count < Mathf.Min(maxConcurrentCustomers, (customerSpawnPoints?.Length ?? 0)))
        {
            TryFillCustomerSlots();
        }

        if (CustomerNumber == 1)
        {
            if (GameObject.FindWithTag("fixeditemOpen") && !GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled)
            {
                GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled = false;
                if (IteamOpenPrefab) IteamOpenPrefab.name = "fixeditemOpenFinished1";
            }
        }
        if (CustomerNumber == 2)
        {
            if (GameObject.FindWithTag("fixeditemOpen") && !GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled)
            {
                GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled = false;
                if (IteamOpenPrefab) IteamOpenPrefab.name = "fixeditemOpenFinished2";
            }
        }
        if (CustomerNumber == 3)
        {
            if (GameObject.FindWithTag("fixeditemOpen") && !GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled)
            {
                GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled = false;
                if (IteamOpenPrefab) IteamOpenPrefab.name = "fixeditemOpenFinished3";
            }
        }
        if (CustomerNumber == 4)
        {
            if (GameObject.FindWithTag("fixeditemOpen") && !GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled)
            {
                GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled = false;
                if (IteamOpenPrefab) IteamOpenPrefab.name = "fixeditemOpenFinished4";
            }
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

        Transform spawn = customerSpawnPoints[seatIndex];
        GameObject customerGO = Instantiate(Customer, StartPos.position, StartPos.rotation);
        customerGO.GetComponent<CustomerGM>().PosName = "顧客定位點" + (CustomerNumber + 1);
        customerGO.GetComponent<CustomerGM>().ID = CustomerNumber;
        //GameObject IteamGO = Instantiate(Iteam, customerSpawnPoints[CustomerNumber].position, Iteam.transform.rotation) as GameObject;
        //IteamGO.transform.parent = customerGO.transform;
        //IteamGO.transform.localPosition = Vector3.zero;



        //IteamPrefab.Add(IteamGO);
        seatOccupied[seatIndex] = true;
        activeCustomers.Add(customerGO);
        CustomerNumber++;

        // 可選：讓顧客面向指定方向
        // customerGO.transform.rotation = spawn.rotation;

        // 讓顧客知道自己的座位（若顧客需要移動/歸位可用）
        var marker = customerGO.GetComponent<FirstGameCustomerMarker>();
        if (marker == null) marker = customerGO.AddComponent<FirstGameCustomerMarker>();
        marker.SeatIndex = seatIndex;
        marker.Owner = this;

    }
    public void SpawnIteamGO(Transform parentCustomer, int seatIndex)
    {
        if (Iteam == null) return;

        // 使用傳進來的 seatIndex 確保位置正確
        GameObject IteamGO = Instantiate(Iteam, customerSpawnPoints[seatIndex].position, Iteam.transform.rotation);

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
        AudioManager.Instance.PlaySfx(4);             //音效

        // 釋放座位
        var marker = customerGO.GetComponent<FirstGameCustomerMarker>();
        if (marker != null && marker.SeatIndex >= 0 && marker.SeatIndex < seatOccupied.Length)
        {
            seatOccupied[marker.SeatIndex] = false;
        }

        activeCustomers.Remove(customerGO);

        // 顧客離場（若外部已 Destroy，這裡就不再重複）
        if (customerGO != null) Destroy(customerGO);

        // 如果尚未到時間，離場後立刻補位；若已到時間，不再補新客
        if (Time.time < levelEndTime)
        {
            TryFillCustomerSlots();
        }
        else
        {
            // 若時間已到且清空，走收尾流程
            if (activeCustomers.Count == 0)
            {
                GoLevel2();  // 或 GoOtherScene();
            }
        }
    }

    public void ProduceIteam(int ID)
    {

        IteamPrefab.Add(Instantiate(Iteam, customerSpawnPoints[ID].position, Iteam.transform.rotation) as GameObject);

    }

    public void ClearIteamPrefab()
    {
        IteamPrefab = null;
    }

    public void ProduceIteamOpen()
    {
        if (IteamOpen != null && IteamOpenProduce != null)
        {
            IteamOpenPrefab = Instantiate(IteamOpen, IteamOpenProduce.position, IteamOpenProduce.rotation);
            var set = IteamOpenPrefab.GetComponent<SetIteamOpenObj>();
            if (set != null)
            {
                set.ID = 0;
                set.ProcessorID = 0;
            }
            GameObject.FindGameObjectWithTag("brokePCB").GetComponent<DraggableReturn2D>().enabled = false;
        }
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

    private void GoLevel2()
    {
        ScoreGM scoreManager = FindObjectOfType<ScoreGM>();
        if (scoreManager != null && scoreManager.TotalScore == 50)
        {
            GoOtherScene();
        }
    }

    private void GoOtherScene()
    {
        if (SceneManager.GetActiveScene().name == "FirstGame" && PlayerPrefs.GetInt("TutorialUnlocked", 1) == 1)
        {
            int currentUnlocked = PlayerPrefs.GetInt("UnLockLevelIndex", 1);
            PlayerPrefs.SetInt("UnLockLevelIndex", currentUnlocked + 1); // 解鎖下一關
            PlayerPrefs.SetInt("TutorialUnlocked", 2);
            PlayerPrefs.Save();
            SceneManager.LoadScene("lobby");
        }
    }
}



