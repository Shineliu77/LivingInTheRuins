using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokeProgressBar : MonoBehaviour
{
    private Vector3 machinefixOriginalPos;
    private GameObject machinefix;
    public List<GameObject> CrashToTriggerMachineFixAn;

    void Start()
    {
        machinefix = GameObject.FindGameObjectWithTag("machinefix");
        if (machinefix != null)
        {
            machinefixOriginalPos = machinefix.transform.position;
        }
        else
        {
            Debug.LogWarning("Machinefix object not found!");
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (CrashToTriggerMachineFixAn.Contains(coll.gameObject))
        {
            machinefix.transform.position = machinefixOriginalPos;
            Debug.Log("Machinefix has been reset to its original position.");
        }
    }
}