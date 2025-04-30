using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject[] spawnPoints; // 怪物生成點

    public GameObject GetRandomSpawnPoint(HashSet<GameObject> occupiedPoints)
    {
        List<GameObject> availablePoints = new List<GameObject>();

        // 找出未被佔用的生成點
        foreach (var point in spawnPoints)
        {
            if (!occupiedPoints.Contains(point))
            {
                availablePoints.Add(point);
            }
        }

        if (availablePoints.Count == 0)
        {
            Debug.Log("沒有可用的怪物生成點！");
            return null;
        }

        return availablePoints[Random.Range(0, availablePoints.Count)];
    }
}