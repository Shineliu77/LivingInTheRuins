using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlDialogueState : MonoBehaviour
{
    bool State;
    public GameObject DialogueObj;
    public Sprite OpenEye,CloseEye;
    public Button EyeButton;
    private void Start()
    {
        State = true;
    }
    public void Control()
    {
        State = !State;
        DialogueObj.SetActive(State);
        EyeButton.GetComponent<Image>().sprite = State ? OpenEye : CloseEye;
    }
}
