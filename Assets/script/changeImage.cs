using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeImage : MonoBehaviour
{
    public CircularProgressBar progressBar;
    public Image targetImage; // 要換圖的目標
    public Sprite newSprite; // 要換的圖片
    public GameObject energizedEffect;




    private void Start()
    {
        // 倒數即是結束換圖
        if (progressBar != null)
        {
            progressBar.onCountdownFinished.AddListener(ChangeImage);

        }

        else
        {
            //Debug.LogError("ProgressBar is not assigned in Moveto script.");
        }
    }

    public void ChangeImage()
    {
        // 倒數即是結束換圖
        if (targetImage != null && newSprite != null)
        {
            targetImage.sprite = newSprite;
            // Debug.Log("Image changed successfully!");
        }

        else
        {
            Debug.LogWarning("Target Image or New Sprite is null!");
        }

    }


}



