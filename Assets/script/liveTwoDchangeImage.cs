using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveTwoDChangeImage : MonoBehaviour
{
    // 碰撞後要變換的圖片陣列，索引需與 Foropener 中的 currentImageIndex 對應
    public Sprite[] changeSprites;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // 獲取 SpriteRenderer 組件
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Application.loadedLevelName == "TeachGame")
        {
            // 確認碰撞的物件是 brokecircle 時的處理
            if (collision.gameObject.CompareTag("brokecircle"))
            {
                Debug.Log("碰到 brokecircle！");
                // 當 Foropener.currentImageIndex 為 0 時才更換圖片

                if (changeSprites.Length > 0)
                {
                    spriteRenderer.sprite = changeSprites[0]; // 變更為 changeSprites[0]
                    Debug.Log("圖片已更換為 changeSprites[0]");

                    /* NewPlayerTeach teachScript = FindObjectOfType<NewPlayerTeach>(); //僅在碰撞到 brokecircle 才開啟教學
                     if (teachScript != null)
                     {
                         teachScript.IsAfterChangeImage();
                     }*/
                    transform.parent.GetComponent<SetIteamOpenObj>().ResetSize();
                    if (Application.loadedLevelName == "TeachGame")
                    {
                        FindObjectOfType<TeachGM>().OpenTeach4();
                    }
                    else
                    {
                        if (transform.parent.GetComponent<DraggableReturn2D>())
                        {
                            transform.parent.GetComponent<DraggableReturn2D>().enabled = true;
                        }
                    }

                }
                else
                {
                    Debug.LogWarning("changeSprites 陣列為空，無法變更圖片！");
                }
            }
        }

        if (Application.loadedLevelName == "FirstGame")
        {
            SetIteamOpenObj obj = GetComponentInParent<SetIteamOpenObj>();
            if (collision.gameObject.CompareTag("brokecircle"))
            {
                Debug.Log("碰到 brokecircle！");
                // 當 ProcessorImage 為 1 時才更換圖片

                if (obj.currentProcessorIndex == 0)  //改這邊
                {
                    if (changeSprites.Length > 0)
                    {
                        spriteRenderer.sprite = changeSprites[0]; // 變更為 changeSprites[1]
                        transform.parent.GetComponent<SetIteamOpenObj>().ResetSize();

                        if (transform.parent.GetComponent<DraggableReturn2D>())
                        {
                            transform.parent.GetComponent<DraggableReturn2D>().enabled = true;
                        }
                        Debug.Log("圖片已更換為 changeSprites[0]");

                        RabbitGM.RemoveSpawnedObject(collision.gameObject);   //刪除物件與恢復場景數
                        Destroy(collision.gameObject);

                    }
                    else
                    {
                        Debug.LogWarning("changeSprites 陣列長度不足，無法變更圖片！");
                    }
                }
            }
            // 確認碰撞的物件是 square 時的處理
            if (collision.gameObject.CompareTag("square"))
            {
                Debug.Log("碰到 square！");
                // 當 ProcessorImage 為 1 時才更換圖片

                if (obj.currentProcessorIndex == 1)  //改這邊
                {
                    if (changeSprites.Length > 1)
                    {
                        spriteRenderer.sprite = changeSprites[1]; // 變更為 changeSprites[1]
                        transform.parent.GetComponent<SetIteamOpenObj>().ResetSize();

                        if (transform.parent.GetComponent<DraggableReturn2D>())
                        {
                            transform.parent.GetComponent<DraggableReturn2D>().enabled = true;
                        }
                        Debug.Log("圖片已更換為 changeSprites[1]");
                        RabbitGM.RemoveSpawnedObject(collision.gameObject);   //刪除物件與恢復場景數
                        Destroy(collision.gameObject);  //刪除
                    }
                    else
                    {
                        Debug.LogWarning("changeSprites 陣列長度不足，無法變更圖片！");
                    }
                }
            }

            // 確認碰撞的物件是 triangle 時的處理
            if (collision.gameObject.CompareTag("triangle"))
            {
                Debug.Log("碰到 triangle！");
                // 當 ProcessorImage 為 2 時才更換圖片
                if (obj.currentProcessorIndex == 2)
                {
                    if (changeSprites.Length > 2)
                    {
                        spriteRenderer.sprite = changeSprites[2]; // 變更為 changeSprites[2]
                        transform.parent.GetComponent<SetIteamOpenObj>().ResetSize();

                        if (transform.parent.GetComponent<DraggableReturn2D>())
                        {
                            transform.parent.GetComponent<DraggableReturn2D>().enabled = true;
                        }
                        Debug.Log("圖片已更換為 changeSprites[2]");
                        RabbitGM.RemoveSpawnedObject(collision.gameObject);   //刪除物件與恢復場景數
                        Destroy(collision.gameObject);  //刪除
                    }
                    else
                    {
                        Debug.LogWarning("changeSprites 陣列長度不足，無法變更圖片！");
                    }
                }


                //如果物件是電路板
                if (GameObject.FindWithTag("brokePCB") != null && collision.gameObject.CompareTag("PCB"))
                {
                    Debug.Log("碰到 PCB！");
                    // 當 Foropener.currentImageIndex 為 1 時才更換圖片
                    if (Foropener.currentImageIndex == 0 && changeSprites[0])
                    {
                        if (changeSprites.Length > 1)
                        {
                            spriteRenderer.sprite = changeSprites[0]; // 變更為 changeSprites[1]
                                                                      // transform.parent.GetComponent<SetIteamOpenObj>().ResetSize();

                            if (transform.parent.GetComponent<DraggableReturn2D>())
                            {
                                transform.parent.GetComponent<DraggableReturn2D>().enabled = true;
                            }
                            Debug.Log("圖片已更換為 changeSprites[0]");
                        }
                        else
                        {
                            Debug.LogWarning("changeSprites 陣列長度不足，無法變更圖片！");
                        }
                    }
                }
            }

        }
    }
}