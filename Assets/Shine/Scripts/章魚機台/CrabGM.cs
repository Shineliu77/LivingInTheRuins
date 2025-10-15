using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrabGM : MonoBehaviour
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

    public GameObject PCBPop;  //電路板生成
    public Transform PCBPopPlace;  //電路板生成
    GameObject CurrentPCB;
    private bool canSpawnPCB = false;

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
        if (stateInfo.IsName("get in"))
        {

            if (stateInfo.normalizedTime >= 0.99f)
            {
                // if (Application.loadedLevelName == "TeachGame")
                // {
                //   FindObjectOfType<TeachGM>().ProduceIteamOpen();
                // }

                MachineUI.gameObject.SetActive(false);
                MachineDurability_Script = SaveRemainingValue;

            }
            if (stateInfo.normalizedTime < 0.99f)
            {
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
                MachineUI.transform.GetChild(1).GetComponent<Image>().fillAmount = 1f - (currentTimeInSeconds / animationLength);
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
        if (canSpawnPCB && CurrentPCB == null)   //生成PCB
        {
            AnimatorStateInfo stateInfo2 = MachineAni.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("hold") && stateInfo.normalizedTime > 0.25f)
            {
                CurrentPCB = Instantiate(PCBPop, PCBPopPlace.position, PCBPopPlace.rotation);
                canSpawnPCB = false;
            }
            // if (GameObject.FindWithTag("PCB").GetComponent<DraggableReturn2D>().isDragging == true)  //將其改成public仍無法切換動畫
            // {
            //   TakePCB();
            //  }

        }
    }

    // 碰撞進入
    private void OnCollisionEnter2D(Collision2D coll) //碰撞觸發動畫
    {
        if (coll.gameObject.CompareTag("brokePCB"))
        {
            Destroy(coll.gameObject);
            canSpawnPCB = true;
            MachineAni.SetTrigger("IdleToWalk");

        }
    }
    public void HoldPCB() //撥放持續拿PCB動畫
    {
        MachineAni.SetBool("hold", true);

        // if(GameObject.FindWithTag("PCB"))
        //{

        // GameObject.FindWithTag("PCB").GetComponent<DraggableReturn2D>().OnMouseDown();
        // MachineAni.SetTrigger("takeout");
        //}

    }
    public void TakePCB()
    {
        MachineAni.SetTrigger("takeout");
    }
}
