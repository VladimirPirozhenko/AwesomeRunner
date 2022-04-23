using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Chunk : MonoBehaviour
{
    private ChunkSpawner spawner;
   // private new Renderer renderer;
    public BoxCollider Collider { get; private set; }
    //State<Chunk> State;
    private void Awake()
    {
        //renderer = GetComponent<Renderer>();
        spawner =  GameObject.Find("ChunkSpawner").GetComponent<ChunkSpawner>();
        Collider = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Player"))
        //{
        //    Player player = other.GetComponent<Player>();
        //    renderer.material.SetColor("_Color", Color.red);
        //}
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            StartCoroutine(spawner.DelayedSpawn());
        }
    }
}
