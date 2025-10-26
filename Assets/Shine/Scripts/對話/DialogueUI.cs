using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.IO;
using System.Collections;
public class DialogueUI : MonoBehaviour
{
     DialogueManager dialogueManager; // 從Excel讀取的資料
    public Text speakerText;
    public Text contentText;
    public Button nextButton;

    int currentLine = 0;
    [Header("對話結束 一同要關閉的物件")]
    public GameObject CloseObj;

    public Image dialogueImage;
    public AudioSource audioSource;
    public AudioClip clip;
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
            //contentText.text ="\u3000\u3000"+line.content;
            contentText.text =line.content;
            Debug.Log(line.image.name);
          
                StartCoroutine(LoadImage(line.imageFile));

                StartCoroutine(LoadAudio(line.audioFile));

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
    IEnumerator LoadImage(string fileName)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Img", fileName);

        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(path))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D tex = DownloadHandlerTexture.GetContent(www);
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                dialogueImage.sprite = sprite;
            }
            else
            {
                Debug.LogError("載入圖片失敗：" + path);
            }
        }
    }
    IEnumerator LoadAudio(string fileName)
    {

        // 檔案系統路徑
        string fsPath = Path.Combine(Application.streamingAssetsPath, "Music", fileName);

        // 非 Android 直接先檢查檔案是否存在
        if (!System.IO.File.Exists(fsPath))
        {
            Debug.LogError($"[DialogueUI] 找不到檔案：{fsPath}");
            yield break;
        }

        // 這行是關鍵：用 GetAudioClip，而不是 GetTexture
        string fileUrl = "file:///" + fsPath.Replace("\\", "/");  // ← 若你堅持不要 URL，見下個段落
        AudioType aType;
        switch (System.IO.Path.GetExtension(fileName).ToLowerInvariant())
        {
            case ".mp3": aType = AudioType.MPEG; break;
            case ".ogg": aType = AudioType.OGGVORBIS; break;
            default: aType = AudioType.WAV; break;
        }

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(fileUrl, aType))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                var clip = DownloadHandlerAudioClip.GetContent(www);
                if (clip != null) audioSource.PlayOneShot(clip);
                else Debug.LogError($"[DialogueUI] 取得到空的 AudioClip：{fileUrl}");
            }
            else
            {
                Debug.LogError($"[DialogueUI] 載入音樂失敗：{fileUrl} | {www.error}");
            }
        }
    }

}
