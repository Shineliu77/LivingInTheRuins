using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foropener : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // ��ܹϤ��� SpriteRenderer
    public Sprite[] OpenerChangeImage;    // �i�H����ܪ��Ϥ��}�C
    public static int currentImageIndex = -1; // �O����e�Ϥ������ޡ]�Ω�I����󴫹Ϥ��^

    void Start()
    {
        // �ˬd OpenerChangeImage �O�_���Ϥ�
        if (OpenerChangeImage != null && OpenerChangeImage.Length > 0)
        {
            // �H������@�i�Ϥ�
            currentImageIndex = Random.Range(0, OpenerChangeImage.Length);
            spriteRenderer.sprite = OpenerChangeImage[currentImageIndex]; // ��ܹϤ�
        }
        else
        {
            Debug.LogWarning("OpenerChangeImage �}�C���šA�Цb Inspector ���K�[�Ϥ��C");
        }
    }
}
