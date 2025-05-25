using System.Collections.Generic;
using UnityEngine;

public class FixedItemTwoD : MonoBehaviour
{
    public GameObject[] itemPlaces; // �ײz����ͦ��I
    public GameObject fixedItemPrefab; // �ײz����w�s��
    public CustomerPlace customerPlace; // �ʴ����ȤH�޲z��

    public Dictionary<Vector3, GameObject> spawnedItems = new Dictionary<Vector3, GameObject>();
    private HashSet<Vector3> returnedItemSeats = new HashSet<Vector3>(); // �ȤH���e�٪���(hp>=0)
    void Start()
    {
        // ���o LimitScene �}��
        //limitScene = FindObjectOfType<limitScene>();
        if (customerPlace == null)
        {
            customerPlace = FindObjectOfType<CustomerPlace>();
        }

        if (customerPlace == null)
        {
            Debug.LogError("CustomerPlace ���]�m�I");
            return;
        }

        //if (itemPlaces == null || itemPlaces.Length < 3) { Debug.LogError("�г]�w������ ItemPlace (�ܤ�3��)�I"); return; }

        if (fixedItemPrefab == null)
        {
            Debug.LogError("FixedItemPrefab ���]�w�I");
            return;
        }

    }

    void Update()
    {
        //MonitorCustomerSeats();
        if (customerPlace != null)
        {
            MonitorCustomerSeats();
        }
    }

    // �ʴ��y��O�_���ȤH�A�æb������ ItemPlace �ͦ��ײz����
    private void MonitorCustomerSeats()
    {
        Vector3[] seatPositions = customerPlace.GetSeatPositions(); // ���o�y���m

        for (int i = 0; i < seatPositions.Length; i++)
        {
            bool seatOccupied = false;

            foreach (Customer customer in customerPlace.customerList)
            {
                // �O�_���ȤH�b���y��
                if (Vector3.Distance(customer.targetPos, seatPositions[i]) < 1f)
                {
                    seatOccupied = true;

                    // ���o�ӫȤH���W�� �@�߭�
                    Machine customerMachine = customer.GetComponent<Machine>();
                    if (customerMachine != null && customerMachine.HP <= 0f)
                    {
                        //  �p�G HP <= 0  �R�ײz����
                        if (spawnedItems.ContainsKey(seatPositions[i]))
                        {
                            Destroy(spawnedItems[seatPositions[i]]);
                            spawnedItems.Remove(seatPositions[i]);
                        }
                    }
                    // �p�G HP > 0 �S�ͦ��L�ײz����A�ȤH���e�٪���(hp>=0)
                    else if (!spawnedItems.ContainsKey(seatPositions[i]) || returnedItemSeats.Contains(seatPositions[i]))
                    {
                        GameObject newFixedItem = Instantiate(fixedItemPrefab, itemPlaces[i].transform.position, Quaternion.identity);
                        newFixedItem.tag = "fixeditem";
                        spawnedItems[seatPositions[i]] = newFixedItem;

                        // �Y�O���e�ٹL���󪺮y��(hp>=0)�A�M�Ůy��
                        returnedItemSeats.Remove(seatPositions[i]);
                    }

                    break; // �T�O�u�ͦ��@�ӭײz�����A�ߧY�����k
                }
            }
            //�p�G�ȤH���}�y��R���ͦ��ײz����A�÷P���s�ȤH�A�ͦ��s��
            if (!seatOccupied && spawnedItems.ContainsKey(seatPositions[i]))
            {
                Destroy(spawnedItems[seatPositions[i]]);
                spawnedItems.Remove(seatPositions[i]);
                returnedItemSeats.Remove(seatPositions[i]);
            }
        }
    }



    private void OnTriggerEnter(Collider other) //��N��a�n�ײz�o�����ٵ��ȤH
    {
        Customer customer = other.GetComponent<Customer>();

        if (customer != null)
        {
            Vector3 customerSeat = customer.targetPos;

            if (spawnedItems.ContainsKey(customerSeat))
            {
                Destroy(spawnedItems[customerSeat]);
                spawnedItems.Remove(customerSeat);
                returnedItemSeats.Add(customerSeat);  //�b���ͦ�
            }
        }
    }
}