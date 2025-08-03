using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCustomerAnime : MonoBehaviour
{
    public Animator animator;
    private bool isTriggered = false;
    private CountdownFill CountdownFillScript;  // 客人耐久值程式

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("BrokebarEmpty", false);

        CountdownFillScript = GetComponent<CountdownFill>();

    }

    public void CountdownFillEmpty() //耐心值程式
    {
        animator.SetBool("BrokebarEmpty", true);
        isTriggered = true; // 避免多次觸發
        StartCoroutine(WaitForAnimationToEnd());
    }

    public void CountdownFillEmptyAngry() //客人生氣
    {
        animator.SetBool("BrokebarEmptyAngry", true);
        isTriggered = true; // 避免多次觸發
    }


    private IEnumerator WaitForAnimationToEnd()
    {

        yield return null; // 等一幀讓動畫狀態切換

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 等待動畫結束
        while (stateInfo.IsName("angry face") && stateInfo.normalizedTime < 0.1f)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(1f);
            break;
        }
        animator.SetBool("BrokebarEmptyAngry", true);
    }

}
