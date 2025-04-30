using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomOneToThree : MonoBehaviour
{
    public GameObject spriteRendererPrefab; // 主物件（包含子物件）
    public GameObject[] OneToThree; // 存放子物件的 Prefab 陣列

    void Start()
    {
        if (OneToThree == null || OneToThree.Length == 0)
        {
            Debug.LogWarning("OneToThree 陣列為空，請在 Inspector 中添加子物件。");
            return;
        }

        // 確保所有子物件一開始都是隱藏的
        foreach (GameObject obj in OneToThree)
        {
            obj.SetActive(false);
        }

        // 隨機決定顯示幾個（1 到 3）
        int count = Random.Range(1, 4);

        // 用來存放已選擇的子物件索引，確保不重複
        List<int> selectedIndexes = new List<int>();

        for (int i = 0; i < count; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, OneToThree.Length);
            } while (selectedIndexes.Contains(randomIndex)); // 確保不重複

            selectedIndexes.Add(randomIndex);

            // 啟用選中的子物件
            OneToThree[randomIndex].SetActive(true);
        }
    }
}