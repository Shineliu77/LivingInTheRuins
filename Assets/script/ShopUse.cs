using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
public class ShopUse : MonoBehaviour
{

    public SaveLoad saveLoadSystem;
    [Header("商品頁")]
    public GameObject[] ShopMenu;     //第幾頁商品
    private int ShopMenuPage = 0;       //計算第幾頁商品 
    [Serializable]
    public class Product
    {
        [Header("商品資料")]
        public string ProductID;  //商品id
        public TextMeshProUGUI itemNameUI;  // 商品名
        public string itemName;
        public int[] itemPrice;                // 商品價錢
        public TextMeshProUGUI buyButtonUI;  // 價格
        public Button buyButton;
        public int buyCount = 0;                  // 購買次數 
    }
    public Product[] allProducts;
    public int selectedProductIndex;
    [Header("購買面板")]
    public GameObject SureToBuyPanel;               //購買面板
    public TextMeshProUGUI itemDescriptionUIupsise;       // 顯示說明文字
    public TextMeshProUGUI itemDescriptionUIdownsise;       // 顯示說明文字
    public Button SurebuyButton;                   // 確認購買按鈕
    public Button NoMoneyButton;                   // 無錢購買按鈕
    public Button CancelbuyButton;                   // 取消購買按鈕

    [Header("對話面板")]
    private DialogueUI dialogue;
    public GameObject DialoguePanel;               //購買面板

    //左右鍵
    public Button RightButton;
    public Button LeftButton;
    void Start()
    {
        saveLoadSystem.Load();
        foreach (var p in allProducts)    //讀入商品資訊
        {
            int price = p.itemPrice[p.buyCount];
            p.buyCount = PlayerPrefs.GetInt("BuyCount_" + p.ProductID, 0);
            if (p.buyCount >= p.itemPrice.Length)
            {
                if (p.buyButton != null)
                {
                    p.buyButton.interactable = false;
                    p.buyButtonUI.text = "MAX";
                }
            }
            else
            {
                // 還可升級
                if (p.buyButton != null)
                {
                    p.buyButton.interactable = true;
                }
                if (ShopMenuPage == 0)
                {
                    int LevelPrice = p.itemPrice[p.buyCount];
                    p.buyButtonUI.text = "$" + LevelPrice.ToString();
                }
                if (ShopMenuPage != 0)
                {
                    int LevelPrice = p.itemPrice[p.buyCount];
                    p.buyButtonUI.text = "$ ???";
                }
            }
        }

        // playerMoney = PlayerPrefs.GetInt("SavedScore", 0);  //讀入大廳金額
        //currentDescIndex = PlayerPrefs.GetInt("SaveitemDescriptionUI", 0); // 讀入說明段落
        // buyCount = PlayerPrefs.GetInt("SaveitemBuyCountUI", 0);         // 讀入購買次數
        //  UpdateUI();
        LeftButton.interactable = (ShopMenuPage > 0);
        DialoguePanel.SetActive(true);
        dialogue = FindObjectOfType<DialogueUI>();
        if (dialogue == null)
        {
            dialogue.currentLine = 1;
            Debug.LogError(" 找不到 DialogueUI");
            //return;
        }
    }


    public void UpdatePageButtons()
    {
        // 如果只有一頁，兩個都鎖起
        // if (ShopMenu.Length <= 1)
        //  {
        // LeftButton.interactable = false;
        //  RightButton.interactable = false;
        // return;
        // }

        // 在第 0 頁，左鍵不能按
        LeftButton.interactable = (ShopMenuPage > 0);

        // 在最後一頁，右鍵不能按
        RightButton.interactable = (ShopMenuPage < ShopMenu.Length - 1);
        UpdatebuyButtonUI();
    }

    public void RChagePageButton()
    {
        if (ShopMenuPage < ShopMenu.Length - 1)
        {
            ShopMenuPage++;
            ChagePageUI();
            UpdatePageButtons(); // 每次翻頁都更新按鈕狀態
        }
    }

