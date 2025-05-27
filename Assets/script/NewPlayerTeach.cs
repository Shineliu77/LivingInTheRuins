using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewPlayerTeach : MonoBehaviour
{
    public NewPlayerPlayUse newPlayerPlayUse;  // 維修物件出現顯示   程式
    public CustomerPlace customerPlace; // 監測的客人管理區   程式
    public GameObject CustomerCome; //客人入場用  (客人入場 拿核心)  (教如何使用opener)

    public GameObject openerfinish; //播完opener用  (教放到工作檯)
    public GameObject collBecameLarge; //碰撞BecameLarge用  (教判斷怎麼修) (教如何更換零件)
    public GameObject AfterChangeImage; //修零件LiveTwoDChangeImage之後用 (解釋還給客人)
    public GameObject GiveCustomer; //物件還給客人用 (解釋加分機制)

    //public GameObject CustomerComeRoundTwo;  //客人入場用(要玩家重複上一次流程)
    public GameObject collBecameLargeRoundTwo;  //碰撞BecameLarge第二輪用  (教判斷怎麼修) (教如何更換試管)
    public GameObject blenderfinish; //播完blender用
    public GameObject monsterAttackMachine; //monsterhitanime用


    public GameObject GiveCustomereRoundTwo;  //物件還給客人用  (結束新手教學)
    private bool hasPausedForSeat = false; // 避免重複暫停


    //  public Dragging dragging;
    void Start()
    {
        openerfinish.SetActive(false);
        CustomerCome.SetActive(false);  //關閉顯示
        collBecameLarge.SetActive(false);
        AfterChangeImage.SetActive(false);
        GiveCustomer.SetActive(false);
        monsterAttackMachine.SetActive(false);
        blenderfinish.SetActive(false);
        //CustomerComeRoundTwo.SetActive(false);
        collBecameLargeRoundTwo.SetActive(false);
        GiveCustomereRoundTwo.SetActive(false);

    }
    void Update()
    {
        //檢查人是否到座位 到 暫停遊戲 顯示面板
        if (!hasPausedForSeat)
        {
            Vector3[] seatPositions = customerPlace.GetSeatPositions();

            for (int i = 0; i < seatPositions.Length; i++)
            {
                bool seatOccupied = false;

                foreach (Customer customer in customerPlace.customerList)
                {
                    if (Vector3.Distance(customer.targetPos, seatPositions[i]) < 1f)
                    {
                        seatOccupied = true;
                        break;
                    }
                }
                if (seatOccupied)
                {
                    KlarraAnime klarraAnimeScript = FindObjectOfType<KlarraAnime>();
                    if (klarraAnimeScript != null)
                    {
                        klarraAnimeScript.animator.SetBool("idelTOmove", true);
                        klarraAnimeScript.PickPhone();
                    }
                    Time.timeScale = 0f; // 暫停遊戲
                    CustomerCome.SetActive(true);  //開啟顯示
                    hasPausedForSeat = true;
                    StartCoroutine(WaitForClickThenResume()); //點擊滑鼠 繼續遊戲、關閉面板
                    break; // 不需檢查其他座位
                }
            }
        }
    }

    public void OnOpenerFinished()
    {
        GameObject[] fixedItems = GameObject.FindGameObjectsWithTag("fixeditemOpen");
        int fixCount = fixedItems.Length;
        if (fixCount == 3)
        {
            gameObject.GetComponent<ForopenerOutside>();
            openerfinish.SetActive(false);  //關閉顯示

        }
        else//執行第一輪教學
        {
            //當opener動畫結束
            openerfinish.SetActive(true);  //關閉顯示
            Time.timeScale = 0;

            StartCoroutine(WaitForClickThenResume()); //點擊滑鼠 繼續遊戲、關閉面板
                                                     
        }


    }


    public void IscollBecameLarge()
    {
        // 判斷fixeditemOpen是否為3（從 NewPlayerPlayUse 讀取）
        GameObject[] fixedItems = GameObject.FindGameObjectsWithTag("fixeditemOpen");
        int fixCount = fixedItems.Length;
        if (fixCount == 3)
        {
            collBecameLargeRoundTwo.SetActive(true);  //開啟顯示
            Time.timeScale = 0;
            StartCoroutine(WaitForClickThenResume());
        }

        else //執行第一輪教學
        {
            collBecameLarge.SetActive(true);  //開啟顯示
            Time.timeScale = 0;
            StartCoroutine(WaitForClickThenResume());
        }


    }
    public void IsAfterChangeImage()
    {
        // 判斷fixeditemOpen是否為3（從 NewPlayerPlayUse 讀取）
        GameObject[] fixedItems = GameObject.FindGameObjectsWithTag("fixeditemOpen");
        int fixCount = fixedItems.Length;

        if (fixCount == 3)
        {
            AfterChangeImage.SetActive(false);  //關閉顯示

        }
        else
        {
            AfterChangeImage.SetActive(true);  //關閉顯示
            Time.timeScale = 0;
            StartCoroutine(WaitForClickThenResume());
        }
    }

    public void Isblenderfinish()
    {

        blenderfinish.SetActive(true);  //關閉顯示
        Time.timeScale = 0;
        StartCoroutine(WaitForClickThenResume());
    }

    public void IsmonsterAttackMachine()
    {

        monsterAttackMachine.SetActive(true);  //關閉顯示
        Time.timeScale = 0;
        StartCoroutine(WaitForClickThenResume());
    }

    public void IsGiveCustomer()
    {
        /// 判斷fixeditemOpen是否為3（從 NewPlayerPlayUse 讀取）
        GameObject[] fixedItems = GameObject.FindGameObjectsWithTag("fixeditemOpen");
        int fixCount = fixedItems.Length;
        if (fixCount == 3)
        {

            GiveCustomereRoundTwo.SetActive(true);  //關閉顯示
            Time.timeScale = 0;
            KlarraAnime klarraAnimeScript = FindObjectOfType<KlarraAnime>();
            if (klarraAnimeScript != null)
            {
                klarraAnimeScript.HangUpPhone();
            }
            StartCoroutine(WaitForClickThenResume());
        }
        else   //執行第一輪教學
        {
            GiveCustomer.SetActive(true);  //關閉顯示
            Time.timeScale = 0;
            StartCoroutine(WaitForClickThenResume());
        }

    }




    IEnumerator WaitForClickThenResume()  //點擊滑鼠 繼續遊戲、關閉面板
    {
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        openerfinish.SetActive(false);
        CustomerCome.SetActive(false);  //關閉顯示
        collBecameLarge.SetActive(false);
        AfterChangeImage.SetActive(false);
        GiveCustomer.SetActive(false);
        blenderfinish.SetActive(false);
        monsterAttackMachine.SetActive(false);
        //CustomerComeRoundTwo.SetActive(false);
        collBecameLargeRoundTwo.SetActive(false);
        GiveCustomereRoundTwo.SetActive(false);
        Time.timeScale = 1;
    }
}

