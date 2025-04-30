using UnityEngine;

public class Dragging : MonoBehaviour
{
    private bool drag = false;
    private Vector3 offset;
    private float zDistance;

    void Start()
    {
        // 計算物件到攝影機的距離
        zDistance = Mathf.Abs(Camera.main.WorldToScreenPoint(transform.position).z);
    }

    void Update()
    {
        if (drag)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = zDistance; // 記得補上 z (3d軸，物件不是3d會消失)
            transform.position = Camera.main.ScreenToWorldPoint(mousePos) + offset;
        }
    }

    private void OnMouseDown()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = zDistance;
        offset = transform.position - Camera.main.ScreenToWorldPoint(mousePos);
        drag = true;
    }

    private void OnMouseUp()
    {
        drag = false;
    }

}