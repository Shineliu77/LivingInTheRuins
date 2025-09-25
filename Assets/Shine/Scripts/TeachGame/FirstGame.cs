using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FirstGame : MonoBehaviour
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

    #region 產生打開的組件
    [Header("打開的組件")]
    public GameObject IteamOpen;
    GameObject IteamOpenPrefab;
    [Header("組件生成位置")]
    public Transform IteamOpenProduce;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        ProductCustomer();
    }

    // Update is called once per frame
    void Update()
    {
       
        IteamPrefab.GetComponent<BoxCollider2D>().enabled = true;
        IteamOpenPrefab.GetComponent<BoxCollider2D>().enabled = true;
       
        if (CustomerNumber == 1)
        {
            if (GameObject.FindWithTag("fixeditemOpen") && !GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled)
            {
                GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled = false;
                IteamOpenPrefab.name = "fixeditemOpenFinished1";

            }
        }
        if (CustomerNumber == 2)
        {
            if (GameObject.FindWithTag("fixeditemOpen") && !GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled)
            {
                GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled = false;
                IteamOpenPrefab.name = "fixeditemOpenFinished2";

            }
        }
        if (CustomerNumber == 3)
        {
            if (GameObject.FindWithTag("fixeditemOpen") && !GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled)
            {
                GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled = false;
                IteamOpenPrefab.name = "fixeditemOpenFinished3";

            }
        }
        if (CustomerNumber == 4)
        {
            if (GameObject.FindWithTag("fixeditemOpen") && !GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled)
            {
                GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled = false;
                IteamOpenPrefab.name = "fixeditemOpenFinished4";

            }
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
        //非新手教學

        if (IteamPrefab == null)
        {

            IteamPrefab = Instantiate(Iteam, IteamProduce.position, Iteam.transform.rotation) as GameObject;

        }

    }

    public void ClearIteamPrefab()  //g刪除物件
    {
        IteamPrefab = null;
    }
    public void ProduceIteamOpen()
    {
        IteamOpenPrefab = Instantiate(IteamOpen, IteamOpenProduce.position, IteamOpenProduce.transform.rotation) as GameObject;

        IteamOpenPrefab.GetComponent<SetIteamOpenObj>().ID = 0;    //這邊有問題  似乎換不出試管   //如果開出兩次電路板會故障
        IteamOpenPrefab.GetComponent<SetIteamOpenObj>().ProcessorID = 0;

    }

    public void OpenTeach10()
    {

        Time.timeScale = 0;
        KlarraAnime klarraAnimeScript = FindObjectOfType<KlarraAnime>();   //當教學結束  掛電話 
        if (klarraAnimeScript != null)
        {
            klarraAnimeScript.HangUpPhone();
        }
        // StartCoroutine(Teach10Closed());
    }
    //  private IEnumerator Teach10Closed()  //關掉Teach9
    // {

    //    GoOtherScene();
    //}


    private void GoLevel2()  //到第二關
    {
        ScoreGM scoreManager = FindObjectOfType<ScoreGM>();

        if (scoreManager != null && scoreManager.TotalScore == 50)
        {
            GoOtherScene();
        }
    }

    private void GoOtherScene()  //換場景與解鎖關卡
    {
        if (SceneManager.GetActiveScene().name == "FirstGame" && PlayerPrefs.GetInt("TutorialUnlocked", 1) == 1)
        {
            int currentUnlocked = PlayerPrefs.GetInt("UnLockLevelIndex", 1);
            PlayerPrefs.SetInt("UnLockLevelIndex", currentUnlocked + 1); // 解鎖下一關
            PlayerPrefs.SetInt("TutorialUnlocked", 2);
            PlayerPrefs.Save();


            PlayerPrefs.Save();  //儲存
            SceneManager.LoadScene("lobby");
        }
    }
}
