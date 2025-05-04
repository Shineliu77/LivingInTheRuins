using System.Collections;
using UnityEngine;

public class OpenderAnime : MonoBehaviour
{
    public Animator animator;
    private bool isTriggered = false;
    public GameObject NewOpenerPrefab; // �s�ͦ�������Prefab
    public GameObject collidedObject;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("idelTOmove", false);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("fixeditem") && !isTriggered)
        {
            // Debug.Log("�I�� fixeditem�I");
            animator.SetBool("idelTOmove", true);
            isTriggered = true; // �קK�h��Ĳ�o

            //  �T�O�O��������������ӫD�w�s��
            collidedObject = coll.gameObject;

            StartCoroutine(WaitForAnimationToEnd());

        }
    }

    private IEnumerator WaitForAnimationToEnd()
    {
        yield return null; // ���@�V���ʵe���A����

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // ���ݰʵe����
        while (stateInfo.IsName("work") && stateInfo.normalizedTime < 0.1f)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }
        yield return new WaitForSeconds(10f);
        // �ʵe������۰ʦ^idle
        animator.SetBool("idelTOmove", false);
        isTriggered = false; // ���\�U��Ĳ�o
                             // �P���I������
        Destroy(collidedObject);

        //Debug.Log("�ʵe�����A�^��idle");

        if (NewOpenerPrefab != null)
        {
            GameObject newObject = Instantiate(NewOpenerPrefab, transform.position, Quaternion.identity);
            newObject.AddComponent<Dragging>(); // �T�O�s����i�즲
        }
        else
        {
            Debug.LogError("NewOpenerPrefab ���]�w�I");
        }

        Destroy(GameObject.FindWithTag("fixeditem"));
    }
}
