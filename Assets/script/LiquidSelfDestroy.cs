using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidSelfDestroy : MonoBehaviour
{
    public LiquidPopOut liquidPopOut; // �~���|�� LiquidPopOut �ǤJ

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("bgchange"))
        {
            if (liquidPopOut != null)
            {
                Debug.Log("�G��I�� fixeditemOpen�A�q������P���I");

                liquidPopOut.LiquidDefeated(gameObject); //  ���T�I�s�������k
            }
            else
            {
                Debug.LogWarning("liquidPopOut �|���]�m�A�L�k�q�C�������A�����P���ۤv");
                Destroy(gameObject);
            }
        }
    }
}
