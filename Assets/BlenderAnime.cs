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
                Debug.Log("�I�� Blue�I");
                animator.SetBool("idelTOmove", true);
                isTriggered = true;
                StartCoroutine(WaitForAnimationToEnd("blue work", "idelTOmove"));
            }
            else if (coll.gameObject.CompareTag("Red"))
            {
                Debug.Log("�I�� Red�I");
                animator.SetBool("idelTOmoveR", true);
                isTriggered = true;
                StartCoroutine(WaitForAnimationToEnd("red work", "idelTOmoveR"));
            }
            else if (coll.gameObject.CompareTag("Green"))
            {
                Debug.Log("�I�� Green�I");
                animator.SetBool("idelTOmoveG", true);
                isTriggered = true;
                StartCoroutine(WaitForAnimationToEnd("green work", "idelTOmoveG"));
            }
            else if (coll.gameObject.CompareTag("Yellow"))
            {
                Debug.Log("�I�� Yellow�I");
                animator.SetBool("idelTOmoveY", true);
                isTriggered = true;
                StartCoroutine(WaitForAnimationToEnd("yellow work", "idelTOmoveY"));
            }
        }
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

        // �ʵe������۰ʦ^idle
        animator.SetBool(boolParameter, false);
        isTriggered = false; // ���\�U��Ĳ�o
        Debug.Log(animationName + " �ʵe�����A�^�� idle");
    }
}
