using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidPopOut : MonoBehaviour
{
    public MonsterSpawner monsterSpawner;
    public GameObject[] LiquidPrefab; // ���ǡGBlue, Red, Green, Yellow
    public BlenderAnime blenderAnime;

    private Dictionary<GameObject, GameObject> occupiedSpawnPoints = new Dictionary<GameObject, GameObject>();

    void Update()
    {
        if (blenderAnime != null && blenderAnime.animationFinished)
        {
            // �bblender�ʵe������Ĳ�o
            blenderAnime.animationFinished = false; // ���m�ͦ�
            StartCoroutine(SpawnLiquid(blenderAnime.finishedColor));
        }
    }

    IEnumerator SpawnLiquid(string color)
    {
        if (monsterSpawner == null || LiquidPrefab.Length < 4)
        {
            Debug.LogError("monsterSpawner �|�����w�βG��Prefab�����I");
            yield break;
        }

        GameObject spawnPoint = monsterSpawner.GetRandomSpawnPoint(new HashSet<GameObject>(occupiedSpawnPoints.Keys));
        if (spawnPoint == null) yield break;

        int index = ColorToIndex(color);
        if (index == -1)
        {
            Debug.LogError("�L���C��G" + color);
            yield break;
        }

        GameObject newLiquid = Instantiate(LiquidPrefab[index], spawnPoint.transform.position, Quaternion.identity);

        occupiedSpawnPoints[spawnPoint] = newLiquid;

        yield return new WaitForSeconds(0.1f); // �y���@�U�A���m�ʵe���A

        //  �M���ʵe���A
        blenderAnime.ResetFinishedState();

        // yield return null;
    }

    private int ColorToIndex(string color)
    {
        //�̸I��Ĳ�o�ͦ������G��
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
        //�R���ͦ�����覡
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