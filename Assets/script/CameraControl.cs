using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private GameObject Scene;
    private GameObject Scene2;

    private void Awake()
    {
        Scene = GameObject.FindGameObjectWithTag("bg");
        Scene2 = GameObject.FindGameObjectWithTag("bgchange");
        DontDestroyOnLoad(Scene);
        DontDestroyOnLoad(Scene2);
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Scene == null) return;

        if (Input.GetKeyDown(KeyCode.S))
        {
            Scene.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Scene.SetActive(true);
        }
    }
}
