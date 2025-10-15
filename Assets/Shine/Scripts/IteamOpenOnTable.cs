using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IteamOpenOnTable : MonoBehaviour
{
    public float SetScale;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D coll)
    {//tag的fixiem物品碰撞
        if (coll.gameObject.CompareTag("fixeditemOpen"))
        {
            this.GetComponent<Collider2D>().enabled = false;
            coll.gameObject.GetComponent<DraggableReturn2D>().enabled = false;
            coll.transform.parent = this.transform;
            coll.transform.localPosition = Vector3.zero;
            coll.transform.localScale = Vector3.one * SetScale;

            if (Application.loadedLevelName == "FirstGame") //讓brokePCB可以拿出來給crab
            {

                if (Application.loadedLevelName == "FirstGame")
                {
                    Transform child = null;
                    foreach (Transform t in coll.transform.GetComponentsInChildren<Transform>(true))  //把外組件打開的子物件brokePCB解除拖曳
                    {
                        if (t.CompareTag("brokePCB"))
                        {
                            child = t; break;
                        }
                    }
                    if (child != null)
                    {
                        DraggableReturn2D childDrag = child.GetComponent<DraggableReturn2D>();
                        GameObject.FindGameObjectWithTag("brokePCB").GetComponent<DraggableReturn2D>().enabled = true;
                        if (childDrag != null) childDrag.enabled = true;
                    }
                }
            }


            if (Application.loadedLevelName == "TeachGame") {
                if (FindObjectOfType<TeachGM>().CustomerNumber == 1)
                {
                    FindObjectOfType<TeachGM>().OpenTeach3();
                }
                if (FindObjectOfType<TeachGM>().CustomerNumber == 2)
                {
                    FindObjectOfType<TeachGM>().OpenTeach6();
                }
            }
        }
    }
}
