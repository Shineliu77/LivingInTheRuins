using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrog : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{

    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform; //�p�⪫�󲾰�
    private CanvasGroup canvasGroup;  //�s�ժ���i�T�O�Q�즲
    private Vector2 offset; //�즲�����q
    private GameObject setupitem;
    private Vector3 originalPosition;

    public System.Action<GameObject> OnEndDragEvent;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogWarning($"GameObject {gameObject.name} �S�� CanvasGroup�A���L���]�w");
        }

        //�]�m�� setupitem �i�H�b�����즲�ɦ^����
        if (gameObject.CompareTag("setupitem"))
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            originalPosition = rectTransform.position;
        }

    }

    //�}�l�즲
    public void OnBeginDrag(PointerEventData eventData)
    {

        if (canvasGroup != null)
        {
            canvasGroup.alpha = .6f; //�]�m�z��(�� �Q�즲)
            canvasGroup.blocksRaycasts = false;  //�T�O���|�v�T��Lui
        }

    }

    //���즲���������H�۷ƹ����ʡA�ýT�O�ھ� Canvas �Y���ҽվ㲾�ʶZ���A�קK����
    public void OnDrag(PointerEventData eventData)
    {

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    //�즲����
    public void OnEndDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f; //��_�z����
            canvasGroup.blocksRaycasts = true; //�T�O����i�H�椬
        }


    }

    //���լO�_�I�쪫��
    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("onPointerDown");
    }

    //�즲������ƹ���}
    public void OnDrop(PointerEventData eventData)
    {

        if (gameObject.CompareTag("setupitem"))
        {
            rectTransform.position = originalPosition;
        }

        if (gameObject.CompareTag("fixeditem"))
        {

        }
    }
}
