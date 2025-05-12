using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CustomerMoney : MonoBehaviour
{
    //public Customer customer;  //呼叫 客人程式
    public static int Score;  //得錢分數
    public TextMeshProUGUI ShowScore; //文字UI
                                      //void GetMoney() { }  WinGame() {if i++ >= 50; print("finish");}}
    void Start()
    {
        //初始錢為 0
        Score = 0;
        ShowScore = GetComponentInChildren<TextMeshProUGUI>();
    }
    void Update()
    {
        ShowScore.text = Score.ToString();  //讓UI文字與分數同步
    }
    public void GetMoney(int amount)
    {
        Score += amount;
    }
}