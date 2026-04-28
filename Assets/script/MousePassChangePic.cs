using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class MousePassChangePic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image TargetPic;
    public Sprite OriginPic;
    public Sprite ChangePic;
    private bool isDragging = false;
    // Start is called before the first frame update
    void Start()
    {
        TargetPic.sprite = OriginPic;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isDragging)
        {
            TargetPic.sprite = ChangePic;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isDragging)
        {
            TargetPic.sprite = OriginPic;
        }
    }
}
