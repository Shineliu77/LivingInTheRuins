using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;//�v�J����
public class Timer : MonoBehaviour
{
    //timelimit for level
    [SerializeField] TextMeshProUGUI timerText;  //�ɶ���r
    [SerializeField] float remainingTime; //�p�ɾ��ɶ��Ѿl�]�w
    void Update()
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

        else if (remainingTime <= 0)
        {
            //the way to reset thing
            //Coin.CoinCount = 0;
           // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//�i���J����
            //SceneManager.LoadScene("");
        }
        int minutes = Mathf.FloorToInt(remainingTime / 60); //�Ѿl ��
        int seconds = Mathf.FloorToInt(remainingTime % 60); //�Ѿl ��
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); //ui��ܤ覡
                                                                           //�˼Ƶ������s�C��

    }
}
