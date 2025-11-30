using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryFirst : MonoBehaviour
{
    [Header("劇情面板")]
    public GameObject Story1;  //第一段劇情黑幕
    public float fadeDuration = 1.5f;  // 淡入時間
    private bool hasChangedLine1 = false;


    private bool isStory1 = false;

    private DialogueUI dialogue;
  

    void Start()
    {
        Story1.SetActive(true);
        dialogue = FindObjectOfType<DialogueUI>();
        if (dialogue == null )
        {
            Debug.LogError(" 找不到 DialogueUI");
            return;
        }
    }

    void Update()
    {
        DialogueUI dialogue = FindObjectOfType<DialogueUI>();
        if (dialogue == null) return;
        {
            if (!hasChangedLine1 && dialogue.currentLine == 1)    //關閉對話面板
            {
                dialogue.CloseDialoguePanel.SetActive(false);
                hasChangedLine1 = true;
            }
            if (dialogue.currentLine == 6)
            {
                dialogue.CloseDialoguePanel.SetActive(true);
            }

            if (dialogue.currentLine == 61)   //全部看完後
            {
                SceneManager.LoadScene("TeachGame");
            }
        }
    }
}
