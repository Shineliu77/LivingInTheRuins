using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterClickHandler : MonoBehaviour
{
    public MonsterHit monsterHit; // 指向主控制器

    void OnMouseDown()
    {
        if (gameObject.CompareTag("monster") && monsterHit != null)
        {
            Debug.Log("有觸發點即刪除喔!");
            monsterHit.MonsterClicked(gameObject);
        }
    }
}
