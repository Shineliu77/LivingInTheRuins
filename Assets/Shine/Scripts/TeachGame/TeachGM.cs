using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeachGM : MonoBehaviour
{
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
    [Header("組件生成位置")]
    public Transform IteamOpenProduce;
    #endregion
    #region 第二段說明
    public GameObject Teach2; //第一段
    public GameObject TeachTwo;
    private bool teach2 = false;//第一段
    private bool teachTwo = false;
    #endregion
    #region 第三段說明
    public GameObject Teach3;//第一段
    public GameObject TeachThree;
    public GameObject TeachThree2;

    #endregion
    #region 第四段說明
    public GameObject Teach4; //第一段
    public GameObject TeachFour;
    bool isTeach4;         //第一段
    private bool teach4 = false;
    #endregion
    #region 第五段說明
    public GameObject Teach5;
    bool isTeach5;

    #endregion
    #region 第六段說明
    public GameObject Teach6;
    #endregion
    #region 第七段說明
    public Collider2D[] MakeAPotionIteams;
    public GameObject Teach7;
    public bool isTeach7;

    #endregion
    #region 第8段說明
    public GameObject Teach8;  //第一段
    public GameObject TeachEight;
    private bool teach8 = false;//第一段
    private bool teachEight = false;
    #endregion
    #region 第9段說明
    public GameObject Teach9;
    #endregion
    #region 第10段說明
    public GameObject Teach10;
    #endregion

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
        ProductCustomer();
    }

    void Update()
    {

        if (IteamPrefab && !Teach1.active & !IteamPrefab.GetComponent<BoxCollider2D>().enabled)
        {
            IteamPrefab.GetComponent<BoxCollider2D>().enabled = true;
        }


        if (teach2 == true && !Teach2.active)  //打開第二個教學面板的第二段
        {
            TeachTwo.SetActive(true);

        }

        if (IteamOpenPrefab && !Teach2.active && !TeachTwo.active & !IteamOpenPrefab.GetComponent<BoxCollider2D>().enabled)
        {
            IteamOpenPrefab.GetComponent<BoxCollider2D>().enabled = true;
        }
        if (!Teach4.active && isTeach4 && CustomerNumber == 1)
        {
            if (GameObject.FindWithTag("fixeditemOpen") && !GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled)
            {
                GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled = true;
                IteamOpenPrefab.name = "fixeditemOpenFinished1";
                TeachFour.SetActive(true);

            }
        }
        if (!Teach5.active && isTeach5 && CustomerNumber == 1)
        {
            Time.timeScale = 1;
            CustomerPrefab.GetComponent<CustomerGM>().Finished = true;
            IteamPrefab = null;
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
        }
    }
    public void ProduceIteamOpen()
    {
        IteamOpenPrefab = Instantiate(IteamOpen, IteamOpenProduce.position, IteamOpenProduce.transform.rotation) as GameObject;
        if (CustomerNumber == 1)
        {
            Teach2.SetActive(true);
            IteamOpenPrefab.GetComponent<SetIteamOpenObj>().ID = 0;
            IteamOpenPrefab.GetComponent<SetIteamOpenObj>().ProcessorID = 0;
            teach2 = true;
            Debug.Log("教學2代");
        }
        if (CustomerNumber == 2)
        {
            IteamOpenPrefab.GetComponent<SetIteamOpenObj>().ID = 2;
            IteamOpenPrefab.GetComponent<SetIteamOpenObj>().ReagentsID = 2;
        }

    }
    public void CloseTeachTwo() //關掉第二個教學面板的第二段 掛回文字面板按鈕
    {
        TeachTwo.SetActive(false);
        teach2 = false;
    }
    public void OpenTeach3() //IteamOpenOnTable
    {
        Teach3.SetActive(true);
    }

    public void OpenTeacheachThree()
    {
        Teach3.SetActive(false);
        TeachThree.SetActive(true);
    }

    public void OpenTeacheachThree2()
    {
        TeachThree.SetActive(false);
        TeachThree2.SetActive(true); ;
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
    public void OpenTeach5()
    {
        if (!isTeach5)
        {

            Teach5.SetActive(true);
            Time.timeScale = 0;
            isTeach5 = true;
        }
    }
    public void OpenTeach6()
    {
        Teach6.SetActive(true);
        MakeAPotionIteams[0].enabled = false;
        MakeAPotionIteams[1].enabled = false;
        MakeAPotionIteams[2].enabled = true;
        MakeAPotionIteams[3].enabled = false;
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

}

