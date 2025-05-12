using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BrokeProgressAnime : MonoBehaviour
{
    public GameObject FixItemMachine; //  �ײz�����󪺾���

    public GameObject[] ShouldFixed; //  �n�Q�ײz������
    public Image brokebar; //�@�[�ȱ�
                           // public GameObject energizedEffect; //  �״_�ʵe�ĪG
    public bool isEnergized = false; // �O�_���b�״_
    public CircularProgressBar progressBar; // �I�s CircularProgressBar(�Q���ת����~��)
    public bool isDamaged = false; // �O�_�B��@�[�ȱ����� �U��  ���A

    private Coroutine brokeCoroutine; // �@�[�ȱ����� �U��
    private Coroutine fixCoroutine;   // �@�[�ȱ����� �״_

    private bool isDamaging = false; // ����O�_���b�@�[�ȱ����� �U��

    private int collidingObjects = 0; // ��e�P ShouldFixed ����Ĳ���ƶq


    private void Start()
    {
        // �j�w CircularProgressBar �����ƥ�A�Ω�״_�����ɰ����
        if (progressBar != null)
        {
            progressBar.onCountdownFinished.AddListener(StopDamageOverTime);
        }

    }

    // ��s�@�[�� UI
    public void Refreshbrokebar()
    {
        if (FixItemMachine != null)
        {
            Machine machineScript = FixItemMachine.GetComponent<Machine>();
            brokebar.fillAmount = machineScript.HP / machineScript.HPMax;
        }
    }

    // �I���i�J
    private void OnCollisionEnter2D(Collision2D coll)
    {
        // �I�� ShouldFixed ������
        if (System.Array.Exists(ShouldFixed, obj => obj == coll.gameObject))
        {
            collidingObjects++; // �O���I��������ƶq
            isDamaged = true;

            // �p�G���״_��{�A����״_
            if (fixCoroutine != null)
            {
                StopCoroutine(fixCoroutine);
                fixCoroutine = null;
            }

            // �p�G�٨S�b����A�Ұʱ����{
            //if (brokeCoroutine == null)
            // {
            //    brokeCoroutine = StartCoroutine(DamageOverTime());
            //}
            if (!isDamaging)
            {
                brokeCoroutine = StartCoroutine(DamageOverTime());
            }

            // �����״_�ʵe
            //energizedEffect.SetActive(false);
            isEnergized = false;
        }

        //tag��fixiem���~�I��
        if (coll.gameObject.CompareTag("fixeditem"))
        {
            Debug.Log("�I����: " + coll.gameObject.name + ", Tag: " + coll.gameObject.tag);
            isDamaged = true;

            // �p�G���״_��{�A����״_
            if (fixCoroutine != null)
            {
                StopCoroutine(fixCoroutine);
                fixCoroutine = null;
            }

            // �p�G�٨S�b����A�Ұʱ����{
            if (brokeCoroutine == null)
            {
                brokeCoroutine = StartCoroutine(DamageOverTime());
            }

            // �����״_�ʵe
            //energizedEffect.SetActive(false);
            isEnergized = false;
        }

        // �I�� machinefix (�ײz��)
        // if (coll.gameObject.CompareTag("machinefix"))
        // {
        // �u���b�S���l�a�ɤ~�i�H�״_
        // if (!isDamaged && fixCoroutine == null)
        // {
        //   fixCoroutine = StartCoroutine(FixTime());
        //   isEnergized = true;
        // energizedEffect.SetActive(true);
        //}
        //}
    }

    // �I������
    void OnCollisionExit2D(Collision2D coll)
    {
        // ���} ShouldFixed ������
        if (System.Array.Exists(ShouldFixed, obj => obj == coll.gameObject))
        {
            // collidingObjects--; // ��֭p��
            //if (collidingObjects <= 0)
            //{
            //   isDamaged = false;
            //StopDamageOverTime();
            //  }
        }


        // ���} tag��fixiem���~�I��
        if (coll.gameObject.CompareTag("fixeditem"))
        {
            collidingObjects--; // ��֭p��
            if (collidingObjects <= 0)
            {
                isDamaged = false;
                StopDamageOverTime();
            }
            fixCoroutine = StartCoroutine(FixTime());   //�����D   �|�����^��
            isEnergized = true;
        }


        // ���} machinefix
        // if (coll.gameObject.CompareTag("machinefix"))
        // {
        // ���b�l�a���A�~�i�H���s�Ұʭ״_
        // if (!isDamaged && fixCoroutine == null)
        // {
        //    fixCoroutine = StartCoroutine(FixTime());
        //    isEnergized = true;
        //  energizedEffect.SetActive(true);
        //}


        // ���} �I��
        if (!isDamaged && fixCoroutine == null)
        {
            isDamaged = false;
            StopDamageOverTime();
            fixCoroutine = StartCoroutine(FixTime());   //�����D   �|�����^��
            isEnergized = true;
            //  energizedEffect.SetActive(true);
        }

    }

    // �C���C������ HP
    IEnumerator DamageOverTime()
    {
        isDamaging = true; // �}�l����

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
        Debug.Log("���հ���@�[�ȤU��");

        isDamaging = false; // �����j��
        if (brokeCoroutine != null)
        {
            StopCoroutine(brokeCoroutine);
            brokeCoroutine = null;
            Debug.Log("���\������{");
        }
        Refreshbrokebar();
    }

    // �״_�����@�[��
    IEnumerator FixTime()
    {
        if (FixItemMachine != null)
        {
            Machine machineScript = FixItemMachine.GetComponent<Machine>();
            if (machineScript == null) yield break;

            while (machineScript.HP < machineScript.HPMax)
            {
                // �C��^�_ 10%
                machineScript.HP += machineScript.HPMax * 0.1f;

                if (machineScript.HP >= machineScript.HPMax)
                {
                    machineScript.HP = machineScript.HPMax;

                    // �״_���������ʵe
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