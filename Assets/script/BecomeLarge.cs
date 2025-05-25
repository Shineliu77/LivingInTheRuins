using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BecomeLarge : MonoBehaviour
{
    public bool isColliding = false;  // �O�_�Q�I��
    private Vector3 originalScale;     // �x�s�쥻�j�p
    public Vector3 scaleChange = new Vector3(1.5f, 1.5f, 1.5f);   // �i�թ�j�ؤo

    void Start()
    {
        originalScale = transform.localScale;  // �����쥻�ؤo
    }

    // ��i�J�I����
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BecomeLarge") && !isColliding)
        {
            isColliding = true;
            transform.localScale = originalScale + scaleChange;  // ��j
            NewPlayerTeach teachScript = FindObjectOfType<NewPlayerTeach>(); //�Ȧb�I���� BecomeLarge �~�}�ұо�
            if (teachScript != null)
            {
                teachScript.IscollBecameLarge();
            }
        }
        if (!isColliding && collision.gameObject.CompareTag("BecomeLarge2"))
        {
            isColliding = true;
            transform.localScale = originalScale + scaleChange;  // ��j

        }
    }


    // �����}�I����
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BecomeLarge") && isColliding)
        {
            isColliding = false;
            transform.localScale = originalScale;  // �ܦ^�쥻�ؤo
        }

        if (collision.gameObject.CompareTag("BecomeLarge2") && isColliding)
        {
            isColliding = false;
            transform.localScale = originalScale;  // �ܦ^�쥻�ؤo
        }
    }
}