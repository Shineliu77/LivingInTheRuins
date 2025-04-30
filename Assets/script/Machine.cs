using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Machine : MonoBehaviour
{
    //設定 耐久值條最高與最低值
    public float HPMax { get; set; } = 100;

    private float hp = 100;
    public float HP
    {
        get => hp;
        set { hp = Mathf.Clamp(value, 0, HPMax); }
    }

}

