using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DestroyPrefabButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button ClickButton;
    public int ClickTime = 2;
    public Image ButtonTarget;
    //public SpriteRenderer ButtonTarget;
    public Sprite ButtonOrigin;
    public Sprite ButtonChangeOrigin;
    public Sprite ButtonChangeCantClick;
    private bool controlRabbit = false;
    public void Start()
    {
        ButtonTarget.sprite = ButtonOrigin;

    }

    public static List<GameObject> allSpawnOrder = new List<GameObject>();

    //加入生成物件
    public static void Add(GameObject obj)
    {
        allSpawnOrder.Add(obj);
    }

    //移除生成物件
    public static void Remove(GameObject obj)
    {
        allSpawnOrder.Remove(obj);
    }

    // void Update()
    // {
    //if (allSpawnOrder.Count == 0)
    //{
    //     ButtonTarget.sprite = ButtonOrigin;
    //}
    //  if (FindObjectOfType<RabbitGM>().SpawnOK == true && controlRabbit&& ClickTime != -1)  //確定生成好與有回收 有刪除次數
    // {
    //   ControlRabbit();
    //   controlRabbit = false;
    // FindObjectOfType<RabbitGM>().SpawnOK = false;
    // }
    // }
    void OnDestroy()
    {
        var rabbit = FindObjectOfType<RabbitGM>();

    }

    void OnRabbitWaitFinished()
    {
        if (!controlRabbit || ClickTime < 0) return;

        FindObjectOfType<RabbitGM>().PlayTakeAnimation();
        controlRabbit = false;
    }

    public void DestroyPrev()
    {
        if (ClickTime <= 0)
        {
            Debug.Log("按鈕次數已經用完");
            return;
        }
        // 計算總生成數量


        if (allSpawnOrder.Count == 0)
        {
            Debug.Log("沒有可刪除的物件");
            return;
        }

        //取出最後生成的物件（真正的最後一個）
        GameObject target = allSpawnOrder[allSpawnOrder.Count - 1];

        if (target == null)
        {
            Debug.LogWarning("要刪除的物件不存在");
            return;
        }
        // 從各自系統刪除
        //RabbitGM.allSpawnedObjects.Remove(target);
        bool isRabbit = RabbitGM.allSpawnedObjects.Remove(target);
        MakeAPotion.allSpawnedPotions.Remove(target);
        if (isRabbit)
        {
            //controlRabbit = true;
            var rabbit = FindObjectOfType<RabbitGM>();
            rabbit.PlayTakeAnimation();
        }
        Remove(target);
        //真的刪除物件
        Destroy(target);

        Debug.Log($"刪除：{target.name}");

        ClickTime--;

        if (ClickTime == 0)  //只能按兩次
        {
            //ClickButton.interactable = false;
            ButtonTarget.sprite = ButtonChangeCantClick;
        }
    }

    //點擊換圖
    public void OnPointerDown(PointerEventData eventData)
    {
        if (allSpawnOrder.Count == 0 || ClickTime == 0)  //沒內容不啟動
            return;
        else if (allSpawnOrder.Count == 0)
            return;

        if (ClickTime > 0)
            ButtonTarget.sprite = ButtonChangeOrigin;

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (ClickTime > 0)
            ButtonTarget.sprite = ButtonOrigin;
    }

    public void ControlRabbit()
    {
        FindObjectOfType<RabbitGM>().RequestTake();
    }
}