    public void LChagePageButton()
    {
        if (ShopMenuPage > 0)
        {
            ShopMenuPage--;
            ChagePageUI();
            UpdatePageButtons();
        }
    }
    public void ChagePageUI()
    {

        for (int i = 0; i < ShopMenu.Length; i++)
        {
            // 如果 i 等於當前頁數，就開啟  不等於就關閉
            ShopMenu[i].SetActive(i == ShopMenuPage);
        }

    }
    public void OnClickProduct(int index)               //一道下面那邊  這邊無亂如何都只會開啟 SureToBuyPane並顯示  "是否確認購買 這個           hasEnoughMoney也移到下面
    {
        selectedProductIndex = index; // 記錄玩家點了陣列中的第幾個商品
        Product p = allProducts[index]; // 取得該商品的完整資料包

        //  取得當前價格 (從對應的價格陣列抓取)
        int currentPrice = 0;
        bool hasEnoughMoney = saveLoadSystem.savedScore >= currentPrice;
        if (ShopMenuPage == 0)          //確認是第一頁
        {

            //  if (p.buyCount < p.itemPrice.Length && hasEnoughMoney)    //如果沒超過金錢長度與夠錢
            // {
            SureToBuyPanel.SetActive(true);
            currentPrice = p.itemPrice[p.buyCount];
            //SureToBuyPanel.SetActive(true);

            // 直接從該商品的 nameTextUI 抓取文字顯示在確認視窗
            itemDescriptionUIupsise.text = "是否確認購買" + p.itemNameUI.text + " ?";
            itemDescriptionUIdownsise.text = "購買後可縮短" + p.itemNameUI.text + "作業時間";

            SurebuyButton.gameObject.SetActive(true);
            CancelbuyButton.gameObject.SetActive(true);
            NoMoneyButton.gameObject.SetActive(false);
            // }
            // if (p.buyCount < p.itemPrice.Length && !hasEnoughMoney)    //如果沒超過金錢長度與不夠錢
            // {
            //  SureToBuyPanel.SetActive(true);
            //  itemDescriptionUIupsise.text = "費用不足，無法購買 " + p.itemNameUI.text + " !";
            //  itemDescriptionUIdownsise.text = "遊玩關卡賺取更多報酬吧";
            // NoMoneyButton.gameObject.SetActive(true);
            //  SurebuyButton.gameObject.SetActive(false);
            // CancelbuyButton.gameObject.SetActive(false);
            // }
            //  if(p.buyCount >= p.itemPrice.Length)     //如果超過金錢長度 把按鈕鎖起來
            //   {
            //     p.buyButton.interactable = false;   

            // }
        }
        if (ShopMenuPage == 1 || ShopMenuPage == 2)          //確認是第二頁                之後要有紀錄關卡的是否通過的東西         
        {
            if (dialogue != null)
            {
                dialogue.currentLine = 11;
                dialogue.ShowNextLine();
            }

        }
        // 2. 顯示面板

        //SurebuyButton.gameObject.SetActive(hasEnoughMoney);
        //NoMoneyButton.gameObject.SetActive(!hasEnoughMoney);
    }
    public void SurebuyButtonUse()         //確認鈕錢夠
    {

        Product p = allProducts[selectedProductIndex];
        int price = p.itemPrice[p.buyCount];

        if (saveLoadSystem.savedScore >= price)       //錢夠
        {

            saveLoadSystem.savedScore -= price;
            p.buyCount++;

            PlayerPrefs.SetInt("SavedScore", saveLoadSystem.savedScore);
            PlayerPrefs.SetInt("BuyCount_" + p.ProductID, p.buyCount);
            PlayerPrefs.Save();
            if (p.buyCount < p.itemPrice.Length)       //升級達未上限
            {
                int LevelPrice = p.itemPrice[p.buyCount];
                p.buyButtonUI.text = "$" + LevelPrice.ToString(); //更新錢
            }

            if (p.buyCount >= p.itemPrice.Length)     //升級達上限鎖按鈕
            {
                p.buyButton.interactable = false;
                p.buyButtonUI.text = "MAX";
            }

            SureToBuyPanel.SetActive(false);
            saveLoadSystem.Load();
            var dialogueLineCount = FindObjectOfType<DialogueManager>();
            int total = dialogueLineCount.dialogueLines.Count;
            if (dialogue != null)
            {
                int randomIndex = UnityEngine.Random.Range(1, total - 1);   //第一句是入場的 最後一句是無此商品用的
                dialogue.currentLine = randomIndex;
                dialogue.ShowNextLine();
                // Debug.LogError(" 找不到 DialogueUI");
                //return;
            }
        }
        else                                       //錢不夠
        {
            itemDescriptionUIupsise.text = "費用不足，無法購買 " + p.itemNameUI.text + " !";
            itemDescriptionUIdownsise.text = "遊玩關卡賺取更多報酬吧";

            SurebuyButton.gameObject.SetActive(false);
            CancelbuyButton.gameObject.SetActive(false);
            NoMoneyButton.gameObject.SetActive(true);
        }


        // 存檔時使用 saveID
        // PlayerPrefs.SetInt("SavedScore", saveLoadSystem.savedScore); // 記得存金錢
        // PlayerPrefs.SetInt("BuyCount_" + p.ProductID, p.buyCount);
        //PlayerPrefs.Save();
        // if (p.buyCount >= p.itemPrice.Length)           //按鈕狀態存取
        // {
        //     p.buyButton.interactable = false;
        // }

    }

    void UpdatebuyButtonUI()  //換價格
    {
        foreach (var p in allProducts)
        {
            // 如果現在不是第一頁，就把價格遮起來
            if (ShopMenuPage != 0)
            {
                p.buyButtonUI.text = "$ ???";
            }
            else
            {
                // 如果回到了第一頁，顯示正常價格
                if (p.buyCount < p.itemPrice.Length)
                {
                    p.buyButtonUI.text = "$" + p.itemPrice[p.buyCount];
                }
                else
                {
                    p.buyButtonUI.text = "MAX";
                }
            }
        }
    }
}