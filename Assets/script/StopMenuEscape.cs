using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMenuEscape : MonoBehaviour
{
    public GameObject stopMenuObject;  // ���w�n�}��������
    private bool isMenuOpen = false;   // �O���ثe�O�_���}�Ҫ��A

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuOpen = !isMenuOpen;  // �C�����UESC�N�������A
            stopMenuObject.SetActive(isMenuOpen);
        }
    }
}
