using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerPlayUse : MonoBehaviour
{
    public GameObject spriteRenderer; // ��ܹϤ��� SpriteRenderer
    public Sprite[] OpenerChangeImage; // �i��ܪ��Ϥ��}�C
    public static int currentImageIndex = -1; // �O����e�Ϥ������ޡ]�Ω�I����󴫹Ϥ��^
    public GameObject First;   // �Ĥ@����ܪ�����
    public GameObject Second;  // �ĤG����ܪ�����
    public GameObject Third;   // �û����|��ܪ�����

    private int showCount = 0; // �p�ƾ��A�O����ܤF�X��

    void Start()
    {
        // �@�}�l���éҦ�����
        First.SetActive(false);
        Second.SetActive(false);
        Third.SetActive(false);

        // �Ұʨ�{�}�l�����������O�_�� fixitem
        StartCoroutine(CheckForFixItem());
    }

    // ��{�G�����ˬd�������O�_�����Ҭ� "fixitem" ������
    IEnumerator CheckForFixItem()
    {
        while (true)
        {
            // ��X�Ҧ� tag �� "fixeditemOpen" ������
            GameObject[] fixItems = GameObject.FindGameObjectsWithTag("fixeditemOpen");
            int count = fixItems.Length;

            // �C���������� First �M Second�]�קK������ܡ^
            First.SetActive(false);
            Second.SetActive(false);

            // �ھڼƶq��ܹ����� GameObject
            if (count == 2)
            {
                currentImageIndex = 0;

                // spriteRenderer.sprite = OpenerChangeImage[currentImageIndex];
                First.SetActive(true);
                Debug.Log("�������� 1 �� fixeditemOpen�A��� First");

            }

            else if (count == 3)
            {
                currentImageIndex = 1;
                //spriteRenderer.sprite = OpenerChangeImage[currentImageIndex]; // ��ܹϤ�
                Second.SetActive(true);
                Debug.Log("�������� 2 �� fixeditemOpen�A��� Second");

            }
            else
            {
                // ��l���p�]0�өζW�L2�ӡ^�������
                Debug.Log($"�������� {count} �� fixeditemOpen�A�S��������ܪ���");
            }

            // �C�@�V�����ˬd�]�A�i�H�令�C0.2��H��֮į���ӡ^
            yield return null;
        }
    }
}
