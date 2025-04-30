using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fixtime : MonoBehaviour
{
    public GameObject energizedEffect; //倒數圖示
    public bool isEnergized; //倒數圖示開啟
    public float duration = 5.0f;  // 倒數時間
    public GameObject[] CrashToTriggerCountdown;   // 觸發倒數的物件

    private Machine[] machineScripts; //呼叫 machin程式
    private BrokeProgressAnime[] brokeProgressScripts; //呼叫 BrokeProgressAnime程式

    bool isCounter = false;  // 是否倒數
    bool isFixed = false;    // 是否維修

    private void Start()
    {
        if (CrashToTriggerCountdown == null || CrashToTriggerCountdown.Length == 0)
        {
            Debug.LogWarning($"GameObject {gameObject.name} 的 CrashToTriggerCountdown 尚未指定或為空，請於 Inspector 指定以避免錯誤");
            return; // 中斷 Start，避免後面程式繼續執行出錯
        }
        isCounter = false;
        isFixed = false;

        machineScripts = new Machine[CrashToTriggerCountdown.Length];  // 定義觸發倒數的物件陣列
        brokeProgressScripts = new BrokeProgressAnime[CrashToTriggerCountdown.Length];

        for (int i = 0; i < CrashToTriggerCountdown.Length; i++)  // 定義觸發倒數的物件陣列 增加
        {

            machineScripts[i] = CrashToTriggerCountdown[i].GetComponent<Machine>();     // 定義觸發倒數的物件 呼叫 Machine腳本
            brokeProgressScripts[i] = CrashToTriggerCountdown[i].GetComponent<BrokeProgressAnime>();   // 定義倒數計時 呼叫 BrokeProgressAnime腳本
        } 
    }

    private void Update()
    {
        if (!isFixed && isCounter)
        {
            // 檢查是否有 machine HP 歸零
            for (int i = 0; i < machineScripts.Length; i++)
            {
                if (machineScripts[i].HP <= 0.0f)
                {
                    //如果耐久值小於等於零
                    energizedEffect.SetActive(false);
                    energizedEffect.transform.Find("RadialProgressBar").GetComponent<CircularProgressBar>().StopCountdown();

                    // 直接停止該機器的耐久度 Coroutine
                    if (brokeProgressScripts[i] != null)
                    {
                        brokeProgressScripts[i].StopDamageOverTime();
                    }

                    isCounter = false;
                    return;
                }
            }

            // 正常倒數
            duration -= Time.deltaTime;

            // 倒數結束
            if (duration <= 0.0f)
            { 
                // 結束倒數條件
                isFixed = true;   // 不再維修
                isEnergized = false;  // 倒數圖示
                energizedEffect.SetActive(false); //倒數圖示關閉
                energizedEffect.transform.Find("RadialProgressBar").GetComponent<CircularProgressBar>().onCountdownFinished.Invoke(); //到數計時結束

                // 結束時也要停耐久值下降
                for (int i = 0; i < brokeProgressScripts.Length; i++)
                {
                    if (brokeProgressScripts[i] != null)
                    {
                        brokeProgressScripts[i].StopDamageOverTime();
                    }
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)  //當碰撞到修理物件
    {
        if (System.Array.Exists(CrashToTriggerCountdown, obj => obj == coll.gameObject))
        {
            isFixed = false;
            isCounter = true;
            isEnergized = true;
            energizedEffect.SetActive(true);
            duration = 5.0f;   //倒數計時時間

            energizedEffect.transform.Find("RadialProgressBar").GetComponent<CircularProgressBar>().ActivateCountdown(duration);
        }
    }

    void OnCollisionExit2D(Collision2D coll)   //當  結束碰撞到修理物件
    {
        if (System.Array.Exists(CrashToTriggerCountdown, obj => obj == coll.gameObject))
        {
            isFixed = true;
            isCounter = false;
            isEnergized = false;
            energizedEffect.SetActive(false);
        }
    }
    private void OnDrawGizmos()
    {
        if (CrashToTriggerCountdown == null || CrashToTriggerCountdown.Length == 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 1f);

#if UNITY_EDITOR
            UnityEditor.Handles.Label(transform.position + Vector3.up * 2f, "! CrashToTriggerCountdown 未設定");
#endif
        }
    }
}
