using UnityEngine;
using System.Collections;

public class ForopenerOutside : MonoBehaviour
{
    public GameObject NewOpenerPrefab; // 新生成的物件Prefab
    public Animator animator;
    private bool isTriggered = false; // 確保只觸發一次

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
            Debug.LogError("Animator 未設定！");
            yield break;
        }

        animator.SetBool("idelTOmove", true); // 播放動畫

        yield return null; // 確保動畫狀態切換
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 等待動畫 "work" 播放完畢
        while (stateInfo.IsName("work") && stateInfo.normalizedTime < 1f)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        // 等待額外的時間以確保狀態更新（如有需要）
        yield return new WaitForSeconds(0.5f); // 防止動畫狀態未更新的延遲

        // 生成新的物件
        if (NewOpenerPrefab != null)
        {
            GameObject newObject = Instantiate(NewOpenerPrefab, transform.position, Quaternion.identity);
            newObject.AddComponent<Dragging>(); // 確保新物件可拖曳
        }
        else
        {
            Debug.LogError("NewOpenerPrefab 未設定！");
        }

        // 銷毀自身
        Destroy(gameObject);
    }
}