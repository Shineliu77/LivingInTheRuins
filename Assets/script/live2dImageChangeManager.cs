using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class live2dImageChangeManager : MonoBehaviour
{
    public static live2dImageChangeManager Instance { get; private set; }

    // �x�s�Ҧ��ܴ����Ϥ�
    public Sprite[] commonChangeSprites; // �����ϵ{��1�ϥΪ�
    public Sprite[] liquidChangeSprites; // �����ϵ{��2�ϥΪ�

    public int currentImageIndex = 0; // �Τ@�޲z currentImageIndex

    private void Awake()
    {
        // Singleton �]�p�Ҧ��A�T�O�u���@�ӹ��
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
