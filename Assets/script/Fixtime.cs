using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fixtime : MonoBehaviour
{
    public GameObject energizedEffect; //�˼ƹϥ�
    public bool isEnergized; //�˼ƹϥܶ}��
    public float duration = 5.0f;  // �˼Ʈɶ�
    public GameObject[] CrashToTriggerCountdown;   // Ĳ�o�˼ƪ�����

    private Machine[] machineScripts; //�I�s machin�{��
    private BrokeProgressAnime[] brokeProgressScripts; //�I�s BrokeProgressAnime�{��

    bool isCounter = false;  // �O�_�˼�
    bool isFixed = false;    // �O�_����

    private void Start()
    {
        if (CrashToTriggerCountdown == null || CrashToTriggerCountdown.Length == 0)
        {
            Debug.LogWarning($"GameObject {gameObject.name} �� CrashToTriggerCountdown �|�����w�ά��šA�Щ� Inspector ���w�H�קK���~");
            return; // ���_ Start�A�קK�᭱�{���~�����X��
        }
        isCounter = false;
        isFixed = false;

        machineScripts = new Machine[CrashToTriggerCountdown.Length];  // �w�qĲ�o�˼ƪ�����}�C
        brokeProgressScripts = new BrokeProgressAnime[CrashToTriggerCountdown.Length];

        for (int i = 0; i < CrashToTriggerCountdown.Length; i++)  // �w�qĲ�o�˼ƪ�����}�C �W�[
        {

            machineScripts[i] = CrashToTriggerCountdown[i].GetComponent<Machine>();     // �w�qĲ�o�˼ƪ����� �I�s Machine�}��
            brokeProgressScripts[i] = CrashToTriggerCountdown[i].GetComponent<BrokeProgressAnime>();   // �w�q�˼ƭp�� �I�s BrokeProgressAnime�}��
        } 
    }

    private void Update()
    {
        if (!isFixed && isCounter)
        {
            // �ˬd�O�_�� machine HP �k�s
            for (int i = 0; i < machineScripts.Length; i++)
            {
                if (machineScripts[i].HP <= 0.0f)
                {
                    //�p�G�@�[�Ȥp�󵥩�s
                    energizedEffect.SetActive(false);
                    energizedEffect.transform.Find("RadialProgressBar").GetComponent<CircularProgressBar>().StopCountdown();

                    // ��������Ӿ������@�[�� Coroutine
                    if (brokeProgressScripts[i] != null)
                    {
                        brokeProgressScripts[i].StopDamageOverTime();
                    }

                    isCounter = false;
                    return;
                }
            }

            // ���`�˼�
            duration -= Time.deltaTime;

            // �˼Ƶ���
            if (duration <= 0.0f)
            { 
                // �����˼Ʊ���
                isFixed = true;   // ���A����
                isEnergized = false;  // �˼ƹϥ�
                energizedEffect.SetActive(false); //�˼ƹϥ�����
                energizedEffect.transform.Find("RadialProgressBar").GetComponent<CircularProgressBar>().onCountdownFinished.Invoke(); //��ƭp�ɵ���

                // �����ɤ]�n���@�[�ȤU��
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

    void OnCollisionEnter2D(Collision2D coll)  //��I����ײz����
    {
        if (System.Array.Exists(CrashToTriggerCountdown, obj => obj == coll.gameObject))
        {
            isFixed = false;
            isCounter = true;
            isEnergized = true;
            energizedEffect.SetActive(true);
            duration = 5.0f;   //�˼ƭp�ɮɶ�

            energizedEffect.transform.Find("RadialProgressBar").GetComponent<CircularProgressBar>().ActivateCountdown(duration);
        }
    }

    void OnCollisionExit2D(Collision2D coll)   //��  �����I����ײz����
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
            UnityEditor.Handles.Label(transform.position + Vector3.up * 2f, "! CrashToTriggerCountdown ���]�w");
#endif
        }
    }
}
