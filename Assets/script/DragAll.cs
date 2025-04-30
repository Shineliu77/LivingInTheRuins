using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAll : MonoBehaviour
{
    private Transform drag = null;
    private Vector3 offset;
    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                drag = hit.transform;
                offset = drag.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            drag = null;
        }
        if (drag!= null)
        {
            drag.position =Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        }
        }
    }

