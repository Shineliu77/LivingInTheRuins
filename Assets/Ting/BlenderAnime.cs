using System.Collections;
using UnityEngine;

public class BlenderAnime : MonoBehaviour
{
    public Animator animator;
    private bool isTriggered = false;

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
            if (coll.gameObject.CompareTag("Blue"))
            {
                Debug.Log("碰到 Blue！");
                animator.SetBool("idelTOmove", true);
                isTriggered = true;
                StartCoroutine(WaitForAnimationToEnd("blue work", "idelTOmove"));
            }
            else if (coll.gameObject.CompareTag("Red"))
            {
                Debug.Log("碰到 Red！");
                animator.SetBool("idelTOmoveR", true);
                isTriggered = true;
                StartCoroutine(WaitForAnimationToEnd("red work", "idelTOmoveR"));
            }
            else if (coll.gameObject.CompareTag("Green"))
            {
                Debug.Log("碰到 Green！");
                animator.SetBool("idelTOmoveG", true);
                isTriggered = true;
                StartCoroutine(WaitForAnimationToEnd("green work", "idelTOmoveG"));
            }
            else if (coll.gameObject.CompareTag("Yellow"))
            {
                Debug.Log("碰到 Yellow！");
                animator.SetBool("idelTOmoveY", true);
                isTriggered = true;
                StartCoroutine(WaitForAnimationToEnd("yellow work", "idelTOmoveY"));
            }
        }
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

        // 動畫結束後自動回idle
        animator.SetBool(boolParameter, false);
        isTriggered = false; // 允許下次觸發
        Debug.Log(animationName + " 動畫結束，回到 idle");
    }
}
