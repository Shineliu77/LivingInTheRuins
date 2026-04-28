using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    public Button[] ClickButton;
    private bool isClearLevel;
    public void PlayGame()
    {
        int ISClearLevel = PlayerPrefs.GetInt("ClearLevel", 0);
        isClearLevel = (ISClearLevel == 1);
        if (isClearLevel == true)
        {
            SceneManager.LoadSceneAsync("lobby");
        }
        else
        {
            SceneManager.LoadSceneAsync("StoryFirst");
        }
        AudioManager.Instance.PlaySfx(25);                                              //音效

    }
    public void HomeGame()
    {
        SceneManager.LoadSceneAsync("main menu");
        AudioManager.Instance.PlaySfx(25);                                              //音效

    }
    //public void NextGame()
    // {
    //  SceneManager.LoadSceneAsync("fix");

    //}
    public void QuitGame()
    {
        Application.Quit();
        AudioManager.Instance.PlaySfx(25);                                              //音效
    }


    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        AudioManager.Instance.PlaySfx(25);
    }

    public void ClickButtonSfx()
    {
        AudioManager.Instance.PlaySfx(25);
    }
    public void EscClickButtonSfx()
    {
        AudioManager.Instance.PlaySfx(25);
    }
    public void DeletData()
    {
        PlayerPrefs.DeleteAll();
        // SceneManager.LoadSceneAsync("main menu");
    }
}
