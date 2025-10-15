using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ItemSlot : MonoBehaviour
{
    private GameObject pcbInContact = null; // 紀錄目前碰撞到的 PCB
    private bool hasPCB = false; // 這個 slot 是否已經放入 PCB
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PCB") && !hasPCB)
        {
            pcbInContact = collision.gameObject;
            pcbInContact.transform.SetParent(transform);
        }
    }
    private void Update()
    {
        if (pcbInContact != null)
        {
            var drag = pcbInContact.GetComponent<DraggableReturn2D>();
            if (drag != null && !drag.isDragging && !hasPCB)
            {
                pcbInContact.transform.position = transform.position;
                var rb = pcbInContact.GetComponent<Rigidbody2D>();
                if (rb != null) rb.velocity = Vector2.zero; hasPCB = true; // 標記此 slot 已被佔用 Debug.Log("PCB 放入 Slot 成功！");
                drag.enabled = false;
            }
        }
    }
}