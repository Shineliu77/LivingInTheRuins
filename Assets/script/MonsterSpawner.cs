using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject[] spawnPoints; // �Ǫ��ͦ��I

    public GameObject GetRandomSpawnPoint(HashSet<GameObject> occupiedPoints)
    {
        List<GameObject> availablePoints = new List<GameObject>();

        // ��X���Q���Ϊ��ͦ��I
        foreach (var point in spawnPoints)
        {
            if (!occupiedPoints.Contains(point))
            {
                availablePoints.Add(point);
            }
        }

        if (availablePoints.Count == 0)
        {
            Debug.Log("�S���i�Ϊ��Ǫ��ͦ��I�I");
            return null;
        }

        return availablePoints[Random.Range(0, availablePoints.Count)];
    }
}