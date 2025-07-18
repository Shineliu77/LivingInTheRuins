using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreGM : MonoBehaviour
{
    public int[] RepairScore;
    public int TotalScore;
    public Text ScoreText;
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
