using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstGameCustomerMarker : MonoBehaviour
{
    public int SeatIndex = -1;
    public FirstGame Owner;
    private bool hasNotified = false; // 增加防護開關
    private void Start()
    {
        Owner = FindObjectOfType<FirstGame>();
    }
    public void OnCustomerLeave()
    {
        if (!hasNotified && Owner != null)
        {
            hasNotified = true;
            Owner.NotifyCustomerFinished(this.gameObject);
        }
    }
    //  private void OnDestroy()
    // {
    // 若是外部直接 Destroy 顧客，這裡確保座位能被釋放
    // if (Owner != null)
    //  {
    //  Owner.NotifyCustomerFinished(gameObject);
    // }
    // }

    private void OnDestroy()
    {
        // 只有在還沒主動通報的情況下才在銷毀時通報
        if (!hasNotified && Owner != null)
        {
            Owner.NotifyCustomerFinished(this.gameObject);
        }
    }
}