using UnityEngine;
[System.Serializable]
public class DialogueLine
{
    public string speaker;
    public string content;
    public string imageFile;
    public string audioFile;

    public Sprite image;       // 圖片資源
    public AudioClip audio;    // 音效資源
}
