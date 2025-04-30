using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUpItem : MonoBehaviour
{
    public GameObject energizedEffect;
    public bool isEnergized;
    public Image immediateTargetImage;
    public Sprite immediateNewSprite;
    private Vector3 setupitemOriginalPos;
    private GameObject setupitem;
    private CircularProgressBar progressBar;
    private DragDrog OnDrop;


    private void Start()
    {
        if (OnDrop)
        {
            setupitem.transform.position = setupitemOriginalPos; ;
        }
      
        if (progressBar != null)
        {
            progressBar.onCountdownFinished.AddListener(ChangeImage);
            setupitemOriginalPos = setupitem.transform.position;
        }

        else
        {
            Debug.LogError("ProgressBar is not assigned in Moveto script.");
        }
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag ( "fixeditem"))
            {
                ChangeImage();

            }
        }

    public void ChangeImage()
    {

        //change image after countdown
        if (immediateTargetImage != null && immediateNewSprite != null)
        {
        immediateTargetImage.sprite = immediateNewSprite;
            Debug.Log("immediateTargetImage changed successfully!");
        }

        else
        {
            Debug.LogWarning("immediateTargetImage or immediateNewSprite is null!");
        }

    }

}