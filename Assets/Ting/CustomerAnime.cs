using System.Collections;
using UnityEngine;

public class CustomerAnime : MonoBehaviour
{
    public Animator animator;
    private bool isTriggered = false;
    private Customer customerScript;  // 取得 Customer 程式
    private Machine machineScript;  // 取得 Machine 程式

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("BrokebarEmpty", false);
        // 嘗試獲取 Customer 腳本
        customerScript = GetComponent<Customer>();
        if (customerScript != null)
        {
            machineScript = customerScript.GetComponent<Machine>();  // 確保獲取 Machine 腳本
        }

    }

    private void Update()
    {
        // 檢查耐心值是否歸零，並確保只觸發一次
        if (machineScript != null && machineScript.HP <= 0.0f && !isTriggered)
        {
            TriggerBrokebarEmpty();
        }
        // 如果機器耐心值不為零，持續撥放idel
        if (machineScript != null && machineScript.HP >= 0.0f && !isTriggered)
        {
            animator.SetBool("BrokebarEmpty", false);
            animator.SetBool("BrokebarEmptyAngry", false);
        }
    }
    private void TriggerBrokebarEmpty()
    {
        animator.SetBool("BrokebarEmpty", true);
        isTriggered = true; // 避免多次觸發
        StartCoroutine(WaitForAnimationToEnd());
    }

    private IEnumerator WaitForAnimationToEnd()
    {

        yield return null; // 等一幀讓動畫狀態切換

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 等待動畫結束
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


