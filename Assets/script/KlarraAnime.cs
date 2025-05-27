using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KlarraAnime : MonoBehaviour
{
    public Animator animator;
    private bool isTriggered = false;
    private NewPlayerTeach NewPlayerTeachScript;  // ���o NewPlayerTeach �s��о�
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("idelTOmove", false);

        NewPlayerTeachScript = GetComponent<NewPlayerTeach>();

    }

    public void PickPhone()
    {
        animator.SetBool("idelTOmove", true);
        isTriggered = true; // �קK�h��Ĳ�o
        StartCoroutine(WaitForAnimationToEnd());
    }

    public void KeepTakePhone()
    {
        PickPhone(); //���_�q�ܫ� �}�l�`������
        animator.SetBool("KeepTake", true);
        isTriggered = true; // �קK�h��Ĳ�o
    }

    public void HangUpPhone()
    {

        animator.SetBool("HangPhone", true);  //���q��
        isTriggered = true; // �קK�h��Ĳ�o
                            // StartCoroutine(WaitForAnimationToEnd());
    }

    public void AfterHangUpPhone()     //���q�ܫ�^�ݾ�
    {
        animator.SetBool("moveTOidel", true);  //���q��
        isTriggered = true; // �קK�h��Ĳ�o
                            // StartCoroutine(WaitForAnimationToEnd());
    }

    private IEnumerator WaitForAnimationToEnd()
    {
        yield return null; // ���@�V���ʵe���A����

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        while (!stateInfo.IsName("pick up the phone"))    // ���ݰʵe����  PickPhone
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }
        while (stateInfo.normalizedTime < 0.87f)        // ���ݰʵe���񵲧�  PickPhone
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }
        animator.SetBool("idelTOmove", false);
        KeepTakePhone();



        while (!stateInfo.IsName("hang up the phone"))   // ���ݰʵe����  PickPhone
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }
        while (stateInfo.normalizedTime < 0.90f)        // ���ݰʵe���񵲧�  PickPhone
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }
        animator.SetBool("moveTOidel", false);
        AfterHangUpPhone();
        animator.SetBool("idelTOmove", false);
    }
}


