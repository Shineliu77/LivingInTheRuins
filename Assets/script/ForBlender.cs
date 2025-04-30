using UnityEngine;
using System.Collections;

public class DraggableObject : MonoBehaviour
{
    private Vector3 originalPosition; // ������l��m
    private bool isDragging = false;

    void Start()
    {
        originalPosition = transform.position; // �b�}�l�ɰO����l��m
    }

    void OnMouseDown()
    {
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            // ��������H�ƹ����ʡ]���@�ɮy�С^
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z; // �O��Z�b��m
            transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        StartCoroutine(ReturnToOriginalPosition()); // ��}�ɥ��ƪ�^���
    }

    private IEnumerator ReturnToOriginalPosition()
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = originalPosition;
        float duration = 0.3f; // �^����һݮɶ�
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos; // �T�O�̲צ�m��T
    }
}