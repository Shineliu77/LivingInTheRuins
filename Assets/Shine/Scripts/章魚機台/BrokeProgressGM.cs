using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrokeProgressGM : MonoBehaviour
{
    public float MachineDurability;
    float MachineDurability_Script;
    public Animator MachineAni;
    public Image MachineUIBar;
    public Sprite[] MachineUIBarSprites;

    public Image MachineUIBarOutside; //耐久值外框
    public Sprite[] MachineUIBarSpritesOutside;

    public Collider2D Placement;
    public Image MachineUI;
    public Sprite[] MachineUISprites;

    public Image MachineUIOutside; //圓形計時器外框
    public Sprite[] MachineUISpritesOutside;
    public float SaveRemainingValue;

    public Transform needle; // 指針物件（需拖曳到 Inspector）
    public float maxRotation = -360f; // 旋轉範圍（滿格時的角度）
    // Start is called before the first frame update
    void Start()
    {
        MachineDurability_Script = MachineDurability;
        SaveRemainingValue = MachineUIBar.fillAmount;

    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = MachineAni.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("work")) {

            if (stateInfo.normalizedTime >= 0.99f && GameObject.FindGameObjectsWithTag("fixeditemOpen").Length <= 0)
            {
                if (Application.loadedLevelName == "TeachGame")
                {
                    FindObjectOfType<TeachGM>().ProduceIteamOpen();
                }
                Placement.enabled = true;
                MachineUI.gameObject.SetActive(false);
                MachineDurability_Script = SaveRemainingValue;
            }
            if (stateInfo.normalizedTime <0.99f) {
                MachineUI.gameObject.SetActive(true);
                float animationLength = stateInfo.length; // 動畫總秒數
                float normalizedTime = stateInfo.normalizedTime; // 播放進度（1.0代表播放完1次）
                float currentTimeInSeconds = animationLength * Mathf.Min(normalizedTime, 1f);
                
                SaveRemainingValue = MachineDurability_Script - currentTimeInSeconds;

                MachineUIBar.fillAmount = SaveRemainingValue / MachineDurability;

                if (MachineUIBar.fillAmount > 0.5f)
                {
                    MachineUIBar.sprite = MachineUIBarSprites[0];
                    MachineUIBarOutside.sprite = MachineUIBarSpritesOutside[0]; //耐久值外框原色
                }
                else
                {
                    MachineUIBar.sprite = MachineUIBarSprites[1];
                    MachineUIBarOutside.sprite = MachineUIBarSpritesOutside[1]; //耐久值外框變色
                }
                MachineUI.transform.GetChild(1).GetComponent<Image>().fillAmount = 1f-(currentTimeInSeconds / animationLength);
                float fillAmount = 1f - (currentTimeInSeconds / animationLength);
                float zRotation = fillAmount * maxRotation; // 比例轉角度，例如 1.0 * -360 = -360°
                needle.localEulerAngles = new Vector3(0, 0, -zRotation);
                if (stateInfo.normalizedTime < 0.5f)
                {
                    MachineUI.transform.GetChild(1).GetComponent<Image>().sprite = MachineUISprites[0];
                    MachineUIOutside.sprite = MachineUISpritesOutside[0]; //圓形計時器外框原色
                }
                else
                {
                    MachineUI.transform.GetChild(1).GetComponent<Image>().sprite = MachineUISprites[1];
                    MachineUIOutside.sprite = MachineUISpritesOutside[1]; //圓形計時器外框變色
                }

            }
        }
    }

    // 碰撞進入
    private void OnCollisionEnter2D(Collision2D coll)
    {//tag的fixiem物品碰撞
        if (coll.gameObject.CompareTag("fixeditem"))
        {
            coll.gameObject.SetActive(false);
            MachineAni.SetTrigger("IdleToWalk");

        }
    }

}
