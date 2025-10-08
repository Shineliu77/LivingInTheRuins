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
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        if (Application.loadedLevelName == "TeachGame")
        {
            targetPoint = GameObject.Find("顧客定位點").transform;
            ExitTargetPoint = GameObject.Find("顧客離開定位點").transform;
        }
            if (Application.loadedLevelName == "FirstGame")
        {
            targetPoint = GameObject.Find(PosName).transform;
            ExitTargetPoint = GameObject.Find("顧客離開定位點").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        # region 控制顧客腳色移動
        transform.localScale = Vector3.one * 5.169395f;
        if (GetComponent<CountdownFill>().timer <= 0f || Finished)
        {
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
                }
                Destroy(gameObject);
                if (Application.loadedLevelName == "FirstGame")
                {
                    if (FindObjectOfType<FirstGame>().CustomerNumber < 4)
                    {
                        FindObjectOfType<FirstGame>()?.NotifyCustomerFinished(gameObject);
                    }
                }
                Destroy(gameObject);


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
               
                    OnArrived();
                
                hasArrived = true;

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
    private void OnTriggerEnter2D(Collider2D hit)
    {
        // 確認碰撞的物件是fixeditemOpenFinished時的處理
        if (Application.loadedLevelName == "TeachGame")
        {
            if (hit.gameObject.name == "fixeditemOpenFinished1")
            {
                FindObjectOfType<ScoreGM>().AddScore();
                FindObjectOfType<TeachGM>().OpenTeach5();
                Destroy(hit.gameObject);
            }
            if (hit.gameObject.name == "fixeditemOpenFinished2")
            {
                FindObjectOfType<ScoreGM>().AddScore();
                FindObjectOfType<TeachGM>().OpenTeach10();
                Destroy(hit.gameObject);
            }
        }


        //第一關使用  
        if (Application.loadedLevelName == "FirstGame")
        {
            if (hit.gameObject.name == "fixeditemOpenFinished1")
            {
                FindObjectOfType<ScoreGM>().AddScore();

                Destroy(hit.gameObject);
                Finished = true;

                FindObjectOfType<FirstGame>().ClearIteamPrefab();
                // if(hasArrived == true)
                // { 
                //FindObjectOfType<FirstGame>().ProduceIteam(); 
                // }


            }
            if (hit.gameObject.name == "fixeditemOpenFinished2")
            {
                FindObjectOfType<ScoreGM>().AddScore();

                Destroy(hit.gameObject);
                Finished = true;

                FindObjectOfType<FirstGame>().ClearIteamPrefab();
            }
            if (hit.gameObject.name == "fixeditemOpenFinished3")
            {
                FindObjectOfType<ScoreGM>().AddScore();

                Destroy(hit.gameObject);
                Finished = true;

                FindObjectOfType<FirstGame>().ClearIteamPrefab();
            }
            if (hit.gameObject.name == "fixeditemOpenFinished4")
            {
                FindObjectOfType<ScoreGM>().AddScore();

                Destroy(hit.gameObject);
                Finished = true;

                FindObjectOfType<FirstGame>().ClearIteamPrefab();
            }
        }
    }
}
