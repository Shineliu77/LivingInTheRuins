using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BrokeProgressAnime : MonoBehaviour
{
    public GameObject FixItemMachine; //  修理的物件的機器

    public GameObject[] ShouldFixed; //  要被修理的物件
    public Image brokebar; //耐久值條
                           // public GameObject energizedEffect; //  修復動畫效果
    public bool isEnergized = false; // 是否正在修復
    public CircularProgressBar progressBar; // 呼叫 CircularProgressBar(被維修的物品用)
    public bool isDamaged = false; // 是否處於耐久值條持續 下降  狀態

    private Coroutine brokeCoroutine; // 耐久值條持續 下降
    private Coroutine fixCoroutine;   // 耐久值條持續 修復

    private bool isDamaging = false; // 控制是否正在耐久值條持續 下降

    private int collidingObjects = 0; // 當前與 ShouldFixed 物件接觸的數量


    private void Start()
    {
        // 綁定 CircularProgressBar 完成事件，用於修復結束時停止掉血
        if (progressBar != null)
        {
            progressBar.onCountdownFinished.AddListener(StopDamageOverTime);
        }

    }

    // 刷新耐久條 UI
    public void Refreshbrokebar()
    {
        if (FixItemMachine != null)
        {
            Machine machineScript = FixItemMachine.GetComponent<Machine>();
            brokebar.fillAmount = machineScript.HP / machineScript.HPMax;
        }
    }

    // 碰撞進入
    private void OnCollisionEnter2D(Collision2D coll)
    {
        // 碰到 ShouldFixed 的物件
        if (System.Array.Exists(ShouldFixed, obj => obj == coll.gameObject))
        {
            collidingObjects++; // 記錄碰撞的物件數量
            isDamaged = true;

            // 如果有修復協程，停止修復
            if (fixCoroutine != null)
            {
                StopCoroutine(fixCoroutine);
                fixCoroutine = null;
            }

            // 如果還沒在掉血，啟動掉血協程
            //if (brokeCoroutine == null)
            // {
            //    brokeCoroutine = StartCoroutine(DamageOverTime());
            //}
            if (!isDamaging)
            {
                brokeCoroutine = StartCoroutine(DamageOverTime());
            }

            // 關閉修復動畫
            //energizedEffect.SetActive(false);
            isEnergized = false;
        }

        //tag的fixiem物品碰撞
        if (coll.gameObject.CompareTag("fixeditem"))
        {
            Debug.Log("碰撞到: " + coll.gameObject.name + ", Tag: " + coll.gameObject.tag);
            isDamaged = true;

            // 如果有修復協程，停止修復
            if (fixCoroutine != null)
            {
                StopCoroutine(fixCoroutine);
                fixCoroutine = null;
            }

            // 如果還沒在掉血，啟動掉血協程
            if (brokeCoroutine == null)
            {
                brokeCoroutine = StartCoroutine(DamageOverTime());
            }

            // 關閉修復動畫
            //energizedEffect.SetActive(false);
            isEnergized = false;
        }

        // 碰到 machinefix (修理物)
        // if (coll.gameObject.CompareTag("machinefix"))
        // {
        // 只有在沒有損壞時才可以修復
        // if (!isDamaged && fixCoroutine == null)
        // {
        //   fixCoroutine = StartCoroutine(FixTime());
        //   isEnergized = true;
        // energizedEffect.SetActive(true);
        //}
        //}
    }

    // 碰撞結束
    void OnCollisionExit2D(Collision2D coll)
    {
        // 離開 ShouldFixed 的物件
        if (System.Array.Exists(ShouldFixed, obj => obj == coll.gameObject))
        {
            // collidingObjects--; // 減少計數
            //if (collidingObjects <= 0)
            //{
            //   isDamaged = false;
            //StopDamageOverTime();
            //  }
        }


        // 離開 tag的fixiem物品碰撞
        if (coll.gameObject.CompareTag("fixeditem"))
        {
            collidingObjects--; // 減少計數
            if (collidingObjects <= 0)
            {
                isDamaged = false;
                StopDamageOverTime();
            }
            fixCoroutine = StartCoroutine(FixTime());   //有問題   會直接回滿
            isEnergized = true;
        }


        // 離開 machinefix
        // if (coll.gameObject.CompareTag("machinefix"))
        // {
        // 不在損壞狀態才可以重新啟動修復
        // if (!isDamaged && fixCoroutine == null)
        // {
        //    fixCoroutine = StartCoroutine(FixTime());
        //    isEnergized = true;
        //  energizedEffect.SetActive(true);
        //}


        // 離開 碰撞
        if (!isDamaged && fixCoroutine == null)
        {
            isDamaged = false;
            StopDamageOverTime();
            fixCoroutine = StartCoroutine(FixTime());   //有問題   會直接回滿
            isEnergized = true;
            //  energizedEffect.SetActive(true);
        }

    }

    // 每秒降低機器的 HP
    IEnumerator DamageOverTime()
    {
        isDamaging = true; // 開始掉血

        while (isDamaging)
        {
            if (FixItemMachine != null)
            {
                Machine machineScript = FixItemMachine.GetComponent<Machine>();
                if (machineScript != null)
                {
                    machineScript.HP -= machineScript.HPMax * 0.03f;
                    Refreshbrokebar();
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void StopDamageOverTime()
    {
        Debug.Log("嘗試停止耐久值下降");

        isDamaging = false; // 停止掉血迴圈
        if (brokeCoroutine != null)
        {
            StopCoroutine(brokeCoroutine);
            brokeCoroutine = null;
            Debug.Log("成功停止掉血協程");
        }
        Refreshbrokebar();
    }

    // 修復機器耐久度
    IEnumerator FixTime()
    {
        if (FixItemMachine != null)
        {
            Machine machineScript = FixItemMachine.GetComponent<Machine>();
            if (machineScript == null) yield break;

            while (machineScript.HP < machineScript.HPMax)
            {
                // 每秒回復 10%
                machineScript.HP += machineScript.HPMax * 0.1f;

                if (machineScript.HP >= machineScript.HPMax)
                {
                    machineScript.HP = machineScript.HPMax;

                    // 修復完成移除動畫
                    // Transform animeTransform = energizedEffect.transform.Find("machinefixAnime");
                    // if (animeTransform != null)
                    // {
                    //     Destroy(animeTransform.gameObject);
                    //x }

                    // energizedEffect.SetActive(false);
                    isEnergized = false;
                    fixCoroutine = null;
                    yield break;
                }
                Refreshbrokebar();
                yield return new WaitForSeconds(1f);
            }
        }
    }
}