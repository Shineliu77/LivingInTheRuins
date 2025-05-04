using System.Collections;
using UnityEngine;

public class OpenderAnime : MonoBehaviour
{
    public Animator animator;
    private bool isTriggered = false;
    public GameObject NewOpenerPrefab; // 新生成的物件Prefab
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
            // Debug.Log("碰到 fixeditem！");
            animator.SetBool("idelTOmove", true);
            isTriggered = true; // 避免多次觸發

            //  確保記錄場景中的物件而非預製體
            collidedObject = coll.gameObject;

            StartCoroutine(WaitForAnimationToEnd());

        }
    }

    private IEnumerator WaitForAnimationToEnd()
    {
        yield return null; // 等一幀讓動畫狀態切換

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 等待動畫結束
        while (stateInfo.IsName("work") && stateInfo.normalizedTime < 0.1f)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }
        yield return new WaitForSeconds(10f);
        // 動畫結束後自動回idle
        animator.SetBool("idelTOmove", false);
        isTriggered = false; // 允許下次觸發
                             // 銷毀碰撞物件
        Destroy(collidedObject);

        //Debug.Log("動畫結束，回到idle");

        if (NewOpenerPrefab != null)
        {
            GameObject newObject = Instantiate(NewOpenerPrefab, transform.position, Quaternion.identity);
            newObject.AddComponent<Dragging>(); // 確保新物件可拖曳
        }
        else
        {
            Debug.LogError("NewOpenerPrefab 未設定！");
        }

        Destroy(GameObject.FindWithTag("fixeditem"));
    }
}
