using UnityEngine;
using System.IO;
using System.Data;
using Excel;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public string ExcelFileName = "Dialog.xlsx"; // 放在 StreamingAssets 資料夾
    public string SheetName;

    public List<DialogueLine> dialogueLines = new List<DialogueLine>();

    void Awake()
    {
        string path = Path.Combine(Application.streamingAssetsPath, ExcelFileName);

        if (!File.Exists(path))
        {
            Debug.LogError("❌ 找不到檔案：" + path);
            return;
        }

        FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);
        IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(stream); // .xlsx 使用這個
        DataSet result = reader.AsDataSet();
        reader.Close();

        DataTable table = null;

        // 找出指定的工作表
        foreach (DataTable t in result.Tables)
        {
            if (t.TableName == SheetName)
            {
                table = t;
                break;
            }
        }

        if (table == null)
        {
            Debug.LogError("❌ 找不到工作表：" + SheetName);
            return;
        }

        dialogueLines.Clear();

        // 跳過標題列，從第1列開始讀（索引從 1）
        for (int i = 1; i < table.Rows.Count; i++)
        {
            var row = table.Rows[i];
            string speaker = row[1].ToString().Trim(); // B欄：角色
            string content = row[2].ToString().Trim(); // C欄：對話內容
            string imageFile = row[3].ToString().Trim();    // D欄
            string audioFile = row[4].ToString().Trim();    // E欄
            string imagePath = Path.Combine(Application.streamingAssetsPath, "Img", imageFile);
            string audioPath = Path.Combine(Application.streamingAssetsPath, "Sound", audioFile);

            DialogueLine dialogue = new DialogueLine
            {
                speaker = speaker,
                content = content,
                imageFile = imageFile,
                audioFile = audioFile
            };

            // 載入圖片
            if (File.Exists(imagePath))
            {
                byte[] imgData = File.ReadAllBytes(imagePath);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(imgData);
                dialogue.image = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            }

            // 載入音檔（建議為 .wav）
            if (File.Exists(audioPath))
            {
                WWW www = new WWW("file://" + audioPath);
                while (!www.isDone) { }
                dialogue.audio = www.GetAudioClip();
            }

            dialogueLines.Add(dialogue);
        }

        Debug.Log($"✅ 成功載入 {dialogueLines.Count} 筆對話");
        foreach (var line in dialogueLines)
        {
            Debug.Log($"【{line.speaker}】：{line.content}｜圖片：{line.imageFile}｜音檔：{line.audioFile}");
        }
    }
}