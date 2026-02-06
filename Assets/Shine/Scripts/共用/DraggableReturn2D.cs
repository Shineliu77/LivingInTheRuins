using UnityEngine;
using System;
public class DraggableReturn2D : MonoBehaviour
{
    private Vector3 originalPosition;      // 原始位置
    public bool isDragging = false;       // 是否正在拖曳
    private Vector3 offset;                // 滑鼠點擊時的偏移
    public static event Action<DraggableReturn2D> OnReleased;  //放開滑鼠用
    private FixMachineDurabilityChangeImage ChangeImageOBJ;//換圖共用
    private Collider2D col;
    void Start()
    {
        originalPosition = transform.position; // 記錄原始位置
        col = GetComponent<Collider2D>();
        ChangeImageOBJ = GetComponent<FixMachineDurabilityChangeImage>();
    }

    public void OnMouseDown()
    {
        if (this.enabled)
        {
            // 記錄偏移量，避免物件跳動
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            offset = transform.position - new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z);
            isDragging = true;
            // if (Application.loadedLevelName == "FirstGame")
            {
                Debug.Log("PCB 開始拖曳1");
                // if (GameObject.FindWithTag("PCB").GetComponent<DraggableReturn2D>().isDragging == true)
                if (gameObject.CompareTag("PCB"))
                {

                    Debug.Log("撥放crab拿走動畫");
                    //GameObject.FindWithTag("PCB").GetComponent<CrabGM>().MachineAni.SetTrigger("takeout");
                    FindObjectOfType<CrabGM>().TakePCB();
                    Debug.Log("元件 開始拖曳1");
                }

                else if (gameObject.CompareTag("brokecircle"))  //將其改成public仍無法切換動畫
                {

                    Debug.Log("元件brokecircle開始拖曳");

                    FindObjectOfType<RabbitGM>().Takecircle();
                }

                else if (gameObject.CompareTag("square"))  //將其改成public仍無法切換動畫
                {

                    Debug.Log("元件brokecircle開始拖曳");

                    FindObjectOfType<RabbitGM>().Takesquare();
                }

                else if (gameObject.CompareTag("triangle"))  //將其改成public仍無法切換動畫
                {

                    Debug.Log("元件brokecircle開始拖曳");

                    FindObjectOfType<RabbitGM>().Taketriangle();
                }
            }
        }
    }

    void OnMouseDrag()
    {
        if (isDragging && this.enabled)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z) + offset;
        }
    }

    public void OnMouseUp()
    {
        if (this.enabled)
        {
            isDragging = false;
            OnReleased?.Invoke(this);
            if (this.gameObject.tag == "fixeditemOpen")
            {
                if (this.transform.parent.childCount > 0)
                    transform.localPosition = Vector3.zero;
            }
            else
            {
                // 放開滑鼠，回到原位（可改成用協程慢慢移動）
                transform.position = originalPosition;

            }

        }
    }

    //滑鼠經過換圖
    public void OnMouseEnter()
    {
        if (ChangeImageOBJ != null && this.enabled == true)
        {
            ChangeImageOBJ.ChangePicture();
        }
    }
    public void OnMouseExit()
    {
        if (ChangeImageOBJ != null)
        {
            ChangeImageOBJ.ChangeOrigin();
        }
    }

    public Collider2D GetCollider() => col;
    public void SetNewOrigin(Vector3 newPos)
    {
        //if (!gameObject.CompareTag("brokePCB"))
        // {
        originalPosition = newPos;
        //  }

    }
}
