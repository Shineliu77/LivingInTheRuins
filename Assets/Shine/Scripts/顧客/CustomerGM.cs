using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerGM : MonoBehaviour
{
    #region 控制顧客腳色移動
    public Transform targetPoint; // 目標位置
    public float moveSpeed;  // 移動速度
    public float stopDistance; // 到達的距離判斷
    public bool hasArrived = false;
    public Transform ExitTargetPoint; // 離開目標位置
    public bool Finished;
    public string PosName;
    public int ID;
    public bool isProduceIteam;
    private Collider2D isCollCustomer;//是否碰撞客人
                                      // private bool collProduceIteam;
                                      // private GameObject currentcollProduceIteam;
    private bool isArrive = false;
    private bool hasSpawnedItem = false; // 紀錄是否已經生成過
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        if (Application.loadedLevelName == "TeachGame")
        {
            // AudioManager.Instance.PlaySfx(4);             //音效
            targetPoint = GameObject.Find("顧客定位點").transform;
            ExitTargetPoint = GameObject.Find("顧客離開定位點").transform;
        }
        if (Application.loadedLevelName == "FirstGame")
        {
            // AudioManager.Instance.PlaySfx(4);             //音效
            targetPoint = GameObject.Find(PosName).transform;
            ExitTargetPoint = GameObject.Find("顧客離開定位點").transform;
        }
        isCollCustomer = gameObject.GetComponent<Collider2D>();
    }
    void OnEnable()    //檢查滑鼠放開事件
    {
        DraggableReturn2D.OnReleased += OnItemReleased;
    }

    void OnDisable()   //取消滑鼠放開事件
    {
        DraggableReturn2D.OnReleased -= OnItemReleased;
    }
    // private void OnCollisionStay2D(Collision2D coll)
    //{
    // if (Application.loadedLevelName == "TeachGame" || Application.loadedLevelName != "TeachGame")
    // {
    //  if (coll.gameObject.name == "fixeditemOpenFinished1" || coll.gameObject.name == "fixeditemOpenFinished2" || coll.gameObject.name == "fixeditemOpenFinished3" || coll.gameObject.name == "fixeditemOpenFinished4")
    //  {
    //      collProduceIteam = true;
    // }
    // }
    // }

    // private void OnCollisionExit2D(Collision2D coll)
    // {
    //  if (Application.loadedLevelName == "TeachGame" || Application.loadedLevelName != "TeachGame")
    // {
    //   if (coll.gameObject.name == "fixeditemOpenFinished1" || coll.gameObject.name == "fixeditemOpenFinished2" || coll.gameObject.name == "fixeditemOpenFinished3" || coll.gameObject.name == "fixeditemOpenFinished4")
    //  {
    //     collProduceIteam = false;
    //  }
    //}


    //  }

    void Update()
    {
        # region 控制顧客腳色移動
        transform.localScale = Vector3.one * 5.169395f;
        if (GetComponent<CountdownFill>().timer <= 0f || Finished)
        {
            if (!isArrive)
            {
                //AudioManager.Instance.PlaySfx(2);             //音效          
                isArrive = true;
            }
            float distance = Vector2.Distance(transform.position, ExitTargetPoint.position);

            if (distance > stopDistance)
            {
                // 向目標移動

                Vector2 newPosition = Vector2.MoveTowards(transform.position, ExitTargetPoint.position, moveSpeed * Time.deltaTime);
                transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
            }
            else
            {
                if (Application.loadedLevelName == "TeachGame")
                {
                    if (FindObjectOfType<TeachGM>().CustomerNumber < 2)
                    {
                        FindObjectOfType<TeachGM>().ProductCustomer();
                    }
                    Destroy(gameObject);
                }
                //Destroy(gameObject);
                if (Application.loadedLevelName == "FirstGame")
                {
                    //if (FindObjectOfType<FirstGame>().CustomerNumber < 4)
                    if (FindObjectOfType<FirstGame>() != null)
                    {
                        FindObjectOfType<FirstGame>()?.NotifyCustomerFinished(gameObject);
                        FindObjectOfType<FirstGame>().NotifyCustomerFinished(transform.root.gameObject);

                    }
                }
                // Destroy(gameObject);


            }
        }
        else
        {

            // 計算與目標的距離
            float distance = Vector2.Distance(transform.position, targetPoint.position);
            if (distance > stopDistance)
            {
                // 向目標移動
                Vector2 newPosition = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
                transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
            }
            else
            {
                if (!hasArrived)
                {
                    hasArrived = true;
                    OnArrived();
                }
            }

        }
        #endregion
    }
    public void OnArrived()
    {
        //Debug.Log("顧客已抵達目標點！");
        // TODO: 例如播放動畫、改變狀態、通知管理器等
        if (Application.loadedLevelName == "TeachGame")
        {
            if (!GameObject.FindWithTag("fixeditem") && !GameObject.FindWithTag("fixeditemOpen"))
                FindObjectOfType<TeachGM>().ProduceIteam();
        }

        if (Application.loadedLevelName != "TeachGame")      //其他關
        {
            FirstGame fg = FindObjectOfType<FirstGame>();
            if (hasSpawnedItem) return;
            if (fg != null)
            {
                // 關鍵：傳入自己的 transform 以及存在 marker 裡的 seatIndex
                var marker = GetComponent<FirstGameCustomerMarker>();
                int myIndex = (marker != null) ? marker.SeatIndex : 0;

                fg.SpawnIteamGO(this.transform, myIndex);
                hasSpawnedItem = true;
            }
        }
        //第一關使用
        /* if (Application.loadedLevelName == "FirstGame")
        {
            if (!isProduceIteam)
            {
                FindObjectOfType<FirstGame>().ProduceIteam(ID);
                isProduceIteam = true;
            }
        
        }*/


    }

    private void OnItemReleased(DraggableReturn2D hit)
    {
        Collider2D CollShouldDestroy = hit.GetComponent<Collider2D>();
        if (isCollCustomer != null && CollShouldDestroy != null && isCollCustomer.IsTouching(CollShouldDestroy))
        {
            // 確認碰撞的物件是fixeditemOpenFinished時的處理
            if (Application.loadedLevelName == "TeachGame")
            {
                if (hit.gameObject.name == "fixeditemOpenFinished1")
                {
                    AudioManager.Instance.PlaySfx(12);             //音效
                    FindObjectOfType<ScoreGM>().AddScore();
                    FindObjectOfType<TeachGM>().OpenTeach5();
                    FindObjectOfType<IteamOpenOnTable>().touchingFixedItemOpen = false;   //重製外組件打開觸發
                    FindObjectOfType<IteamOpenOnTable>().hasitem = false;
                    Destroy(hit.gameObject);
                    CountdownFill countdownScript = GetComponent<CountdownFill>();
                    if (countdownScript != null && countdownScript.ShouldDestroy != null)
                    {
                        // 叫 CountdownFill 的子物件去死
                        Destroy(countdownScript.ShouldDestroy);
                        isArrive = false;
                    }
                }
                if (hit.gameObject.name == "fixeditemOpenFinished2")
                {
                    AudioManager.Instance.PlaySfx(12);             //音效
                    FindObjectOfType<ScoreGM>().AddScore();
                    FindObjectOfType<TeachGM>().OpenTeach10();
                    FindObjectOfType<IteamOpenOnTable>().touchingFixedItemOpen = false;  //重製外組件打開觸發
                    FindObjectOfType<IteamOpenOnTable>().hasitem = false;
                    Destroy(hit.gameObject);
                    CountdownFill countdownScript = GetComponent<CountdownFill>();
                    if (countdownScript != null && countdownScript.ShouldDestroy != null)
                    {
                        // 叫 CountdownFill 的子物件去死
                        Destroy(countdownScript.ShouldDestroy);
                        isArrive = false;
                    }
                    //Finished = true;
                    // Destroy(hit.gameObject);
                }
            }


            //其他關使用  
            if (Application.loadedLevelName != "TeachGame")
            {
                if (hit.name.Contains("fixeditemOpenFinished"))
                {
                    AudioManager.Instance.PlaySfx(12);             //音效
                    FindObjectOfType<ScoreGM>().AddScore();
                    FindObjectOfType<IteamOpenOnTable>().touchingFixedItemOpen = false;  //重製外組件打開觸發
                    FindObjectOfType<IteamOpenOnTable>().hasitem = false;

                    Destroy(hit.gameObject);
                    CountdownFill countdownScript = GetComponent<CountdownFill>();
                    if (countdownScript != null && countdownScript.ShouldDestroy != null)
                    {
                        // 叫 CountdownFill 的子物件去死
                        Destroy(countdownScript.ShouldDestroy);
                        isArrive = false;
                    }
                    Finished = true;
                    FindObjectOfType<FirstGame>().ClearIteamPrefab();
                    // if(hasArrived == true)
                    // { 
                    //FindObjectOfType<FirstGame>().ProduceIteam(); 
                    // }


                }

            }
        }
    }

    public void AngrySfx()
    {
        AudioManager.Instance.PlaySfx(7);
    }
}
