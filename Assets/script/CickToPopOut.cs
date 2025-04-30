using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CickToPopOut : MonoBehaviour
{

    private Renderer objRenderer;
    private Color originalColor;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();

        if (objRenderer != null)
        {
            originalColor = objRenderer.material.color;

            // 設定初始為隱形
            Color transparentColor = originalColor;
            transparentColor.a = 0f;
            objRenderer.material.color = transparentColor;
        }
    }

    private void OnMouseDown()
    {
        if (objRenderer != null)
        {
            Color visibleColor = originalColor;
            visibleColor.a = 1f;
            objRenderer.material.color = visibleColor;
        }
    }
}
