using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerGM : MonoBehaviour
{
# region 控制顧客腳色移動
    public Transform targetPoint; // 目標位置
    public float moveSpeed;  // 移動速度
    public float stopDistance; // 到達的距離判斷
    private bool hasArrived = false;
#endregion
    // Start is called before the first frame update
    void Start()
    {
        targetPoint = GameObject.Find("顧客定位點").transform;
    }

    // Update is called once per frame
    void Update()
    {
        # region 控制顧客腳色移動
        if (hasArrived || targetPoint == null) return;

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
        #endregion
    }
    void OnArrived()
    {
        Debug.Log("顧客已抵達目標點！");
        // TODO: 例如播放動畫、改變狀態、通知管理器等
    }
}
