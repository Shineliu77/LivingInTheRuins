using System.Collections.Generic;
using UnityEngine;

public class FixedItemTwoD : MonoBehaviour
{
    public GameObject[] itemPlaces; // 修理物件生成點
    public GameObject fixedItemPrefab; // 修理物件預製體
    public CustomerPlace customerPlace; // 監測的客人管理區
                                       
    public Dictionary<Vector3, GameObject> spawnedItems = new Dictionary<Vector3, GameObject>();

    void Start()
    {
        // 取得 LimitScene 腳本
        //limitScene = FindObjectOfType<limitScene>();
        if (customerPlace == null)
        {
            customerPlace = FindObjectOfType<CustomerPlace>();
        }

        if (customerPlace == null)
        {
            Debug.LogError("CustomerPlace 未設置！");
            return;
        }

        //if (itemPlaces == null || itemPlaces.Length < 3) { Debug.LogError("請設定足夠的 ItemPlace (至少3個)！"); return; }

        if (fixedItemPrefab == null)
        {
            Debug.LogError("FixedItemPrefab 未設定！");
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

    // 監測座位是否有客人，並在對應的 ItemPlace 生成修理物件
    private void MonitorCustomerSeats()
    {
        // 如果已經有修理物件存在，不再生成新的
        //  if (spawnedItems.Count >= 1) return;

        Vector3[] seatPositions = customerPlace.GetSeatPositions(); // 取得座位位置

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

            // 若座位上有客人，且目前尚未有修理物件，則生成
            if (seatOccupied && !spawnedItems.ContainsKey(seatPositions[i]))
            {
                GameObject newFixedItem = Instantiate(fixedItemPrefab, itemPlaces[i].transform.position, Quaternion.identity);
                spawnedItems[seatPositions[i]] = newFixedItem;
                newFixedItem.tag = "fixeditem";
                break; // 確保只生成一個修理物件後，立即停止方法
            }
            //如果客人離開座位摧毀生成修理物件，並感應新客人，生成新物
            if (!seatOccupied && spawnedItems.ContainsKey(seatPositions[i]))
            {
                Destroy(spawnedItems[seatPositions[i]]);
                spawnedItems.Remove(seatPositions[i]);
            }
        }
    }
  
    private void OnTriggerEnter(Collider other) //當將攜帶要修理得物件還給客人
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