using System.Collections;
using UnityEngine;

public class CustomerAnime : MonoBehaviour
{
    public Animator animator;
    private bool isTriggered = false;
    private Customer customerScript;  // ���o Customer �{��
    private Machine machineScript;  // ���o Machine �{��

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("BrokebarEmpty", false);
        // ������� Customer �}��
        customerScript = GetComponent<Customer>();
        if (customerScript != null)
        {
            machineScript = customerScript.GetComponent<Machine>();  // �T�O��� Machine �}��
        }

    }

    private void Update()
    {
        // �ˬd�@�߭ȬO�_�k�s�A�ýT�O�uĲ�o�@��
        if (machineScript != null && machineScript.HP <= 0.0f && !isTriggered)
        {
            TriggerBrokebarEmpty();
        }
        // �p�G�����@�߭Ȥ����s�A���򼷩�idel
        if (machineScript != null && machineScript.HP >= 0.0f && !isTriggered)
        {
            animator.SetBool("BrokebarEmpty", false);
            animator.SetBool("BrokebarEmptyAngry", false);
        }
    }
    private void TriggerBrokebarEmpty()
    {
        animator.SetBool("BrokebarEmpty", true);
        isTriggered = true; // �קK�h��Ĳ�o
        StartCoroutine(WaitForAnimationToEnd());
    }

    private IEnumerator WaitForAnimationToEnd()
    {

        yield return null; // ���@�V���ʵe���A����

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // ���ݰʵe����
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


