using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMenuEscape : MonoBehaviour
{
    public GameObject stopMenuObject;  // 指定要開關的物件
    private bool isMenuOpen = false;   // 記錄目前是否為開啟狀態

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuOpen = !isMenuOpen;  // 每次按下ESC就切換狀態
            stopMenuObject.SetActive(isMenuOpen);
        }
    }
}
