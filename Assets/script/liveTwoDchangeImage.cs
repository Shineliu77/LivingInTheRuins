using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveTwoDChangeImage : MonoBehaviour
{
    // 碰撞後要變換的圖片陣列，索引需與 Foropener 中的 currentImageIndex 對應
    public Sprite[] changeSprites;
    //換圖
    private SpriteRenderer spriteRenderer;
    public bool LiveTDChangeImgOk = false;  //給TeachGame使用
    public bool isLiveTDChangeImgOk = false;
    //當前換圖
    public bool CurrenrLiveTDChangeImg;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // 獲取 SpriteRenderer 組件
    }
    void OnEnable()//檢查滑鼠放開事件
    {
        DraggableReturn2D.OnReleased += OnItemReleased;
    }
    private void OnDisable()//取消滑鼠放開事件
    {
        DraggableReturn2D.OnReleased -= OnItemReleased;
    }
    private void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("brokecircle") || coll.gameObject.CompareTag("triangle") || coll.gameObject.CompareTag("square") || coll.gameObject.CompareTag("square"))
        {
            CurrenrLiveTDChangeImg = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        CurrenrLiveTDChangeImg = false;
    }
    private void OnItemReleased(DraggableReturn2D item)
    {
        if (CurrenrLiveTDChangeImg == true)
        {
            if (item.gameObject.CompareTag("brokecircle") && LiveTDChangeImgOk == true)
            {
                Debug.Log("碰到 brokecircle！");
                // 當 Foropener.currentImageIndex 為 0 時才更換圖片

                if (changeSprites.Length > 0)
                {
                    AudioManager.Instance.PlaySfx(22);                                                 //音效
                    spriteRenderer.sprite = changeSprites[0]; // 變更為 changeSprites[0]
                    Debug.Log("圖片已更換為 changeSprites[0]");
                    RabbitGM.RemoveSpawnedObject(item.gameObject);   //刪除物件與恢復場景數
                    DestroyPrefabButton.Remove(item.gameObject);
                    Destroy(item.gameObject);
                    /* NewPlayerTeach teachScript = FindObjectOfType<NewPlayerTeach>(); //僅在碰撞到 brokecircle 才開啟教學
                     if (teachScript != null)
                     {
                         teachScript.IsAfterChangeImage();
                     }*/
                    transform.parent.GetComponent<SetIteamOpenObj>().ResetSize();

                    LiveTDChangeImgOk = false;

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

            if (Application.loadedLevelName == "FirstGame")
            {
                SetIteamOpenObj obj = GetComponentInParent<SetIteamOpenObj>();
                if (item.gameObject.CompareTag("brokecircle") && isLiveTDChangeImgOk == false)
                {
                    Debug.Log("碰到 brokecircle！");
                    // 當 ProcessorImage 為 1 時才更換圖片

                    if (obj.currentProcessorIndex == 0)  //改這邊
                    {
                        if (changeSprites.Length > 0)
                        {
                            AudioManager.Instance.PlaySfx(22);                                                 //音效
                            spriteRenderer.sprite = changeSprites[0]; // 變更為 changeSprites[1]
                            transform.parent.GetComponent<SetIteamOpenObj>().ResetSize();
                            if (transform.parent.GetComponent<DraggableReturn2D>())
                            {
                                if (transform.parent.GetComponent<SetIteamOpenObj>().OpenCount == 0)
                                {
                                    transform.parent.GetComponent<DraggableReturn2D>().enabled = true;
                                }
                            }
                            Debug.Log("圖片已更換為 changeSprites[0]");

                            RabbitGM.RemoveSpawnedObject(item.gameObject);   //刪除物件與恢復場景數
                            Destroy(item.gameObject);

                            isLiveTDChangeImgOk = true;

                        }
                        else
                        {
                            Debug.LogWarning("changeSprites 陣列長度不足，無法變更圖片！");
                        }
                    }
                }
                // 確認碰撞的物件是 square 時的處理
                if (item.gameObject.CompareTag("square") && isLiveTDChangeImgOk == false)
                {
                    Debug.Log("碰到 square！");
                    // 當 ProcessorImage 為 1 時才更換圖片

                    if (obj.currentProcessorIndex == 1)  //改這邊
                    {
                        if (changeSprites.Length > 1)
                        {
                            AudioManager.Instance.PlaySfx(22);                                                //音效
                            spriteRenderer.sprite = changeSprites[1]; // 變更為 changeSprites[1]
                            transform.parent.GetComponent<SetIteamOpenObj>().ResetSize();
                            if (transform.parent.GetComponent<DraggableReturn2D>())
                            {
                                if (transform.parent.GetComponent<SetIteamOpenObj>().OpenCount == 0)
                                {
                                    transform.parent.GetComponent<DraggableReturn2D>().enabled = true;
                                }
                            }
                            Debug.Log("圖片已更換為 changeSprites[1]");
                            RabbitGM.RemoveSpawnedObject(item.gameObject);   //刪除物件與恢復場景數
                            Destroy(item.gameObject);  //刪除

                            isLiveTDChangeImgOk = true;

                        }
                        else
                        {
                            Debug.LogWarning("changeSprites 陣列長度不足，無法變更圖片！");
                        }
                    }
                }

                // 確認碰撞的物件是 triangle 時的處理
                if (item.gameObject.CompareTag("triangle") && isLiveTDChangeImgOk == false)
                {
                    Debug.Log("碰到 triangle！");
                    // 當 ProcessorImage 為 2 時才更換圖片
                    if (obj.currentProcessorIndex == 2)
                    {
                        if (changeSprites.Length > 2)
                        {
                            AudioManager.Instance.PlaySfx(22);                                             //音效
                            spriteRenderer.sprite = changeSprites[2]; // 變更為 changeSprites[2]
                            transform.parent.GetComponent<SetIteamOpenObj>().ResetSize();
                            if (transform.parent.GetComponent<DraggableReturn2D>())
                            {
                                if (transform.parent.GetComponent<SetIteamOpenObj>().OpenCount == 0)
                                {
                                    transform.parent.GetComponent<DraggableReturn2D>().enabled = true;
                                }
                            }
                            Debug.Log("圖片已更換為 changeSprites[2]");
                            RabbitGM.RemoveSpawnedObject(item.gameObject);   //刪除物件與恢復場景數
                            Destroy(item.gameObject);  //刪除

                            isLiveTDChangeImgOk = true;
                        }
                        else
                        {
                            Debug.LogWarning("changeSprites 陣列長度不足，無法變更圖片！");
                        }
                    }


                    //如果物件是電路板
                    if (GameObject.FindWithTag("brokePCB") != null && item.gameObject.CompareTag("PCB"))
                    {
                        Debug.Log("碰到 PCB！");
                        // 當 Foropener.currentImageIndex 為 1 時才更換圖片
                        if (Foropener.currentImageIndex == 0 && changeSprites[0])
                        {
                            if (changeSprites.Length > 1)
                            {
                                AudioManager.Instance.PlaySfx(22);                                             //音效
                                spriteRenderer.sprite = changeSprites[0]; // 變更為 changeSprites[1]
                                                                          //transform.parent.GetComponent<SetIteamOpenObj>().ResetSize();    但應該不是你讓東西變-1 但應該不是
                                if (transform.parent.GetComponent<DraggableReturn2D>())
                                {
                                    if (transform.parent.GetComponent<SetIteamOpenObj>().OpenCount == 0)
                                    {
                                        transform.parent.GetComponent<DraggableReturn2D>().enabled = true;
                                    }
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
}
