using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotOnTable : MonoBehaviour
{
    private GameObject itemInContact = null; // 紀錄目前在 Slot 內的物件
    public bool hasItem = false; // 是否已放入物件
    public bool hasItemRealPut = false; // 是否已放入物件
    public bool PutItem;

    [Header("放入物件的位置")]
    public Transform LiquidslotPoint;   // 液體放置點
    public Transform SpriteslotPoint;   // 元件放置點
    private Collider2D coll;

    void Start()
    {
        coll = gameObject.GetComponent<Collider2D>();
    }
    void OnEnable()    //檢查滑鼠放開事件
    {
        DraggableReturn2D.OnReleased += OnItemReleased;
    }

    void OnDisable()   //取消滑鼠放開事件
    {
        DraggableReturn2D.OnReleased -= OnItemReleased;
    }
    // void OnCollisionStay2D(Collision2D coll)
    //  {
    //  if(coll.gameObject.CompareTag("redIiquid") || coll.gameObject.CompareTag("yellowIiquid") || coll.gameObject.CompareTag("blueIiquid") || coll.gameObject.CompareTag("greenIiquid") || coll.gameObject.CompareTag("brokecircle") || coll.gameObject.CompareTag("square") || coll.gameObject.CompareTag("triangle"))
    //  {
    //   PutItem = true;
    //}

    // }
    void OnCollisionExit2D(Collision2D coll)
    {

        //  if (coll.gameObject == itemInContact)
        // {


        Rigidbody2D rb = itemInContact.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            PutItem = false;
            hasItem = false;
            hasItemRealPut = false;
            itemInContact = null;
            this.GetComponent<Collider2D>().enabled = true;
            Debug.Log($" {coll.gameObject.name} 離開 slot，可再次拖曳");
        }
        // }

    }
    void Update()
    {

        // 檢查是否有物件且按下
        if (hasItem && itemInContact != null && Input.GetMouseButtonDown(0))
        //if (hasItem && itemInContact != null && itemInContact.GetComponent<DraggableReturn2D>())
        {
            // 取得滑鼠在世界空間的位置
            Vector3 mousePos = Input.mousePosition;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            worldPos.z = -1f;
            Collider2D hit = Physics2D.OverlapCircle(worldPos, 4f);
            if (hit != null && hit.gameObject == itemInContact)
            {
                Debug.Log($"成功從 {name} 點擊並取出 {itemInContact.name}");

                // 執行取出邏輯
                TakeOutItem();
            }

            //有 就不能再放
            //if (LiquidslotPoint != null && hasItem == true && (hit.CompareTag("redIiquid") || hit.CompareTag("yellowIiquid") || hit.CompareTag("blueIiquid") || hit.CompareTag("greenIiquid")))
            // {
            //   hasItemRealPut = true;
            //   PutItem = true;
            //  this.GetComponent<Collider2D>().enabled = false;
            // Debug.Log("預置欄有物件");
            //}
            // if (SpriteslotPoint != null && hasItem == true && (hit.CompareTag("brokecircle") || hit.CompareTag("square") || hit.CompareTag("triangle")))
            //{
            //   hasItemRealPut = true;
            //   PutItem = true;
            //  this.GetComponent<Collider2D>().enabled = false;
            //  Debug.Log("預置欄有物件");
            //}
        }

        //滑鼠放開回彈
        //  if (itemInContact != null &&Vector2.Distance(transform.position, itemInContact.transform.position) < 0.005)
        // {
        //有 就不能再放
        //  if (LiquidslotPoint != null && itemInContact != null &&  (itemInContact.CompareTag("redIiquid") || itemInContact.CompareTag("yellowIiquid") || itemInContact.CompareTag("blueIiquid") || itemInContact.CompareTag("greenIiquid")))
        //  {
        //  hasItem = true;
        //  hasItemRealPut = true;
        // PutItem = true;
        //this.GetComponent<Collider2D>().enabled = false;
        //  Debug.Log("預置欄有物件回彈");
        // }
        // if (SpriteslotPoint != null && itemInContact != null  && (itemInContact.CompareTag("brokecircle") || itemInContact.CompareTag("square") || itemInContact.CompareTag("triangle")))
        // {
        //    hasItem = true;
        //   hasItemRealPut = true;
        //   PutItem = true;
        //    this.GetComponent<Collider2D>().enabled = false;
        //     Debug.Log("預置欄有物件回彈");
        //  }
        //}
        if (itemInContact != null && Vector2.Distance(transform.position, itemInContact.transform.position) > 0.005)
        {
            hasItem = false;
            hasItemRealPut = false;
            PutItem = false;
            this.GetComponent<Collider2D>().enabled = true;
            Debug.Log("預置欄有物件回彈");
        }
        //預置體被刪掉 位置清空
        // else if ((LiquidslotPoint != null || SpriteslotPoint != null) &&FindObjectOfType<DestroyPrefabButton>().controlDestory == true && itemInContact != null)
        // {
        //  PutItem = false;
        //  hasItem = false;
        // hasItemRealPut = false;
        //itemInContact = null;
        // this.GetComponent<Collider2D>().enabled = true;
        // FindObjectOfType<DestroyPrefabButton>().controlDestory = false;
        // Debug.Log("預置欄因刪除清空");

        //}

        else if (LiquidslotPoint != null && FindObjectOfType<liveTwoDchangeImageIiquid>().isliveTwoDchangeImageIiquidOk == true && itemInContact != null)
        {
            PutItem = false;
            hasItem = false;
            hasItemRealPut = false;
            itemInContact = null;
            this.GetComponent<Collider2D>().enabled = true;
            Debug.Log("預置欄藥水清空");
        }
        else if (SpriteslotPoint != null && FindObjectOfType<LiveTwoDChangeImage>().isLiveTDChangeImgOk == true && itemInContact != null)
        {
            PutItem = false;
            hasItem = false;
            hasItemRealPut = false;
            itemInContact = null;
            this.GetComponent<Collider2D>().enabled = true;
            Debug.Log("預置欄元件清空");
        }
    }
    public void NotifyIfMyItemDestroyed(GameObject destroyedObject)   //刪除預置體
    {
        if (itemInContact != null && itemInContact == destroyedObject)
        {
            PutItem = false;
            hasItem = false;
            hasItemRealPut = false;
            itemInContact = null;
            if (coll != null) coll.enabled = true;
            else GetComponent<Collider2D>().enabled = true;

            Debug.Log($"{name} 預置欄清空，重新開啟碰撞器。");
        }
    }
    // 將取出邏輯獨立出來，確保清理乾淨
    private void TakeOutItem()
    {
        Rigidbody2D rb = itemInContact.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.simulated = true;
        }

        DraggableReturn2D drag = itemInContact.GetComponent<DraggableReturn2D>();
        if (drag != null)
        {
            //drag.enabled = true;
            // 重要的是：點擊取出的那一刻，物件應該立刻跟隨滑鼠，否則它會留在原位
            drag.OnMouseDown();
        }
        AudioManager.Instance.PlaySfx(22);              //音效
        hasItem = false;
        hasItemRealPut = false;
        PutItem = false;
        itemInContact = null;
        this.GetComponent<Collider2D>().enabled = true;
        Debug.Log("預置欄清空");
    }
    private void OnItemReleased(DraggableReturn2D item)
    {
        //if (hasItem == true || itemInContact != null || hasItemRealPut == true)     //有 就不能再放
        //  {
        //   Debug.Log($"{name} 已經滿了，拒絕物件 {item.name} 進入");
        //  return;
        // }
        Collider2D itemColl = item.GetComponent<Collider2D>();
        if (coll != null && itemColl != null && coll.IsTouching(itemColl))
        {
            // 僅放入液體
            if (LiquidslotPoint != null && hasItem == false && (item.CompareTag("redIiquid") || item.CompareTag("yellowIiquid") || item.CompareTag("blueIiquid") || item.CompareTag("greenIiquid")))
            {
                PlaceItemInSlot(item.gameObject, LiquidslotPoint);
                hasItem = true;
                //this.GetComponent<Collider2D>().enabled = false;
                AudioManager.Instance.PlaySfx(22);              //音效
            }

            // 僅放入元件
            if (SpriteslotPoint != null && hasItem == false && (item.CompareTag("brokecircle") || item.CompareTag("square") || item.CompareTag("triangle")))
            {
                PlaceItemInSlot(item.gameObject, SpriteslotPoint);
                hasItem = true;
                //this.GetComponent<Collider2D>().enabled = false;
                AudioManager.Instance.PlaySfx(22);              //音效
            }
        }
    }


    private void PlaceItemInSlot(GameObject obj, Transform slotPoint)
    {
        itemInContact = obj;
        // 放入 slot 位置
        //  obj.transform.position = slotPoint.position;
        // obj.transform.rotation = slotPoint.rotation;
        //obj.transform.position = new Vector3(slotPoint.position.x, slotPoint.position.y, slotPoint.position.z - 1f);
        Vector3 targetPosition = new Vector3(slotPoint.position.x, slotPoint.position.y, slotPoint.position.z - 1f);
        obj.transform.position = targetPosition;
        this.GetComponent<Collider2D>().enabled = false;
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
            //drag.enabled = false;
            //drag.SetNewOrigin(slotPoint.position);
            drag.SetNewOrigin(targetPosition);
        }



        Debug.Log($" {obj.name} 放入 {slotPoint.name}");
    }

    //  private void OnTriggerExit2D(Collider2D collision)
    // {
    // 當物件離開 slot 區域，允許再次拖曳
    // if (collision.gameObject == itemInContact)
    // {
    //  Rigidbody2D rb = itemInContact.GetComponent<Rigidbody2D>();
    //  if (rb != null)
    // {
    //    rb.bodyType = RigidbodyType2D.Dynamic;
    //}

    //  Dragging dragScript = itemInContact.GetComponent<Dragging>();
    //  if (dragScript != null)
    // {
    //     dragScript.enabled = true; // 重新允許拖曳
    //}

    // hasItem = false;
    // itemInContact = null;
    // Debug.Log($" {collision.gameObject.name} 離開 slot，可再次拖曳");
    // }
    // }
}