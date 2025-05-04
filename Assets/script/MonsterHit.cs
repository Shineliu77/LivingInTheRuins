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
    private bool isSpawning = false; //是否有生成怪物
    private GameObject currentMonster = null; // 追蹤目前生成的怪物


    void Update()
    {
        //如果 沒有怪物生成  brokeProgressAnime 欄位有掛載並且有減損  怪物生成點有設
        if (!isSpawning && brokeProgressAnime != null && brokeProgressAnime.isDamaged && occupiedSpawnPoints.Count == 0)
        {
            if (Random.value < monsterSpawnChance)
            {
                StartCoroutine(SpawnMonster());
            }
        }
    }
    void Awake()
    {
        Debug.Log("MonsterHit 掛載於: " + this.name);
    }
    IEnumerator SpawnMonster()  //怪物生成條件
    {
        isSpawning = true;

        if (monsterSpawner == null)
        {
            Debug.LogError("MonsterSpawner 尚未指定！");
            yield break;  //無可用生成點則不生成
        }

        // 取得未被佔用的隨機生成點
        GameObject spawnPoint = monsterSpawner.GetRandomSpawnPoint(new HashSet<GameObject>(occupiedSpawnPoints.Keys));
        if (spawnPoint == null)
        {
            isSpawning = false;
            yield break;
        }

        // 確保該生成點沒有多餘的怪物（理論上應該不會發生）
        if (occupiedSpawnPoints.ContainsKey(spawnPoint) && occupiedSpawnPoints[spawnPoint] != null)
        {
            Destroy(occupiedSpawnPoints[spawnPoint]);
        }

        // 生成新怪物
        GameObject newMonster = Instantiate(monsterPrefab, spawnPoint.transform.position, Quaternion.identity);
        newMonster.tag = "monster";
        occupiedSpawnPoints[spawnPoint] = newMonster; // 記錄該生成點有怪物
        currentMonster = newMonster; // 設定目前怪物
        newMonster.tag = "monster";   //設定tag

        MonsterClickHandler clickHandler = newMonster.AddComponent<MonsterClickHandler>();  // 加入怪物點擊事件處理器
        clickHandler.monsterHit = this;

        yield return new WaitForSeconds(15f);  //15秒後攻擊

        // 只有當怪物還存在才進行攻擊
        if (currentMonster != null)
        {
            MonsterAttack();
        }
    }

    void MonsterAttack()  //怪物攻擊條件
    {
        if (brokeProgressAnime != null && brokeProgressAnime.FixItemMachine != null)  //有設brokeProgressAnime 欄位有掛載並且有在維修物品
        {
            Machine machineScript = brokeProgressAnime.FixItemMachine.GetComponent<Machine>();
            if (machineScript != null)
            {
                machineScript.HP -= machineScript.HPMax * 0.1f;  // 造成 10% 傷害
                brokeProgressAnime.Refreshbrokebar();
            }
        }
    }

    public void MonsterClicked(GameObject monster)  // 怪物被點擊後呼叫
    {
        if (currentMonster == monster)
        {
            Debug.Log("被點擊");
            StartCoroutine(HandleMonsterClick(monster));


        }

        IEnumerator HandleMonsterClick(GameObject monster)  // 點擊處理流程
        {
            Debug.Log("要銷毀囉");
            MonsterDefeated(monster);
            isSpawning = false;
            yield return new WaitForSeconds(0.1f);

            if (brokeProgressAnime != null && brokeProgressAnime.isDamaged)  // 如果符合條件可再生成
            {
                StartCoroutine(SpawnMonster());

            }
        }
    }

    public void MonsterDefeated(GameObject defeatedMonster)   // 移除怪物記錄與刪除怪物物件
    {
        Debug.Log("移除怪物囉");
        GameObject keyToRemove = null;

        foreach (var kvp in occupiedSpawnPoints)
        {
            Debug.Log("怪物掰掰囉");
            if (kvp.Value == defeatedMonster)
            {
                Debug.Log("再見掰掰囉");
                keyToRemove = kvp.Key;
                break;
            }
        }

        if (keyToRemove != null)
        {
            Debug.Log("怪物");
            occupiedSpawnPoints.Remove(keyToRemove);
        }

        if (currentMonster == defeatedMonster)
        {
            Debug.Log("怪物ok");
            currentMonster = null; // 清空目前怪物追蹤，阻止攻擊
        }
        Debug.Log("怪物die");
        Destroy(defeatedMonster);
    }
}