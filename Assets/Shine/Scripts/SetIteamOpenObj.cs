using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetIteamOpenObj : MonoBehaviour
{
    public int ID, ProcessorID, ReagentsID;
    public GameObject[] IteamObj;
    public Sprite[] ProcessorImage;
    public Sprite[] ReagentsImage;
    public Sprite CircuitBoard;
    public string MaintenanceProjectName;
    public Vector3 OriginalSize;

    public int currentProcessorIndex = -1;
    public int currentReagentsIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        if (Application.loadedLevelName == "TeachGame")
        {
            CloseIteamObj();
            IteamObj[ID].SetActive(true);
            switch (ID)
            {
                case 0:
                    IteamObj[ID].GetComponent<SpriteRenderer>().sprite = ProcessorImage[ProcessorID];

                    MaintenanceProjectName = ProcessorImage[ProcessorID].name;
                    break;
                case 1:
                    IteamObj[ID].GetComponent<SpriteRenderer>().sprite = CircuitBoard;
                    MaintenanceProjectName = CircuitBoard.name;
                    break;
                case 2:
                    IteamObj[ID].GetComponent<SpriteRenderer>().sprite = ReagentsImage[ReagentsID];
                    MaintenanceProjectName = ReagentsImage[ReagentsID].name;

                    break;
            }
        }

        //第一關使用
        else if (Application.loadedLevelName == "FirstGame")  //隨機開出1～3個 修理元件
        {
            foreach (GameObject obj in IteamObj)
            {
                obj.SetActive(false);
            }

            //修理元件中隨機開出一樣
            int showCount = Random.Range(1, 4);
            List<int> selectedIndexes = new List<int>();

            for (int i = 0; i < showCount; i++)
            {
                int randomIndex;
                do
                {
                    randomIndex = Random.Range(0, IteamObj.Length);
                } while (selectedIndexes.Contains(randomIndex)); // 確保不重複

                selectedIndexes.Add(randomIndex);

                // 開啟物件
                GameObject target = IteamObj[randomIndex];
                target.SetActive(true);


                SpriteRenderer sr = target.GetComponent<SpriteRenderer>();
                if (sr == null) continue;

                // 選中的修理元件中隨機顯示
                switch (randomIndex)
                {
                    case 0:
                        // sr.sprite = ProcessorImage[Random.Range(0, ProcessorImage.Length)];
                        currentProcessorIndex = Random.Range(0, ProcessorImage.Length);
                        sr.sprite = ProcessorImage[currentProcessorIndex];
                        // currentReagentsIndex = -1;
                        break;

                    case 1:
                        sr.sprite = CircuitBoard;
                        break;

                    case 2:
                        //sr.sprite = ReagentsImage[Random.Range(0, ReagentsImage.Length)];
                        currentReagentsIndex = Random.Range(0, ReagentsImage.Length);
                        sr.sprite = ReagentsImage[currentReagentsIndex];
                        break;
                }
            }
        }
        OriginalSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void CloseIteamObj()
    {
        for (int i = 0; i < IteamObj.Length; i++)
        {
            IteamObj[i].SetActive(false);
        }
    }
    public void ResetSize()
    {
        transform.localScale = OriginalSize;
    }
}