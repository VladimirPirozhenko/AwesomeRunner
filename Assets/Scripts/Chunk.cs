using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public event Action<Chunk> OnChunkEntered;
    public BoxCollider Collider { get; private set; }
    private void Awake()
    {
        Collider = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnChunkEntered?.Invoke(this);
        }
    }
}
