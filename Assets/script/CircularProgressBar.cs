using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CircularProgressBar : MonoBehaviour
{
    private bool isActive = false; //�O�_�˼�
    private float indicatorTimer;
    private float maxIndicatorTimer;

    public Image radialProgressBar;
    public UnityEvent onCountdownFinished;

    public BrokeProgressAnime brokeProgressAnime;
    public GameObject[] CrashToTriggerPICountdown; // �I��Ĳ�o�������s��
    private Machine[] machineScripts; // �h�� machine ���}�C

    private void Awake()
    {
        radialProgressBar = GetComponent<Image>(); //�] �Ϥ���˼ƶi�ױ�

        if (onCountdownFinished == null) //�T�O  UnityEvent ���O�Ū�
        {
            onCountdownFinished = new UnityEvent();
        }
    }

    private void Start()
    {
        //��l�� machineScripts �}�C
        machineScripts = new Machine[CrashToTriggerPICountdown.Length];

        for (int i = 0; i < CrashToTriggerPICountdown.Length; i++)
        {
            machineScripts[i] = CrashToTriggerPICountdown[i].GetComponent<Machine>();
        }
    }

    private void Update()
    {
        if (isActive)  //�Ұ� �˼� �˼ƹϤ�
        {
            indicatorTimer -= Time.deltaTime;
            radialProgressBar.fillAmount = (indicatorTimer / maxIndicatorTimer);
            
            if (indicatorTimer <= 0)   //�˼Ƶ��� ������ �P�ϥ�
            {
                StopCountdown();
                onCountdownFinished.Invoke();
            }
        }
    }

    //�}�l�˼�
    public void ActivateCountdown(float countdownTime)
    {
        isActive = true;
        maxIndicatorTimer = countdownTime;
        indicatorTimer = maxIndicatorTimer;
    }

    //����˼ơ]�����^
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

    //�Ȱ��˼� (HP <= 0)
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