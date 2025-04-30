using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerPlace : MonoBehaviour
{
    public GameObject customers; // 客人預製體
    public GameObject customerPlace; // 座位中心點
    public float moveSpeed = 100; // 客人移動速度
    public int spawnDelay = 3; // 生成間隔時間（秒）
    public GameObject BringFixedItem;//攜帶要修理得物件

    public List<Customer> customerList = new List<Customer>(); // 座位上的客人列表
    public Vector3[] seatPositions = new Vector3[3]; // 三個座位的位置
    private GameObject currentSpawnedCustomer; // 生成點上的客人

    private float rangeMin = -9.4f; // 檢查範圍最小值
    private float rangeMax = 9.4f;  // 檢查範圍最大值
    private bool canSpawn = true; // 控制是否允許生成

    void Start()
    {
        //if (customerPlace == null){Debug.LogWarning($"GameObject {gameObject.name} 缺少參考欄位：customerPlace 尚未指定！", gameObject);return; // 中斷 Start，避免後面程式繼續執行出錯}
        // 設定三個座位的位置：中心、右邊6、左邊-5
        Vector3 center = customerPlace.transform.position;
        seatPositions[0] = center;
        seatPositions[1] = center + new Vector3(6f, 0, 0);
        seatPositions[2] = center + new Vector3(-5f, 0, 0);

        
        // 啟動生成協程
        StartCoroutine(SpawnCustomerLoop());
    }

    private void OnDrawGizmos()
    {
        if (customerPlace == null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 1f); // 畫紅圈

#if UNITY_EDITOR
            UnityEditor.Handles.Label(transform.position + Vector3.up * 2f, "! customerPlace 未設定");
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

            // 確保生成點只在有空位且範圍內客人小於3名時啟動
            if (!canSpawn || currentSpawnedCustomer != null || customerList.Count >= 3)
                continue;

            // 生成新的客人到生成點（1300為緩衝位置）
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

       
        CheckCustomersInRange();     //檢查生成範圍

        if (currentSpawnedCustomer != null)
        {
            Customer customer = currentSpawnedCustomer.GetComponent<Customer>();
            int seatIndex = GetFirstAvailableSeat();
            if (seatIndex != -1)
            {
                // 移動到空的座位
                customer.targetPos = seatPositions[seatIndex];
                customer.transform.position = Vector3.MoveTowards(
                    customer.transform.position,
                    customer.targetPos,
                    moveSpeed * Time.deltaTime
                );

                // 到達座位，正式入座
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
                // 理論上不應該走到這裡，保險邏輯  //破壞多的人
                Destroy(currentSpawnedCustomer);
                currentSpawnedCustomer = null;
            }
        }
    }

    // 取得第一個可用的座位
    private int GetFirstAvailableSeat()
    {
        bool[] seatsOccupied = new bool[3];
        foreach (Customer customer in customerList)
        {
            if (Vector3.Distance(customer.targetPos, seatPositions[0]) < 1f)
            {
                seatsOccupied[0] = true;  // 如果座位0被佔用，標記為已佔用
            }

            // 檢查座位 1
            if (Vector3.Distance(customer.targetPos, seatPositions[1]) < 1f)
            {
                seatsOccupied[1] = true;  // 如果座位1被佔用，標記為已佔用
            }

            // 檢查座位 2
            if (Vector3.Distance(customer.targetPos, seatPositions[2]) < 1f)
            {
                seatsOccupied[2] = true;  // 如果座位2被佔用，標記為已佔用
            }
        }

        // 檢查座位 0
        if (!seatsOccupied[0])
        {
            seatsOccupied[0] = true;  // 標記為已佔用
            return 0;  // 返回座位 0 的索引
        }

        // 檢查座位 1
        if (!seatsOccupied[1])
        {
            seatsOccupied[1] = true;  // 標記為已佔用
            return 1;  // 返回座位 1 的索引
        }

        // 檢查座位 2
        if (!seatsOccupied[2])
        {
            seatsOccupied[2] = true;  // 標記為已佔用
            return 2;  // 返回座位 2 的索引
        }

        return -1;  // 如果沒有空位，返回 -1
    }

    // 範圍內客人數量檢查
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

    //當將攜帶要修理得物件還給客人
    private void OnTriggerEnter(Collider other)
    {
        // 檢查碰撞的物件是否有 CustomerPlace 腳本
        CustomerPlace customerPlace = other.GetComponent<CustomerPlace>();

        if (customerPlace != null) // 確保它是客人管理區
        {
            // 嘗試找到 customerPlace 內的客人
            foreach (Customer customer in customerPlace.customerList)
            {
                RemoveCustomer(customer); // 呼叫移除客人方法

                // 檢查客人是否攜帶物件，並刪除
                if (customer.BringFixedItem != null)
                {
                    Destroy(customer.BringFixedItem);
                }
            }
        }
    }

    // 客人耐心值歸零時離場，移除列表
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
       

        // 重新檢查範圍內的客人數量
        CheckCustomersInRange();

        // 若範圍內客人數 < 3，允許生成
        int countInRange = 0;
        foreach (Customer c in FindObjectsOfType<Customer>())
        {
            if (c.transform.position.x >= rangeMin && c.transform.position.x <= rangeMax)
            {
                countInRange++;
            }
        }

        canSpawn = countInRange < 3; // 只有當客人少於 3 人時，才允許生成
    }
}
