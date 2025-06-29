using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CustomerBarCallTry : MonoBehaviour
{
    private Coroutine patienceCoroutine; // 控制耐久倒數的協程
    public float HPMax { get; set; } = 100;

    private Image brokeBarImage; // 自己身上的 Image 組件

    public Sprite targetSwichImage;     //被更換的 耐心值
    public Sprite brokebarRed;     //當耐心值低於 更換耐心值
    private bool SwichImage = false;


    private float hp = 100;
    public float HP
    {
        get => hp;
        set { hp = Mathf.Clamp(value, 0, HPMax); }
    }

    void Awake()
    {
        // 嘗試在子物件中找到 Image 元件
        brokeBarImage = GetComponentInChildren<Image>();
        if (brokeBarImage == null)
        {
            Debug.LogError("找不到 Image 組件！");
        }
    }

    public void StartPatience()
    {
        // 避免重複啟動
        if (patienceCoroutine == null)
        {
            patienceCoroutine = StartCoroutine(PatienceDownTime());
        }
    }

    private void RefreshPatiencebar()
    {
        if (brokeBarImage != null)
        {
            brokeBarImage.fillAmount = HP / HPMax;
        }
    }

    private IEnumerator PatienceDownTime()
    {
        while (HP > 0)
        {
            HP -= HPMax * 0.03f; // 每秒減少 0.3%
            RefreshPatiencebar();
            yield return new WaitForSeconds(1f);

            if (!SwichImage && HP <= 50.0f)
            {
                SwitchImage();
            }
        }
        // TODO：耐久為 0 時可加入動畫、銷毀等
    }

    public void SwitchImage()
    {
        brokeBarImage.sprite = targetSwichImage;
        if (brokeBarImage != null && brokebarRed != null)
        {
            brokeBarImage.sprite = brokebarRed;
            SwichImage = true;
        }
    }
}
