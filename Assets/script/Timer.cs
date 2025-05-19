using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;//�v�J����
public class Timer : MonoBehaviour
{
    //timelimit for level
    [SerializeField] TextMeshProUGUI timerText;  //�ɶ���r
    [SerializeField] float remainingTime = 75f; //�p�ɾ��ɶ��Ѿl�]�w

    public GameObject ScorePanel;  // ���w�n�}��������
    private bool ScorePanelOpen = false;   // �O���ثe�O�_���}�Ҫ��A

    public CustomerMoney customerMoney; //�������Ƶ{��

    private bool isTimerPaused = false;  //����ɶ��O�_�Ȱ�
    void Update()
    {
        // �p�G�S���Ȱ��A�~����˼ƻP��ܧ�s
        if (!isTimerPaused)
        {
            //�Ѿl�ɶ��j��s�i�~��˼�
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime; //��C���g�L�ɶ�
            }
            //�T�O���|�X�{�t�ơA�ñN�ɶ��k�s
            else if (remainingTime < 0)
            {
                remainingTime = 0;
            }

            else if (remainingTime <= 0 && !ScorePanelOpen)  //�p�G�˼Ƭ���k�s ���}���ƭ��O�Ȱ��C��
            {
                ScorePanel.SetActive(true); // ��ܤ��ƭ��O
                ScorePanelOpen = true;          // �����Ĳ�o
                isTimerPaused = true;  // �[�o��G�Ȱ��ɶ��˼�
                Time.timeScale = 0;  //�C���Ȱ�
                //the way to reset thing
                // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//�i���J����
                //SceneManager.LoadScene("");
            }
            int minutes = Mathf.FloorToInt(remainingTime / 60); //�Ѿl ��
            int seconds = Mathf.FloorToInt(remainingTime % 60); //�Ѿl ��
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); //ui��ܤ覡

           if (CustomerMoney.Score >= 100 && !ScorePanelOpen)  //��������100�� ���}���ƭ��O�Ȱ��C��
           {
            ScorePanel.SetActive(true); // ��ܤ��ƭ��O
            ScorePanelOpen = true;          // �����Ĳ�o
             isTimerPaused = true;  // �[�o��G�Ȱ��ɶ��˼�
             Time.timeScale = 0;  //�C���Ȱ�
           }

        }
    }
}
