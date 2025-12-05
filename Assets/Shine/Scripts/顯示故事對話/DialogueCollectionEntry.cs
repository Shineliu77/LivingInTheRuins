using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueCollectionEntry : MonoBehaviour
{
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI contentText;
    public DialogueCollectionUI collectionUI;

    public void OnOpenCollection()
    {
        collectionUI.RefreshCollection();
        collectionUI.gameObject.SetActive(true);
    }
    public void Setup(string speaker, string content)
    {
        if (speakerText != null)
            speakerText.text = speaker;

        if (contentText != null)
            contentText.text = content;
    }
}
