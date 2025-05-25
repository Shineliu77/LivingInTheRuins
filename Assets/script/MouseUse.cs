using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseUse : MonoBehaviour
{
    public Texture2D cursorTexture;  //放鼠標圖位置
    public Texture2D cursorTexture2;  //放鼠標圖位置
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;


    void Start()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))  //按下滑鼠顯示
        {
            Cursor.SetCursor(cursorTexture2, hotSpot, cursorMode);
        }
        if (Input.GetMouseButtonUp(0))  //放開滑鼠顯示
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }
    }
}
