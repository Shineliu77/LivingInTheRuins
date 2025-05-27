using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewPlayerTeach : MonoBehaviour
{
    public NewPlayerPlayUse newPlayerPlayUse;  // ���ת���X�{���   �{��
    public CustomerPlace customerPlace; // �ʴ����ȤH�޲z��   �{��
    public GameObject CustomerCome; //�ȤH�J����  (�ȤH�J�� ���֤�)  (�Цp��ϥ�opener)

    public GameObject openerfinish; //����opener��  (�Щ��u�@�i)
    public GameObject collBecameLarge; //�I��BecameLarge��  (�ЧP�_����) (�Цp��󴫹s��)
    public GameObject AfterChangeImage; //�׹s��LiveTwoDChangeImage����� (�����ٵ��ȤH)
    public GameObject GiveCustomer; //�����ٵ��ȤH�� (�����[������)

    //public GameObject CustomerComeRoundTwo;  //�ȤH�J����(�n���a���ƤW�@���y�{)
    public GameObject collBecameLargeRoundTwo;  //�I��BecameLarge�ĤG����  (�ЧP�_����) (�Цp��󴫸պ�)
    public GameObject blenderfinish; //����blender��
    public GameObject monsterAttackMachine; //monsterhitanime��


    public GameObject GiveCustomereRoundTwo;  //�����ٵ��ȤH��  (�����s��о�)
    private bool hasPausedForSeat = false; // �קK���ƼȰ�


    //  public Dragging dragging;
    void Start()
    {
        openerfinish.SetActive(false);
        CustomerCome.SetActive(false);  //�������
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
        //�ˬd�H�O�_��y�� �� �Ȱ��C�� ��ܭ��O
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
                    Time.timeScale = 0f; // �Ȱ��C��
                    CustomerCome.SetActive(true);  //�}�����
                    hasPausedForSeat = true;
                    StartCoroutine(WaitForClickThenResume()); //�I���ƹ� �~��C���B�������O
                    break; // �����ˬd��L�y��
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
            openerfinish.SetActive(false);  //�������

        }
        else//����Ĥ@���о�
        {
            //��opener�ʵe����
            openerfinish.SetActive(true);  //�������
            Time.timeScale = 0;

            StartCoroutine(WaitForClickThenResume()); //�I���ƹ� �~��C���B�������O
                                                     
        }


    }


    public void IscollBecameLarge()
    {
        // �P�_fixeditemOpen�O�_��3�]�q NewPlayerPlayUse Ū���^
        GameObject[] fixedItems = GameObject.FindGameObjectsWithTag("fixeditemOpen");
        int fixCount = fixedItems.Length;
        if (fixCount == 3)
        {
            collBecameLargeRoundTwo.SetActive(true);  //�}�����
            Time.timeScale = 0;
            StartCoroutine(WaitForClickThenResume());
        }

        else //����Ĥ@���о�
        {
            collBecameLarge.SetActive(true);  //�}�����
            Time.timeScale = 0;
            StartCoroutine(WaitForClickThenResume());
        }


    }
    public void IsAfterChangeImage()
    {
        // �P�_fixeditemOpen�O�_��3�]�q NewPlayerPlayUse Ū���^
        GameObject[] fixedItems = GameObject.FindGameObjectsWithTag("fixeditemOpen");
        int fixCount = fixedItems.Length;

        if (fixCount == 3)
        {
            AfterChangeImage.SetActive(false);  //�������

        }
        else
        {
            AfterChangeImage.SetActive(true);  //�������
            Time.timeScale = 0;
            StartCoroutine(WaitForClickThenResume());
        }
    }

    public void Isblenderfinish()
    {

        blenderfinish.SetActive(true);  //�������
        Time.timeScale = 0;
        StartCoroutine(WaitForClickThenResume());
    }

    public void IsmonsterAttackMachine()
    {

        monsterAttackMachine.SetActive(true);  //�������
        Time.timeScale = 0;
        StartCoroutine(WaitForClickThenResume());
    }

    public void IsGiveCustomer()
    {
        /// �P�_fixeditemOpen�O�_��3�]�q NewPlayerPlayUse Ū���^
        GameObject[] fixedItems = GameObject.FindGameObjectsWithTag("fixeditemOpen");
        int fixCount = fixedItems.Length;
        if (fixCount == 3)
        {

            GiveCustomereRoundTwo.SetActive(true);  //�������
            Time.timeScale = 0;
            KlarraAnime klarraAnimeScript = FindObjectOfType<KlarraAnime>();
            if (klarraAnimeScript != null)
            {
                klarraAnimeScript.HangUpPhone();
            }
            StartCoroutine(WaitForClickThenResume());
        }
        else   //����Ĥ@���о�
        {
            GiveCustomer.SetActive(true);  //�������
            Time.timeScale = 0;
            StartCoroutine(WaitForClickThenResume());
        }

    }




    IEnumerator WaitForClickThenResume()  //�I���ƹ� �~��C���B�������O
    {
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        openerfinish.SetActive(false);
        CustomerCome.SetActive(false);  //�������
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

