using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassWay : MonoBehaviour
{
    public Transform destination; //目標傳送地點
                                  // public Vector2 offset = new Vector2(1f, 0f); //傳送偏移
    private bool isTeleporting = false;  //是否重複傳送(防止重複傳送)
    GameObject passthing;


    private void Awake()
    {
        passthing = GameObject.FindGameObjectWithTag("passthing");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //如果 passthing 碰撞，傳送 passthing到 目標傳送地點
        if (collision.CompareTag("passthing") && !isTeleporting)
        {

            passthing.transform.position = destination.transform.position;
            StartCoroutine(TeleportObject(collision.gameObject));
        }

    }
    private IEnumerator TeleportObject(GameObject obj)
    {
        isTeleporting = true; //防止重複觸發傳送
        DontDestroyOnLoad(passthing.transform.root.gameObject); //必免物件被銷毀
        GameObject bgChangeObject = GameObject.Find("bg change"); //找到要換的場景
        if (bgChangeObject == null)
        {

            isTeleporting = false;
            yield return null;
        }




        if (destination != null)
        {
            //reset to drag 
            //Vector2 newPosition = (Vector2)destination.position + offset;
            //obj.transform.position = newPosition;
            SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>(); //確保被傳誦物件在場景可見
            if (renderer != null)
            {
                renderer.enabled = true;
            }
        }

        isTeleporting = false; //傳送完成，可再次觸發傳送
    }
}