using UnityEngine;
using System.Collections.Generic;

public class DialogueCollectionUI : MonoBehaviour
{
    [Header("來源腳本")]
    public DialogueManager dialogueManager; // 讀 Excel 的那支
    public DialogueUI dialogueUI;           // 正在顯示對話的 UI

    [Header("合集列表 UI")]
    public Transform contentRoot;           // ScrollView Content
    public GameObject entryPrefab;          // 合集單一項目的 prefab

    private readonly List<GameObject> spawnedEntries = new List<GameObject>();

    void Start()
    {
        // 若沒有手動指定 就試著在同一個物件上找
        if (dialogueManager == null)
            dialogueManager = GetComponent<DialogueManager>();

        if (dialogueUI == null)
            dialogueUI = GetComponent<DialogueUI>();

      
    }

    private void OnEnable()
    {
        // 一開始如果想馬上顯示已看過的台詞 可以先刷新一次
        RefreshCollection();
    }

    /// <summary>
    /// 重新產生整個合集列表
    /// 只顯示 index 小於 currentLine 的對話
    /// </summary>
    public void RefreshCollection()
    {
        if (dialogueManager == null || dialogueUI == null || contentRoot == null || entryPrefab == null)
        {
            Debug.LogWarning("DialogueCollectionUI 設定尚未完成");
            return;
        }

        // 先清掉舊的項目
        foreach (var go in spawnedEntries)
        {
            if (go != null)
                Destroy(go);
        }
        spawnedEntries.Clear();

        int maxIndex = Mathf.Min(dialogueUI.currentLine, dialogueManager.dialogueLines.Count);

        for (int i = 0; i < maxIndex; i++)
        {
            var line = dialogueManager.dialogueLines[i];
            if (line == null)
                continue;

            // 這裡只處理純文字 你如果要把 CG 或 BG 也顯示出來 可以再擴充
            GameObject entryObj = Instantiate(entryPrefab, contentRoot);
            entryObj.SetActive(true);
            var entry = entryObj.GetComponent<DialogueCollectionEntry>();
            if (entry != null)
            {
                entry.Setup(line.speaker, line.content);
            }

            spawnedEntries.Add(entryObj);
        }
    }

    /// <summary>
    /// 只新增最新一行 已經看過的台詞
    /// 可以在每次 ShowNextLine 後呼叫 比較省
    /// </summary>
    public void AddLatestLine()
    {
        if (dialogueManager == null || dialogueUI == null || contentRoot == null || entryPrefab == null)
            return;

        int index = dialogueUI.currentLine - 1; // currentLine 已經+1 所以回到上一行

        if (index < 0 || index >= dialogueManager.dialogueLines.Count)
            return;

        var line = dialogueManager.dialogueLines[index];
        if (line == null)
            return;

        GameObject entryObj = Instantiate(entryPrefab, contentRoot);
        var entry = entryObj.GetComponent<DialogueCollectionEntry>();
        if (entry != null)
        {
            entry.Setup(line.speaker, line.content);
        }

        spawnedEntries.Add(entryObj);
    }
}
