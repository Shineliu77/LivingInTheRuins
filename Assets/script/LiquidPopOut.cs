using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidPopOut : MonoBehaviour
{
    public MonsterSpawner monsterSpawner;
    public GameObject[] LiquidPrefab; // 順序：Blue, Red, Green, Yellow
    public BlenderAnime blenderAnime;

    private Dictionary<GameObject, GameObject> occupiedSpawnPoints = new Dictionary<GameObject, GameObject>();

    void Update()
    {
        if (blenderAnime != null && blenderAnime.animationFinished)
        {
            // 在blender動畫結束時觸發
            blenderAnime.animationFinished = false; // 重置生成
            StartCoroutine(SpawnLiquid(blenderAnime.finishedColor));
        }
    }

    IEnumerator SpawnLiquid(string color)
    {
        if (monsterSpawner == null || LiquidPrefab.Length < 4)
        {
            Debug.LogError("monsterSpawner 尚未指定或液體Prefab不足！");
            yield break;
        }

        GameObject spawnPoint = monsterSpawner.GetRandomSpawnPoint(new HashSet<GameObject>(occupiedSpawnPoints.Keys));
        if (spawnPoint == null) yield break;

        int index = ColorToIndex(color);
        if (index == -1)
        {
            Debug.LogError("無效顏色：" + color);
            yield break;
        }

        GameObject newLiquid = Instantiate(LiquidPrefab[index], spawnPoint.transform.position, Quaternion.identity);

        occupiedSpawnPoints[spawnPoint] = newLiquid;

        yield return new WaitForSeconds(0.1f); // 稍等一下再重置動畫狀態

        //  清除動畫狀態
        blenderAnime.ResetFinishedState();

        // yield return null;
    }

    private int ColorToIndex(string color)
    {
        //依碰撞觸發生成對應液體
        switch (color)
        {
            case "Blue": return 0;
            case "Red": return 1;
            case "Green": return 2;
            case "Yellow": return 3;
            default: return -1;
        }
    }

    public void LiquidDefeated(GameObject defeatedLiquid)
    {
        //刪除生成物件方式
        GameObject keyToRemove = null;

        foreach (var kvp in occupiedSpawnPoints)
        {
            if (kvp.Value == defeatedLiquid)
            {
                keyToRemove = kvp.Key;
                break;
            }
        }

        if (keyToRemove != null)
        {
            occupiedSpawnPoints.Remove(keyToRemove);
        }

        Destroy(defeatedLiquid);
    }


}