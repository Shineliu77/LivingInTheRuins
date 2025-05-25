using System.Collections;
using UnityEngine;

public class BlenderAnime : MonoBehaviour
{
    public Animator animator;
    private bool isTriggered = false;
    public BrokeProgressAnime brokeProgressAnime;

    [HideInInspector] public bool animationFinished = false;
    [HideInInspector] public string finishedColor = ""; // �šB���B��B��

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("idelTOmove", false);
        animator.SetBool("idelTOmoveY", false);
        animator.SetBool("idelTOmoveG", false);
        animator.SetBool("idelTOmoveR", false);
    }


    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (!isTriggered)
        {
            brokeProgressAnime = GetComponent<BrokeProgressAnime>();

            //�I���������Ĳ�o�ʵe
            if (coll.gameObject.CompareTag("Blue"))
            {
                Debug.Log("�I�� Blue�I");
                animator.SetBool("idelTOmove", true);
                isTriggered = true;
                LiquidPop("Blue");
                StartCoroutine(WaitForAnimationToEnd("blue work", "idelTOmove"));
            }
            else if (coll.gameObject.CompareTag("Red"))
            {
                Debug.Log("�I�� Red�I");
                animator.SetBool("idelTOmoveR", true);
                isTriggered = true;
                LiquidPop("Red");
                StartCoroutine(WaitForAnimationToEnd("red work", "idelTOmoveR"));
            }
            else if (coll.gameObject.CompareTag("Green"))
            {
                Debug.Log("�I�� Green�I");
                animator.SetBool("idelTOmoveG", true);
                isTriggered = true;
                LiquidPop("Green");
                StartCoroutine(WaitForAnimationToEnd("green work", "idelTOmoveG"));
            }
            else if (coll.gameObject.CompareTag("Yellow"))
            {
                Debug.Log("�I�� Yellow�I");
                animator.SetBool("idelTOmoveY", true);
                isTriggered = true;
                LiquidPop("Yellow");
                StartCoroutine(WaitForAnimationToEnd("yellow work", "idelTOmoveY"));

            }
        }
    }

    private void LiquidPop(string color)  //�ͦ��G��
    {
        //  Debug.Log($"�I�� {color}�I");
        isTriggered = true;
        finishedColor = color;
    }

    private IEnumerator WaitForAnimationToEnd(string animationName, string boolParameter)
    {
        yield return null; // ���@�V���ʵe���A����

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // ���ݰʵe���񧹦�
        while (stateInfo.IsName(animationName) && stateInfo.normalizedTime < 1f)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        yield return new WaitForSeconds(12f); // �����B�~�ɶ��T�O�ʵe����

        if (brokeProgressAnime != null) //��������@�[
        {
            brokeProgressAnime.StopDamageOverTime();
        }
        // �ʵe������۰ʦ^idle
        animator.SetBool(boolParameter, false);
        isTriggered = false; // ���\�U��Ĳ�o
        Debug.Log(animationName + " �ʵe�����A�^�� idle");
        animationFinished = true; // �q���G��i�ͦ�

        //  �q��NewPlayerTeach�ʵe���񵲧�
        NewPlayerTeach teachScript = FindObjectOfType<NewPlayerTeach>();
        if (teachScript != null)
        {
            teachScript.Isblenderfinish();
        }
    }
    public void ResetFinishedState()  //LiquidPopOut �q���ͦ������A�i���m���A
    {
        animationFinished = false;
        finishedColor = "";
    }
}
