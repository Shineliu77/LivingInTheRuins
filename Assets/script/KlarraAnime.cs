using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KlarraAnime : MonoBehaviour
{
    public Animator animator;
    private bool isTriggered = false;
    private NewPlayerTeach NewPlayerTeachScript;  // 取得 NewPlayerTeach 新手教學
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("idelTOmove", false);

        NewPlayerTeachScript = GetComponent<NewPlayerTeach>();

    }

    public void PickPhone()
    {
        animator.SetBool("idelTOmove", true);
        isTriggered = true; // 避免多次觸發
        StartCoroutine(WaitForAnimationToEnd());
    }

    public void KeepTakePhone()
    {
        PickPhone(); //拿起電話後 開始循環拿著
        animator.SetBool("KeepTake", true);
        isTriggered = true; // 避免多次觸發
    }

    public void HangUpPhone()
    {

        animator.SetBool("HangPhone", true);  //掛電話
        isTriggered = true; // 避免多次觸發
                            // StartCoroutine(WaitForAnimationToEnd());
    }

    public void AfterHangUpPhone()     //掛電話後回待機
    {
        animator.SetBool("moveTOidel", true);  //掛電話
        isTriggered = true; // 避免多次觸發
                            // StartCoroutine(WaitForAnimationToEnd());
    }

    private IEnumerator WaitForAnimationToEnd()
    {
        yield return null; // 等一幀讓動畫狀態切換

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        while (!stateInfo.IsName("pick up the phone"))    // 等待動畫結束  PickPhone
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }
        while (stateInfo.normalizedTime < 0.87f)        // 等待動畫撥放結束  PickPhone
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }
        animator.SetBool("idelTOmove", false);
        KeepTakePhone();



        while (!stateInfo.IsName("hang up the phone"))   // 等待動畫結束  PickPhone
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }
        while (stateInfo.normalizedTime < 0.90f)        // 等待動畫撥放結束  PickPhone
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }
        animator.SetBool("moveTOidel", false);
        AfterHangUpPhone();
        animator.SetBool("idelTOmove", false);
    }
}


