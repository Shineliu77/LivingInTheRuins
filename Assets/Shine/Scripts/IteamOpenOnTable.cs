using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IteamOpenOnTable : MonoBehaviour
{
    public float SetScale;
    public bool touchingFixedItemOpen;  //碰撞外組件打開
    private GameObject currentFixedItemOpen;
    public bool hasitem;//只能放一件
    private TeachGM TeachgameManager;  //取得新手關程式
    private SpriteRenderer myRenderer;//圖片

    //public bool hasitem = false;//只能放一件
    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();

        TeachgameManager = FindObjectOfType<TeachGM>();

    }
    void Update()
    {
        //  GameObject currentItem = GameObject.FindWithTag("fixeditemOpen"); // 或是你對應的標籤
        //  if (currentItem == null)
        // {
        //   hasitem = false;
        //   touchingFixedItemOpen = false;
        //}
        // 如果桌子上已經沒有任外組件打開 重設
        if (Application.loadedLevelName == "TeachGame") return;
        GameObject[] allFixedItems = GameObject.FindGameObjectsWithTag("fixeditemOpen");
        SetIteamOpenObj currentPart = GetComponentInChildren<SetIteamOpenObj>();

        // if (Application.loadedLevelName != "TeachGame" && allFixedItems == null || FindObjectOfType<SetIteamOpenObj>().OpenCount == 0)
        if (Application.loadedLevelName != "TeachGame" && currentPart == null)
        {
            hasitem = false;
            touchingFixedItemOpen = false;
            // if(!touchingFixedItemOpen)       //如果修理台有外組件打開紀指拖曳
            // {

            // GameObject.FindWithTag("fixeditemOpen").GetComponent<DraggableReturn2D>().enabled = true;
            // GameObject.FindWithTag("fixeditemOpen").GetComponent<Collider2D>().enabled = true;
            //}
            //   foreach (GameObject item in allFixedItems)
            //  {
            // if (item.transform.root != this.transform.root)
            //  {
            // 只要是在桌子外面的，通通恢復解鎖
            // if (item.transform.parent != this.transform && hasitem == false)
            // {
            //   SetItemInteraction(item, true);
            // }
            // }
            // }
        }
        if (currentPart == null && hasitem == false)
        {
            foreach (GameObject item in allFixedItems)
            {
                // if (item.transform.root != this.transform.root)
                //  {
                // 只要是在桌子外面的，通通恢復解鎖
                if (item.transform.parent != this.transform)
                {
                    SetItemInteraction(item, true);
                }
                // }
            }
        }
        if (currentPart == null && hasitem == false)
        {
            foreach (GameObject item in allFixedItems)
            {
                // if (item.transform.root != this.transform.root)
                //  {
                // 只要是在桌子外面的，通通恢復解鎖
                if (item.transform.parent != this.transform)
                {
                    SetItemInteraction(item, true);
                }
                // }
            }
        }
        //鎖住其他不適在修理台的
        // if ( currentPart != null && currentPart.OpenCount > 0&&hasitem)
        if (currentPart != null && hasitem)
        {
            foreach (GameObject item in allFixedItems)
            {
                // if (item.transform.root == this.transform.root) continue;

                // 另外，為了保險，如果物件的名字叫 brokePCB 也不要鎖
                // if (item.name.Contains("brokePCB")) continue;
                // if (item.transform.parent != this.transform)
                // if (item.transform.root != this.transform.root)
                if (item.transform.parent != this.transform)
                {

                    SetItemInteraction(item, false);

                }
            }
        }
        if (currentPart != null && hasitem)
        {
            // 檢查這個殼底下的電路板
            foreach (Transform child in currentPart.transform)
            {
                if (child.CompareTag("brokePCB"))
                {
                    var drag = child.GetComponent<DraggableReturn2D>();

                    // 【關鍵判斷】如果電路板還沒被啟用，才執行開啟邏輯
                    // 這樣就不會每幀都去重設座標，導致抓不起來
                    if (drag != null && !drag.enabled)
                    {
                        child.GetComponent<Collider2D>().enabled = true;
                        drag.enabled = true;

                        Vector3 pcbLocal = child.localPosition;
                        pcbLocal.z = -1f;
                        // pcbLocal = new Vector3(-3.7101f, -0.3002f, -0.1f);
                        child.localPosition = pcbLocal;
                        drag.originalPosition = child.position;  //滑鼠放開回原位
                                                                 //    drag.originalPosition = new Vector3(-3.7101f, -0.3002f, -0.1f);  //滑鼠放開回原位
                                                                 //child.localPosition = new Vector3(-3.7101f, -0.3002f, -0.1f);
                                                                 //      Debug.Log("Update 自動偵測並啟用了電路板");
                    }
                }
            }
        }
    }

    //private void LateUpdate()
    //{
    //  SetIteamOpenObj currentPart = GetComponentInChildren<SetIteamOpenObj>();
    //  if (currentPart != null && hasitem)
    //  {
    // 檢查這個殼底下的電路板
    //   foreach (Transform child in currentPart.transform)
    //   {
    //  if (child.CompareTag("brokePCB"))
    //  {
    //  var drag = child.GetComponent<DraggableReturn2D>();

    // 【關鍵判斷】如果電路板還沒被啟用，才執行開啟邏輯
    // 這樣就不會每幀都去重設座標，導致抓不起來
    // if (drag != null && !drag.enabled)
    //  {
    //    child.GetComponent<Collider2D>().enabled = true;
    //    drag.enabled = true;

    //   Vector3 pcbLocal = child.localPosition;
    //    pcbLocal.z = -1f;
    // pcbLocal = new Vector3(-3.7101f, -0.3002f, -0.1f);
    //    child.localPosition = pcbLocal;
    //     drag.originalPosition = child.position;  //滑鼠放開回原位
    //drag.originalPosition = new Vector3(-3.7101f, -0.3002f, -0.1f);  //滑鼠放開回原位
    //child.localPosition = new Vector3(-3.7101f, -0.3002f, -0.1f);
    //     Debug.Log("Update 自動偵測並啟用了電路板");
    //  }
    //  }//
    //  }
    //}
    //  }
    // 簡化代碼，避免重複寫 GetComponent
    void SetItemInteraction(GameObject obj, bool isEnable)
    {
        var drag = obj.GetComponent<DraggableReturn2D>();
        var coll = obj.GetComponent<Collider2D>();
        if (drag != null) drag.enabled = isEnable;
        if (coll != null) coll.enabled = isEnable;

        foreach (Transform child in obj.transform)  //處理電路板
        {
            if (child.CompareTag("brokePCB"))
            {
                var pcbDrag = child.GetComponent<DraggableReturn2D>();
                var pcbColl = child.GetComponent<Collider2D>();

                // 如果殼在桌外，PCB不能單獨拖出
                // 如果殼在桌上，由Update 處理
                if (pcbDrag != null) pcbDrag.enabled = false;
                if (pcbColl != null) pcbColl.enabled = isEnable;
            }
        }
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
            if (TeachgameManager != null)                    //避免顯示拖曳
            {
                TeachgameManager.IteamOpenOriginPicRenderer(myRenderer);　　　//修好換圖ｇ　
            }
        }
    }
    private void OnCollisionExit2D(Collision2D coll)  //結束碰撞
    {
        if (coll.gameObject.CompareTag("fixeditemOpen"))
        {
            // touchingFixedItemOpen = false;
            currentFixedItemOpen = null;
            if (TeachgameManager != null)                    //避免顯示拖曳
            {
                TeachgameManager.IteamOpenOriginPicRenderer(myRenderer);　　　//修好換圖ｇ　
            }

        }
    }

    void OnItemReleased(DraggableReturn2D item)
    {
        if (item.gameObject != currentFixedItemOpen) return;
        if (touchingFixedItemOpen) return;  //避免重觸發
        if (currentFixedItemOpen != null)
        {
            if (Application.loadedLevelName == "TeachGame" && item.gameObject.CompareTag("fixeditemOpen") && !hasitem)
            {
                AudioManager.Instance.PlaySfx(27);                                             //音效
                hasitem = true;
                TeachgameManager.IteamOpenOriginPicRenderer(myRenderer);　　　//修好換圖ｇ　
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
            //  if (Application.loadedLevelName != "TeachGame" && item.gameObject.CompareTag("fixeditemOpen") && !hasitem && FindObjectOfType<SetIteamOpenObj>().OpenCount != 0)
            SetIteamOpenObj itemScript = currentFixedItemOpen.GetComponent<SetIteamOpenObj>();
            if (Application.loadedLevelName != "TeachGame" && item.gameObject.CompareTag("fixeditemOpen") && !hasitem && itemScript.OpenCount != 0)
            {
                hasitem = true;
                AudioManager.Instance.PlaySfx(27);                                                         //音效
                item.GetComponent<DraggableReturn2D>().originalPosition = this.transform.position;
                item.GetComponent<Collider2D>().enabled = false;
                item.GetComponent<DraggableReturn2D>().enabled = false;
                item.transform.parent = this.transform;
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one * SetScale;
                touchingFixedItemOpen = true;
                Debug.Log("偵測到 fixeditemOpen 碰撞！");
                // 檢查是否有顯示 CircuitBoard
                //  if (SetIteamOpenObj.HasActiveCircuitBoard)  
                // {
                // 檢查這個物件是否有子物件
                // Debug.Log("找到brokePCB了！");
                // if (item.gameObject.transform.childCount > 0)
                // {
                // Debug.Log("找到brokePCB了2！");
                // 嘗試找到 brokePCB 子物件
                //  Transform brokeChild = null;
                //  foreach (Transform child in item.gameObject.transform)
                // {
                //  if (child.CompareTag("brokePCB"))
                //  {
                //    Debug.Log("找到brokePCB的tag！");
                //   brokeChild = child;
                //  Debug.Log("找到子物件brokePCB！");
                //  break;

                // }
                // }

                //  if (brokeChild != null)
                // {
                // 啟用子物件的拖曳腳本
                //  var drag = brokeChild.GetComponent<DraggableReturn2D>();
                // var col2d = brokeChild.GetComponent<Collider2D>();
                // Debug.Log("brokePCB 得到拖曳程式！");
                //  if (drag != null)
                // {
                //   if (col2d != null && !col2d.enabled)
                //    col2d.enabled = true;
                //   drag.enabled = true;
                //  brokeChild.localPosition = new Vector3(-3.7101f, -0.3002f, -0.1f);
                //  Debug.Log("brokePCB 拖曳啟用！");
                // }
                //  else
                // { Debug.LogWarning("brokePCB 找不到 DraggableReturn2D 程式"); }
                // }
                // else { Debug.LogWarning("fixeditemOpen 底下找不到 brokePCB 子物件"); }
                //}
                // else { Debug.LogWarning("fixeditemOpen 沒有任何子物件"); }
                // }
                // else { Debug.Log("目前沒有啟用的 CircuitBoard，因此不允許拖曳"); }
                // }
            }
        }
    }
    void EnableBrokePCB(GameObject parentItem)
    {
        foreach (Transform child in parentItem.transform)
        {
            if (child.CompareTag("brokePCB"))
            {
                var drag = child.GetComponent<DraggableReturn2D>();
                var col = child.GetComponent<Collider2D>();
                if (drag != null)
                {
                    drag.enabled = true;
                    if (col != null) col.enabled = true;
                    child.localPosition = new Vector3(-3.7101f, -0.3002f, -0.1f);
                    Debug.Log("專屬開啟：" + child.name);
                }
            }
        }
    }
}



// }
//}
