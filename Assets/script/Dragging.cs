using UnityEngine;

public class Dragging : MonoBehaviour
{
    private bool drag = false;
    private Vector3 offset;
    private float zDistance;

    void Start()
    {
        // �p�⪫�����v�����Z��
        zDistance = Mathf.Abs(Camera.main.WorldToScreenPoint(transform.position).z);
    }

    void Update()
    {
        if (drag)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = zDistance; // �O�o�ɤW z (3d�b�A���󤣬O3d�|����)
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