using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMenuEscape : MonoBehaviour
{
    public GameObject stopMenuObject;  // 選單
    public GameObject stopAdjust;     // 調整項目
    private bool isMenuOpen = false;   // 記錄目前是否為開啟狀態
    private bool isAdjustOpen = false;   // 記錄目前是否為開啟狀態
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuOpen = !isMenuOpen;  // 每次按下ESC就開關選單
            stopMenuObject.SetActive(isMenuOpen);
            stopAdjust.SetActive(isAdjustOpen);
            AudioManager.Instance.PlaySfx(5);
            if (isMenuOpen == false && isAdjustOpen == false)  //關起時間繼續
            {
                Time.timeScale = 1;

            }

            if (isMenuOpen == true && isAdjustOpen == true)  //打開時間暫停
            {
                //音效
                Time.timeScale = 0;

            }
        }

        else if (isMenuOpen == false && Input.GetKeyDown(KeyCode.Escape))
        {
            // 調整項目可用esc關閉
            stopAdjust.SetActive(false);
            Time.timeScale = 1;

            if (isAdjustOpen == false && Input.GetKeyDown(KeyCode.Escape))
            {
                AudioManager.Instance.PlaySfx(5);                                              //音效
                isMenuOpen = !isMenuOpen;
            }
        }
    }
}


