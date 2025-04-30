using System.Collections.Generic;
using UnityEngine;

public class FixedItemTwoD : MonoBehaviour
{
    public GameObject[] itemPlaces; // �ײz����ͦ��I
    public GameObject fixedItemPrefab; // �ײz����w�s��
    public CustomerPlace customerPlace; // �ʴ����ȤH�޲z��
    //private limitScene limitScene; // ��������޲z��

    public Dictionary<Vector3, GameObject> spawnedItems = new Dictionary<Vector3, GameObject>();

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
        // �p�G�w�g���ײz����s�b�A���A�ͦ��s��
        if (spawnedItems.Count >= 1) return;

        Vector3[] seatPositions = customerPlace.GetSeatPositions(); // ���o�y���m

        for (int i = 0; i < seatPositions.Length; i++)
        {
            bool seatOccupied = false;

            foreach (Customer customer in customerPlace.customerList)
            {
                if (Vector3.Distance(customer.targetPos, seatPositions[i]) < 1f)
                {
                    seatOccupied = true;
                    break;
                }
            }

            // �Y�y��W���ȤH�A�B�ثe�|�����ײz����A�h�ͦ�
            if (seatOccupied && !spawnedItems.ContainsKey(seatPositions[i]))
            {
                GameObject newFixedItem = Instantiate(fixedItemPrefab, itemPlaces[i].transform.position, Quaternion.identity);
                spawnedItems[seatPositions[i]] = newFixedItem;
                break; // **�T�O�u�ͦ��@�ӭײz�����A�ߧY�����k**
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Customer customer = other.GetComponent<Customer>();

        if (customer != null)
        {
            Vector3 customerSeat = customer.targetPos;

            if (spawnedItems.ContainsKey(customerSeat))
            {
                Destroy(spawnedItems[customerSeat]);
                spawnedItems.Remove(customerSeat);
            }
        }
    }
}