using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreGM : MonoBehaviour
{
    public int TotalScore;

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
    public void AddScore()
    {
        TotalScore+=50;
        UpdateScoreUI();

    }
    void UpdateScoreUI()
    {
        // 限制分數最大到 999（避免超出圖片範圍）
        int score = Mathf.Clamp(TotalScore, 0, 999);

        int hundreds = score / 100;       // 百位
        int tens = (score / 10) % 10;     // 十位
        int units = score % 10;           // 個位

        // 設定對應的圖片
        HundredsDigitImage.sprite = numberSprites[hundreds];
        TensDigitImage.sprite = numberSprites[tens];
        UnitsDigitImage.sprite = numberSprites[units];
    }
}
