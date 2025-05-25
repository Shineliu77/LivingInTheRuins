using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHit : MonoBehaviour
{
    public MonsterSpawner monsterSpawner; // �Ǫ��ͦ��I�޲z��
    public GameObject monsterPrefab; // �Ǫ��w�s��
    public BrokeProgressAnime brokeProgressAnime; // �����@�[�Ⱥ޲z��  
    public float monsterSpawnChance = 1f; // �Ǫ��ͦ����v

    public float moveSpeed = 10f; // ���ʳt��
    public Transform monsterStartPoint; // �Ǫ���l�ͦ��I
    public Transform monsterLeavePoint; // �Ǫ������I�]�s�W�^

    private Dictionary<GameObject, GameObject> occupiedSpawnPoints = new Dictionary<GameObject, GameObject>(); // �O���w���Ϊ��ͦ��I
    private bool isSpawning = false;
    public GameObject currentMonster = null;
    private bool readyToRespawn = false; // ����O�_��A���ͦ��Ǫ�

    public string FindHaveBrokeProgressAnimeObj;   //��즳�����@�[�ȵ{��������W
    public MonsterState CurrentState = MonsterState.Entrance; //��e���A�ʵe  (���վ� �����Ǽ�)

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
        Debug.Log("MonsterHit ������: " + this.name);
        // �ھګ��w�W�ٴM�����������������


        GameObject BrokeProgressAnime = GameObject.Find(FindHaveBrokeProgressAnimeObj);
        // brokeProgressAnime = GameObject.Find(FindHaveBrokeProgressAnimeObj).GetComponent<BrokeProgressAnime>();

    }

    void Update()
    {
        // �p�G�|���ͦ��Ǫ��B�����׾����B�����l�a�B�ثe�S��L�Ǫ��B�B���\�ͦ�
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
            Debug.LogError("MonsterSpawner �|�����w�I");
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

        // �[�J�I���B�z��
        MonsterClickHandler clickHandler = newMonster.AddComponent<MonsterClickHandler>();
        clickHandler.monsterHit = this;

        yield return StartCoroutine(MoveToTarget(newMonster.transform, spawnPoint.transform.position));
        // �Ǫ����ʵ�����
        CurrentState = MonsterState.Attacking;
        //  �q��NewPlayerTeach�Ǫ��X�{����  (�ȩ�)
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
            //   Debug.Log("��F�ت� - �}�l�����ʵe");
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

                CurrentState = MonsterState.Success;  // �������\
                Debug.Log("�������\�A�}�l���\�ʵe");
            }
        }
    }

    public void MonsterClicked(GameObject monster)
    {
        if (currentMonster == monster)
        {

            CurrentState = MonsterState.Fail;  // �Q�I��
            StartCoroutine(HandleMonsterClick(monster));
        }
    }

    IEnumerator HandleMonsterClick(GameObject monster)
    {
        Debug.Log("�Ǫ��I�� - �}�l���}�ʵe");

        //  ���ʨ����}�I�]�ӫD�ͦ��I�^
        if (monsterLeavePoint != null)
        {
            CurrentState = MonsterState.Leaving;  // �n���}
            yield return StartCoroutine(MoveToTarget(monster.transform, monsterLeavePoint.position));
        }

        MonsterDefeated(monster);
        isSpawning = false;
        readyToRespawn = true;
    }

    public void MonsterDefeated(GameObject defeatedMonster)
    {
        Debug.Log("�����Ǫ��o");
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
        //    new WaitForSeconds(12f);   // ����blender�ʵe���� �Ȧb���I���ɦ���
        //   readyToRespawn = false;
        //   StartCoroutine(SpawnMonster());
        //}
    }
    void OnCollisionEnter2D(Collision2D coll)  //�Ǫ��A���ͦ�����   �����D
    {
        if (brokeProgressAnime != null && brokeProgressAnime.isDamaged && readyToRespawn && coll.gameObject.CompareTag("Red"))
        {
            Debug.Log("�I��Ĳ�o���s�ͦ��I");
            readyToRespawn = false;
            StartCoroutine(SpawnMonster());
        }
    }
}
//}