using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;//影入場景
public class Timer : MonoBehaviour
{
    //timelimit for level
    [SerializeField] TextMeshProUGUI timerText;  //時間文字
    [SerializeField] float remainingTime = 75f; //計時器時間剩餘設定

    public GameObject ScorePanel;  // 指定要開關的物件
    private bool ScorePanelOpen = false;   // 記錄目前是否為開啟狀態

    public CustomerMoney customerMoney; //金錢分數程式

    private bool isTimerPaused = false;  //控制時間是否暫停
    void Update()
    {
        // 如果沒有暫停，才執行倒數與顯示更新
        if (!isTimerPaused)
        {
            //剩餘時間大於零可繼續倒數
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime; //減有遊戲經過時間
            }
            //確保不會出現負數，並將時間歸零
            else if (remainingTime < 0)
            {
                remainingTime = 0;
            }

            else if (remainingTime <= 0 && !ScorePanelOpen)  //如果倒數秒數歸零 打開分數面板暫停遊戲
            {
                ScorePanel.SetActive(true); // 顯示分數面板
                ScorePanelOpen = true;          // 防止重複觸發
                isTimerPaused = true;  // 加這行：暫停時間倒數
                Time.timeScale = 0;  //遊戲暫停
                //the way to reset thing
                // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//可載入場景
                //SceneManager.LoadScene("");
            }
            int minutes = Mathf.FloorToInt(remainingTime / 60); //剩餘 分
            int seconds = Mathf.FloorToInt(remainingTime % 60); //剩餘 秒
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); //ui顯示方式

           if (CustomerMoney.Score >= 100 && !ScorePanelOpen)  //金錢分數100分 打開分數面板暫停遊戲
           {
            ScorePanel.SetActive(true); // 顯示分數面板
            ScorePanelOpen = true;          // 防止重複觸發
             isTimerPaused = true;  // 加這行：暫停時間倒數
             Time.timeScale = 0;  //遊戲暫停
           }

        }
    }
}
