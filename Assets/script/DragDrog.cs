using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrog : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{

    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform; //計算物件移動
    private CanvasGroup canvasGroup;  //群組物件可確保被拖曳
    private Vector2 offset; //拖曳偏移量
    private GameObject setupitem;
    private Vector3 originalPosition;

    public System.Action<GameObject> OnEndDragEvent;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogWarning($"GameObject {gameObject.name} 沒有 CanvasGroup，跳過此設定");
        }

        //設置讓 setupitem 可以在結束拖曳時回到原位
        if (gameObject.CompareTag("setupitem"))
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            originalPosition = rectTransform.position;
        }

    }

    //開始拖曳
    public void OnBeginDrag(PointerEventData eventData)
    {

        if (canvasGroup != null)
        {
            canvasGroup.alpha = .6f; //設置透明(表 被拖曳)
            canvasGroup.blocksRaycasts = false;  //確保不會影響其他ui
        }

    }

    //讓拖曳中的物件隨著滑鼠移動，並確保根據 Canvas 縮放比例調整移動距離，避免偏移
    public void OnDrag(PointerEventData eventData)
    {

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    //拖曳結束
    public void OnEndDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f; //恢復透明度
            canvasGroup.blocksRaycasts = true; //確保物件可以交互
        }


    }

    //測試是否點到物件
    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("onPointerDown");
    }

    //拖曳結束當滑鼠放開
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
