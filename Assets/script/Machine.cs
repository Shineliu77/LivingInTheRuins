using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Machine : MonoBehaviour
{
    //�]�w �@�[�ȱ��̰��P�̧C��
    public float HPMax { get; set; } = 100;

    private float hp = 100;
    public float HP
    {
        get => hp;
        set { hp = Mathf.Clamp(value, 0, HPMax); }
    }

}

