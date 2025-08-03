using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelSelectionManager : MonoBehaviour
{
    public GameObject LevelSelectPanel;  //管理全按鈕
    public Button[] LevelSelectButton;   //按鈕選擇
    public int UnLockLevelIndex = -1; //解鎖

    void Start()
    {
        UnLockLevelIndex = PlayerPrefs.GetInt("UnLockLevelIndex", -1);//紀錄解鎖關卡
        //將LevelSelectPanel子物件存入LevelSelectButton
        LevelSelectButton = new Button[LevelSelectPanel.transform.childCount];
        for (int i = 0; i < LevelSelectPanel.transform.childCount; i++)
        {
            LevelSelectButton[i] = LevelSelectPanel.transform.GetChild(i).GetComponent<Button>();
        }

        for (int i = 0; i < LevelSelectButton.Length; i++)
        {
            LevelSelectButton[i].interactable = false;  //按鈕不可點
        }

        for (int i = 0; i < UnLockLevelIndex + 1; i++)
        {
            LevelSelectButton[i].interactable = true;  //依解鎖關卡 讓按鈕變得可以點擊

        }
    }

    public void ChangeScene(string Scence) //開始切換場景
    {
        SceneManager.LoadScene(Scence);

    }

    public void UnlockkLevel()  //解鎖關卡使用
    {
        UnLockLevelIndex++;
        PlayerPrefs.SetInt("UnLockLevelIndex", UnLockLevelIndex);//儲存新解鎖關卡
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
