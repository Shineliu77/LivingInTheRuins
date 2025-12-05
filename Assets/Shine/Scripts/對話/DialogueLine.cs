using UnityEngine;
[System.Serializable]
public class DialogueLine
{
    public string speaker;
    public string content;
    public string imageFile;
    public string imageFile2;
    public string imageFile3;
    public string audioFile;
    public string CGFile;
    public string BGFile;
    public string bgmFile;   // O 欄：背景音樂檔名
    public string sfxFile;   // P 欄：特效音檔名



    public Sprite image;       // 圖片資源
    public Sprite image2;
    public Sprite image3;
    public Sprite CGImage;
    public Sprite BGImage;
    public AudioClip audio;    // 音效資源
    public AudioClip bgmClip; // 從 Resources/BGM 載入
    public AudioClip sfxClip; // 從 Resources/SFX 載入
}
