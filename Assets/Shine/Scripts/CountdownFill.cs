using UnityEngine;
using UnityEngine.UI;

public class CountdownFill : MonoBehaviour
{
    public Image fillImage;     // 指向 UI 的 Image (type 設定為 Filled)
    public float countdownTime; // 倒數總時間（秒）

    public float timer;

    void Start()
    {
        timer = countdownTime;
        fillImage.fillAmount = 1f;
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            // 計算填滿比例
            float fill = Mathf.Clamp01(timer / countdownTime);
            fillImage.fillAmount = fill;
        }
    }
}
