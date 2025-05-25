using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHit : MonoBehaviour
{
    public MonsterSpawner monsterSpawner; // 怪物生成點管理器
    public GameObject monsterPrefab; // 怪物預製體
    public BrokeProgressAnime brokeProgressAnime; // 機器耐久值管理器  
    public float monsterSpawnChance = 1f; // 怪物生成機率

    public float moveSpeed = 10f; // 移動速度
    public Transform monsterStartPoint; // 怪物初始生成點
    public Transform monsterLeavePoint; // 怪物離場點（新增）

    private Dictionary<GameObject, GameObject> occupiedSpawnPoints = new Dictionary<GameObject, GameObject>(); // 記錄已佔用的生成點
    private bool isSpawning = false;
    public GameObject currentMonster = null;
    private bool readyToRespawn = false; // 控制是否能再次生成怪物

    public string FindHaveBrokeProgressAnimeObj;   //找到有機器耐久值程式的物件名
    public MonsterState CurrentState = MonsterState.Entrance; //當前狀態動畫  (不調整 按順序播)

    public enum MonsterState
    {
        Entrance,
        Attacking,
        Success,
        Fail,
        Leaving
    }
    void Awake()
    {
        Debug.Log("MonsterHit 掛載於: " + this.name);
        // 根據指定名稱尋找場景中的機器物件


        GameObject BrokeProgressAnime = GameObject.Find(FindHaveBrokeProgressAnimeObj);
        // brokeProgressAnime = GameObject.Find(FindHaveBrokeProgressAnimeObj).GetComponent<BrokeProgressAnime>();

    }

    void Update()
    {
        // 如果尚未生成怪物、有維修機器且機器損壞、目前沒其他怪物、且允許生成
        if (!isSpawning && brokeProgressAnime.isDamaged && occupiedSpawnPoints.Count == 0 && !readyToRespawn)
        {

            if (Random.value < monsterSpawnChance)
            {
                StartCoroutine(SpawnMonster());
            }
        }
    }

    IEnumerator SpawnMonster()
    {
        isSpawning = true;

        if (monsterSpawner == null)
        {
            Debug.LogError("MonsterSpawner 尚未指定！");
            yield break;
        }

        GameObject spawnPoint = monsterSpawner.GetRandomSpawnPoint(new HashSet<GameObject>(occupiedSpawnPoints.Keys));
        if (spawnPoint == null)
        {
            isSpawning = false;
            yield break;
        }

        GameObject newMonster = Instantiate(monsterPrefab, monsterStartPoint.position, Quaternion.identity);
        newMonster.tag = "monster";

        occupiedSpawnPoints[spawnPoint] = newMonster;
        currentMonster = newMonster;

        // 加入點擊處理器
        MonsterClickHandler clickHandler = newMonster.AddComponent<MonsterClickHandler>();
        clickHandler.monsterHit = this;

        yield return StartCoroutine(MoveToTarget(newMonster.transform, spawnPoint.transform.position));
        // 怪物移動結束後
        CurrentState = MonsterState.Attacking;
        //  通知NewPlayerTeach怪物出現攻擊  (暫放)
        NewPlayerTeach teachScript = FindObjectOfType<NewPlayerTeach>();
        if (teachScript != null)
        {
            teachScript.IsmonsterAttackMachine();
        }
        yield return new WaitForSeconds(15f);
        if (currentMonster != null)
        {
            MonsterAttack();
        }
    }

    IEnumerator MoveToTarget(Transform monsterTransform, Vector3 targetPosition)
    {
        while (Vector3.Distance(monsterTransform.position, targetPosition) > 0.1f)
        {
            monsterTransform.position = Vector3.MoveTowards(
                monsterTransform.position,
                targetPosition,
                moveSpeed * Time.deltaTime);
            yield return null;
            //   Debug.Log("抵達目的 - 開始攻擊動畫");
        }
    }

    void MonsterAttack()
    {
        if (brokeProgressAnime != null && brokeProgressAnime.FixItemMachine != null)
        {
            Machine machineScript = brokeProgressAnime.FixItemMachine.GetComponent<Machine>();
            if (machineScript != null)
            {
                machineScript.HP -= machineScript.HPMax * 0.1f;
                brokeProgressAnime.Refreshbrokebar();

                CurrentState = MonsterState.Success;  // 攻擊成功
                Debug.Log("攻擊成功，開始成功動畫");
            }
        }
    }

    public void MonsterClicked(GameObject monster)
    {
        if (currentMonster == monster)
        {

            CurrentState = MonsterState.Fail;  // 被點擊
            StartCoroutine(HandleMonsterClick(monster));
        }
    }

    IEnumerator HandleMonsterClick(GameObject monster)
    {
        Debug.Log("怪物點擊 - 開始離開動畫");

        //  移動到離開點（而非生成點）
        if (monsterLeavePoint != null)
        {
            CurrentState = MonsterState.Leaving;  // 要離開
            yield return StartCoroutine(MoveToTarget(monster.transform, monsterLeavePoint.position));
        }

        MonsterDefeated(monster);
        isSpawning = false;
        readyToRespawn = true;
    }

    public void MonsterDefeated(GameObject defeatedMonster)
    {
        Debug.Log("移除怪物囉");
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

        if (currentMonster == defeatedMonster)
        {
            currentMonster = null;
        }

        Destroy(defeatedMonster);


        // if (brokeProgressAnime != null && brokeProgressAnime.isDamaged)
        // {
        //    new WaitForSeconds(12f);   // 等待blender動畫結束 僅在有碰撞時有效
        //   readyToRespawn = false;
        //   StartCoroutine(SpawnMonster());
        //}
    }
    void OnCollisionEnter2D(Collision2D coll)  //怪物再次生成條件   有問題
    {
        if (brokeProgressAnime != null && brokeProgressAnime.isDamaged && readyToRespawn && coll.gameObject.CompareTag("Red"))
        {
            Debug.Log("碰撞觸發重新生成！");
            readyToRespawn = false;
            StartCoroutine(SpawnMonster());
        }
    }
}
//}