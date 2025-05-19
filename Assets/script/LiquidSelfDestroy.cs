using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidSelfDestroy : MonoBehaviour
{
    public LiquidPopOut liquidPopOut; // 外部會由 LiquidPopOut 傳入

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("bgchange"))
        {
            if (liquidPopOut != null)
            {
                Debug.Log("液體碰到 fixeditemOpen，通知控制器銷毀！");

                liquidPopOut.LiquidDefeated(gameObject); //  正確呼叫控制器的方法
            }
            else
            {
                Debug.LogWarning("liquidPopOut 尚未設置，無法從列表中移除，直接銷毀自己");
                Destroy(gameObject);
            }
        }
    }
}
