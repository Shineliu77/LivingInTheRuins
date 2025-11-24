using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IteamOpenOnTable : MonoBehaviour
{
    public float SetScale;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame

    private void OnCollisionEnter2D(Collision2D coll)
    {//tag的fixiem物品碰撞
        if (Application.loadedLevelName == "TeachGame")
        {
            if (coll.gameObject.CompareTag("fixeditemOpen"))
            {
                this.GetComponent<Collider2D>().enabled = false;
                coll.gameObject.GetComponent<DraggableReturn2D>().enabled = false;
                coll.transform.parent = this.transform;
                coll.transform.localPosition = Vector3.zero;
                coll.transform.localScale = Vector3.one * SetScale;

                if (Application.loadedLevelName == "TeachGame")
                {
                    if (FindObjectOfType<TeachGM>().CustomerNumber == 1)
                    {
                        FindObjectOfType<TeachGM>().OpenTeach3();
                        LiveTwoDChangeImage LiveTwoDChangeImageManager = FindObjectOfType<LiveTwoDChangeImage>();
                        LiveTwoDChangeImageManager.LiveTDChangeImgOk = true;
                    }
                    if (FindObjectOfType<TeachGM>().CustomerNumber == 2)
                    {
                        FindObjectOfType<TeachGM>().OpenTeach6();
                    }
                }
            }
        }

        //讓brokePCB可以拿出來給crab
        if (Application.loadedLevelName == "FirstGame")
        {
            if (coll.gameObject.CompareTag("fixeditemOpen"))
            {
                this.GetComponent<Collider2D>().enabled = false;
                coll.gameObject.GetComponent<DraggableReturn2D>().enabled = false;
                coll.transform.parent = this.transform;
                coll.transform.localPosition = Vector3.zero;
                coll.transform.localScale = Vector3.one * SetScale;
                Debug.Log("偵測到 fixeditemOpen 碰撞！");

                // 檢查是否有顯示 CircuitBoard
                if (SetIteamOpenObj.HasActiveCircuitBoard)
                {
                    // 檢查這個物件是否有子物件
                    Debug.Log("找到brokePCB了！");
                    if (coll.transform.childCount > 0)
                    {
                        Debug.Log("找到brokePCB了2！");
                        // 嘗試找到 brokePCB 子物件
                        Transform brokeChild = null;
                        foreach (Transform child in coll.transform)
                        {
                            if (child.CompareTag("brokePCB"))
                            {
                                Debug.Log("找到brokePCB的tag！");
                                brokeChild = child;
                                Debug.Log("找到子物件brokePCB！");
                                break;

                            }
                        }

                        if (brokeChild != null)
                        {
                            // 啟用子物件的拖曳腳本
                            var drag = brokeChild.GetComponent<DraggableReturn2D>();
                            var col2d = brokeChild.GetComponent<Collider2D>();
                            Debug.Log("brokePCB 得到拖曳程式！");
                            if (drag != null)
                            {
                                if (col2d != null && !col2d.enabled)
                                    col2d.enabled = true;
                                drag.enabled = true;
                                Debug.Log("brokePCB 拖曳啟用！");
                            }
                            else
                            { Debug.LogWarning("brokePCB 找不到 DraggableReturn2D 程式"); }
                        }
                        else { Debug.LogWarning("fixeditemOpen 底下找不到 brokePCB 子物件"); }
                    }
                    else { Debug.LogWarning("fixeditemOpen 沒有任何子物件"); }
                }
                else { Debug.Log("目前沒有啟用的 CircuitBoard，因此不允許拖曳"); }
            }
        }

    }
}
