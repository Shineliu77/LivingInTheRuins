using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // 這行很重要

public class Fixtime : MonoBehaviour
{
    public GameObject energizedEffect; //倒數圖示
    public bool isEnergized; //倒數圖示開啟
    public float duration = 10.0f;  // 倒數時間
    public GameObject[] CrashToTriggerCountdown;   // 觸發倒數的物件

    public string[] FindToTriggerCountdownNames;  // 新增：場景內自動尋找的物件名字
    public GameObject[] findInToTriggerObjects; // 找到的場景物件

    private Machine[] machineScripts; //呼叫 machin程式
    private BrokeProgressAnime[] brokeProgressScripts; //呼叫 BrokeProgressAnime程式

    bool isCounter = false;  // 是否倒數
    bool isFixed = false;    // 是否維修

    private void Start()
    {
        isCounter = false;
        isFixed = false;

        // 如果 CrashToTriggerCountdown 沒有手動設定，就嘗試用Find的  (找到場景物件)
        if ((CrashToTriggerCountdown == null || CrashToTriggerCountdown.Length == 0) && FindToTriggerCountdownNames != null && FindToTriggerCountdownNames.Length > 0)
        {
            findInToTriggerObjects = new GameObject[FindToTriggerCountdownNames.Length];  //定義 findInToTriggerObjects 陣列

            for (int i = 0; i < FindToTriggerCountdownNames.Length; i++)
            {
                GameObject obj = GameObject.Find(FindToTriggerCountdownNames[i]);
                if (obj != null)
                {
                    findInToTriggerObjects[i] = obj;
                }
                else
                {
                    Debug.LogWarning($"Fixtime: 無法在場景中找到名稱為 {FindToTriggerCountdownNames[i]} 的物件");
                }

            }
        }

        if (CrashToTriggerCountdown != null && CrashToTriggerCountdown.Length > 0)   //初始 CrashToTriggerCountdown 跟 CrashToTriggerCountdown.Length
        {
            machineScripts = new Machine[CrashToTriggerCountdown.Length];
            brokeProgressScripts = new BrokeProgressAnime[CrashToTriggerCountdown.Length];

            for (int i = 0; i < CrashToTriggerCountdown.Length; i++)  // 定義觸發倒數的物件陣列 增加
            {
                if (CrashToTriggerCountdown[i] != null)   //多一層保護(防止CrashToTriggerCountdown為空報錯)
                {
                    machineScripts[i] = CrashToTriggerCountdown[i].GetComponent<Machine>();     // 定義觸發倒數的物件 呼叫 Machine腳本
                    brokeProgressScripts[i] = CrashToTriggerCountdown[i].GetComponent<BrokeProgressAnime>();   // 定義倒數計時 呼叫 BrokeProgressAnime腳本

                }
            }
        }
        // ---【初始化機器與倒數動畫腳本陣列】---
        int arrayLength = 0;

        if (CrashToTriggerCountdown != null && CrashToTriggerCountdown.Length > 0)
        {
            arrayLength = CrashToTriggerCountdown.Length; // 使用手動設定的陣列
        }
        else if (findInToTriggerObjects != null && findInToTriggerObjects.Length > 0)
        {
            arrayLength = findInToTriggerObjects.Length; // 使用自動找到的陣列
        }

        machineScripts = new Machine[arrayLength];
        brokeProgressScripts = new BrokeProgressAnime[arrayLength];

        // ---【設定machineScripts和brokeProgressScripts】---
        for (int i = 0; i < arrayLength; i++)
        {
            GameObject targetObj = null;

            // 優先使用CrashToTriggerCountdown，沒有才用findInToTriggerObjects
            if (CrashToTriggerCountdown != null && i < CrashToTriggerCountdown.Length)
            {
                targetObj = CrashToTriggerCountdown[i];
            }
            else if (findInToTriggerObjects != null && i < findInToTriggerObjects.Length)
            {
                targetObj = findInToTriggerObjects[i];
            }

            if (targetObj != null)
            {
                machineScripts[i] = targetObj.GetComponent<Machine>();
                brokeProgressScripts[i] = targetObj.GetComponent<BrokeProgressAnime>();

                // 防呆：如果找不到腳本，提示警告
                if (machineScripts[i] == null)
                    Debug.LogWarning($"Fixtime: {targetObj.name} 沒有 Machine 腳本！");
                if (brokeProgressScripts[i] == null)
                    Debug.LogWarning($"Fixtime: {targetObj.name} 沒有 BrokeProgressAnime 腳本！");
            }
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
        if (findInToTriggerObjects.Contains(coll.gameObject))   //當找到碰撞物件 
        {
            isFixed = false;
            isCounter = true;
            isEnergized = true;
            energizedEffect.SetActive(true);
            duration = 5.0f;   //倒數計時時間

            energizedEffect.transform.Find("RadialProgressBar").GetComponent<CircularProgressBar>().ActivateCountdown(duration);
        }

        //查找tag觸發
        if (coll.gameObject.CompareTag("fixeditem"))   //當找到碰撞物件        客人帶來的物件  目前無效
        {
            isFixed = false;
            isCounter = true;
            isEnergized = true;
            energizedEffect.SetActive(true);
            duration = 5.0f;   //倒數計時時間

            energizedEffect.transform.Find("RadialProgressBar").GetComponent<CircularProgressBar>().ActivateCountdown(duration);
        }

        //查找tag觸發
        if (coll.gameObject.CompareTag("Red"))   //當找到碰撞物件  場景試管機器的材料 
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
        if (coll.gameObject.CompareTag("Red"))
        {
            isFixed = false;
            isCounter = true;
            isEnergized = true;
            energizedEffect.SetActive(true);
            return; // 若 Red 有特殊處理邏輯，return 可避免進入下方檢查
        }



        if (coll.gameObject.CompareTag("fixeditem") && System.Array.Exists(findInToTriggerObjects, obj => obj == coll.gameObject))
        {
            //coll.gameObject.CompareTag("fixeditem")不符合觸發條件
            isFixed = true;
            isCounter = false;
            isEnergized = false;
            energizedEffect.SetActive(false);
        }
        //達到其中個條件就觸發
        //if (System.Array.Exists(CrashToTriggerCountdown, obj => obj == coll.gameObject) || (findInToTriggerObjects != null && System.Array.Exists(findInToTriggerObjects, obj => obj == coll.gameObject)))
        // {
        //isFixed = true;
        //isCounter = false;
        //isEnergized = false;
        //energizedEffect.SetActive(false);
        // }
    }
    // private void OnDrawGizmos(){ if (CrashToTriggerCountdown == null || CrashToTriggerCountdown.Length == 0){ Gizmos.color = Color.red;Gizmos.DrawWireSphere(transform.position, 1f);

#if UNITY_EDITOR
    //  UnityEditor.Handles.Label(transform.position + Vector3.up * 2f, "! CrashToTriggerCountdown 未設定");
#endif
}