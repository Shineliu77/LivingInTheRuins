using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreGM : MonoBehaviour
{
    public int[] RepairScore;
    public int TotalScore;
    public Text ScoreText;

    public int AllScore = 0; //紀錄到大廳的總分數 
    public Image HundredsDigitImage;     // 百位數的圖片
    public Image TensDigitImage;     // 十位數的圖片
    public Image UnitsDigitImage;     // 個位數的圖片
    public Sprite[] numberSprites;   // 0~9 對應的圖片

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddScore(int ID)
    {
        TotalScore+=RepairScore[ID];
        ScoreText.text = TotalScore + "";
    }
}
