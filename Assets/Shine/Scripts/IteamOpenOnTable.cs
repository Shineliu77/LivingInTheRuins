using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IteamOpenOnTable : MonoBehaviour
{
    public float SetScale;
    public bool touchingFixedItemOpen;  //碰撞外組件打開
    private GameObject currentFixedItemOpen;
    public bool hasitem = false;//只能放一件
    // Start is called before the first frame update
    void Start()
    {

    }

    void OnEnable()   //檢查滑鼠放開事件
    {
        DraggableReturn2D.OnReleased += OnItemReleased;
        Debug.Log(gameObject.name + " 已啟用，開始監聽拖拽放開事件");

    }
    void OnDisable()    //取消滑鼠放開事件
                        // private void OnDisableMouse()    //取消滑鼠放開事件
    {
        DraggableReturn2D.OnReleased -= OnItemReleased;
        Debug.Log(gameObject.name + " 已停用，移除監聽");
    }

    private void OnCollisionStay2D(Collision2D coll)  //碰撞
    {
        Debug.Log("事件廣播已接收！目前放開的物件是: " + coll.gameObject.name);
        if (coll.gameObject.CompareTag("fixeditemOpen"))
        {
            //touchingFixedItemOpen = true;
            currentFixedItemOpen = coll.gameObject;
        }
    }
    private void OnCollisionExit2D(Collision2D coll)  //結束碰撞
    {
        if (coll.gameObject.CompareTag("fixeditemOpen"))
        {
            // touchingFixedItemOpen = false;
            currentFixedItemOpen = null;
        }
    }

    void OnItemReleased(DraggableReturn2D item)
    {
        if (touchingFixedItemOpen) return;  //避免重觸發
        if (currentFixedItemOpen != null)
        {
            if (Application.loadedLevelName == "TeachGame" && item.gameObject.CompareTag("fixeditemOpen") && hasitem == false)
            {
                AudioManager.Instance.PlaySfx(22);                                             //音效
                hasitem = true;
                item.GetComponent<Collider2D>().enabled = false;
                item.GetComponent<DraggableReturn2D>().enabled = false;
                item.transform.parent = this.transform;
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one * SetScale;
                touchingFixedItemOpen = true;
                if (FindObjectOfType<TeachGM>().CustomerNumber == 1)
                {
                    FindObjectOfType<TeachGM>().OpenTeach3();
                    LiveTwoDChangeImage LiveTwoDChangeImageManager = FindObjectOfType<LiveTwoDChangeImage>();
                    LiveTwoDChangeImageManager.LiveTDChangeImgOk = true;
                    //touchingFixedItemOpen = true;
                }
                if (FindObjectOfType<TeachGM>().CustomerNumber == 2)
                {
                    FindObjectOfType<TeachGM>().OpenTeach6();
                }

            }

            //讓brokePCB可以拿出來給crab
            // if (Application.loadedLevelName == "FirstGame")
            // {
            if (Application.loadedLevelName != "TeachGame" && item.gameObject.CompareTag("fixeditemOpen") && hasitem == false)
            {
                hasitem = true;
                AudioManager.Instance.PlaySfx(22);                                                         //音效
                item.GetComponent<Collider2D>().enabled = false;
                item.GetComponent<DraggableReturn2D>().enabled = false;
                item.transform.parent = this.transform;
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one * SetScale;
                touchingFixedItemOpen = true;
                Debug.Log("偵測到 fixeditemOpen 碰撞！");

                // 檢查是否有顯示 CircuitBoard
                if (SetIteamOpenObj.HasActiveCircuitBoard)
                {
                    // 檢查這個物件是否有子物件
                    Debug.Log("找到brokePCB了！");
                    if (item.gameObject.transform.childCount > 0)
                    {
                        Debug.Log("找到brokePCB了2！");
                        // 嘗試找到 brokePCB 子物件
                        Transform brokeChild = null;
                        foreach (Transform child in item.gameObject.transform)
                        {
                            if (child.CompareTag("brokePCB"))
                            {
                                Debug.Log("找到brokePCB的tag！");
                                brokeChild = child;
                                Debug.Log("找到子物件brokePCB！");
                                break;

                            }
                        }

                        if (brokeChild != null)
                        {
                            // 啟用子物件的拖曳腳本
                            var drag = brokeChild.GetComponent<DraggableReturn2D>();
                            var col2d = brokeChild.GetComponent<Collider2D>();
                            Debug.Log("brokePCB 得到拖曳程式！");
                            if (drag != null)
                            {
                                if (col2d != null && !col2d.enabled)
                                    col2d.enabled = true;
                                drag.enabled = true;
                                brokeChild.localPosition = new Vector3(-5.7101f, -0.3002f, -0.1f);
                                Debug.Log("brokePCB 拖曳啟用！");
                            }
                            else
                            { Debug.LogWarning("brokePCB 找不到 DraggableReturn2D 程式"); }
                        }
                        else { Debug.LogWarning("fixeditemOpen 底下找不到 brokePCB 子物件"); }
                    }
                    else { Debug.LogWarning("fixeditemOpen 沒有任何子物件"); }
                }
                else { Debug.Log("目前沒有啟用的 CircuitBoard，因此不允許拖曳"); }
            }
        }
    }
}


// }
//}
