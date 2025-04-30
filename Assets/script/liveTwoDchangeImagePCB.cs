using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class liveTwoDchangeImagePCB : MonoBehaviour
{
    public Sprite newSprite; // �n�󴫪��Ϥ�
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // ��� SpriteRenderer �ե�
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PCB")) // �T�O�I�����O���w����
        {
            Debug.Log("�I�� PCB�I");

            if (newSprite != null && spriteRenderer != null)
            {
                spriteRenderer.sprite = newSprite; // �󴫹Ϥ�
                Debug.Log("�Ϥ��w�󴫡I");
            }
        }
    }
}

