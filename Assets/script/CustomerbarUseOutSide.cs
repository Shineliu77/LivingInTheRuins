using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CustomerbarUseOutSide : MonoBehaviour
{
    public CustomerPlace customerPlace;
    public GameObject[] itemPlaces; // 耐久值外框生成點
    public GameObject customerBarOutside;  // 外框預製體
    public CustomerbarUse customerbarUse;  // 引用主耐久條生成器

    public Dictionary<Vector3, GameObject> spawnedItems = new Dictionary<Vector3, GameObject>();

    void Start()
    {
        if (customerPlace == null)
            customerPlace = FindObjectOfType<CustomerPlace>();

        if (customerPlace == null)
        {
            Debug.LogError("CustomerPlace 未設置！");
            return;
        }

        if (customerBarOutside == null)
        {
            Debug.LogError("customerBarOutside 未設定！");
            return;
        }
    }

    void Update()
    {
        if (customerPlace != null && customerbarUse != null)
        {

            CustomerbarOutsidePop();                // 外框也生成
        }
    }

    public void CustomerbarOutsidePop()
    {
        Vector3[] seatPositions = customerPlace.GetSeatPositions();

        for (int i = 0; i < seatPositions.Length; i++)
        {
            bool seatOccupied = false;
            GameObject newbrokebar = null;

            foreach (Customer customer in customerPlace.customerList)
            {
                if (Vector3.Distance(customer.targetPos, seatPositions[i]) < 1f)
                {
                    seatOccupied = true;

                    if (!spawnedItems.ContainsKey(seatPositions[i]))
                    {
                        GameObject canvas = GameObject.Find("Canvas");
                        newbrokebar = Instantiate(customerBarOutside, itemPlaces[i].transform.position, Quaternion.identity, canvas.transform);
                        spawnedItems[seatPositions[i]] = newbrokebar;

                        CustomerBarCallTry barScript = newbrokebar.GetComponent<CustomerBarCallTry>();
                        if (barScript != null)
                            barScript.StartPatience();
                    }
                    break;
                }
            }

            if (!seatOccupied && spawnedItems.ContainsKey(seatPositions[i]))
            {
                Destroy(spawnedItems[seatPositions[i]]);
                spawnedItems.Remove(seatPositions[i]);
            }
        }
    }
}