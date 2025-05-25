using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField]private Sprite _normalSprite;  //未點擊
    [SerializeField] private Sprite _pressedSprite;   //點擊
    private SpriteRenderer _skinRenderer;  //MouseUse層級下的

    
    private void Awake()
    {
        
        _skinRenderer = GetComponentInChildren<SpriteRenderer>();  //取得MouseUse層級下的Sprite圖片
    }

    
    // Update is called once per frame
    private void Update()
    {
        if ( _skinRenderer == null) return;
        if (Input .GetMouseButtonDown(0))  //當按下滑鼠
        {
            Cursor.visible = false;  //鼠標不可見
            _skinRenderer.sprite = _pressedSprite;
        }

        if (Input.GetMouseButtonUp(0))
        {
           
            _skinRenderer.sprite = _normalSprite;
        }
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnApplicationFocus(bool focus)  // 回傳焦點給玩家
    {
            Cursor.visible = false;
        }
    }

