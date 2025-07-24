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
    public Collider2D Placement;
    public Image MachineUI;
    public Sprite[] MachineUISprites;

    // Start is called before the first frame update
    void Start()
    {
        MachineDurability_Script = MachineDurability;
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

            }
            if (stateInfo.normalizedTime <0.99f) {
                MachineUI.gameObject.SetActive(true);
                float animationLength = stateInfo.length; // 動畫總秒數
                float normalizedTime = stateInfo.normalizedTime; // 播放進度（1.0代表播放完1次）
                float currentTimeInSeconds = animationLength * Mathf.Min(normalizedTime, 1f);
                MachineUIBar.fillAmount = (MachineDurability_Script-currentTimeInSeconds) / MachineDurability;
                if ((MachineDurability_Script - currentTimeInSeconds) / MachineDurability > 0.5f)
                {
                    MachineUIBar.sprite = MachineUIBarSprites[0];
                }
                else
                {
                    MachineUIBar.sprite = MachineUIBarSprites[1];
                }
                MachineUI.transform.GetChild(0).GetComponent<Image>().fillAmount = 1f-(currentTimeInSeconds / animationLength);
                if (stateInfo.normalizedTime < 0.5f)
                {
                    MachineUI.transform.GetChild(0).GetComponent<Image>().sprite = MachineUISprites[0];
                }
                else
                {
                    MachineUI.transform.GetChild(0).GetComponent<Image>().sprite = MachineUISprites[1];
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
