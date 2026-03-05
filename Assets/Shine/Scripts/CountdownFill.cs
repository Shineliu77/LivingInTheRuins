using UnityEngine;
using UnityEngine.UI;

public class CountdownFill : MonoBehaviour
{
    public Image fillImage;     // 指向 UI 的 Image (type 設定為 Filled)
    public float countdownTime; // 倒數總時間（秒）
    public Sprite changeTargetfillImage;  //要被換得圖
    public Sprite fillImageChange; //當耐心值低於50%換圖

    public Image fillImageOutside;     // 耐心值外框
    public Sprite changeTargetfillImageeOutside;  //要被換得耐心值
    public Sprite fillImageChangeeOutside; //當耐心值低於50%換耐心值外框

    public GameObject ShouldDestroy; //當耐心值歸零 刪掉整個耐心值物
    public float timer;
    private Collider2D isShouldDestroycollCoustomer; //是否碰到客人
    public bool isAngry;
    void Start()
    {
        timer = countdownTime;
        fillImage.fillAmount = 1f;
        fillImageOutside.sprite = changeTargetfillImageeOutside;
        isShouldDestroycollCoustomer = gameObject.GetComponent<Collider2D>();
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (Application.loadedLevelName == "TeachGame")
            {
                timer = Mathf.Clamp(timer, countdownTime / 3, countdownTime);
            }
            // 計算填滿比例
            float fill = Mathf.Clamp01(timer / countdownTime);
            fillImage.fillAmount = fill;

            if (fillImage.fillAmount > 0.5f) // 當耐心值低於50%顧客生氣 耐心值換圖
            {
                isAngry = false;
            }
            if (fillImage.fillAmount <= 0.5f) // 當耐心值低於50%顧客生氣 耐心值換圖
            {
                if (isAngry == false)
                {
                    isAngry = true;
                    AudioManager.Instance.PlaySfx(7);
                }
                // AudioManager.Instance.PlaySfx(4);             //音效
                //NewCustomerAnime[] NewCustomerAnimeScript = FindObjectsOfType<NewCustomerAnime>();
                NewCustomerAnime NewCustomerAnimeScript = GetComponent<NewCustomerAnime>();
                // foreach (NewCustomerAnime customer in NewCustomerAnimeScript)
                // {
                if (NewCustomerAnimeScript != null)
                //if (NewCustomerAnimeScript != null)
                {
                    NewCustomerAnimeScript.CountdownFillEmpty();
                    //NewCustomerAnimeScript.CountdownFillEmpty();
                    fillImage.sprite = fillImageChange;
                    fillImageOutside.sprite = fillImageChangeeOutside;
                }
                // }
            }
            if (fillImage.fillAmount <= 0f) //當耐心值歸零 摧毀耐心值物件
            {

                Destroy(ShouldDestroy);
                FindObjectOfType<FirstGame>().CountdownFillEmptyUse();
                //有物件在桌上必須回到原設定
                //FindObjectOfType<IteamOpenOnTable>().touchingFixedItemOpen = false;  //重製外組件打開觸發      必須先鎖定物件
                //FindObjectOfType<IteamOpenOnTable>().hasitem = false;
            }

        }
    }

    //  private void OnTriggerEnter2D(Collider2D hit)
    // {
    // 確認碰撞的物件是fixeditemOpenFinished時的處理
    //   if (Application.loadedLevelName == "TeachGame" || Application.loadedLevelName == "FirstGame")     //新手教學與關卡共通
    //  {
    //   if (hit.gameObject.name == "fixeditemOpenFinished1")
    //   {
    //      Destroy(ShouldDestroy);
    //  }
    //  if (hit.gameObject.name == "fixeditemOpenFinished2")
    //   {
    //      Destroy(ShouldDestroy);
    //  }
    //  if (hit.gameObject.name == "fixeditemOpenFinished3")
    //  {
    //      Destroy(ShouldDestroy);
    //   }
    //  if (hit.gameObject.name == "fixeditemOpenFinished4")
    //  {
    //      Destroy(ShouldDestroy);
    //  }
    // }
    // }
}
