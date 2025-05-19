using System.Collections.Generic;
using UnityEngine;

public class CustomerbarUse : MonoBehaviour
{
    public GameObject[] itemPlaces; // 耐久值生成點
    public GameObject customerBar;  // 客人耐久值預製體 (需有 CustomerBarCallTry 腳本)
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
            Debug.LogError("CustomerPlace 未設置！");
            return;
        }

        if (customerBar == null)
        {
            Debug.LogError("customerBar 未設定！");
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

    // 檢查座位是否有客人，並生成耐久值
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
                        // 刪除已存在的耐久值
                        if (spawnedItems.ContainsKey(seatPositions[i]))
                        {
                            Destroy(spawnedItems[seatPositions[i]]);
                            spawnedItems.Remove(seatPositions[i]);
                        }
                    }
                    else if (!spawnedItems.ContainsKey(seatPositions[i]))
                    {
                        // 若尚未生成耐久值，則生成並啟動耐久倒數
                        GameObject canvas = GameObject.Find("Canvas");
                        GameObject newbrokebar = Instantiate(customerBar, itemPlaces[i].transform.position, Quaternion.identity, canvas.transform);
                        spawnedItems[seatPositions[i]] = newbrokebar;

                        // 啟動自身倒數
                        CustomerBarCallTry barScript = newbrokebar.GetComponent<CustomerBarCallTry>();
                        if (barScript != null)
                        {
                            barScript.StartPatience();
                        }
                        else
                        {
                            Debug.LogError("生成的 bar 缺少 CustomerBarCallTry 腳本！");
                        }
                    }
                }
            }
        }
    }
}