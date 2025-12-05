using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogueAutoPlayer : MonoBehaviour
{
    [Header("對話 UI 腳本")]
    public DialogueUI dialogueUI;

    [Header("自動播放間隔（秒）")]
    public float interval = 5f;

    [Header("啟動時自動播放")]
    public bool autoPlayOnStart = false;

    private Coroutine autoPlayCoroutine;
    public Text AutoState;
    bool SetOnOff;
    void Start()
    {
        if (dialogueUI == null)
        {
            dialogueUI = GetComponent<DialogueUI>();
        }

       
    }

    /// <summary>
    /// 開始自動播放
    /// </summary>
    public void StartAutoPlay()
    {
        AutoState.text = "On";
        if (dialogueUI == null)
        {
            Debug.LogWarning("[DialogueAutoPlayer] 尚未指定 DialogueUI");
            return;
        }

        // 避免重複啟動
        if (autoPlayCoroutine != null)
        {
            StopCoroutine(autoPlayCoroutine);
        }

        autoPlayCoroutine = StartCoroutine(AutoPlayRoutine());
    }

    /// <summary>
    /// 停止自動播放
    /// </summary>
    public void StopAutoPlay()
    {
        AutoState.text = "Off";

        if (autoPlayCoroutine != null)
        {
            StopCoroutine(autoPlayCoroutine);
            autoPlayCoroutine = null;
        }
    }

    /// <summary>
    /// 提供給 Toggle / 按鈕使用
    /// SetAutoPlay(true) 開啟，SetAutoPlay(false) 關閉
    /// </summary>
    public void SetAutoPlay()
    {
        SetOnOff = !SetOnOff;
        if (SetOnOff)
            StartAutoPlay();
        else
            StopAutoPlay();
    }

    private IEnumerator AutoPlayRoutine()
    {
        while (true)
        {
            // 如果對話物件被關掉了，就結束自動播放
            if (dialogueUI == null || !dialogueUI.gameObject.activeSelf)
            {
                autoPlayCoroutine = null;
                yield break;
            }

            // 等待 interval 秒
            yield return new WaitForSeconds(interval);

            // 呼叫下一句
            dialogueUI.ShowNextLine();
        }
    }

    //給跳過使用
    public void StopTimer()
    {
        if (SetOnOff) StopCoroutine(autoPlayCoroutine);
    }
    public void RunTimer() {
        if (SetOnOff) {
            // 避免重複啟動
            if (autoPlayCoroutine != null)
            {
                StopCoroutine(autoPlayCoroutine);
            }

            autoPlayCoroutine = StartCoroutine(AutoPlayRoutine());
        }
    }
}
