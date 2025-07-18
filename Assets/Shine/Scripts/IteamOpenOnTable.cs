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
    {//tagªºfixiemª««~¸I¼²
        if (coll.gameObject.CompareTag("fixeditemOpen"))
        {
            this.GetComponent<Collider2D>().enabled = false;
            coll.gameObject.GetComponent<DraggableReturn2D>().enabled = false;
            coll.transform.parent = this.transform;
            coll.transform.localPosition = Vector3.zero;
            coll.transform.localScale = Vector3.one * SetScale;
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
