using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeAPotion : MonoBehaviour
{
    //製藥水材料碰到機器
    public Image Stopwatch; //原色圖片
    public Sprite[] StopwatchUISprites;

    public Image StopwatchOutside; //外框圖片
    public Sprite[] StopwatchUIOutsideSprites;

    public float StopwatchTimer;
    public float ScriptStopwatchTimer;
    bool isStopwatch;

    //隨機判斷要不要產生怪物 但 新手教學關卡要產生怪物
    bool isProduceMonster;
    public GameObject Monster;
    GameObject MonsterPrefab;
    public Transform ProducePos;

    //製藥水機的耐力值
    public float MachineDurability;
    float MachineDurability_Script;
    public float DeductMachineDurability;
    public Image MachineDurabilityBar; 
    public Sprite[] MachineDurabilityBarSprite;

    public Image MachineDurabilityBarOustside;   //耐久外框圖片
    public Sprite[] MachineDurabilityBarSpriteOustside;

    public GameObject[] Potions;
    public int SelectPotionID;
    // Start is called before the first frame update
    void Start()
    {
        ScriptStopwatchTimer=StopwatchTimer;
        MachineDurability_Script = MachineDurability;

    }

    // Update is called once per frame
    void Update()
    {
        if (isStopwatch&& ScriptStopwatchTimer>0) {
            Stopwatch.gameObject.SetActive(true);
            ScriptStopwatchTimer -= Time.deltaTime;
            Stopwatch.transform.GetChild(1).GetComponent<Image>().fillAmount = ScriptStopwatchTimer / StopwatchTimer;
            if (ScriptStopwatchTimer / StopwatchTimer > 0.5f)
            {
                Stopwatch.transform.GetChild(1).GetComponent<Image>().sprite = StopwatchUISprites[0];
                StopwatchOutside.sprite = StopwatchUIOutsideSprites[0];

            }
            else
            {
                Stopwatch.transform.GetChild(1).GetComponent<Image>().sprite = StopwatchUISprites[1];
                StopwatchOutside.sprite = StopwatchUIOutsideSprites[1];
            }
            if (Stopwatch.transform.GetChild(1).GetComponent<Image>().fillAmount == 0) {
                Potions[SelectPotionID].SetActive(true);
                Stopwatch.gameObject.SetActive(false);

                if (Application.loadedLevelName == "TeachGame") {
                    FindObjectOfType<TeachGM>().OpenTeach8();
                }
            }
        }
   
    }

    private void OnCollisionEnter2D(Collision2D hit)
    {
      
        if (hit.collider.tag == "Red") {
            Reset();
            SelectPotionID = 0;
            ProduceMonster();

        }
        if (hit.collider.tag == "Yellow")
        {
            Reset();
            SelectPotionID = 1;
            ProduceMonster();

        }
        if (hit.collider.tag == "Blue")
        {
            Reset();
            SelectPotionID = 2;
            ProduceMonster();

        }
        if (hit.collider.tag == "Green")
        {
            Reset();
            SelectPotionID = 3;
            ProduceMonster();

        }
    }
    //製作藥水判斷要不要產生怪物
    void ProduceMonster()
    {
        if (Application.loadedLevelName == "TeachGame")
        {          
             MonsterPrefab = Instantiate(Monster, ProducePos.position, Monster.transform.rotation) as GameObject;
        }
        else {
            //Random.Range(0, 2) 回傳整數 0 或 1,等於 0 回傳 true，否則為 false。
            isProduceMonster = Random.Range(0, 2) == 0;
            if (isProduceMonster && !MonsterPrefab)
            {
                MonsterPrefab = Instantiate(Monster, ProducePos.position, Monster.transform.rotation) as GameObject;
            }
        }
    }
    //怪物攻擊機台扣的耐力值
   public void ProduceMachineDurability() {
        MachineDurability_Script -= DeductMachineDurability;
        MachineDurabilityBar.fillAmount = MachineDurability_Script / MachineDurability;
        if (MachineDurability_Script / MachineDurability > 0.5f)
        {
            MachineDurabilityBar.sprite = MachineDurabilityBarSprite[0];
            MachineDurabilityBarOustside.sprite = MachineDurabilityBarSpriteOustside[0];  //耐久值外框原色
        }
        else
        {
            MachineDurabilityBar.sprite = MachineDurabilityBarSprite[1];
            MachineDurabilityBarOustside.sprite = MachineDurabilityBarSpriteOustside[1];  //耐久值外框變色
        }
        if (MachineDurabilityBar.fillAmount == 0) {
            GameObject.FindWithTag("Monster").GetComponent<MonsterGM>().MonsterAni.SetTrigger("Win");
        }
    }
    public void Reset()
    {
        isStopwatch = true;
        ScriptStopwatchTimer = StopwatchTimer;
        for (int i = 0; i < Potions.Length; i++)
        {
            Potions[i].SetActive(false);
        }
    }
}
