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

    public int currentLine = 0;
    [Header("對話結束 一同要關閉的物件")]
    public GameObject CloseObj;

    public Image dialogueImage;
    public Image dialogueImage2; //圖片2
    public Image dialogueImage3; //圖片3
    public Image CGImageUse; //cg圖
    public Image BGImageUse; //cg圖
    private string currentCGFileName = null; // 記錄目前顯示的CG檔名
    private string currentBGFileName = null; // 記錄目前顯示的BG檔名
    public float Duration;
    public AudioSource audioSource;
    public AudioClip clip;

    public GameObject CloseDialoguePanel;  //關對話面板
    void Start()
    {
        dialogueManager = this.GetComponent<DialogueManager>();
        nextButton.onClick.AddListener(ShowNextLine);
        ShowNextLine(); // 一開始顯示第一句
    }

    public void ShowNextLine()
    {
        if (currentLine < dialogueManager.dialogueLines.Count)
        {
            var line = dialogueManager.dialogueLines[currentLine];

            // 避免 null 導致中斷
            if (line == null)
            {
                Debug.LogError($"第 {currentLine} 行對話資料為 null");
                currentLine++;
                return;
            }

            speakerText.text = line.speaker;
            contentText.text = line.content;
            //contentText.text ="\u3000\u3000"+line.content;
            // Debug.Log(line.image.name);

            //    StartCoroutine(LoadImage(line.imageFile));
            StartCoroutine(LoadAudio(line.audioFile));
            // 先關閉圖片  
            // if (dialogueImage != null)
            //    dialogueImage.gameObject.SetActive(false);

            //  若 Excel 有圖片檔名，才嘗試載入
            if (!string.IsNullOrEmpty(line.imageFile))
            {
                StartCoroutine(LoadImage(line.imageFile));
            }
            //  若 Excel 有圖片檔名，才嘗試載入
            if (!string.IsNullOrEmpty(line.imageFile2))
            {
                StartCoroutine(LoadImage2(line.imageFile2));
            }
            if (!string.IsNullOrEmpty(line.imageFile3))
            {
                StartCoroutine(LoadImage3(line.imageFile3));
            }
            // if (!string.IsNullOrEmpty(line.CGFile))  //讀入cg
            // {
            StartCoroutine(LoadCGImage(line.CGFile));
            //}
            StartCoroutine(LoadBGImage(line.BGFile));

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
        if (string.IsNullOrEmpty(fileName))
        {
            dialogueImage.gameObject.SetActive(false);
            yield break;
        }

        string path = Path.Combine(Application.streamingAssetsPath, "Img", fileName);

        if (!File.Exists(path))
        {
            Debug.LogWarning($" 找不到圖片檔案：{path}");
            dialogueImage.gameObject.SetActive(false);  //沒圖把圖關起來
            yield break;
        }

        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(path))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D tex = DownloadHandlerTexture.GetContent(www);
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                if (dialogueImage != null)
                    dialogueImage.sprite = sprite;
                dialogueImage.gameObject.SetActive(true);  //有圖把圖打開
            }
            else
            {
                Debug.LogWarning($"載入圖片失敗：{path}");
                dialogueImage.gameObject.SetActive(false); //沒圖把圖關起來
            }
        }
    }
    IEnumerator LoadImage2(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            dialogueImage2.gameObject.SetActive(false);
            yield break;
        }

        string path = Path.Combine(Application.streamingAssetsPath, "Img", fileName);

        if (!File.Exists(path))
        {
            Debug.LogWarning($" 找不到圖片檔案：{path}");
            dialogueImage2.gameObject.SetActive(false);  //沒圖把圖關起來
            yield break;
        }

        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(path))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D tex = DownloadHandlerTexture.GetContent(www);
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                if (dialogueImage2 != null)
                    dialogueImage2.sprite = sprite;
                dialogueImage2.gameObject.SetActive(true);  //有圖把圖打開
            }
            else
            {
                Debug.LogWarning($"載入圖片失敗：{path}");
                dialogueImage2.gameObject.SetActive(false); //沒圖把圖關起來
            }
        }
    }
    IEnumerator LoadImage3(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            dialogueImage3.gameObject.SetActive(false);
            yield break;
        }

        string path = Path.Combine(Application.streamingAssetsPath, "Img", fileName);

        if (!File.Exists(path))
        {
            Debug.LogWarning($" 找不到圖片檔案：{path}");
            dialogueImage3.gameObject.SetActive(false);  //沒圖把圖關起來
            yield break;
        }

        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(path))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D tex = DownloadHandlerTexture.GetContent(www);
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                if (dialogueImage3 != null)
                    dialogueImage3.sprite = sprite;
                dialogueImage3.gameObject.SetActive(true);  //有圖把圖打開
            }
            else
            {
                Debug.LogWarning($"載入圖片失敗：{path}");
                dialogueImage3.gameObject.SetActive(false); //沒圖把圖關起來
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



    IEnumerator LoadBGImage(string fileName)  //bg
    {
        // 如果檔名和目前一樣 直接跳過
        if (fileName == currentBGFileName)
            yield break;

        if (string.IsNullOrEmpty(fileName))
        {
            if (!string.IsNullOrEmpty(currentBGFileName))
            {
                yield return StartCoroutine(FadeOut(BGImageUse, Duration));
                BGImageUse.gameObject.SetActive(false);
                currentBGFileName = null;
            }
            yield break;
        }

        string path = Path.Combine(Application.streamingAssetsPath, "Img", fileName);

        if (!File.Exists(path))
        {
            Debug.LogWarning($" 找不到圖片檔案：{path}");
            if (!string.IsNullOrEmpty(currentBGFileName))
            {
                yield return StartCoroutine(FadeOut(BGImageUse, Duration));
                BGImageUse.gameObject.SetActive(false);
                currentBGFileName = null;
            }
            yield break;
        }

        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(path))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D tex = DownloadHandlerTexture.GetContent(www);
                Sprite newSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));

                BGImageUse.sprite = newSprite;
                BGImageUse.gameObject.SetActive(true);
                currentBGFileName = fileName;

                yield return StartCoroutine(FadeIn(BGImageUse, Duration));
            }
            else
            {
                Debug.LogWarning($"載入圖片失敗：{path}");
                if (!string.IsNullOrEmpty(currentBGFileName))
                {
                    yield return StartCoroutine(FadeOut(BGImageUse, Duration));
                    BGImageUse.gameObject.SetActive(false);
                    currentBGFileName = null;
                }
            }
        }
    }

    IEnumerator LoadCGImage(string fileName)  //CG
    {
        // 如果檔名和目前一樣 直接跳過
        if (fileName == currentCGFileName)
            yield break;

        if (string.IsNullOrEmpty(fileName))
        {
            if (!string.IsNullOrEmpty(currentCGFileName))
            {
                yield return StartCoroutine(FadeOut(CGImageUse, Duration));
                CGImageUse.gameObject.SetActive(false);
                currentCGFileName = null;
            }
            yield break;
        }

        string path = Path.Combine(Application.streamingAssetsPath, "Img", fileName);

        if (!File.Exists(path))
        {
            Debug.LogWarning($" 找不到圖片檔案：{path}");
            if (!string.IsNullOrEmpty(currentCGFileName))
            {
                yield return StartCoroutine(FadeOut(CGImageUse, Duration));
                CGImageUse.gameObject.SetActive(false);
                currentCGFileName = null;
            }
            yield break;
        }

        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(path))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D tex = DownloadHandlerTexture.GetContent(www);
                Sprite newSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));

                CGImageUse.sprite = newSprite;
                CGImageUse.gameObject.SetActive(true);
                currentCGFileName = fileName;

                yield return StartCoroutine(FadeIn(CGImageUse, Duration));
            }
            else
            {
                Debug.LogWarning($"載入圖片失敗：{path}");
                if (!string.IsNullOrEmpty(currentCGFileName))
                {
                    yield return StartCoroutine(FadeOut(CGImageUse, Duration));
                    CGImageUse.gameObject.SetActive(false);
                    currentCGFileName = null;
                }
            }
        }
    }
    IEnumerator FadeIn(Image img, float duration)  //淡入
    {
        Color c = img.color;
        c.a = 0f;
        img.color = c;

        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / duration);
            img.color = c;
            yield return null;
        }

        c.a = 1f;
        img.color = c;
    }

    IEnumerator FadeOut(Image img, float duration)  //淡出
    {
        Color c = img.color;
        c.a = 1f;
        img.color = c;

        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / duration);
            img.color = c;
            yield return null;
        }

        c.a = 0f;
        img.color = c;
    }

}
