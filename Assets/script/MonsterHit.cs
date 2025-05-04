using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHit : MonoBehaviour
{
    public MonsterSpawner monsterSpawner; // �s����Ǫ��ͦ��I�޲z��
    public GameObject monsterPrefab; // �Ǫ��w�s��
    public BrokeProgressAnime brokeProgressAnime; // �����@�[�Ⱥ޲z��
    public float monsterSpawnChance = 1f; // �ͦ����v
    private Dictionary<GameObject, GameObject> occupiedSpawnPoints = new Dictionary<GameObject, GameObject>(); // �O���C�ӥͦ��I���Ǫ�
    private bool isSpawning = false; //�O�_���ͦ��Ǫ�
    private GameObject currentMonster = null; // �l�ܥثe�ͦ����Ǫ�


    void Update()
    {
        //�p�G �S���Ǫ��ͦ�  brokeProgressAnime ��즳�����åB����l  �Ǫ��ͦ��I���]
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
        Debug.Log("MonsterHit ������: " + this.name);
    }
    IEnumerator SpawnMonster()  //�Ǫ��ͦ�����
    {
        isSpawning = true;

        if (monsterSpawner == null)
        {
            Debug.LogError("MonsterSpawner �|�����w�I");
            yield break;  //�L�i�Υͦ��I�h���ͦ�
        }

        // ���o���Q���Ϊ��H���ͦ��I
        GameObject spawnPoint = monsterSpawner.GetRandomSpawnPoint(new HashSet<GameObject>(occupiedSpawnPoints.Keys));
        if (spawnPoint == null)
        {
            isSpawning = false;
            yield break;
        }

        // �T�O�ӥͦ��I�S���h�l���Ǫ��]�z�פW���Ӥ��|�o�͡^
        if (occupiedSpawnPoints.ContainsKey(spawnPoint) && occupiedSpawnPoints[spawnPoint] != null)
        {
            Destroy(occupiedSpawnPoints[spawnPoint]);
        }

        // �ͦ��s�Ǫ�
        GameObject newMonster = Instantiate(monsterPrefab, spawnPoint.transform.position, Quaternion.identity);
        newMonster.tag = "monster";
        occupiedSpawnPoints[spawnPoint] = newMonster; // �O���ӥͦ��I���Ǫ�
        currentMonster = newMonster; // �]�w�ثe�Ǫ�
        newMonster.tag = "monster";   //�]�wtag

        MonsterClickHandler clickHandler = newMonster.AddComponent<MonsterClickHandler>();  // �[�J�Ǫ��I���ƥ�B�z��
        clickHandler.monsterHit = this;

        yield return new WaitForSeconds(15f);  //15������

        // �u����Ǫ��٦s�b�~�i�����
        if (currentMonster != null)
        {
            MonsterAttack();
        }
    }

    void MonsterAttack()  //�Ǫ���������
    {
        if (brokeProgressAnime != null && brokeProgressAnime.FixItemMachine != null)  //���]brokeProgressAnime ��즳�����åB���b���ת��~
        {
            Machine machineScript = brokeProgressAnime.FixItemMachine.GetComponent<Machine>();
            if (machineScript != null)
            {
                machineScript.HP -= machineScript.HPMax * 0.1f;  // �y�� 10% �ˮ`
                brokeProgressAnime.Refreshbrokebar();
            }
        }
    }

    public void MonsterClicked(GameObject monster)  // �Ǫ��Q�I����I�s
    {
        if (currentMonster == monster)
        {
            Debug.Log("�Q�I��");
            StartCoroutine(HandleMonsterClick(monster));


        }

        IEnumerator HandleMonsterClick(GameObject monster)  // �I���B�z�y�{
        {
            Debug.Log("�n�P���o");
            MonsterDefeated(monster);
            isSpawning = false;
            yield return new WaitForSeconds(0.1f);

            if (brokeProgressAnime != null && brokeProgressAnime.isDamaged)  // �p�G�ŦX����i�A�ͦ�
            {
                StartCoroutine(SpawnMonster());

            }
        }
    }

    public void MonsterDefeated(GameObject defeatedMonster)   // �����Ǫ��O���P�R���Ǫ�����
    {
        Debug.Log("�����Ǫ��o");
        GameObject keyToRemove = null;

        foreach (var kvp in occupiedSpawnPoints)
        {
            Debug.Log("�Ǫ��T�T�o");
            if (kvp.Value == defeatedMonster)
            {
                Debug.Log("�A���T�T�o");
                keyToRemove = kvp.Key;
                break;
            }
        }

        if (keyToRemove != null)
        {
            Debug.Log("�Ǫ�");
            occupiedSpawnPoints.Remove(keyToRemove);
        }

        if (currentMonster == defeatedMonster)
        {
            Debug.Log("�Ǫ�ok");
            currentMonster = null; // �M�ťثe�Ǫ��l�ܡA�������
        }
        Debug.Log("�Ǫ�die");
        Destroy(defeatedMonster);
    }
}