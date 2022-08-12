using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Coin : PoolingObject<Coin>, ICollectable,IResettable
{
    [SerializeField] private int coinValue;

    private SinAnimator sinAnimator;
    public Renderer Renderer { get; private set; }
    public BoxCollider Collider { get; private set; }
    
    public static event Action<int> OnCoinCollected;
    public static event Action<Coin> OnCoinDissapeared;
    private void Awake()
    {
        Renderer = GetComponent<Renderer>();
        Collider = GetComponent<BoxCollider>();
        sinAnimator = GetComponent<SinAnimator>();
    }
    public void Collect()
    {
        gameObject.SetActive(false);
        OnCoinCollected?.Invoke(coinValue);
        OnCoinDissapeared?.Invoke(this);
    }
    public void UpdateStartPositionForSinAnimator()
    {
        sinAnimator.UpdateStartPosition();
    }
    public void ResetToDefault()
    {
        //gameObject.transform.localRotation = Quaternion.identity;
        //gameObject.transform.localScale = Vector3.one;  
        //gameObject.transform.localPosition = Vector3.zero;
        //gameObject.SetActive(true);
       // gameObject.transform.SetParent(null);
    }
}
