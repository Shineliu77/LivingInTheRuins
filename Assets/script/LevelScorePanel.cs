using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelScorePanel : MonoBehaviour
{
    public Image HundredsDigitImage;     // 百位數的圖片
    public Image TensDigitImage;     // 十位數的圖片
    public Image UnitsDigitImage;     // 個位數的圖片
    public Sprite[] numberSprites;   // 0~9 對應的圖片

    private ScoreGM scoreGM;
    private int lastScore = -1;  // 用來判斷分數是否有變動

    void Awake()
    {
        // 自動找 ScoreGM（也可手動拖）
        scoreGM = FindObjectOfType<ScoreGM>();
    }

    void Update()
    {
        if (scoreGM == null) return;

        // 只有當分數變化時才更新 UI
        if (scoreGM.TotalScore != lastScore)
        {
            lastScore = scoreGM.TotalScore;
            ShowScore(lastScore);
        }
    }

    public void ShowScore(int score)
    {
        int clamped = Mathf.Clamp(score, 0, 999);
        int hundreds = clamped / 100;
        int tens = (clamped / 10) % 10;
        int units = clamped % 10;

        HundredsDigitImage.sprite = numberSprites[hundreds];
        TensDigitImage.sprite = numberSprites[tens];
        UnitsDigitImage.sprite = numberSprites[units];
    }

}
