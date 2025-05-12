using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterHit;
public class MonsterHitAnime : MonoBehaviour
{
    public Animator animator;
    private bool isTriggered = false; // �T�O�uĲ�o�@��
    private MonsterHit MonsterHitScript;  // ���o MonsterHit �}��                                  

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Attack", false);

        // ������� MonsterHit �}��
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
        yield return null; // ���@�V���ʵe���A����

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // ���ݰʵe����
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
