using UnityEngine;

public class DraggableReturn2D : MonoBehaviour
{
    private Vector3 originalPosition;      // 原始位置
    public bool isDragging = false;       // 是否正在拖曳
    private Vector3 offset;                // 滑鼠點擊時的偏移

    void Start()
    {
        originalPosition = transform.position; // 記錄原始位置
    }

    void OnMouseDown()
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
                if (GameObject.FindWithTag("PCB").GetComponent<DraggableReturn2D>().isDragging == true)
                {

                    Debug.Log("撥放crab拿走動畫");
                    //GameObject.FindWithTag("PCB").GetComponent<CrabGM>().MachineAni.SetTrigger("takeout");
                    FindObjectOfType<CrabGM>().TakePCB();
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

    void OnMouseUp()
    {
        if (this.enabled)
        {
            isDragging = false;
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


    public void SetNewOrigin(Vector3 newPos)
    {
        //if (!gameObject.CompareTag("brokePCB"))
        // {
        originalPosition = newPos;
        //  }

    }
}
