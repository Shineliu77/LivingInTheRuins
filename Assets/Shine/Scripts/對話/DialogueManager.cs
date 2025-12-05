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
        // string path2 = Path.Combine(Application.streamingAssetsPath, ExcelFileName2);
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

        // 跳過標題列，從第1列開始讀（索引從 1）   //避免回傳錯誤
        for (int i = 1; i < table.Rows.Count; i++)
        {
            var row = table.Rows[i];
            string speaker = "";
            string content = "";
            string imageFile = "";
            string imageFile2 = "";
            string imageFile3 = "";
            string audioFile = "";
            string CGFile = "";
            string BGFile = "";

            string bgmFile = ""; // O 欄
            string sfxFile = ""; // P 欄

            if (ExcelFileName == "Dialog.xlsx")   //教學使用
            {
                // var row = table.Rows[i];
                //string speaker = row[1].ToString().Trim(); // B欄：角色
                // string content = row[2].ToString().Trim(); // C欄：對話內容
                //string imageFile = row[3] == null ? "" : row[3].ToString().Trim();    // D欄
                speaker = GetCellString(row, 1); // B欄：角色
                content = GetCellString(row, 2); // C欄：對話內容
                imageFile = GetCellString(row, 3);   // D欄
                audioFile = GetCellString(row, 4);  // E欄
            }

            if (ExcelFileName == "Story.xlsx")   //劇情使用
            {
                speaker = GetCellString(row, 1); // B欄：角色
                content = GetCellString(row, 2); // C欄：對話內容
                imageFile = GetCellString(row, 3);   // D欄  角色圖1
                imageFile2 = GetCellString(row, 4); // E    角色圖2
                imageFile3 = GetCellString(row, 5); // F    角色圖3
                audioFile = GetCellString(row, 9); // J 音效
                CGFile = GetCellString(row, 10); // k CG圖
                BGFile = GetCellString(row, 11); // L bg圖
                bgmFile = GetCellString(row, 14);    // O 背景音樂
                sfxFile = GetCellString(row, 15);    // P 特效音
            }

            DialogueLine dialogue = new DialogueLine
            {
                speaker = speaker,
                content = content,
                imageFile = imageFile,
                imageFile2 = imageFile2,
                imageFile3 = imageFile3,
                audioFile = audioFile,
                CGFile = CGFile,
                BGFile = BGFile,
                bgmFile = bgmFile,
                sfxFile = sfxFile
            };
            //路徑
            string imagePath = Path.Combine(Application.streamingAssetsPath, "Img", "Louise",imageFile);
            string imagePath2 = Path.Combine(Application.streamingAssetsPath, "Img", imageFile2);
            string imagePath3 = Path.Combine(Application.streamingAssetsPath, "Img", imageFile3);
            string audioPath = Path.Combine(Application.streamingAssetsPath, "Sound", audioFile);
            string CGimagePath = Path.Combine(Application.streamingAssetsPath, "Img", CGFile);
            string BGimagePath = Path.Combine(Application.streamingAssetsPath, "Img", BGFile);

            // 載入圖片
            if (File.Exists(imagePath))
            {
                byte[] imgData = File.ReadAllBytes(imagePath);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(imgData);
                dialogue.image = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            }

            if (File.Exists(imagePath2))  //仔入圖片2
            {
                byte[] imgData = File.ReadAllBytes(imagePath2);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(imgData);
                dialogue.image2 = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            }
            if (File.Exists(imagePath3))  //仔入圖片3
            {
                byte[] imgData = File.ReadAllBytes(imagePath3);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(imgData);
                dialogue.image3 = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            }

            // 載入音檔（建議為 .wav）
            if (File.Exists(audioPath))
            {
                WWW www = new WWW("file://" + audioPath);
                while (!www.isDone) { }
                dialogue.audio = www.GetAudioClip();
            }

            if (File.Exists(CGimagePath))  //仔入CG圖片
            {
                byte[] imgData = File.ReadAllBytes(CGimagePath);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(imgData);
                dialogue.CGImage = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            }

            if (File.Exists(BGimagePath))  //仔入bg圖片
            {
                byte[] imgData = File.ReadAllBytes(BGimagePath);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(imgData);
                dialogue.BGImage = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            }
            // 從 Resources/BGM 載入背景音樂 只要填檔名即可
            if (!string.IsNullOrEmpty(bgmFile))
            {
                string bgmName = Path.GetFileNameWithoutExtension(bgmFile);
                dialogue.bgmClip = Resources.Load<AudioClip>("BGM/" + bgmName);

                if (dialogue.bgmClip == null)
                    Debug.LogWarning("找不到 BGM 音檔 Resources/BGM/" + bgmName);
            }

            // 從 Resources/SFX 載入特效音
            if (!string.IsNullOrEmpty(sfxFile))
            {
                string sfxName = Path.GetFileNameWithoutExtension(sfxFile);
                dialogue.sfxClip = Resources.Load<AudioClip>("SFX/" + sfxName);

                if (dialogue.sfxClip == null)
                    Debug.LogWarning("找不到 SFX 音檔 Resources/SFX/" + sfxName);
            }
            dialogueLines.Add(dialogue);
        }

        Debug.Log($"✅ 成功載入 {dialogueLines.Count} 筆對話");
        foreach (var line in dialogueLines)
        {
            Debug.Log($"【{line.speaker}】：{line.content}｜圖片：{line.imageFile}｜音檔：{line.audioFile}");
        }
    }

    private string GetCellString(DataRow row, int index)  //防止欄數不同報錯
    {
        if (index >= row.ItemArray.Length)
            return "";

        if (row[index] == null)
            return "";

        return row[index].ToString().Trim();
    }
}