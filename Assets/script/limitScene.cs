using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class limitScene : MonoBehaviour
{
    public float rangeMin = -9.4f; // �ˬd�d��̤p��
    public float rangeMax = 9.4f;  // �ˬd�d��̤j��

    // ���o�d�򤺪��ײz����ƶq
    public int GetFixedItemInRange()
    {
        FixedItemTwoD[] allFixedItems = FindObjectsOfType<FixedItemTwoD>(); // ����Ҧ��ײz����
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

    // �O�_���\�ͦ��s���ײz����
    public bool CanSpawnManster()
    {
        return GetFixedItemInRange() < 3; // �T�O�d�򤺪��ײz���󤣶W�L 3
        
    }


    public int GetMansterInRange()
    {
        MonsterHit[] allManster = FindObjectsOfType<MonsterHit>(); // ����Ҧ��ײz����
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

    // �O�_���\�ͦ��s���ײz����
    public bool CanSpawnFixedItem()
    {
        return GetFixedItemInRange() < 1; // �T�O�d�򤺪��ײz���󤣶W�L 1
    }
}


