using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Live2DOffsetCorrector : MonoBehaviour
{
    [Header("手動微調")]
    public Vector3 manualOffset = Vector3.zero;
    public bool isApplyInitialCorrection = false;
    private void Awake()
    {
        // 1. 在 Awake 立即計算一次
        ApplyInitialCorrection();
    }

    //private IEnumerator Start()
    private IEnumerator Start()
    {
        isApplyInitialCorrection = false;
        // 2. 在 Start 延遲一幀，再次強制校正（防止 Live2D 腳本在 Start 覆寫）
        yield return null;
        ApplyInitialCorrection();
    }
    private void Update()
    {
        // if(isApplyInitialCorrection ==false)
        // { 
        //ApplyInitialCorrection();
        // isApplyInitialCorrection = true;
        //Debug.Log("校正");
        // }
        if (!isApplyInitialCorrection)
        {
            // 1. 取得所有渲染器
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            if (renderers == null || renderers.Length == 0) return;

            // 2. 檢查邊界 (Bounds) 是否已經計算出來了
            Bounds combinedBounds = renderers[0].bounds;
            foreach (Renderer r in renderers) combinedBounds.Encapsulate(r.bounds);

            // 如果模型寬度太小（代表還在初始化），就跳過這一幀，下一幀再試
            if (combinedBounds.size.magnitude < 0.1f)
            {
                return;
            }

            // 3. 確定有模型資料了，執行校正
            ApplyInitialCorrection();

            // 4. 校正完才鎖死，保證這是一次「成功的校正」
            isApplyInitialCorrection = true;
            Debug.Log($"<color=green>{gameObject.name} 校正成功</color>");
        }
    }

    public void ApplyInitialCorrection()
    {
        // 取得所有渲染器
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (renderers == null || renderers.Length == 0) return;

        // 計算視覺中心 (世界空間)
        Bounds combinedBounds = renderers[0].bounds;
        foreach (Renderer r in renderers)
        {
            combinedBounds.Encapsulate(r.bounds);
        }

        // 計算中心偏離「根物件」多少
        // 重點：我們只修正子物件，不移動掛有移動腳本的「根物件」
        Vector3 worldOffset = combinedBounds.center - transform.position;
        worldOffset.z = 0; // 2D 遊戲通常不改 Z

        if (transform.childCount > 0)
        {
            // 這裡我們移動的是模型層，這樣根物件在移動時，內部已經是對齊好的
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                child.position -= worldOffset;
                child.localPosition += manualOffset;
            }
        }
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box != null)
        {
            // 重設 Offset 為 0，因為我們已經把模型圖層拉回物件原點了
            // 加上 manualOffset 是為了確保如果你有微調模型，碰撞器會跟著走
            box.offset = (Vector2)manualOffset;
            // 這裡「不要」去動 box.size，維持你在 Inspector 設定好的大小
            Debug.Log($"<color=cyan>[碰撞器校正]</color> 已重置 {name} 的 Offset 並保留原始大小。");
        }
    }

}
