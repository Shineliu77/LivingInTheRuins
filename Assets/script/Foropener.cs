using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foropener : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // 顯示圖片的 SpriteRenderer
    public Sprite[] OpenerChangeImage;    // 可隨機選擇的圖片陣列
    public static int currentImageIndex = -1; // 記錄當前圖片的索引（用於碰撞後更換圖片）

    void Start()
    {
        // 檢查 OpenerChangeImage 是否有圖片
        if (OpenerChangeImage != null && OpenerChangeImage.Length > 0)
        {
            // 隨機選取一張圖片
            currentImageIndex = Random.Range(0, OpenerChangeImage.Length);
            spriteRenderer.sprite = OpenerChangeImage[currentImageIndex]; // 顯示圖片
        }
        else
        {
            Debug.LogWarning("OpenerChangeImage 陣列為空，請在 Inspector 中添加圖片。");
        }
    }
}
