using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;//影入場景
public class Timer : MonoBehaviour
{
    //timelimit for level
    [SerializeField] TextMeshProUGUI timerText;  //時間文字
    [SerializeField] float remainingTime; //計時器時間剩餘設定
    void Update()
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

        else if (remainingTime <= 0)
        {
            //the way to reset thing
            //Coin.CoinCount = 0;
           // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//可載入場景
            //SceneManager.LoadScene("");
        }
        int minutes = Mathf.FloorToInt(remainingTime / 60); //剩餘 分
        int seconds = Mathf.FloorToInt(remainingTime % 60); //剩餘 秒
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); //ui顯示方式
                                                                           //倒數結束重製遊戲

    }
}
