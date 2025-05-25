using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseUse : MonoBehaviour
{
    public Texture2D cursorTexture;  //�񹫼йϦ�m
    public Texture2D cursorTexture2;  //�񹫼йϦ�m
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;


    void Start()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))  //���U�ƹ����
        {
            Cursor.SetCursor(cursorTexture2, hotSpot, cursorMode);
        }
        if (Input.GetMouseButtonUp(0))  //��}�ƹ����
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }
    }
}
