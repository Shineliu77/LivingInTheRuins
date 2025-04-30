using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassWay : MonoBehaviour
{
    public Transform destination; //�ؼжǰe�a�I
                                  // public Vector2 offset = new Vector2(1f, 0f); //�ǰe����
    private bool isTeleporting = false;  //�O�_���ƶǰe(����ƶǰe)
    GameObject passthing;


    private void Awake()
    {
        passthing = GameObject.FindGameObjectWithTag("passthing");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�p�G passthing �I���A�ǰe passthing�� �ؼжǰe�a�I
        if (collision.CompareTag("passthing") && !isTeleporting)
        {

            passthing.transform.position = destination.transform.position;
            StartCoroutine(TeleportObject(collision.gameObject));
        }

    }
    private IEnumerator TeleportObject(GameObject obj)
    {
        isTeleporting = true; //�����Ĳ�o�ǰe
        DontDestroyOnLoad(passthing.transform.root.gameObject); //���K����Q�P��
        GameObject bgChangeObject = GameObject.Find("bg change"); //���n��������
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
            SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>(); //�T�O�Q�ǻw����b�����i��
            if (renderer != null)
            {
                renderer.enabled = true;
            }
        }

        isTeleporting = false; //�ǰe�����A�i�A��Ĳ�o�ǰe
    }
}