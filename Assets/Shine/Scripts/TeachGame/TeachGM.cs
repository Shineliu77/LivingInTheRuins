using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TeachGM : MonoBehaviour
{
    [HideInInspector] public GameObject lockedObject; // 被 Rabbit 鎖住的 object (可能是 CurrentObject 或 IteamOpenPrefab)
    #region 產生顧客
    [Header("顧客")]
    public GameObject Customer;
    GameObject CustomerPrefab;
    [Header("顧客生成位置")]
    public Transform CustomerProduce;
    public int CustomerNumber;
    #endregion
    #region 產生組件
    [Header("組件")]
    public GameObject Iteam;
    GameObject IteamPrefab;
    [Header("組件生成位置")]
    public Transform IteamProduce;
    #endregion
    #region 第一段說明
    public GameObject Teach1;
    #endregion
    #region 產生打開的組件
    [Header("打開的組件")]
    public GameObject IteamOpen;
    GameObject IteamOpenPrefab;
    bool isNewIteam = false;
    [Header("組件生成位置")]
    public Transform IteamOpenProduce;
    #endregion
    #region 第二段說明
    public GameObject Teach2; //第一段
    public GameObject TeachTwo;
    public GameObject TeachTwo2;
    public GameObject TeachTwo3;
    private bool teach2 = false;//第二段
    private bool teachTwo = false;
    public bool teachTwo2 = false;
    public bool teachTwo3 = false;
    #endregion
    #region 第三段說明
    public GameObject Teach3;//第一段
    public GameObject TeachThree;
    public GameObject TeachThree2;
    private bool teach3 = false;
    private bool teachThree = false;
    private bool Allteach3Close = false;             //確定關閉所有
    private bool teacheachThree2 = false;
    #endregion
    #region 第四段說明
    public GameObject BeforeTeach4R;                 //兔子使用
    private bool beforeTeach4R = false;
    public GameObject TeachThree4;                 //操作
    public bool teachThree4;
    public bool teachThree4ForbeforeTeach4RWaitRabbit = false;
    public GameObject BeforeTeach4RWaitRabbit;       //等兔子使用
    private bool beforeTeach4RWaitRabbit = false;
    public GameObject TeachThree5;                 //操作
    public bool teacheachThree5 = false;
    public GameObject BeforeTeach4RFix;       //指導玩家修元件
    public bool beforeTeach4RFix = false;
    public GameObject Teach4; //第一段
    public GameObject TeachFour;
    bool isTeach4;         //第一段
    bool isTeachFour = false;
    private bool teach4 = false;
    //public bool teach4Customer = false;  //還客人
    public Button RabbitButton;   //兔子按鈕
    public bool TeachGMLockRabbitButton = false;  // 教學用鎖定
    #endregion
    #region 第五段說明
    public GameObject Teach5;
    bool isTeach5;
    private bool teach5BeingWatched = false;
    #endregion
    #region 第六段說明
    public GameObject Teach6;
    public GameObject BeforeTeach6;
    bool isTeach6;
    bool beforeTeach6 = false;
    #endregion
    #region 第七段說明
    public Collider2D[] MakeAPotionIteams;
    public bool TeachGMLockMakeAPotionIteams = false;  // 教學用鎖定
    public GameObject Teach7;
    public bool isTeach7;

    #endregion
    #region 第8段說明
    public GameObject Teach8;  //第一段
    public GameObject TeachEight;
    public GameObject TeachEightTwo;
    public GameObject TeachEightThree;
    private bool teach8 = false;//第二段
    private bool teachEight = false;
    private bool teachEightThree;
    #endregion
    #region 第9段說明
    public GameObject Teach9;
    #endregion
    #region 第10段說明
    public GameObject Teach10;
    #endregion
    public GameObject FixMachineDurability;  //機器耐久維修物
    public GameObject ScorePanel;  //分數面板
    public GameObject Door; //鐵門
    public GameObject ClickClose; //點擊換場
    public Transform MiddletargetPosition; //中間停頓點
    public Transform FinaltargetPosition; //最終停頓點
    public float MiddledoorSpeed; //中間速度
    public float FinaldoorSpeed;//最後速度
                                // Start is called before the first frame update
    void Start()
    {

        RabbitButton.interactable = false;
        FindObjectOfType<RabbitGM>().RabbitButton[0].interactable = false;
        ProductCustomer();
        // teachThree4 = false;
        TeachThree4.SetActive(false);
        FixMachineDurability.GetComponent<DraggableReturn2D>().enabled = false;
    }

    void Update()
    {
        if (IteamPrefab && !Teach1.active & !IteamPrefab.GetComponent<BoxCollider2D>().enabled)
        {
            IteamPrefab.GetComponent<BoxCollider2D>().enabled = true;
        }
        if (IteamPrefab && CustomerNumber == 2 && !BeforeTeach6.active && beforeTeach6 == true)
        {
            IteamPrefab.GetComponent<DraggableReturn2D>().enabled = true;
        }

        if (teach2 == true && !Teach2.active && teachTwo == false)  //打開第二個教學面板的第二段
        {
            TeachTwo.SetActive(true);
            teachTwo = true;
        }

        if (teach2 == true && !Teach2.active && !TeachTwo.active && teachTwo2 == false)  //打開第二個教學面板的第二段
        {
            TeachTwo2.SetActive(true);
            teachTwo2 = true;
        }

        if (!TeachTwo.active && teachTwo2 == true && !TeachTwo2.active)  //確定開啟才可以拖曳 修理
        {
            FixMachineDurability.GetComponent<DraggableReturn2D>().enabled = true;

        }

        if (teachTwo3 == true && !TeachTwo3.active)  //確定開啟才可以拖曳  修理元計還可拖曳     不可拖曳也要開否則可拿起
                                                     // if (!TeachTwo.active && teachTwo3 == true && !TeachTwo3.active)  //確定開啟才可以拖曳  修理元計不可拖曳
        {
            FixMachineDurability.GetComponent<DraggableReturn2D>().enabled = false;
            IteamOpenPrefab.GetComponent<BoxCollider2D>().enabled = true;

        }
        //  if (!TeachTwo.active && teachTwo3 == true && !TeachTwo3.active)  //確定開啟才可以拖曳  修理元計不可拖曳
        // {
        //    IteamOpenPrefab.GetComponent<BoxCollider2D>().enabled = true;

        //
        //  if (IteamOpenPrefab && !Teach2.active && !TeachTwo.active & !IteamOpenPrefab.GetComponent<BoxCollider2D>().enabled)
        //  {
        // IteamOpenPrefab.GetComponent<BoxCollider2D>().enabled = true;
        // }

        if (teach3 == true && !Teach3.active && teachThree == false)
        {
            TeachThree.SetActive(true);
            teachThree = true;
        }
        if (teach3 == true && !TeachThree2.active && teacheachThree2 == true && beforeTeach4R == false)
        {
            Allteach3Close = true;
            OpenBeforeTeach4R();
            BeforeTeach4R.SetActive(true);
            beforeTeach4R = true;
        }

        if (!TeachThree4.activeSelf && teachThree4ForbeforeTeach4RWaitRabbit == true && beforeTeach4RWaitRabbit == false)
        {
            BeforeTeach4RWaitRabbit.SetActive(true);
            beforeTeach4RWaitRabbit = true;
        }

        //if (beforeTeach4R== true && !BeforeTeach4R.active && teachThree4 == false)
        // {
        //     TeachThree4.SetActive(true);
        //  teachThree4 = true;
        //  }

        if (!BeforeTeach4RWaitRabbit.activeSelf && beforeTeach4RWaitRabbit == true && teacheachThree5 == false)
        {
            TeachThree5.SetActive(true);
            teacheachThree5 = true;
        }

        //  if (!TeachThree4.activeSelf && teachThree4 == true && !TeachThree5.activeSelf && teacheachThree5 == true && teachThree4ForbeforeTeach4RWaitRabbit == true && !BeforeTeach4RWaitRabbit.activeSelf && beforeTeach4RWaitRabbit == true && !TeachThree5.activeSelf && teacheachThree5 == true && !BeforeTeach4RFix.activeSelf && beforeTeach4RFix == true)//元件拖曳
        // if (!TeachThree4.activeSelf && teachThree4 && !TeachThree5.activeSelf && teacheachThree5 == true && teachThree4ForbeforeTeach4RWaitRabbit == true && !BeforeTeach4RWaitRabbit.activeSelf && beforeTeach4RWaitRabbit == true && !TeachThree5.activeSelf && teacheachThree5 == true && !BeforeTeach4RFix.activeSelf && beforeTeach4RFix == true)//元件拖曳
        //if (!BeforeTeach4RFix.activeSelf && beforeTeach4RFix == true)
        // if (!TeachThree4.activeSelf && teachThree4 && !TeachThree5.activeSelf && teacheachThree5 == true && teachThree4ForbeforeTeach4RWaitRabbit == true && !BeforeTeach4RWaitRabbit.activeSelf && beforeTeach4RWaitRabbit == true && !TeachThree5.activeSelf && teacheachThree5 == true && !BeforeTeach4RFix.activeSelf && beforeTeach4RFix == true)//元件拖曳
        //if (!BeforeTeach4RFix.activeSelf && beforeTeach4RFix == true)
        if (teachThree4)
        {
            // UnlockRegisteredObject();
            //GameObject.FindWithTag("brokecircle").GetComponent<DraggableReturn2D>().enabled = false;
            lockRegisteredObject();
        }
        if (TeachThree5.activeSelf)
        {
            // UnlockRegisteredObject();
            //GameObject.FindWithTag("brokecircle").GetComponent<DraggableReturn2D>().enabled = false;
            lockRegisteredObject();
        }
        if (BeforeTeach4RWaitRabbit.activeSelf)
        {
            // UnlockRegisteredObject();
            // GameObject.FindWithTag("brokecircle").GetComponent<DraggableReturn2D>().enabled = false;
            lockRegisteredObject();
        }
        if (!BeforeTeach4RFix.activeSelf && beforeTeach4RFix == true)
        {
            //  FindObjectOfType<RabbitGM>().CurrentObject.GetComponent<BoxCollider2D>().enabled = true;
            UnlockRegisteredObject();
            //GameObject.FindWithTag("brokecircle").GetComponent<DraggableReturn2D>().enabled = true;
        }
        if (!TeachThree4.activeSelf && teachThree4ForbeforeTeach4RWaitRabbit == true && !BeforeTeach4RWaitRabbit.activeSelf && beforeTeach4RWaitRabbit == true && !TeachThree5.activeSelf && teacheachThree5 == true && beforeTeach4RFix == false && FindObjectOfType<RabbitGM>().ShowbeforeTeach4RFix == true)  //確保所有人關起才可以
                                                                                                                                                                                                                                                                                                                 //  if (!TeachThree4.activeSelf && teachThree4ForbeforeTeach4RWaitRabbit == true && !BeforeTeach4RWaitRabbit.activeSelf && beforeTeach4RWaitRabbit == true && !TeachThree5.activeSelf && teacheachThree5 == true && FindObjectOfType<RabbitGM>().ShowbeforeTeach4RFix == true)  //確保所有人關起才可以
        {
            BeforeTeach4RFix.SetActive(true);
            beforeTeach4RFix = true;
            FindObjectOfType<RabbitGM>().ShowbeforeTeach4RFix = false;
        }

        if (!Teach4.active && isTeach4 && isTeachFour == false && CustomerNumber == 1)
        {
            if (GameObject.FindWithTag("fixeditemOpen") && !GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled)
            {
                // GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled = true;
                IteamOpenPrefab.name = "fixeditemOpenFinished1";
                TeachFour.SetActive(true);
                isTeachFour = true;
            }
        }

        if (!Teach4.active && isTeach4 && !TeachFour.active && isTeachFour == true && CustomerNumber == 1)   //還客人
        {
            FixMachineDurability.GetComponent<DraggableReturn2D>().enabled = true;
            if (GameObject.FindWithTag("fixeditemOpen") && !GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled)
            {
                GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled = true;
                // IteamOpenPrefab.name = "fixeditemOpenFinished1";
                // TeachFour.SetActive(true);

            }
        }
        // if (!Teach5.active && isTeach5 && CustomerNumber == 1)     //不明原因無效
        // {
        //    Time.timeScale = 1;
        //   CustomerPrefab.GetComponent<CustomerGM>().Finished = true;
        //   IteamPrefab = null;
        //}

        if (!Teach6.active && isTeach6 && CustomerNumber == 2)
        {
            MakeAPotionIteams[0].enabled = false;
            MakeAPotionIteams[1].enabled = false;
            MakeAPotionIteams[2].enabled = true;
            MakeAPotionIteams[3].enabled = false;
            TeachGMLockMakeAPotionIteams = true;
            FixMachineDurability.GetComponent<DraggableReturn2D>().enabled = true;
        }

        if (!Teach7.active && isTeach7 && CustomerNumber == 2)
        {
            Time.timeScale = 1;
        }


        if (teach8 == true && !Teach8.activeSelf && !teachEight)  //打開第八個教學面板的第二段
        {
            TeachEight.SetActive(true);
            teachEight = true;
        }

    }
    public void ProductCustomer()
    {
        KlarraAnime klarraAnimeScript = FindObjectOfType<KlarraAnime>();   //當客人入場對話框出現  接電話
        if (klarraAnimeScript != null)
        {
            klarraAnimeScript.PickPhone();
        }
        CustomerPrefab = Instantiate(Customer, CustomerProduce.position, Customer.transform.rotation) as GameObject;
        CustomerNumber++;
    }

    public void ProduceIteam()
    {
        if (IteamPrefab == null)
        {
            IteamPrefab = Instantiate(Iteam, IteamProduce.position, Iteam.transform.rotation) as GameObject;
            if (CustomerNumber == 1)
            {
                Teach1.SetActive(true);
            }

            if (CustomerNumber == 2 && !BeforeTeach6.activeSelf && beforeTeach6 == false)  //修是試管前 換玩家試
            {

                BeforeTeach6.SetActive(true);
                IteamPrefab.GetComponent<DraggableReturn2D>().enabled = false;
                beforeTeach6 = true;
            }
        }
    }
    public void ProduceIteamOpen()
    {
        IteamOpenPrefab = Instantiate(IteamOpen, IteamOpenProduce.position, IteamOpenProduce.transform.rotation) as GameObject;
        if (CustomerNumber == 1)
        {
            Teach2.SetActive(true);
            //IteamOpenPrefab.GetComponent<SetIteamOpenObj>().ID = 0;
            //IteamOpenPrefab.GetComponent<SetIteamOpenObj>().ProcessorID = 0;
            var obj = IteamOpenPrefab.GetComponent<SetIteamOpenObj>();
            obj.IDs.Add(0);
            obj.ProcessorID = 0;
            teach2 = true;
            Debug.Log("教學2代");
        }
        if (CustomerNumber == 2)  //試管電路板
        {
            //IteamOpenPrefab.GetComponent<SetIteamOpenObj>().ID = 1;
            //IteamOpenPrefab.GetComponent<SetIteamOpenObj>().ID = 2;

            // IteamOpenPrefab.GetComponent<SetIteamOpenObj>().ReagentsID = 2;
            // IteamOpenPrefab.GetComponent<SetIteamOpenObj>().ID = 1;

            var obj = IteamOpenPrefab.GetComponent<SetIteamOpenObj>();
            GameObject.FindWithTag("brokePCB").GetComponent<DraggableReturn2D>().enabled = false;
            obj.IDs.Add(1);
            obj.IDs.Add(2);
            obj.ReagentsID = 2;
        }
    }


    public void OpenTeach3() //IteamOpenOnTable
    {
        Teach3.SetActive(true);
        teach3 = true;
    }
    //public void OpenTeacheachThree()
    // {

    //TeachThree.SetActive(true);
    // teachThree = true;
    //}

    public void OpenTeacheachThree2()
    {
        TeachThree.SetActive(false);
        TeachThree2.SetActive(true);
        teacheachThree2 = true;
    }
    public void OpenTeachTwo3() //Fixbar
    {
        TeachTwo3.SetActive(true);
        teachTwo3 = true;
    }
    public void OpenBeforeTeach4R()
    {
        // if (Allteach3Close == true)
        // {
        RabbitButton.interactable = true;     //兔子按鈕
        TeachGMLockRabbitButton = true;

        //  }

    }
    public void OpenBTeachThree4()
    {
        if (!TeachThree4.active)
        {
            TeachThree4.SetActive(true);
            teachThree4 = true;

            //Time.timeScale = 0;
        }
        teachThree4ForbeforeTeach4RWaitRabbit = true;
    }
    public void OpenBeforeTeach4RWaitRabbit()
    {

        BeforeTeach4RWaitRabbit.SetActive(true);
        beforeTeach4RWaitRabbit = true;
        //Time.timeScale = 0;
    }




    public void OpenBeforeTeach4RFix()
    {
        if (!BeforeTeach4RWaitRabbit.active)
        {
            BeforeTeach4RFix.SetActive(true);
            beforeTeach4RFix = true;
        }

    }
    public void CloseBeforeTeach4RFix()
    {
        //      BeforeTeach4RFix.SetActive(false);
    }
    public void OpenTeach4()
    {

        if (!isTeach4)
        {
            Teach4.SetActive(true);
            isTeach4 = true;
        }
    }
    public void CloseTeachFour()
    {
        TeachFour.SetActive(false);
        teach4 = false;
    }

    // public void OpenTeach5()
    // {
    //  if (!isTeach5)
    // {
    //      Teach5.SetActive(true);
    //     Time.timeScale = 0;
    //     isTeach5 = true;
    //  }

    //}
    public void OpenTeach5()
    {
        Teach5.SetActive(true);
        Time.timeScale = 0;
        isTeach5 = true;


        StartCoroutine(WaitTeach5Close());
    }

    private IEnumerator WaitTeach5Close()
    {
        // 等待玩家關閉 Teach5
        yield return new WaitUntil(() => !Teach5.activeSelf);


        Time.timeScale = 1; // 確保遊戲時間恢復
        if (CustomerPrefab != null)
            CustomerPrefab.GetComponent<CustomerGM>().Finished = true;
        IteamPrefab = null;
        isTeach5 = false;

        // 重置旗標，避免 Update 再觸發
        teach5BeingWatched = false;
    }


    public void OpenTeach6()
    {

        Teach6.SetActive(true);

        isTeach6 = true;
    }
    public void OpenTeach7()
    {
        Teach7.SetActive(true);
        Time.timeScale = 0;
        isTeach7 = true;
    }
    public void OpenTeach8()
    {
        Teach8.SetActive(true);
        teach8 = true;

    }

    public void CloseTeachEight()
    {
        TeachEight.SetActive(false);
        teach8 = false;
        teachEight = false;
    }

    public void OpenTeachEightTwo()  //處理電路板
    {
        TeachEightTwo.SetActive(true);
        GameObject.FindWithTag("brokePCB").GetComponent<DraggableReturn2D>().enabled = true;
    }


    public void OpenTeachEightThree()
    {
        TeachEightThree.SetActive(true);
        teachEightThree = true;
    }

    public void CloseTeachEightThree()  //處理電路板
    {
        if (teachEightThree == true)
        {
            GameObject.FindWithTag("PCB").GetComponent<DraggableReturn2D>().enabled = true;
        }
    }
    public void OpenTeach9()
    {
        Teach9.SetActive(true);

    }

    public void OpenTeach10()
    {
        Teach10.SetActive(true);
        Time.timeScale = 0;
        KlarraAnime klarraAnimeScript = FindObjectOfType<KlarraAnime>();   //當教學結束  掛電話 
        if (klarraAnimeScript != null)
        {
            klarraAnimeScript.HangUpPhone();
        }
        StartCoroutine(Teach10Closed());
    }
    private IEnumerator Teach10Closed()  //關掉Teach9
    {
        yield return new WaitUntil(() => !Teach10.activeSelf);
        Time.timeScale = 1;
        yield return StartCoroutine(CloseDoor());
    }

    private IEnumerator CloseDoor()
    {
        Door.SetActive(true);

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
        if (Door.transform.position == FinaltargetPosition.position)
        {
            GoOtherScene();
        }
    }
    private void GoOtherScene()  //換場景與解鎖關卡
    {
        if (PlayerPrefs.GetInt("TutorialUnlocked", 0) == 0) // 0 尚未解鎖過
        {
            int currentUnlocked = PlayerPrefs.GetInt("UnLockLevelIndex", -1);//當前關卡
                                                                             //不重複解鎖關卡
            PlayerPrefs.SetInt("UnLockLevelIndex", currentUnlocked + 1);
            PlayerPrefs.SetInt("TutorialUnlocked", 1);
            PlayerPrefs.Save();
        }

        PlayerPrefs.Save();  //儲存
        SceneManager.LoadScene("lobby");
    }

    public void ClearIteamPrefab()
    {
        IteamPrefab = null;
    }

    public void RegisterLockedObject(GameObject obj)
    {

        lockedObject = obj;
        var col = lockedObject.GetComponent<Collider2D>();
        if (col) col.enabled = false; // 鎖住碰撞（避免拖曳）
                                      // 如果你是用 DraggableReturn2D 控制拖曳，也可以關掉那個 component：
        var drag = lockedObject.GetComponent<DraggableReturn2D>();
        if (drag) drag.enabled = false;
    }

    public void lockRegisteredObject()
    {
        RabbitGM rabbit = FindObjectOfType<RabbitGM>();
        if (rabbit == null || rabbit.CurrentObject == null)
        {
            Debug.LogWarning("[TeachGM] 沒找到 RabbitGM 或 CurrentObject");
            return;
        }

        GameObject obj = rabbit.CurrentObject;

        // 鎖住（禁止拖曳 + 禁用碰撞）
        var drag = obj.GetComponent<DraggableReturn2D>();
        if (drag != null) drag.enabled = false;

        Debug.Log("[TeachGM] 已鎖住：" + obj.name);
    }
    public void UnlockRegisteredObject()
    {
        RabbitGM rabbit = FindObjectOfType<RabbitGM>();
        if (rabbit == null || rabbit.CurrentObject == null) return;

        GameObject obj = rabbit.CurrentObject;

        var drag = obj.GetComponent<DraggableReturn2D>();
        if (drag != null) drag.enabled = true;

        Debug.Log("[TeachGM] 已解鎖：" + obj.name);
    }
}