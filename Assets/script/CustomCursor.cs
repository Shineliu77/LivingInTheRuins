using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField]private Sprite _normalSprite;  //���I��
    [SerializeField] private Sprite _pressedSprite;   //�I��
    private SpriteRenderer _skinRenderer;  //MouseUse�h�ŤU��

    
    private void Awake()
    {
        
        _skinRenderer = GetComponentInChildren<SpriteRenderer>();  //���oMouseUse�h�ŤU��Sprite�Ϥ�
    }

    
    // Update is called once per frame
    private void Update()
    {
        if ( _skinRenderer == null) return;
        if (Input .GetMouseButtonDown(0))  //����U�ƹ�
        {
            Cursor.visible = false;  //���Ф��i��
            _skinRenderer.sprite = _pressedSprite;
        }

        if (Input.GetMouseButtonUp(0))
        {
           
            _skinRenderer.sprite = _normalSprite;
        }
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnApplicationFocus(bool focus)  // �^�ǵJ�I�����a
    {
            Cursor.visible = false;
        }
    }

