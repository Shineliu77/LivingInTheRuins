using System.Collections.Generic;
using UnityEngine;

public class CustomerbarUse : MonoBehaviour
{
    public GameObject[] itemPlaces; // �@�[�ȥͦ��I
    public GameObject customerBar;  // �ȤH�@�[�ȹw�s�� (�ݦ� CustomerBarCallTry �}��)
    public CustomerPlace customerPlace;

    public Dictionary<Vector3, GameObject> spawnedItems = new Dictionary<Vector3, GameObject>();

    void Start()
    {
        if (customerPlace == null)
        {
            customerPlace = FindObjectOfType<CustomerPlace>();
        }

        if (customerPlace == null)
        {
            Debug.LogError("CustomerPlace ���]�m�I");
            return;
        }

        if (customerBar == null)
        {
            Debug.LogError("customerBar ���]�w�I");
            return;
        }
    }

    void Update()
    {
        if (customerPlace != null)
        {
            MonitorCustomerSeats();
        }
    }

    // �ˬd�y��O�_���ȤH�A�åͦ��@�[��
    private void MonitorCustomerSeats()
    {
        Vector3[] seatPositions = customerPlace.GetSeatPositions();

        for (int i = 0; i < seatPositions.Length; i++)
        {
            bool seatOccupied = false;

            foreach (Customer customer in customerPlace.customerList)
            {
                if (Vector3.Distance(customer.targetPos, seatPositions[i]) < 1f)
                {
                    seatOccupied = true;

                    Machine customerMachine = customer.GetComponent<Machine>();
                    if (customerMachine != null && customerMachine.HP <= 0f)
                    {
                        // �R���w�s�b���@�[��
                        if (spawnedItems.ContainsKey(seatPositions[i]))
                        {
                            Destroy(spawnedItems[seatPositions[i]]);
                            spawnedItems.Remove(seatPositions[i]);
                        }
                    }
                    else if (!spawnedItems.ContainsKey(seatPositions[i]))
                    {
                        // �Y�|���ͦ��@�[�ȡA�h�ͦ��ñҰʭ@�[�˼�
                        GameObject canvas = GameObject.Find("Canvas");
                        GameObject newbrokebar = Instantiate(customerBar, itemPlaces[i].transform.position, Quaternion.identity, canvas.transform);
                        spawnedItems[seatPositions[i]] = newbrokebar;

                        // �Ұʦۨ��˼�
                        CustomerBarCallTry barScript = newbrokebar.GetComponent<CustomerBarCallTry>();
                        if (barScript != null)
                        {
                            barScript.StartPatience();
                        }
                        else
                        {
                            Debug.LogError("�ͦ��� bar �ʤ� CustomerBarCallTry �}���I");
                        }
                    }
                }
            }
        }
    }
}