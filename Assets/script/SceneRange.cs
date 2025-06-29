using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneRange : MonoBehaviour
{
    public GameObject[] Customer;   // 找場景中的客人
    private float rangeMin = 1f;    // 範圍最小 X 值
    private float rangeMax = 6f;    // 範圍最大 X 值

    public Vector3 scaleChange = new Vector3(1.5f, 1.5f, 1.5f); // 客人放大時的額外尺寸

    private Dictionary<GameObject, Vector3> originalScales = new Dictionary<GameObject, Vector3>(); // 紀錄每個客人的原始大小

    void Update()
    {
        // 找到有Customer 的TAG物件
        Customer = GameObject.FindGameObjectsWithTag("Customer");

        foreach (GameObject customer in Customer)
        {
            float posX = customer.transform.position.x;

            // 如果沒被記錄原始縮放，就記下來
            if (!originalScales.ContainsKey(customer))
            {
                originalScales[customer] = customer.transform.localScale;
            }

            Vector3 originalScale = originalScales[customer];

            // 進入指定範圍放大
            if (posX >= rangeMin && posX <= rangeMax)
            {
                customer.transform.localScale = Vector3.Lerp(customer.transform.localScale, originalScale + scaleChange, Time.deltaTime * 5f);
            }
            else // 離開範圍還原大小
            {
                customer.transform.localScale = Vector3.Lerp(customer.transform.localScale, originalScale, Time.deltaTime * 5f);
            }
        }
    }
}
