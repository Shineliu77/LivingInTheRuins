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
    // Start is called before the first frame update
    void Start()
    {
        CloseIteamObj();
        IteamObj[ID].SetActive(true);
        switch (ID) {
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
        OriginalSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CloseIteamObj() {
        for (int i = 0; i < IteamObj.Length; i++) {
            IteamObj[i].SetActive(false);
        }
    }
    public void ResetSize() {
        transform.localScale = OriginalSize;
    }
}
