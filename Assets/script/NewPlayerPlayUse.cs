using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerPlayUse : MonoBehaviour
{
    public GameObject spriteRenderer; // 顯示圖片的 SpriteRenderer
    public Sprite[] OpenerChangeImage; // 可選擇的圖片陣列
    public static int currentImageIndex = -1; // 記錄當前圖片的索引（用於碰撞後更換圖片）
    public GameObject First;   // 第一次顯示的物件
    public GameObject Second;  // 第二次顯示的物件
    public GameObject Third;   // 永遠不會顯示的物件

    private int showCount = 0; // 計數器，記錄顯示了幾次

    void Start()
    {
        // 一開始隱藏所有物件
        First.SetActive(false);
        Second.SetActive(false);
        Third.SetActive(false);

        // 啟動協程開始偵測場景中是否有 fixitem
        StartCoroutine(CheckForFixItem());
    }

    // 協程：持續檢查場景中是否有標籤為 "fixitem" 的物件
    IEnumerator CheckForFixItem()
    {
        while (true)
        {
            // 找出所有 tag 為 "fixeditemOpen" 的物件
            GameObject[] fixItems = GameObject.FindGameObjectsWithTag("fixeditemOpen");
            int count = fixItems.Length;

            // 每次都先關閉 First 和 Second（避免重複顯示）
            First.SetActive(false);
            Second.SetActive(false);

            // 根據數量顯示對應的 GameObject
            if (count == 2)
            {
                currentImageIndex = 0;

                // spriteRenderer.sprite = OpenerChangeImage[currentImageIndex];
                First.SetActive(true);
                Debug.Log("場景中有 1 個 fixeditemOpen，顯示 First");

            }

            else if (count == 3)
            {
                currentImageIndex = 1;
                //spriteRenderer.sprite = OpenerChangeImage[currentImageIndex]; // 顯示圖片
                Second.SetActive(true);
                Debug.Log("場景中有 2 個 fixeditemOpen，顯示 Second");

            }
            else
            {
                // 其餘狀況（0個或超過2個）都不顯示
                Debug.Log($"場景中有 {count} 個 fixeditemOpen，沒有對應顯示物件");
            }

            // 每一幀持續檢查（你可以改成每0.2秒以減少效能消耗）
            yield return null;
        }
    }
}
