using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassWayTwo : MonoBehaviour
{
    public Transform destination;
    public static Scenetry instance;
    public Vector2 offset = new Vector2(1f, 0f);
    private bool isTeleporting = false;
    GameObject passthing;


    private void Awake()
    {
        passthing = GameObject.FindGameObjectWithTag("passthing");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("passthing"))
        {

            passthing.transform.position = destination.transform.position;
            StartCoroutine(TeleportObject(collision.gameObject));
        }

    }
    private IEnumerator TeleportObject(GameObject obj)
    { // about passing thing and pass it to scene
        isTeleporting = true;
        DontDestroyOnLoad(passthing);
        SceneManager.LoadScene("2");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(2);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        destination = GameObject.FindGameObjectWithTag("destination").transform;


        if (destination != null)
        {
            Vector2 newPosition = (Vector2)destination.position + offset;
            obj.transform.position = newPosition;
            SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.enabled = true;
            }
        }

        isTeleporting = false;
    }
}