using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeachGM : MonoBehaviour
{
    #region 產生顧客
    [Header("顧客")]
    public GameObject Customer;
    GameObject CustomerPrefab;
    [Header("顧客生成位置")]
    public Transform CustomerProduce;
    #endregion
    #region 產生組件
    [Header("組件")]
    public GameObject Iteam;
    GameObject IteamPrefab;
    [Header("組件生成位置")]
    public Transform IteamProduce;
    #endregion
    #region 第一段說明
    public GameObject Teach1;
    #endregion
    #region 產生打開的組件
    [Header("打開的組件")]
    public GameObject IteamOpen;
    GameObject IteamOpenPrefab;
    [Header("組件生成位置")]
    public Transform IteamOpenProduce;
    #endregion
    #region 第二段說明
    public GameObject Teach2;
    #endregion
    #region 第三段說明
    public GameObject Teach3;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        CustomerPrefab= Instantiate(Customer, CustomerProduce.position, Customer.transform.rotation)as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (IteamPrefab && !Teach1.active& !IteamPrefab.GetComponent<BoxCollider2D>().enabled) {
            IteamPrefab.GetComponent<BoxCollider2D>().enabled = true;
        }
        if (IteamOpenPrefab && !Teach2.active & !IteamOpenPrefab.GetComponent<BoxCollider2D>().enabled)
        {
            IteamOpenPrefab.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
    public void ProduceIteam() {
        IteamPrefab=Instantiate(Iteam, IteamProduce.position, Iteam.transform.rotation)as GameObject;
        Teach1.SetActive(true);
    }
    public void ProduceIteamOpen()
    {
        IteamOpenPrefab = Instantiate(IteamOpen, IteamOpenProduce.position, IteamOpenProduce.transform.rotation) as GameObject;
        Teach2.SetActive(true);
    }
    public void OpenTeach3() {
        Teach3.SetActive(true);

    }
}
