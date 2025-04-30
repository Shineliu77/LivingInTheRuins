using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Mainmenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("gamelive2dtrytry2d");


    }
    public void HomeGame()
    {
        SceneManager.LoadSceneAsync("main menu");

    }
    public void NextGame()
    {
        SceneManager.LoadSceneAsync("fix");

    }
    public void QuitGame()
    {
        Application.Quit();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) { SceneManager.LoadScene("gamelive2dtrytry2d"); }
    }
}
