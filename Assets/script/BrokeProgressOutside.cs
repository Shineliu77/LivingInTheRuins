using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BrokeProgressOutside : MonoBehaviour
{
    public GameObject FixItemMachine; //  修理的物件的機器
    public Image brokebar; //耐久值條

    public BrokeProgressAnime brokeProgressAnime; //耐久值條外面的框
    public Image OutsideBrokebar; //耐久值條外面的框
    public Sprite OutsideTargetSwichImage;     //當耐久值條低於 被更換的耐久值條外面的框
    public Sprite OutsidebrokebarRed;     //當耐久值低於 更換耐久值條外面的框
    private bool OutsideSwichImage = false;  //是否符合換的耐久值條外面的框圖條件

    void Start()
    {
        OutsideBrokebar.sprite = OutsideTargetSwichImage;
    }

    // Update is called once per frame
    void Update()
    {
        Machine machineScript = FixItemMachine.GetComponent<Machine>();
        brokebar.fillAmount = machineScript.HP / machineScript.HPMax;

        if (machineScript.HP <= 50.0f)  //如果小於等於 換圖
        {
            OutsideSwichImage = true;
            OutsideBrokebar.sprite = OutsidebrokebarRed;
        }

        else if (machineScript.HP >= 50.0f)  //如果大於等於 換回初始
        {
            OutsideSwichImage = false;
            OutsideBrokebar.sprite = OutsideTargetSwichImage;
        }
    }
}
