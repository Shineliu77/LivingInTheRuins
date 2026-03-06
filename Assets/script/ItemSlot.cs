using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ItemSlot : MonoBehaviour
{
    private GameObject pcbInContact = null; // 紀錄目前碰撞到的 PCB
    private bool hasPCB = false; // 這個 slot 是否已經放入 PCB
    //public Vector3 offset = new Vector3(-0.4138f, -0.0244f, 0f);
    public SetIteamOpenObj SetIteamOpenObj;

    void Start()
    {
        if (SetIteamOpenObj == null)
        {
            SetIteamOpenObj = GetComponentInParent<SetIteamOpenObj>();
        }
    }
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
        if (pcbInContact == null)
        {
            pcbInContact = null; // 確保它是真的 Null
            return;
        }
        if (pcbInContact != null)
        {
            var drag = pcbInContact.GetComponent<DraggableReturn2D>();

            if (drag != null && !drag.isDragging && !hasPCB)
            // if (drag != null && !drag.isDragging )
            {
                if (Application.loadedLevelName == "TeachGame")
                {
                    Vector3 offset = new Vector3(-0.8738f, -0.0244f, 0f);
                    pcbInContact.transform.position = transform.position + offset;
                }
                if (Application.loadedLevelName != "TeachGame")
                {
                    Vector3 offset = new Vector3(-0.8738f, -0.0244f, 0f);
                    pcbInContact.transform.position = transform.position + offset;
                }
                var rb = pcbInContact.GetComponent<Rigidbody2D>();
                if (rb != null) rb.velocity = Vector2.zero;
                hasPCB = true; // PCB 放入
                AudioManager.Instance.PlaySfx(22);                                              //音效
                drag.enabled = false;
                // if (SetIteamOpenObj != null)
                //   {
                //SetIteamOpenObj = GetComponentInParent<SetIteamOpenObj>();
                // if (SetIteamOpenObj != null)  
                // {

                //SetIteamOpenObj.OpenCount -= 1;
                //  Debug.Log("減少 外組件打開待修理");
                // transform.parent.GetComponent<SetIteamOpenObj>().ResetSize();  //無效
                // Debug.Log("縮小 外組件打開");
                //transform.parent.GetComponent<DraggableReturn2D>().enabled = true;  //無效
                //   //Debug.Log("拖曳 外組件打開");
                // } 
                // }
                var parentObj = GetComponentInParent<SetIteamOpenObj>();
                parentObj.ResetSize();
                if (parentObj != null)
                {
                    if (Application.loadedLevelName == "TeachGame")
                    {
                        FindObjectOfType<TeachGM>().OpenTeach9();
                    }
                    //parentObj.ResetSize();
                    Debug.Log("縮小 外組件打開");
                    // }
                }
                else
                {
                    Debug.LogWarning("找不到父層 SetIteamOpenObj！");
                }
                if (parentObj.OpenCount == 0)
                {
                    var parentDrag = GetComponentInParent<DraggableReturn2D>();
                    if (parentDrag != null)
                    {
                        parentDrag.enabled = true;
                        Debug.Log("拖曳 外組件打開");
                    }
                    else
                    {
                        Debug.LogWarning("找不到父層 DraggableReturn2D！");
                    }
                }

            }
        }
    }
}
