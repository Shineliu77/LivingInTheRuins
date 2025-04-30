using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHit : MonoBehaviour
{
    public MonsterSpawner monsterSpawner; // 連接到怪物生成點管理器
    public GameObject monsterPrefab; // 怪物預製體
    public BrokeProgressAnime brokeProgressAnime; // 機器耐久值管理器
    public float monsterSpawnChance = 1f; // 生成機率
    private Dictionary<GameObject, GameObject> occupiedSpawnPoints = new Dictionary<GameObject, GameObject>(); // 記錄每個生成點的怪物
    private MonsterHit monsterHit;
    void Update()
    {
        if (brokeProgressAnime != null && brokeProgressAnime.isDamaged && occupiedSpawnPoints.Count == 0)
        {
            if (Random.value < monsterSpawnChance)
            {
                StartCoroutine(SpawnMonster());
            }
        }
    }

    IEnumerator SpawnMonster()
    {
        if (monsterSpawner == null)
        {
            Debug.LogError("MonsterSpawner 尚未指定！");
            yield break;
        }

        // 取得未被佔用的隨機生成點
        GameObject spawnPoint = monsterSpawner.GetRandomSpawnPoint(new HashSet<GameObject>(occupiedSpawnPoints.Keys));
        if (spawnPoint == null) yield break; // 無可用生成點則不生成

        // 確保該生成點沒有多餘的怪物（理論上應該不會發生）
        if (occupiedSpawnPoints.ContainsKey(spawnPoint) && occupiedSpawnPoints[spawnPoint] != null)
        {
            Destroy(occupiedSpawnPoints[spawnPoint]); // 刪除舊怪物
        }

        // 生成新怪物
        GameObject newMonster = Instantiate(monsterPrefab, spawnPoint.transform.position, Quaternion.identity);
        occupiedSpawnPoints[spawnPoint] = newMonster; // 記錄該生成點的怪物

        yield return new WaitForSeconds(15f); // 15 秒後攻擊
        MonsterAttack();
    }

    void MonsterAttack()
    {
        if (brokeProgressAnime != null && brokeProgressAnime.FixItemMachine != null)
        {
            Machine machineScript = brokeProgressAnime.FixItemMachine.GetComponent<Machine>();
            if (machineScript != null)
            {
                machineScript.HP -= machineScript.HPMax * 0.1f; // 造成 10% 傷害
                brokeProgressAnime.Refreshbrokebar();
            }
        }
    }

    public void MonsterDefeated(GameObject defeatedMonster)
    {
        // 從佔用列表中移除怪物
        GameObject keyToRemove = null;
        foreach (var kvp in occupiedSpawnPoints)
        {
            if (kvp.Value == defeatedMonster)
            {
                keyToRemove = kvp.Key;
                break;
            }
        }

        if (keyToRemove != null)
        {
            occupiedSpawnPoints.Remove(keyToRemove);
        }

        Destroy(defeatedMonster);
    }
    void OnMouseDown()
    {
        if (monsterHit != null)
        {
            monsterHit.MonsterDefeated(gameObject);
        
        }
    }
}