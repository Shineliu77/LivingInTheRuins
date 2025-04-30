using UnityEngine;
using System.Collections;

public class ForopenerOutside : MonoBehaviour
{
    public GameObject NewOpenerPrefab; // �s�ͦ�������Prefab
    public Animator animator;
    private bool isTriggered = false; // �T�O�uĲ�o�@��

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("fixeditem") && !isTriggered)
        {
            isTriggered = true;
            StartCoroutine(HandleAnimationAndSpawn());
        }
    }

    private IEnumerator HandleAnimationAndSpawn()
    {
        if (animator == null)
        {
            Debug.LogError("Animator ���]�w�I");
            yield break;
        }

        animator.SetBool("idelTOmove", true); // ����ʵe

        yield return null; // �T�O�ʵe���A����
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // ���ݰʵe "work" ���񧹲�
        while (stateInfo.IsName("work") && stateInfo.normalizedTime < 1f)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        // �����B�~���ɶ��H�T�O���A��s�]�p���ݭn�^
        yield return new WaitForSeconds(0.5f); // ����ʵe���A����s������

        // �ͦ��s������
        if (NewOpenerPrefab != null)
        {
            GameObject newObject = Instantiate(NewOpenerPrefab, transform.position, Quaternion.identity);
            newObject.AddComponent<Dragging>(); // �T�O�s����i�즲
        }
        else
        {
            Debug.LogError("NewOpenerPrefab ���]�w�I");
        }

        // �P���ۨ�
        Destroy(gameObject);
    }
}