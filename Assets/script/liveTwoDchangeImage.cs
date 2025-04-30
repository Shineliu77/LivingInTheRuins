using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveTwoDChangeImage : MonoBehaviour
{
    // �I����n�ܴ����Ϥ��}�C�A���޻ݻP Foropener ���� currentImageIndex ����
    public Sprite[] changeSprites;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // ��� SpriteRenderer �ե�
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �T�{�I��������O brokecircle �ɪ��B�z
        if (collision.gameObject.CompareTag("brokecircle"))
        {
            Debug.Log("�I�� brokecircle�I");
            // �� Foropener.currentImageIndex �� 0 �ɤ~�󴫹Ϥ�
            if (Foropener.currentImageIndex == 0 && changeSprites[0])
            {
                if (changeSprites.Length > 0)
                {
                    spriteRenderer.sprite = changeSprites[0]; // �ܧ� changeSprites[0]
                    Debug.Log("�Ϥ��w�󴫬� changeSprites[0]");
                }
                else
                {
                    Debug.LogWarning("changeSprites �}�C���šA�L�k�ܧ�Ϥ��I");
                }
            }
        }

        // �T�{�I��������O square �ɪ��B�z
        if (collision.gameObject.CompareTag("square"))
        {
            Debug.Log("�I�� square�I");
            // �� Foropener.currentImageIndex �� 1 �ɤ~�󴫹Ϥ�
            if (Foropener.currentImageIndex == 1 && changeSprites[1])
            {
                if (changeSprites.Length > 1)
                {
                    spriteRenderer.sprite = changeSprites[1]; // �ܧ� changeSprites[1]
                    Debug.Log("�Ϥ��w�󴫬� changeSprites[1]");
                }
                else
                {
                    Debug.LogWarning("changeSprites �}�C���פ����A�L�k�ܧ�Ϥ��I");
                }
            }
        }

        // �T�{�I��������O triangle �ɪ��B�z
        if (collision.gameObject.CompareTag("triangle"))
        {
            Debug.Log("�I�� triangle�I");
            // �� Foropener.currentImageIndex �� 2 �ɤ~�󴫹Ϥ�
            if (Foropener.currentImageIndex == 2 && changeSprites[2])
            {
                if (changeSprites.Length > 2)
                {
                    spriteRenderer.sprite = changeSprites[2]; // �ܧ� changeSprites[2]
                    Debug.Log("�Ϥ��w�󴫬� changeSprites[2]");
                }
                else
                {
                    Debug.LogWarning("changeSprites �}�C���פ����A�L�k�ܧ�Ϥ��I");
                }
            }
        }
    }
}