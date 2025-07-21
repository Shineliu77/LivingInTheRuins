using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class liveTwoDchangeImagePCB : MonoBehaviour
{
    public Sprite newSprite; // 要更換的圖片
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // 獲取 SpriteRenderer 組件
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PCB")) // 確保碰撞的是指定物件
        {
            Debug.Log("碰到 PCB！");

            if (newSprite != null && spriteRenderer != null)
            {
                spriteRenderer.sprite = newSprite; // 更換圖片
                if (transform.parent.GetComponent<DraggableReturn2D>())
                {
                    transform.parent.GetComponent<DraggableReturn2D>().enabled = true;
                }
                Debug.Log("圖片已更換！");
            }
        }
    }
}

