using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BecomeLarge : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("BecomeLarge"))
        {
            gameObject.transform.localScale += new Vector3(1.5f, 1.5f, 1.5f);
        }
    }
}
