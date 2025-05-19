using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class liveTwoDchangeImageIiquid : MonoBehaviour
{

    // 碰撞後要變換的圖片陣列，索引需與 Foropener 中的 currentImageIndex 對應
    public Sprite[] changeSprites; //碰撞後會換得圖

    private SpriteRenderer spriteRenderer;

    public LiquidPopOut liquidPopOut; // 呼叫 LiquidPopOut 程式
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // 獲取 SpriteRenderer 組件
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.gameObject;

        // 確認碰撞的物件是 blueIiquid 時的處理
        if (collision.gameObject.CompareTag("blueIiquid"))
        {
            Debug.Log("碰到 blueIiquid！");
            // 當 Foropener.currentImageIndex 為 0 時才更換圖片

            if (Foropener.currentImageIndex == 0 && changeSprites[0])
            {
                if (changeSprites.Length > 0)
                {
                    spriteRenderer.sprite = changeSprites[0]; // 變更為 changeSprites[0]
                    Debug.Log("圖片已更換為 changeSprites[0]");
                }
                else
                {
                    Debug.LogWarning("changeSprites 陣列為空，無法變更圖片！");
                }

                //  刪除生成液體
                if (liquidPopOut != null)
                {
                    Debug.Log("液體碰到 blueIiquid，通知控制器銷毀！");

                    liquidPopOut.LiquidDefeated(collidedObject);
                }
            }
        }

        // 確認碰撞的物件是 yellowIiquid 時的處理
        if (collision.gameObject.CompareTag("yellowIiquid"))
        {
            Debug.Log("碰到 yellowIiquid！");

            // 當 Foropener.currentImageIndex 為 1 時才更換圖片
            if (Foropener.currentImageIndex == 1 && changeSprites[1])
            {
                if (changeSprites.Length > 1)
                {
                    spriteRenderer.sprite = changeSprites[1]; // 變更為 changeSprites[1]
                    Debug.Log("圖片已更換為 changeSprites[1]");
                }
                else
                {
                    Debug.LogWarning("changeSprites 陣列長度不足，無法變更圖片！");
                }


                if (liquidPopOut != null)
                {
                    Debug.Log("液體碰到 yellowIiquid，通知控制器銷毀！");
                    //  刪除生成液體
                    liquidPopOut.LiquidDefeated(collidedObject);
                }
            }
        }

        // 確認碰撞的物件是 greenIiquid 時的處理
        if (collision.gameObject.CompareTag("greenIiquid"))
        {
            Debug.Log("碰到 greenIiquid！");
            // 當 Foropener.currentImageIndex 為 2 時才更換圖片
            if (Foropener.currentImageIndex == 2 && changeSprites[2])
            {
                if (changeSprites.Length > 2)
                {
                    spriteRenderer.sprite = changeSprites[2]; // 變更為 changeSprites[5]
                    Debug.Log("圖片已更換為 changeSprites[2]");
                }
                else
                {
                    Debug.LogWarning("changeSprites 陣列長度不足，無法變更圖片！");
                }


                if (liquidPopOut != null)
                {
                    Debug.Log("液體碰到 greenIiquid，通知控制器銷毀！");
                    //  刪除生成液體
                    liquidPopOut.LiquidDefeated(collidedObject);
                }
            }

            // 確認碰撞的物件是 redIiquid 時的處理
            if (collision.gameObject.CompareTag("redIiquid"))
            {
                Debug.Log("碰到 redIiquid！");
                // 當 Foropener.currentImageIndex 為 3 時才更換圖片
                if (Foropener.currentImageIndex == 3 && changeSprites[3])
                {
                    if (changeSprites.Length > 3)
                    {
                        spriteRenderer.sprite = changeSprites[3]; // 變更為 changeSprites[2]
                        Debug.Log("圖片已更換為 changeSprites[3]");
                    }
                    else
                    {
                        Debug.LogWarning("changeSprites 陣列長度不足，無法變更圖片！");
                    }

                    //  刪除生成液體
                    if (liquidPopOut != null)
                    {
                        Debug.Log("液體碰到 redIiquid，通知控制器銷毀！");

                        liquidPopOut.LiquidDefeated(collidedObject); //  正確呼叫控制器的方法
                    }
                }
            }
        }
    }
}