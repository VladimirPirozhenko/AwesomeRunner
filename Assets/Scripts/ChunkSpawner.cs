
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{   
    [SerializeField] private List<Chunk> chunkPrefabs;
    [SerializeField] [Range(5,50)] private int chunkCount;

    private ObjectPool<Chunk> chunkPool;
    private bool isSpawnStarted = false;
    private void Awake()
    {
        chunkPool = new ObjectPool<Chunk>(chunkPrefabs, chunkCount, true);
    }
    private void OnEnable()
    {
        foreach (Chunk chunk in chunkPool)
        {
           // chunk.OnChunkEntered += Spawn; // ÕŒ¬€≈ ◊¿Õ » Õ≈ ¡”ƒ”“ œŒƒœ»—¿Õ€!
        }      
    }
    IEnumerator triggerSpawnAfter(int delay)
    {
        yield return new WaitForSeconds(delay);
        isSpawnStarted = true;
    }
    IEnumerator triggerSpawn(int delayMultiplier)
    {

        yield return new WaitForSeconds(delay);
        isSpawnStarted = true;
    }


    void Start()
    {
        StartCoroutine(triggerSpawnAfter(delay:3));
        Vector3 initialPosition = Vector3.zero;
        BoxCollider previousCollider = chunkPool.First.Collider;
        foreach (Chunk chunk in chunkPool)
        {      
            chunk.transform.parent = this.transform;
            Vector3 newPosition = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z + previousCollider.size.z);
            chunk.transform.position = newPosition;
            previousCollider = chunk.Collider;
            initialPosition.z = newPosition.z;
        }
    }
    void Spawn()
    {
        if (isSpawnStarted)
        {   
            Chunk firstChunk = chunkPool.First;
            firstChunk.gameObject.SetActive(false);

            Chunk lastChunk = chunkPool.Last;
            Vector3 lastChunkPosition = lastChunk.transform.position;
                              
            chunkPool.FirstToLast();

            Chunk newChunk = chunkPool.Get();
            newChunk.transform.position = new Vector3(lastChunkPosition.x, lastChunkPosition.y, lastChunkPosition.z + lastChunk.Collider.size.z);
        }
    }
    private void OnDisable()
    {
        foreach (Chunk chunk in chunkPool)
        {
           // chunk.OnChunkEntered -= Spawn;
        }
    }
}
