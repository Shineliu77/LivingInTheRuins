using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Mainmenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("StoryFirst");


    }
    public void HomeGame()
    {
        SceneManager.LoadSceneAsync("main menu");

    }
    //public void NextGame()
    // {
    //  SceneManager.LoadSceneAsync("fix");

    //}
    public void QuitGame()
    {
        Application.Quit();
    }


    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
