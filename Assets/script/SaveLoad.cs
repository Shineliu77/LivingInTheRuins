using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SaveLoad : MonoBehaviour
{
    public Text scoreText;  // 掛在大廳的顯示 UI

    public Image TenThousandsDigitImage;     // 萬位數的圖片
    public Image ThousandsDigitImage;     // 千位數的圖片
    public Image HundredsDigitImage;     // 百位數的圖片
    public Image TensDigitImage;     // 十位數的圖片
    public Image UnitsDigitImage;     // 個位數的圖片
    public Sprite[] numberSprites;   // 0~9 對應的圖片
    public int savedScore;
    void Start()
    {
        Load(); // 一開始就讀取儲存分數
    }

    public void Load()
    {
        savedScore = PlayerPrefs.GetInt("SavedScore", 0);  // 預設為0
        if (scoreText != null)
        {
            scoreText.text = savedScore.ToString(); // 顯示儲存分數
            ChangeShowNunber();
        }
    }

    public void ChangeShowNunber()
    {
        // 位數的圖片的換圖
        int ChangeNumberImg = savedScore;
        int TenThousands = (savedScore / 10000) % 10;
        int Thousands = (savedScore / 1000) % 10;
        int Hundreds = (savedScore / 100) % 10;
        int Tens = (savedScore / 10) % 10;
        int Units = savedScore % 10;

        TenThousandsDigitImage.sprite = numberSprites[TenThousands];
        ThousandsDigitImage.sprite = numberSprites[Thousands];
        HundredsDigitImage.sprite = numberSprites[Hundreds];
        TensDigitImage.sprite = numberSprites[Tens];
        UnitsDigitImage.sprite = numberSprites[Units];
    }
}