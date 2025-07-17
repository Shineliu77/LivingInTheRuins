using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerGM : MonoBehaviour
{
# region 控制顧客腳色移動
    public Transform targetPoint; // 目標位置
    public float moveSpeed;  // 移動速度
    public float stopDistance; // 到達的距離判斷
    public bool hasArrived = false;
    public Transform ExitTargetPoint; // 離開目標位置

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        targetPoint = GameObject.Find("顧客定位點").transform;
        ExitTargetPoint = GameObject.Find("顧客離開定位點").transform;

    }

    // Update is called once per frame
    void Update()
    {
        # region 控制顧客腳色移動

        if (GetComponent<CountdownFill>().timer <= 0f)
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
                gameObject.SetActive(false);
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
                hasArrived = true;
                OnArrived();
            }
        }
        #endregion
    }
    void OnArrived()
    {
        Debug.Log("顧客已抵達目標點！");
        // TODO: 例如播放動畫、改變狀態、通知管理器等
        if (Application.loadedLevelName == "TeachGame")
        {
            FindObjectOfType<TeachGM>().ProduceIteam();
        }
    }
}
