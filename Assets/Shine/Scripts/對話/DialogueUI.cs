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

    [Header("角色與CG、背景圖")]
    public Image dialogueImage;
    public Image dialogueImage2; //圖片2
    public Image dialogueImage3; //圖片3
    public Image CGImageUse;     // CG圖
    public Image BGImageUse;     // 背景圖

    private string currentCGFileName = null; // 記錄目前顯示的CG檔名
    private string currentBGFileName = null; // 記錄目前顯示的BG檔名

    public float Duration;

    [Header("音效相關")]
    public AudioSource bgmSource;   // O欄 BGM 用
    public AudioSource sfxSource;   // P欄 SFX 用
    public AudioSource voiceSource; // 每句對話的語音或音效

    public GameObject CloseDialoguePanel;  //關對話面板

    void Start()
    {
        if (Application.loadedLevelName != "TeachGame" && Application.loadedLevelName != "Shop")
        {
            CGImageUse.gameObject.SetActive(false);
            BGImageUse.gameObject.SetActive(false);
        }

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

            // 文字
            speakerText.text = line.speaker;
            contentText.text = line.content;
            //contentText.text = "\u3000\u3000" + line.content; // 若要前面補全形空格

            // 語音（DialogueManager 已幫你載好 line.audio）
            if (voiceSource != null)
            {
                if (line.audio != null)
                {
                    voiceSource.Stop();
                    voiceSource.clip = line.audio;
                    voiceSource.Play();
                }
                else
                {
                    // 這句沒有語音就停掉
                    voiceSource.Stop();
                    voiceSource.clip = null;
                }
            }

            // BGM：只有在該行有設定 bgmClip 時才切換
            if (bgmSource != null && line.bgmClip != null)
            {
                if (bgmSource.clip != line.bgmClip)
                {
                    bgmSource.clip = line.bgmClip;
                    bgmSource.loop = true;
                    bgmSource.Play();
                }
            }
            // 注意：如果 Excel 該行 O 欄留空，則維持上一首 BGM 不變

            // SFX：只要有設定就播一次
            if (sfxSource != null && line.sfxClip != null)
            {
                sfxSource.PlayOneShot(line.sfxClip);
            }

            // 角色立繪
            if (!string.IsNullOrEmpty(line.imageFile))
            {
                StartCoroutine(LoadImage(line.imageFile));
            }
            else if (dialogueImage != null)
            {
                dialogueImage.gameObject.SetActive(false);
            }

            if (!string.IsNullOrEmpty(line.imageFile2))
            {
                StartCoroutine(LoadImage2(line.imageFile2));
            }
            else if (dialogueImage2 != null)
            {
                dialogueImage2.gameObject.SetActive(false);
            }

            if (!string.IsNullOrEmpty(line.imageFile3))
            {
                StartCoroutine(LoadImage3(line.imageFile3));
            }
            else if (dialogueImage3 != null)
            {
                dialogueImage3.gameObject.SetActive(false);
            }

            // CG / BG（依舊用 StreamingAssets 載圖，不動你的流程）
            StartCoroutine(LoadCGImage(line.CGFile));
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

            // 如果需要重播第一句的音效，也可以在這裡再呼叫一次 ShowNextLine() 或複製上面的播放邏輯
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
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                                              new Vector2(0.5f, 0.5f));
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
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                                              new Vector2(0.5f, 0.5f));
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
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                                              new Vector2(0.5f, 0.5f));
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
                Sprite newSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                                                 new Vector2(0.5f, 0.5f));

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
                Sprite newSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                                                 new Vector2(0.5f, 0.5f));

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
