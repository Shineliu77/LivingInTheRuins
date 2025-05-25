using System.Collections;
using UnityEngine;

public class BlenderAnime : MonoBehaviour
{
    public Animator animator;
    private bool isTriggered = false;
    public BrokeProgressAnime brokeProgressAnime;

    [HideInInspector] public bool animationFinished = false;
    [HideInInspector] public string finishedColor = ""; // 藍、紅、綠、黃

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

            //碰到對應物件觸發動畫
            if (coll.gameObject.CompareTag("Blue"))
            {
                Debug.Log("碰到 Blue！");
                animator.SetBool("idelTOmove", true);
                isTriggered = true;
                LiquidPop("Blue");
                StartCoroutine(WaitForAnimationToEnd("blue work", "idelTOmove"));
            }
            else if (coll.gameObject.CompareTag("Red"))
            {
                Debug.Log("碰到 Red！");
                animator.SetBool("idelTOmoveR", true);
                isTriggered = true;
                LiquidPop("Red");
                StartCoroutine(WaitForAnimationToEnd("red work", "idelTOmoveR"));
            }
            else if (coll.gameObject.CompareTag("Green"))
            {
                Debug.Log("碰到 Green！");
                animator.SetBool("idelTOmoveG", true);
                isTriggered = true;
                LiquidPop("Green");
                StartCoroutine(WaitForAnimationToEnd("green work", "idelTOmoveG"));
            }
            else if (coll.gameObject.CompareTag("Yellow"))
            {
                Debug.Log("碰到 Yellow！");
                animator.SetBool("idelTOmoveY", true);
                isTriggered = true;
                LiquidPop("Yellow");
                StartCoroutine(WaitForAnimationToEnd("yellow work", "idelTOmoveY"));

            }
        }
    }

    private void LiquidPop(string color)  //生成液體
    {
        //  Debug.Log($"碰到 {color}！");
        isTriggered = true;
        finishedColor = color;
    }

    private IEnumerator WaitForAnimationToEnd(string animationName, string boolParameter)
    {
        yield return null; // 等一幀讓動畫狀態切換

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 等待動畫播放完成
        while (stateInfo.IsName(animationName) && stateInfo.normalizedTime < 1f)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        yield return new WaitForSeconds(12f); // 等待額外時間確保動畫結束

        if (brokeProgressAnime != null) //停止扣機器耐久
        {
            brokeProgressAnime.StopDamageOverTime();
        }
        // 動畫結束後自動回idle
        animator.SetBool(boolParameter, false);
        isTriggered = false; // 允許下次觸發
        Debug.Log(animationName + " 動畫結束，回到 idle");
        animationFinished = true; // 通知液體可生成

        //  通知NewPlayerTeach動畫播放結束
        NewPlayerTeach teachScript = FindObjectOfType<NewPlayerTeach>();
        if (teachScript != null)
        {
            teachScript.Isblenderfinish();
        }
    }
    public void ResetFinishedState()  //LiquidPopOut 通知生成結束，可重置狀態
    {
        animationFinished = false;
        finishedColor = "";
    }
}
