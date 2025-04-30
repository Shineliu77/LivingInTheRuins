using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerPlace : MonoBehaviour
{
    public GameObject customers; // �ȤH�w�s��
    public GameObject customerPlace; // �y�줤���I
    public float moveSpeed = 100; // �ȤH���ʳt��
    public int spawnDelay = 3; // �ͦ����j�ɶ��]��^
    public GameObject BringFixedItem;//��a�n�ײz�o����

    public List<Customer> customerList = new List<Customer>(); // �y��W���ȤH�C��
    public Vector3[] seatPositions = new Vector3[3]; // �T�Ӯy�쪺��m
    private GameObject currentSpawnedCustomer; // �ͦ��I�W���ȤH

    private float rangeMin = -9.4f; // �ˬd�d��̤p��
    private float rangeMax = 9.4f;  // �ˬd�d��̤j��
    private bool canSpawn = true; // ����O�_���\�ͦ�

    void Start()
    {
        //if (customerPlace == null){Debug.LogWarning($"GameObject {gameObject.name} �ʤְѦ����GcustomerPlace �|�����w�I", gameObject);return; // ���_ Start�A�קK�᭱�{���~�����X��}
        // �]�w�T�Ӯy�쪺��m�G���ߡB�k��6�B����-5
        Vector3 center = customerPlace.transform.position;
        seatPositions[0] = center;
        seatPositions[1] = center + new Vector3(6f, 0, 0);
        seatPositions[2] = center + new Vector3(-5f, 0, 0);

        
        // �Ұʥͦ���{
        StartCoroutine(SpawnCustomerLoop());
    }

    private void OnDrawGizmos()
    {
        if (customerPlace == null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 1f); // �e����

#if UNITY_EDITOR
            UnityEditor.Handles.Label(transform.position + Vector3.up * 2f, "! customerPlace ���]�w");
#endif
        }
    }
    public Vector3[] GetSeatPositions()
    {
        return seatPositions;
    }
    IEnumerator SpawnCustomerLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);

            // �T�O�ͦ��I�u�b���Ŧ�B�d�򤺫ȤH�p��3�W�ɱҰ�
            if (!canSpawn || currentSpawnedCustomer != null || customerList.Count >= 3)
                continue;

            // �ͦ��s���ȤH��ͦ��I�]1300���w�Ħ�m�^
            currentSpawnedCustomer = Instantiate(customers,
                customerPlace.transform.position + new Vector3(13f, 0, 0),
                Quaternion.identity,
                customerPlace.transform);

            Customer customerScript = currentSpawnedCustomer.GetComponent<Customer>();
            customerScript.customerPlace = this;
            if (!canSpawn)
                continue;
        }
    }

    void Update()
    {

       
        CheckCustomersInRange();     //�ˬd�ͦ��d��

        if (currentSpawnedCustomer != null)
        {
            Customer customer = currentSpawnedCustomer.GetComponent<Customer>();
            int seatIndex = GetFirstAvailableSeat();
            if (seatIndex != -1)
            {
                // ���ʨ�Ū��y��
                customer.targetPos = seatPositions[seatIndex];
                customer.transform.position = Vector3.MoveTowards(
                    customer.transform.position,
                    customer.targetPos,
                    moveSpeed * Time.deltaTime
                );

                // ��F�y��A�����J�y
                if (Vector3.Distance(customer.transform.position, customer.targetPos) < 1f)
                {
                    customer.hasArrived = true;
                    customer.StartPatience();
                    customerList.Add(customer);
                    currentSpawnedCustomer = null;
                }
                if (canSpawn && customerList.Count < 3 && currentSpawnedCustomer == null)
                {
                    StartCoroutine(SpawnCustomerLoop());
                }
            }
            else
            {
                // �z�פW�����Ө���o�̡A�O�I�޿�  //�}�a�h���H
                Destroy(currentSpawnedCustomer);
                currentSpawnedCustomer = null;
            }
        }
    }

    // ���o�Ĥ@�ӥi�Ϊ��y��
    private int GetFirstAvailableSeat()
    {
        bool[] seatsOccupied = new bool[3];
        foreach (Customer customer in customerList)
        {
            if (Vector3.Distance(customer.targetPos, seatPositions[0]) < 1f)
            {
                seatsOccupied[0] = true;  // �p�G�y��0�Q���ΡA�аO���w����
            }

            // �ˬd�y�� 1
            if (Vector3.Distance(customer.targetPos, seatPositions[1]) < 1f)
            {
                seatsOccupied[1] = true;  // �p�G�y��1�Q���ΡA�аO���w����
            }

            // �ˬd�y�� 2
            if (Vector3.Distance(customer.targetPos, seatPositions[2]) < 1f)
            {
                seatsOccupied[2] = true;  // �p�G�y��2�Q���ΡA�аO���w����
            }
        }

        // �ˬd�y�� 0
        if (!seatsOccupied[0])
        {
            seatsOccupied[0] = true;  // �аO���w����
            return 0;  // ��^�y�� 0 ������
        }

        // �ˬd�y�� 1
        if (!seatsOccupied[1])
        {
            seatsOccupied[1] = true;  // �аO���w����
            return 1;  // ��^�y�� 1 ������
        }

        // �ˬd�y�� 2
        if (!seatsOccupied[2])
        {
            seatsOccupied[2] = true;  // �аO���w����
            return 2;  // ��^�y�� 2 ������
        }

        return -1;  // �p�G�S���Ŧ�A��^ -1
    }

    // �d�򤺫ȤH�ƶq�ˬd
    private void CheckCustomersInRange()
    {
        Customer[] allCustomers = FindObjectsOfType<Customer>();
        int countInRange = 0;

        foreach (Customer customer in allCustomers)
        {
            float x = customer.transform.position.x;
            if (x >= rangeMin && x <= rangeMax)
            {
                countInRange++;
                canSpawn = countInRange < 3;

                if (canSpawn = countInRange == 3)
                {


                    canSpawn = false;
                }
            }
        }
    }

    //��N��a�n�ײz�o�����ٵ��ȤH
    private void OnTriggerEnter(Collider other)
    {
        // �ˬd�I��������O�_�� CustomerPlace �}��
        CustomerPlace customerPlace = other.GetComponent<CustomerPlace>();

        if (customerPlace != null) // �T�O���O�ȤH�޲z��
        {
            // ���է�� customerPlace �����ȤH
            foreach (Customer customer in customerPlace.customerList)
            {
                RemoveCustomer(customer); // �I�s�����ȤH��k

                // �ˬd�ȤH�O�_��a����A�çR��
                if (customer.BringFixedItem != null)
                {
                    Destroy(customer.BringFixedItem);
                }
            }
        }
    }

    // �ȤH�@�߭��k�s�������A�����C��
    public void RemoveCustomer(Customer customer)
    {
        if (customerList.Contains(customer))
        {
            customerList.Remove(customer);

            if (customer.BringFixedItem != null)
            {
                Destroy(customer.BringFixedItem);
            }
        }
       

        // ���s�ˬd�d�򤺪��ȤH�ƶq
        CheckCustomersInRange();

        // �Y�d�򤺫ȤH�� < 3�A���\�ͦ�
        int countInRange = 0;
        foreach (Customer c in FindObjectsOfType<Customer>())
        {
            if (c.transform.position.x >= rangeMin && c.transform.position.x <= rangeMax)
            {
                countInRange++;
            }
        }

        canSpawn = countInRange < 3; // �u����ȤH�֩� 3 �H�ɡA�~���\�ͦ�
    }
}
