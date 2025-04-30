using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class limitScene : MonoBehaviour
{
    public float rangeMin = -9.4f; // 檢查範圍最小值
    public float rangeMax = 9.4f;  // 檢查範圍最大值

    // 取得範圍內的修理物件數量
    public int GetFixedItemInRange()
    {
        FixedItemTwoD[] allFixedItems = FindObjectsOfType<FixedItemTwoD>(); // 獲取所有修理物件
        int countInRange = 0;

        foreach (FixedItemTwoD fixedItem in allFixedItems)
        {
            float x = fixedItem.transform.position.x;
            if (x >= rangeMin && x <= rangeMax)
            {
                countInRange++;
            }
        }
        return countInRange;
    }

    // 是否允許生成新的修理物件
    public bool CanSpawnManster()
    {
        return GetFixedItemInRange() < 3; // 確保範圍內的修理物件不超過 3
        
    }


    public int GetMansterInRange()
    {
        MonsterHit[] allManster = FindObjectsOfType<MonsterHit>(); // 獲取所有修理物件
        int countInRange = 0;

        foreach (MonsterHit Manster in allManster)
        {
            float x = Manster.transform.position.x;
            if (x >= rangeMin && x <= rangeMax)
            {
                countInRange++;
            }
        }
        return countInRange;
    }

    // 是否允許生成新的修理物件
    public bool CanSpawnFixedItem()
    {
        return GetFixedItemInRange() < 1; // 確保範圍內的修理物件不超過 1
    }
}


