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
            Debug.LogError("MonsterSpawner �|�����w�I");
            yield break;
        }

        // ���o���Q���Ϊ��H���ͦ��I
        GameObject spawnPoint = monsterSpawner.GetRandomSpawnPoint(new HashSet<GameObject>(occupiedSpawnPoints.Keys));
        if (spawnPoint == null) yield break; // �L�i�Υͦ��I�h���ͦ�

        // �T�O�ӥͦ��I�S���h�l���Ǫ��]�z�פW���Ӥ��|�o�͡^
        if (occupiedSpawnPoints.ContainsKey(spawnPoint) && occupiedSpawnPoints[spawnPoint] != null)
        {
            Destroy(occupiedSpawnPoints[spawnPoint]); // �R���©Ǫ�
        }

        // �ͦ��s�Ǫ�
        GameObject newMonster = Instantiate(monsterPrefab, spawnPoint.transform.position, Quaternion.identity);
        occupiedSpawnPoints[spawnPoint] = newMonster; // �O���ӥͦ��I���Ǫ�

        yield return new WaitForSeconds(15f); // 15 ������
        MonsterAttack();
    }

    void MonsterAttack()
    {
        if (brokeProgressAnime != null && brokeProgressAnime.FixItemMachine != null)
        {
            Machine machineScript = brokeProgressAnime.FixItemMachine.GetComponent<Machine>();
            if (machineScript != null)
            {
                machineScript.HP -= machineScript.HPMax * 0.1f; // �y�� 10% �ˮ`
                brokeProgressAnime.Refreshbrokebar();
            }
        }
    }

    public void MonsterDefeated(GameObject defeatedMonster)
    {
        // �q���ΦC�������Ǫ�
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