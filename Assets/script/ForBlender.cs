using UnityEngine;
using System.Collections;

public class DraggableObject : MonoBehaviour
{
    private Vector3 originalPosition; // 紀錄原始位置
    private bool isDragging = false;

    void Start()
    {
        originalPosition = transform.position; // 在開始時記錄初始位置
    }

    void OnMouseDown()
    {
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            // 讓物件跟隨滑鼠移動（基於世界座標）
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z; // 保持Z軸位置
            transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        StartCoroutine(ReturnToOriginalPosition()); // 放開時平滑返回原位
    }

    private IEnumerator ReturnToOriginalPosition()
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = originalPosition;
        float duration = 0.3f; // 回到原位所需時間
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos; // 確保最終位置精確
    }
}