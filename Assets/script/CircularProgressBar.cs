using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CircularProgressBar : MonoBehaviour
{
    private bool isActive = false; //是否倒數
    private float indicatorTimer;
    private float maxIndicatorTimer;

    public Image radialProgressBar;
    public UnityEvent onCountdownFinished;

    public BrokeProgressAnime brokeProgressAnime;
    public GameObject[] CrashToTriggerPICountdown; // 碰撞觸發的機器群組
    private Machine[] machineScripts; // 多個 machine 的陣列

    private void Awake()
    {
        radialProgressBar = GetComponent<Image>(); //設 圖片當倒數進度條

        if (onCountdownFinished == null) //確保  UnityEvent 不是空的
        {
            onCountdownFinished = new UnityEvent();
        }
    }

    private void Start()
    {
        //初始化 machineScripts 陣列
        machineScripts = new Machine[CrashToTriggerPICountdown.Length];

        for (int i = 0; i < CrashToTriggerPICountdown.Length; i++)
        {
            machineScripts[i] = CrashToTriggerPICountdown[i].GetComponent<Machine>();
        }
    }

    private void Update()
    {
        if (isActive)  //啟動 倒數 倒數圖片
        {
            indicatorTimer -= Time.deltaTime;
            radialProgressBar.fillAmount = (indicatorTimer / maxIndicatorTimer);
            
            if (indicatorTimer <= 0)   //倒數結束 停止到數 與圖示
            {
                StopCountdown();
                onCountdownFinished.Invoke();
            }
        }
    }

    //開始倒數
    public void ActivateCountdown(float countdownTime)
    {
        isActive = true;
        maxIndicatorTimer = countdownTime;
        indicatorTimer = maxIndicatorTimer;
    }

    //停止倒數（結束）
    public void StopCountdown()
    {
        isActive = false;
        indicatorTimer = 0;
        radialProgressBar.fillAmount = 0;

        if (brokeProgressAnime != null)
        {
            brokeProgressAnime.StopDamageOverTime();
        }
    }

    //暫停倒數 (HP <= 0)
    public void TemporaryStopCountdown()
    {
        isActive = false;
        radialProgressBar.fillAmount = (indicatorTimer / maxIndicatorTimer);

        if (brokeProgressAnime != null)
        {
            brokeProgressAnime.StopDamageOverTime();
        }
    }
}