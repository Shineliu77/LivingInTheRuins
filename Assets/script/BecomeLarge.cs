using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BecomeLarge : MonoBehaviour
{
    public bool isColliding = false;  // 是否被碰撞
    private Vector3 originalScale;     // 儲存原本大小
    public Vector3 scaleChange = new Vector3(1.5f, 1.5f, 1.5f);   // 可調放大尺寸

    void Start()
    {
        originalScale = transform.localScale;  // 紀錄原本尺寸
    }

    // 當進入碰撞時
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BecomeLarge") && !isColliding)
        {
            isColliding = true;
            transform.localScale = originalScale + scaleChange;  // 放大
            NewPlayerTeach teachScript = FindObjectOfType<NewPlayerTeach>(); //僅在碰撞到 BecomeLarge 才開啟教學
            if (teachScript != null)
            {
                teachScript.IscollBecameLarge();
            }
        }
        if (!isColliding && collision.gameObject.CompareTag("BecomeLarge2"))
        {
            isColliding = true;
            transform.localScale = originalScale + scaleChange;  // 放大

        }
    }


    // 當離開碰撞時
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BecomeLarge") && isColliding)
        {
            isColliding = false;
            transform.localScale = originalScale;  // 變回原本尺寸
        }

        if (collision.gameObject.CompareTag("BecomeLarge2") && isColliding)
        {
            isColliding = false;
            transform.localScale = originalScale;  // 變回原本尺寸
        }
    }
}