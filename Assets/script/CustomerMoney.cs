using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CustomerMoney : MonoBehaviour
{
    //public Customer customer;  //�I�s �ȤH�{��
    public static int Score;  //�o������
    public TextMeshProUGUI ShowScore; //��rUI
                                      //void GetMoney() { }  WinGame() {if i++ >= 50; print("finish");}}
    void Start()
    {
        //��l���� 0
        Score = 0;
        ShowScore = GetComponentInChildren<TextMeshProUGUI>();
    }
    void Update()
    {
        ShowScore.text = Score.ToString();  //��UI��r�P���ƦP�B
    }
    public void GetMoney(int amount)
    {
        Score += amount;
    }
}