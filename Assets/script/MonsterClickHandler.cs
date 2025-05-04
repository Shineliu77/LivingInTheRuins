using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterClickHandler : MonoBehaviour
{
    public MonsterHit monsterHit; // ���V�D���

    void OnMouseDown()
    {
        if (gameObject.CompareTag("monster") && monsterHit != null)
        {
            Debug.Log("��Ĳ�o�I�Y�R����!");
            monsterHit.MonsterClicked(gameObject);
        }
    }
}
