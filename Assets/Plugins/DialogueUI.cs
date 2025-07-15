using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueUI : MonoBehaviour
{
     DialogueManager dialogueManager; // 從Excel讀取的資料
    public Text speakerText;
    public Text contentText;
    public Button nextButton;

    public int currentLine = 0;
    [Header("對話結束 一同要關閉的物件")]
    public GameObject CloseObj;

    public Image dialogueImage;
    public AudioSource audioSource;
    void Start()
    {
        dialogueManager = this.GetComponent<DialogueManager>();
        nextButton.onClick.AddListener(ShowNextLine);
        ShowNextLine(); // 一開始顯示第一句
    }

    void ShowNextLine()
    {
        if (currentLine < dialogueManager.dialogueLines.Count)
        {
            var line = dialogueManager.dialogueLines[currentLine];
            speakerText.text = line.speaker;
            contentText.text = line.content;

            if (dialogueImage != null && line.image != null)
                dialogueImage.sprite = line.image;

            if (audioSource != null && line.audio != null)
                audioSource.PlayOneShot(line.audio);

            currentLine++;
        }
        else
        {
            gameObject.SetActive(false);
            if (CloseObj != null)
                CloseObj.SetActive(false);
        }
    }

    public void Reset()
    {
        currentLine = 0;

        if (dialogueManager.dialogueLines.Count > 0)
        {
            var line = dialogueManager.dialogueLines[currentLine];
            speakerText.text = line.speaker;
            contentText.text = line.content;
        }

    }
}
