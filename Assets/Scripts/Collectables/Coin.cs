using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Coin : MonoBehaviour, ICollectable
{
    public Renderer Renderer { get; private set; }

    private void Awake()
    {
        Renderer = GetComponent<Renderer>();  
       
    }
    public void Collect()
    {
        gameObject.SetActive(false);
    }

}
