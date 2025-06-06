using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public Image Patiencebar; // 耐心條
    private Coroutine patienceCoroutine; // 耐心條倒數
    private Machine machineScript;  // 呼叫耐心條數值

    [HideInInspector] public Vector3 targetPos;  // 位置
    [HideInInspector] public bool hasArrived = false;  // 客人是否到定位置

    [HideInInspector] public CustomerPlace customerPlace; // 連結 CustomerPlace

    private bool isLeaving = false;  // 客人是否到離開
    private Vector3 leaveTargetPos;  // 離開方向

    public CustomerMoney customerMoney; //金錢分數程式

    public void StartPatience()   //耐心值定義
    {

        GameObject barObject = GameObject.FindWithTag("brokebar");
        if (barObject != null)
        {
            Patiencebar = barObject.GetComponent<Image>();
        }
        else
        {
            Debug.LogWarning("找不到 tag 為 'brokebar' 的物件！");
        }

        machineScript = GetComponent<Machine>();
        if (patienceCoroutine == null)
        {
            patienceCoroutine = StartCoroutine(PatienceDownTime());

        }
    }

    private void RefreshPatiencebar()  //耐心值定義
    {
        //找場景耐久值來用
        // GameObject.FindGameObjectsWithTag("brokebar");
        if (machineScript != null && Patiencebar != null)
        {
            Patiencebar.fillAmount = machineScript.HP / machineScript.HPMax;
        }
    }

    private IEnumerator PatienceDownTime()   //耐心值下降定義 與 歸零離開
    {
        while (machineScript != null)
        {
            machineScript.HP -= machineScript.HPMax * 0.03f; //每秒下降  暫時修改   如果調整的話  FixedItemTwoD 跟  CustomerBarCallTry 也要改
            RefreshPatiencebar();
            yield return new WaitForSeconds(1f);

            if (machineScript.HP <= 0.0f && !isLeaving)
            {
                Leave();

            }

        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("fixeditemOpen"))
        {
            Leave();
            Destroy(coll.gameObject);
            customerMoney.GetMoney(50);

            NewPlayerTeach teachScript = FindObjectOfType<NewPlayerTeach>();
            if (teachScript != null)
            {
                teachScript.IsGiveCustomer();
            }
        }
    }

    public void Leave()
    {
        // 讓客人平滑移動出場外 並 銷毀
        isLeaving = true;
        leaveTargetPos = transform.position + new Vector3(-13f, 0, 0); // 向左滑出
        StartCoroutine(MoveOutAndDestroy());
    }

    private IEnumerator MoveOutAndDestroy()
    {
        while (Vector3.Distance(transform.position, leaveTargetPos) > 1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, leaveTargetPos, 20 * Time.deltaTime);
            yield return null;
        }

        // 確保客人完全離開 (-965) 才移除
        if (customerPlace != null)
        {
            customerPlace.RemoveCustomer(this);
        }

        Destroy(gameObject);
    }

    private void OnDisable()
    {
        if (patienceCoroutine != null)
        {
            StopCoroutine(patienceCoroutine);
            patienceCoroutine = null;
        }
    }
}
