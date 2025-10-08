using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstGameCustomerMarker : MonoBehaviour
{
    public int SeatIndex = -1;
    public FirstGame Owner;
    private void Start()
    {
        Owner = FindObjectOfType<FirstGame>();
    }
    private void OnDestroy()
    {
        // 若是外部直接 Destroy 顧客，這裡確保座位能被釋放
        if (Owner != null)
        {
            Owner.NotifyCustomerFinished(gameObject);
        }
    }
}