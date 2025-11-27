using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotOnTable : MonoBehaviour
{
    private GameObject itemInContact = null; // 紀錄目前在 Slot 內的物件
    private bool hasItem = false; // 是否已放入物件

    [Header("放入物件的位置")]
    public Transform LiquidslotPoint;   // 液體放置點
    public Transform SpriteslotPoint;   // 元件放置點


    void Update()
    {
        // 如果目前有物件在slot裡，檢查玩家是否點擊了它
        if (itemInContact != null && Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
            Collider2D hit = Physics2D.OverlapPoint(worldPos);
            if (hit != null && hit.gameObject == itemInContact)
            {
                Debug.Log($"從 {name} 取出 {itemInContact.name}");

                // 恢復物件物理性質
                Rigidbody2D rb = itemInContact.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                }

                // 重新允許拖曳
                //   var dragScript = itemInContact.GetComponent<Dragging>();
                // if (dragScript != null)
                // {
                //     dragScript.enabled = true;
                //  }

                var dragScript2 = itemInContact.GetComponent<DraggableReturn2D>();  //會返回原位的 (不要的話註解這邊)
                if (dragScript2 != null)
                {
                    dragScript2.enabled = true;
                }

                // 清除slot狀態
                hasItem = false;
                itemInContact = null;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 僅放入液體
        if (LiquidslotPoint != null && hasItem == false &&
        (collision.CompareTag("redIiquid") || collision.CompareTag("yellowIiquid") || collision.CompareTag("blueIiquid") || collision.CompareTag("greenIiquid")) && !hasItem)
        {
            PlaceItemInSlot(collision.gameObject, LiquidslotPoint);
            hasItem = true;
        }

        // 僅放入元件
        if (SpriteslotPoint != null && hasItem == false &&
            (collision.CompareTag("brokecircle") || collision.CompareTag("square") || collision.CompareTag("triangle")) && !hasItem)
        {
            PlaceItemInSlot(collision.gameObject, SpriteslotPoint);
            hasItem = true;
        }
    }


    private void PlaceItemInSlot(GameObject obj, Transform slotPoint)
    {
        itemInContact = obj;

        // 放入 slot 位置
        obj.transform.position = slotPoint.position;
        obj.transform.rotation = slotPoint.rotation;

        // 停止物理移動
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        // 禁用拖曳
        // var dragScript = obj.GetComponent<Dragging>();  //不會返回原位的

        // if (dragScript != null)
        // {
        //     dragScript.enabled = false;
        // }


        var drag = obj.GetComponent<DraggableReturn2D>();  //會返回原位的 (不要的話註解這邊)
        if (drag != null)
        {
            drag.enabled = false;
            drag.SetNewOrigin(slotPoint.position);
        }



        Debug.Log($" {obj.name} 放入 {slotPoint.name}");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 當物件離開 slot 區域，允許再次拖曳
        if (collision.gameObject == itemInContact)
        {
            Rigidbody2D rb = itemInContact.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }

            //  Dragging dragScript = itemInContact.GetComponent<Dragging>();
            //  if (dragScript != null)
            // {
            //     dragScript.enabled = true; // 重新允許拖曳
            //}

            hasItem = false;
            itemInContact = null;
            Debug.Log($" {collision.gameObject.name} 離開 slot，可再次拖曳");
        }
    }
}