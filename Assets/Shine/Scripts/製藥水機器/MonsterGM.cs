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
    public string currentMachineName;  //現在攻擊的機器
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

    IEnumerator DamageLoop()  //所有機台都需一要
    {
        while (true)
        {

            yield return new WaitForSeconds(interval);
            if (currentMachineName == "blender")
            {
                FindObjectOfType<MakeAPotion>().ProduceMachineDurability();
                // 迴圈會自動「重新計 15 秒」
            }
            else if (currentMachineName == "rabbit")
            {
                FindObjectOfType<RabbitGM>().ProduceMachineDurability();
                // 迴圈會自動「重新計 15 秒」
            }

            else if (currentMachineName == "opener0320")
            {
                FindObjectOfType<BrokeProgressGM>().ProduceMachineDurability();
                // 迴圈會自動「重新計 15 秒」
            }
            else if (currentMachineName == "crab")
            {
                FindObjectOfType<CrabGM>().ProduceMachineDurability();
                // 迴圈會自動「重新計 15 秒」
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        ScriptBlood = SetBlood;
    }

    public void InitTarget(string machineName)  //所有機台都需一要
    {
        currentMachineName = machineName;
        if (machineName == "blender")
        {
            targetPoint = GameObject.Find("怪物定位點").transform;
            ExitTargetPoint = GameObject.Find("怪物離開定位點").transform;
        }
        else if (machineName == "rabbit")
        {
            targetPoint = GameObject.Find("怪物定位點R").transform;
            ExitTargetPoint = GameObject.Find("怪物離開定位點R").transform;
        }
        else if (machineName == "opener0320")
        {
            targetPoint = GameObject.Find("怪物定位點O").transform;
            ExitTargetPoint = GameObject.Find("怪物離開定位點O").transform;
        }
        else if (machineName == "crab")
        {
            targetPoint = GameObject.Find("怪物定位點C").transform;
            ExitTargetPoint = GameObject.Find("怪物離開定位點C").transform;
        }
    }
    // Update is called once per frame
    void Update()
    {
        #region 控制怪物腳色移動
        stateInfo = MonsterAni.GetCurrentAnimatorStateInfo(0);

        if (ScriptBlood > 0 && !stateInfo.IsName("leave"))
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

        if (ScriptBlood > 0 && !stateInfo.IsName("leave"))
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
            if (stateInfo.normalizedTime >= 0.01f && stateInfo.IsName("leave"))
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
                    Destroy(gameObject);
                }
            }

            else
            {
                if (stateInfo.normalizedTime >= 0.01f && stateInfo.IsName("success"))
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
                        Destroy(gameObject);
                    }
                }
            }
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
        if (!stateInfo.IsName("leave") && !stateInfo.IsName("success"))
        {
            ScriptBlood -= DeductBlood;
        }
    }
    public void AttackMachine()
    {
        FindObjectOfType<MakeAPotion>().ProduceMachineDurability();
    }

}
