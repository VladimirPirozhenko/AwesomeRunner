using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventBus 
{
    public static UnityEvent<int> OnCoinCollected = new UnityEvent<int>();
    public static void SendCoinCollected(int coinCount)
    {
        OnCoinCollected.Invoke(coinCount);
    }
}
