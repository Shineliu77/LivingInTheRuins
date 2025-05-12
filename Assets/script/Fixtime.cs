using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // �o��ܭ��n

public class Fixtime : MonoBehaviour
{
    public GameObject energizedEffect; //�˼ƹϥ�
    public bool isEnergized; //�˼ƹϥܶ}��
    public float duration = 10.0f;  // �˼Ʈɶ�
    public GameObject[] CrashToTriggerCountdown;   // Ĳ�o�˼ƪ�����

    public string[] FindToTriggerCountdownNames;  // �s�W�G�������۰ʴM�䪺����W�r
    public GameObject[] findInToTriggerObjects; // ��쪺��������

    private Machine[] machineScripts; //�I�s machin�{��
    private BrokeProgressAnime[] brokeProgressScripts; //�I�s BrokeProgressAnime�{��

    bool isCounter = false;  // �O�_�˼�
    bool isFixed = false;    // �O�_����

    private void Start()
    {
        isCounter = false;
        isFixed = false;

        // �p�G CrashToTriggerCountdown �S����ʳ]�w�A�N���ե�Find��  (����������)
        if ((CrashToTriggerCountdown == null || CrashToTriggerCountdown.Length == 0) && FindToTriggerCountdownNames != null && FindToTriggerCountdownNames.Length > 0)
        {
            findInToTriggerObjects = new GameObject[FindToTriggerCountdownNames.Length];  //�w�q findInToTriggerObjects �}�C

            for (int i = 0; i < FindToTriggerCountdownNames.Length; i++)
            {
                GameObject obj = GameObject.Find(FindToTriggerCountdownNames[i]);
                if (obj != null)
                {
                    findInToTriggerObjects[i] = obj;
                }
                else
                {
                    Debug.LogWarning($"Fixtime: �L�k�b���������W�٬� {FindToTriggerCountdownNames[i]} ������");
                }

            }
        }

        if (CrashToTriggerCountdown != null && CrashToTriggerCountdown.Length > 0)   //��l CrashToTriggerCountdown �� CrashToTriggerCountdown.Length
        {
            machineScripts = new Machine[CrashToTriggerCountdown.Length];
            brokeProgressScripts = new BrokeProgressAnime[CrashToTriggerCountdown.Length];

            for (int i = 0; i < CrashToTriggerCountdown.Length; i++)  // �w�qĲ�o�˼ƪ�����}�C �W�[
            {
                if (CrashToTriggerCountdown[i] != null)   //�h�@�h�O�@(����CrashToTriggerCountdown���ų���)
                {
                    machineScripts[i] = CrashToTriggerCountdown[i].GetComponent<Machine>();     // �w�qĲ�o�˼ƪ����� �I�s Machine�}��
                    brokeProgressScripts[i] = CrashToTriggerCountdown[i].GetComponent<BrokeProgressAnime>();   // �w�q�˼ƭp�� �I�s BrokeProgressAnime�}��

                }
            }
        }
        // ---�i��l�ƾ����P�˼ưʵe�}���}�C�j---
        int arrayLength = 0;

        if (CrashToTriggerCountdown != null && CrashToTriggerCountdown.Length > 0)
        {
            arrayLength = CrashToTriggerCountdown.Length; // �ϥΤ�ʳ]�w���}�C
        }
        else if (findInToTriggerObjects != null && findInToTriggerObjects.Length > 0)
        {
            arrayLength = findInToTriggerObjects.Length; // �ϥΦ۰ʧ�쪺�}�C
        }

        machineScripts = new Machine[arrayLength];
        brokeProgressScripts = new BrokeProgressAnime[arrayLength];

        // ---�i�]�wmachineScripts�MbrokeProgressScripts�j---
        for (int i = 0; i < arrayLength; i++)
        {
            GameObject targetObj = null;

            // �u���ϥ�CrashToTriggerCountdown�A�S���~��findInToTriggerObjects
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

                // ���b�G�p�G�䤣��}���A����ĵ�i
                if (machineScripts[i] == null)
                    Debug.LogWarning($"Fixtime: {targetObj.name} �S�� Machine �}���I");
                if (brokeProgressScripts[i] == null)
                    Debug.LogWarning($"Fixtime: {targetObj.name} �S�� BrokeProgressAnime �}���I");
            }
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
        if (findInToTriggerObjects.Contains(coll.gameObject))   //����I������ 
        {
            isFixed = false;
            isCounter = true;
            isEnergized = true;
            energizedEffect.SetActive(true);
            duration = 5.0f;   //�˼ƭp�ɮɶ�

            energizedEffect.transform.Find("RadialProgressBar").GetComponent<CircularProgressBar>().ActivateCountdown(duration);
        }

        //�d��tagĲ�o
        if (coll.gameObject.CompareTag("fixeditem"))   //����I������        �ȤH�a�Ӫ�����  �ثe�L��
        {
            isFixed = false;
            isCounter = true;
            isEnergized = true;
            energizedEffect.SetActive(true);
            duration = 5.0f;   //�˼ƭp�ɮɶ�

            energizedEffect.transform.Find("RadialProgressBar").GetComponent<CircularProgressBar>().ActivateCountdown(duration);
        }

        //�d��tagĲ�o
        if (coll.gameObject.CompareTag("Red"))   //����I������  �����պ޾��������� 
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
        if (coll.gameObject.CompareTag("Red"))
        {
            isFixed = false;
            isCounter = true;
            isEnergized = true;
            energizedEffect.SetActive(true);
            return; // �Y Red ���S��B�z�޿�Areturn �i�קK�i�J�U���ˬd
        }



        if (coll.gameObject.CompareTag("fixeditem") && System.Array.Exists(findInToTriggerObjects, obj => obj == coll.gameObject))
        {
            //coll.gameObject.CompareTag("fixeditem")���ŦXĲ�o����
            isFixed = true;
            isCounter = false;
            isEnergized = false;
            energizedEffect.SetActive(false);
        }
        //�F��䤤�ӱ���NĲ�o
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
    //  UnityEditor.Handles.Label(transform.position + Vector3.up * 2f, "! CrashToTriggerCountdown ���]�w");
#endif
}