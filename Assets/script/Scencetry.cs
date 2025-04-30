using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Scenetry : MonoBehaviour
{
    public static Scenetry instance;
  
    private void Awake()
    {
        if (instance==null)
        { 
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        { 
            Destroy(gameObject); 
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.D))
        {
            SceneManager.LoadScene("2");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("game2");
        }
    }
}
