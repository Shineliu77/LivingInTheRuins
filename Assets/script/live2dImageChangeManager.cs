using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class live2dImageChangeManager : MonoBehaviour
{
    public static live2dImageChangeManager Instance { get; private set; }

    // 儲存所有變換的圖片
    public Sprite[] commonChangeSprites; // 給換圖程式1使用的
    public Sprite[] liquidChangeSprites; // 給換圖程式2使用的

    public int currentImageIndex = 0; // 統一管理 currentImageIndex

    private void Awake()
    {
        // Singleton 設計模式，確保只有一個實例
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
