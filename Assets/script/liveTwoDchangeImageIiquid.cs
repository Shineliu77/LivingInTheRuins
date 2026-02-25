using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class liveTwoDchangeImageIiquid : MonoBehaviour
{

    // 碰撞後要變換的圖片陣列，索引需與 Foropener 中的 currentImageIndex 對應
    public Sprite[] changeSprites; //碰撞後會換得圖
    private SpriteRenderer spriteRenderer;
    public LiquidPopOut liquidPopOut; // 呼叫 LiquidPopOut 程式
    public bool isliveTwoDchangeImageIiquidOk = false;
    private bool CurrentliveTwoDchangeImageIiquid;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // 獲取 SpriteRenderer 組件
    }
    private void OnEnable()//檢查滑鼠放開事件
    {
        DraggableReturn2D.OnReleased += OnItemReleased;
    }
    void OnDisable()   //取消滑鼠放開事件
    {
        DraggableReturn2D.OnReleased -= OnItemReleased;
    }
    private void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("redIiquid") || coll.gameObject.CompareTag("yellowIiquid") || coll.gameObject.CompareTag("blueIiquid") || coll.gameObject.CompareTag("greenIiquid"))
        {
            CurrentliveTwoDchangeImageIiquid = true;
        }
    }
    private void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("redIiquid") || coll.gameObject.CompareTag("yellowIiquid") || coll.gameObject.CompareTag("blueIiquid") || coll.gameObject.CompareTag("greenIiquid"))
        {
            CurrentliveTwoDchangeImageIiquid = false;
        }
    }
    private void OnItemReleased(DraggableReturn2D item)
    {
        if (CurrentliveTwoDchangeImageIiquid == true)
        {
            if (Application.loadedLevelName == "TeachGame")
            {
                if (item.gameObject.CompareTag("blueIiquid"))
                {
                    Debug.Log("碰到 blueIiquid！");
                    // 當 Foropener.currentImageIndex 為 0 時才更換圖片


                    if (changeSprites.Length > 0)
                    {
                        AudioManager.Instance.PlaySfx(5);                                              //音效
                        spriteRenderer.sprite = changeSprites[2]; // 變更為 changeSprites[2]
                                                                  //transform.parent.GetComponent<SetIteamOpenObj>().ResetSize();
                                                                  // if (transform.parent.GetComponent<DraggableReturn2D>())
                                                                  // {
                                                                  //  if (transform.parent.GetComponent<SetIteamOpenObj>().OpenCount == 0)
                                                                  //  {
                                                                  //     transform.parent.GetComponent<DraggableReturn2D>().enabled = true;
                                                                  // }
                                                                  // }
                        Destroy(item.gameObject);
                        transform.parent.gameObject.name = "fixeditemOpenFinished2";
                        //  FindObjectOfType<TeachGM>().OpenTeach9();
                        FindObjectOfType<TeachGM>().OpenTeachEightTwo();

                        Debug.Log("圖片已更換為 changeSprites[3]");
                    }
                    else
                    {
                        Debug.LogWarning("changeSprites 陣列為空，無法變更圖片！");
                    }


                }

            }
            if (Application.loadedLevelName == "FirstGame")                     //第一關使用
            {
                SetIteamOpenObj obj = GetComponentInParent<SetIteamOpenObj>();

                // 確認碰撞的物件是 redIiquid 時的處理
                if (item.gameObject.CompareTag("redIiquid") && isliveTwoDchangeImageIiquidOk == false)
                {
                    Debug.Log("碰到 redIiquid！");
                    // 當 Foropener.currentImageIndex 為 3 時才更換圖片

                    if (obj.currentReagentsIndex == 0 && changeSprites.Length > 0)
                    {
                        AudioManager.Instance.PlaySfx(5);                                              //音效
                        spriteRenderer.sprite = changeSprites[0]; // 變更為 changeSprites[0]
                        transform.parent.GetComponent<SetIteamOpenObj>().ResetSize();
                        if (transform.parent.GetComponent<DraggableReturn2D>())
                        {
                            if (transform.parent.GetComponent<SetIteamOpenObj>().OpenCount == 0)
                            {
                                transform.parent.GetComponent<DraggableReturn2D>().enabled = true;
                            }
                        }
                        isliveTwoDchangeImageIiquidOk = true;
                        Debug.Log("圖片已更換為 changeSprites[0]");
                    }
                    else
                    {
                        Debug.LogWarning("changeSprites 陣列長度不足，無法變更圖片！");
                    }
                    if (spriteRenderer.sprite == changeSprites[0])
                    {
                        MakeAPotion.RemoveSpawnedObject(item.gameObject);   //刪除物件與恢復場景數
                        Destroy(item.gameObject);
                    }
                }
                // 確認碰撞的物件是 yellowIiquid 時的處理
                if (item.gameObject.CompareTag("yellowIiquid") && isliveTwoDchangeImageIiquidOk == false)
                {
                    Debug.Log("碰到 yellowIiquid！");


                    if (obj.currentReagentsIndex == 1 && changeSprites.Length > 1)
                    {
                        AudioManager.Instance.PlaySfx(5);                                              //音效
                        spriteRenderer.sprite = changeSprites[1]; // 變更為 changeSprites[1]
                        transform.parent.GetComponent<SetIteamOpenObj>().ResetSize();
                        if (transform.parent.GetComponent<DraggableReturn2D>())
                        {
                            if (transform.parent.GetComponent<SetIteamOpenObj>().OpenCount == 0)
                            {
                                transform.parent.GetComponent<DraggableReturn2D>().enabled = true;
                            }
                        }
                        isliveTwoDchangeImageIiquidOk = true;
                        Debug.Log("圖片已更換為 changeSprites[1]");
                    }
                    else
                    {
                        Debug.LogWarning("changeSprites 陣列長度不足，無法變更圖片！");
                    }
                    if (spriteRenderer.sprite == changeSprites[1])
                    {
                        MakeAPotion.RemoveSpawnedObject(item.gameObject);   //刪除物件與恢復場景數
                        Destroy(item.gameObject);
                    }
                }

                // 確認碰撞的物件是 blueIiquid 時的處理
                if (item.gameObject.CompareTag("blueIiquid") && isliveTwoDchangeImageIiquidOk == false)
                {
                    Debug.Log("碰到 blueIiquid！");


                    if (obj.currentReagentsIndex == 2 && changeSprites.Length > 2)
                    {
                        AudioManager.Instance.PlaySfx(5);                                              //音效
                        spriteRenderer.sprite = changeSprites[2]; // 變更為 changeSprites[0]
                        transform.parent.GetComponent<SetIteamOpenObj>().ResetSize();
                        if (transform.parent.GetComponent<DraggableReturn2D>())
                        {
                            if (transform.parent.GetComponent<SetIteamOpenObj>().OpenCount == 0)
                            {
                                transform.parent.GetComponent<DraggableReturn2D>().enabled = true;
                            }
                        }

                        isliveTwoDchangeImageIiquidOk = true;
                        Debug.Log("圖片已更換為 changeSprites[2]");
                    }
                    else
                    {
                        Debug.LogWarning("changeSprites 陣列長度不足，無法變更圖片！");
                    }
                    if (spriteRenderer.sprite == changeSprites[2])
                    {
                        MakeAPotion.RemoveSpawnedObject(item.gameObject);   //刪除物件與恢復場景數
                        Destroy(item.gameObject);
                    }

                    // if (liquidPopOut != null)
                    // {
                    //    Debug.Log("液體碰到 yellowIiquid，通知控制器銷毀！");
                    //  刪除生成液體
                    //   liquidPopOut.LiquidDefeated(collidedObject);
                    // }

                }
                // 確認碰撞的物件是 greenIiquid 時的處理
                if (item.gameObject.CompareTag("greenIiquid") && isliveTwoDchangeImageIiquidOk == false)
                {
                    Debug.Log("碰到 greenIiquid！");
                    // 當 Foropener.currentImageIndex 為 3 時才更換圖片

                    if (obj.currentReagentsIndex == 3 && changeSprites.Length > 3)
                    {
                        AudioManager.Instance.PlaySfx(5);                                              //音效
                        spriteRenderer.sprite = changeSprites[3];
                        transform.parent.GetComponent<SetIteamOpenObj>().ResetSize();
                        if (transform.parent.GetComponent<DraggableReturn2D>())
                        {
                            if (transform.parent.GetComponent<SetIteamOpenObj>().OpenCount == 0)
                            {
                                transform.parent.GetComponent<DraggableReturn2D>().enabled = true;
                            }
                        }

                        isliveTwoDchangeImageIiquidOk = true;
                        Debug.Log("圖片已更換為 changeSprites[3]");
                    }
                    else
                    {
                        Debug.LogWarning("changeSprites 陣列長度不足，無法變更圖片！");
                    }
                    if (spriteRenderer.sprite == changeSprites[3])
                    {
                        MakeAPotion.RemoveSpawnedObject(item.gameObject);   //刪除物件與恢復場景數
                        Destroy(item.gameObject);
                    }
                }
            }
        }
    }
}