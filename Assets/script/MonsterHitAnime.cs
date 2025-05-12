using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterHit;
public class MonsterHitAnime : MonoBehaviour
{
    public Animator animator;
    private bool isTriggered = false; // 確保只觸發一次
    private MonsterHit MonsterHitScript;  // 取得 MonsterHit 腳本                                  

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Attack", false);

        // 嘗試獲取 MonsterHit 腳本
        MonsterHitScript = GetComponent<MonsterHit>();
    }

    private void Update()
    {
        if (MonsterHitScript == null || isTriggered) return;

        switch (MonsterHitScript.CurrentState)
        {
            case MonsterHit.MonsterState.Attacking:
                animator.SetBool("Attack", true);
                isTriggered = true;
                break;

            case MonsterHit.MonsterState.Success:
                animator.SetBool("Success", true);
                animator.SetBool("Attack", false);
                animator.SetBool("Fail", false);
                animator.SetBool("Leave", false);
                isTriggered = true;
                break;

            case MonsterHit.MonsterState.Fail:
                animator.SetBool("Fail", true);
                animator.SetBool("Success", false);
                animator.SetBool("Attack", false);
                animator.SetBool("Leave", false);
                isTriggered = true;
                StartCoroutine(WaitForAnimationToEnd());
                break;

            case MonsterHit.MonsterState.Leaving:
                animator.SetBool("Leave", true);
                animator.SetBool("Success", false);
                animator.SetBool("Attack", false);
                animator.SetBool("Fail", false);
                isTriggered = true;
                StartCoroutine(WaitForAnimationToEnd());
                break;
        }
    }

    private IEnumerator WaitForAnimationToEnd()
    {
        yield return null; // 等一幀讓動畫狀態切換

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 等待動畫結束
        while (stateInfo.IsName("fail") && stateInfo.normalizedTime < 0.1f)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(1f);
            break;
        }
        animator.SetBool("Fail", true);
    }
}
