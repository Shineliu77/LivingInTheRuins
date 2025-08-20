using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGM : MonoBehaviour
{
    #region 控制怪物腳色移動
    public Transform targetPoint; // 目標位置
    public float moveSpeed;  // 移動速度
    public float stopDistance; // 到達的距離判斷
    public bool hasArrived = false;
    public Transform ExitTargetPoint; // 離開目標位置
    public bool Finished;
    #endregion
    #region 怪物血量
    public float SetBlood;
    float ScriptBlood;
    public float DeductBlood;
    public Animator MonsterAni;
    #endregion
    #region 怪物每?秒攻擊機臺一次
    public float interval;    // 每次扣血間隔（秒）
    Coroutine loop;
    AnimatorStateInfo stateInfo;
    #endregion
    void OnEnable()
    {
        if (loop == null) loop = StartCoroutine(DamageLoop());
    }

    void OnDisable()
    {
        if (loop != null)
        {
            StopCoroutine(loop);
            loop = null;
        }
    }
    
    IEnumerator DamageLoop()
    {
        while (true)
        {
           
           yield return new WaitForSeconds(interval);
            FindObjectOfType<MakeAPotion>().ProduceMachineDurability();
            // 迴圈會自動「重新計 15 秒」
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        targetPoint = GameObject.Find("怪物定位點").transform;
        ExitTargetPoint = GameObject.Find("怪物離開定位點").transform;
        ScriptBlood = SetBlood;
    }

    // Update is called once per frame
    void Update()
    {
        #region 控制怪物腳色移動
         stateInfo = MonsterAni.GetCurrentAnimatorStateInfo(0);

        if (ScriptBlood > 0&&!stateInfo.IsName("leave"))
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
        else
        {
            MonsterAni.SetTrigger("Fail");
           
        }
        if (stateInfo.normalizedTime >= 0.99f && stateInfo.IsName("leave"))
        {

            Invoke("MonsterFail", 0.5f);
        }
        #endregion
    }

    void OnArrived()
    {
        Debug.Log("怪物已抵達目標點！");
        MonsterAni.SetTrigger("Attack");
        // TODO: 例如播放動畫、改變狀態、通知管理器等
        if (Application.loadedLevelName == "TeachGame" && !FindObjectOfType<TeachGM>().isTeach7)
        {
            FindObjectOfType<TeachGM>().OpenTeach7();
        }

    }
    private void OnMouseDown()
    {
        if (!stateInfo.IsName("leave")&& !stateInfo.IsName("success"))
        {
            ScriptBlood -= DeductBlood;
        }
    }
    public void AttackMachine()
    {
        FindObjectOfType<MakeAPotion>().ProduceMachineDurability();
    }

    public void MonsterFail()
    {
        MonsterAni.SetTrigger("Leave"); //撥放離開動畫

        float distance = Vector2.Distance(transform.position, ExitTargetPoint.position);

        if (distance > stopDistance)
        {
            // 向目標移動
            Vector2 newPosition = Vector2.MoveTowards(transform.position, ExitTargetPoint.position, moveSpeed * Time.deltaTime);
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
