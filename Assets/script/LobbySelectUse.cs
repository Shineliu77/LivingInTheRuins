using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LobbySelectUse : MonoBehaviour
{
    private Vector3 LevelSelectOtigin;     // 儲存LobbySelect原本大小
    public GameObject CurrentSelect;  //現在點的
    public GameObject ChangeTo;  //要切換成
    public GameObject ChangeSize;  //縮放的
    public GameObject TextObject;     //文字 

    public float zoomDuration = 0.3f;  //切換秒數

    private bool isZooming = false;
    private float elapsed = 0f;

    //圖示縮放
    private Vector3 startScale;
    private Vector3 midScale;
    private Vector3 endScale;

    //文字縮放
    private Vector3 textStartScale;
    private Vector3 textMidScale;
    private Vector3 textEndScale;

    public string nextSceneName = "";  //切換場警


    //!!
    private enum ZoomState { None, ZoomIn, ZoomOut }
    private ZoomState state = ZoomState.None;
    void Start()
    {
        LevelSelectOtigin = ChangeSize.transform.localScale;

        if (TextObject != null)
            textStartScale = TextObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {// 若沒有正在縮放，直接跳出
        if (!isZooming) return;

        // 更新時間
        elapsed += Time.deltaTime;

        // 計算時間比例 (0 ~ 1)
        float time = elapsed / zoomDuration;

        //  放大階段：從 startScale 漸變到 midScale
        if (state == ZoomState.ZoomIn)
        {
            ChangeSize.transform.localScale = Vector3.Lerp(startScale, midScale, time);
            if (TextObject != null)
            {
                TextObject.transform.localScale = Vector3.Lerp(textStartScale, textMidScale, time);
            }

            // 當放大結束，切換到縮小階段
            if (time >= 1f)
            {
                state = ZoomState.ZoomOut;
                elapsed = 0f; // 重置時間
            }
        }
        //  縮小階段：從 midScale 漸變回 endScale
        else if (state == ZoomState.ZoomOut)
        {
            ChangeSize.transform.localScale = Vector3.Lerp(midScale, endScale, time);
            if (TextObject != null)
            {
                TextObject.transform.localScale = Vector3.Lerp(textMidScale, textEndScale, time);
            }

            // 當縮小完成，結束動畫並關閉指定物件
            if (time >= 1f)
            {
                isZooming = false;

                CloseLobbySelect();

                if (!string.IsNullOrEmpty(nextSceneName))
                {

                    Debug.Log($"[ZoomEffect] 切換場景：{nextSceneName}");
                    SceneManager.LoadScene(nextSceneName);
                }

            }
        }

    }
    public void ChangeLevelSelect()  //進入關卡選擇  //縮放失效
    {
        // 取得目前物件的初始縮放值
        startScale = ChangeSize.transform.localScale;


        // 設定放大 1.2 倍後的目標縮放
        midScale = startScale * 1.02f;

        // 設定縮小回原始大小的最終縮放
        endScale = startScale;

        if (TextObject != null)
        {
            textStartScale = TextObject.transform.localScale;
            textMidScale = textStartScale * 1.02f;
            textEndScale = textStartScale;
        }

        // 啟動動畫流程
        isZooming = true;
        state = ZoomState.ZoomIn; // 先進入放大階段
        elapsed = 0f;             // 歸零時間
    }
    public void CloseLobbySelect()  //到關卡選擇畫面
    {
        if (CurrentSelect != null)
            CurrentSelect.SetActive(false);
        if (ChangeTo != null)
            ChangeTo.SetActive(true);
    }
    public void DelayLoadSceneByName(string sceneName)
    {
        if (isZooming == false)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
