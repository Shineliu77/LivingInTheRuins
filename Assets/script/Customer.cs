using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public Image Patiencebar; // �@�߱�
    private Coroutine patienceCoroutine; // �@�߱��˼�
    private Machine machineScript;  // �I�s�@�߱��ƭ�

    [HideInInspector] public Vector3 targetPos;  // ��m
    [HideInInspector] public bool hasArrived = false;  // �ȤH�O�_��w��m
    [HideInInspector] public GameObject BringFixedItem; // �s�� CustomerPlace
    [HideInInspector] public CustomerPlace customerPlace; // �s�� CustomerPlace

    private bool isLeaving = false;  // �ȤH�O�_�����}
    private Vector3 leaveTargetPos;  // ���}��V

    public CustomerMoney customerMoney; //�������Ƶ{��

    public void StartPatience()   //�@�߭ȩw�q
    {
        machineScript = GetComponent<Machine>();
        if (patienceCoroutine == null)
        {
            patienceCoroutine = StartCoroutine(PatienceDownTime());
        }
    }

    private void RefreshPatiencebar()  //�@�߭ȩw�q
    {
        if (machineScript != null && Patiencebar != null)
        {
            Patiencebar.fillAmount = machineScript.HP / machineScript.HPMax;
        }
    }

    private IEnumerator PatienceDownTime()   //�@�߭ȤU���w�q �P �k�s���}
    {
        while (machineScript != null)
        {
            machineScript.HP -= machineScript.HPMax * 0.01f; //�C��U��  �Ȯɭק�
            RefreshPatiencebar();
            yield return new WaitForSeconds(1f);

            if (machineScript.HP <= 0.0f && !isLeaving)
            {
                Leave();
            }
            if (BringFixedItem != null)
            {
                Collider[] colliders = Physics.OverlapSphere(BringFixedItem.transform.position, 1f);
                foreach (Collider col in colliders)
                {
                    if (col.gameObject == this.gameObject) // �ˬd�O�_�I��ȤH
                    {
                        Leave(); // Ĳ�o����
                        Destroy(BringFixedItem); // �R������
                        break;
                    }
                }
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
        }
    }

    public void Leave()
    {
        // ���ȤH���Ʋ��ʥX���~ �� �P��
        isLeaving = true;
        leaveTargetPos = transform.position + new Vector3(-13f, 0, 0); // �V���ƥX
        StartCoroutine(MoveOutAndDestroy());
    }

    private IEnumerator MoveOutAndDestroy()
    {
        while (Vector3.Distance(transform.position, leaveTargetPos) > 1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, leaveTargetPos, 20 * Time.deltaTime);
            yield return null;
        }

        // �T�O�ȤH�������} (-965) �~����
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
