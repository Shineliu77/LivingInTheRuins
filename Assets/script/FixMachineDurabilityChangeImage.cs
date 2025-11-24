using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FixMachineDurabilityChangeImage : MonoBehaviour
{
    public SpriteRenderer TargetChange;
    public Sprite OtiangePic;
    public Sprite ChangePic;
    void Start()
    {
        TargetChange.sprite = OtiangePic;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeOrigin()
    {
        TargetChange.sprite = OtiangePic;
    }
    public void ChangePicture()  //¾î¼²«á
    {
        TargetChange.sprite = ChangePic;
    }


}
