
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ADD TURNS
public class ChunkSpawner : MonoBehaviour
{   
    [SerializeField] public float ChunkWidth { get; private set; }
    //[SerializeField] private Chunk chunkPrefab;
    [SerializeField] [Range(5,100)] private int chunkCount;

    private ObjectPool<Chunk> chunkPool;
    [SerializeField] private float spawnDelay;

    private Vector3 firstChunkPosition;
    private Vector3 lastChunkPosition;

    private Chunk firstChunk;
    private Chunk lastChunk;
    [SerializeField] private Obstacle obstaclePrefab;
    [SerializeField] private LaneSystem LaneSystem;

    [SerializeField] private ChunkGenerator chunkGenerator;

    private void Awake()
    {

        chunkPool = new ObjectPool<Chunk>(chunkGenerator.Generate, ResetChunk, HideChunk, chunkCount); // MAKE INACTIVE AND SHOW ONLY HALF
    }
    void Start()
    {  
        Vector3 initialPosition = Vector3.zero;
        BoxCollider previousCollider = chunkPool[0].Collider;
        firstChunk = chunkPool[0];
        firstChunkPosition = chunkPool[0].transform.position;
        ChunkWidth = previousCollider.size.x;
        for (int i = 0; i < chunkCount; i++)
        {
            chunkPool[i].transform.parent = this.transform;
            if (i > chunkCount / 2)
            {
                chunkPool[i].gameObject.SetActive(false);
            }
            Vector3 newPosition = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z + previousCollider.size.z);
            chunkPool[i].transform.position = newPosition;
            previousCollider = chunkPool[i].Collider;
            initialPosition.z = newPosition.z;
            if (chunkPool[i].gameObject.activeInHierarchy)
            {
                lastChunk = chunkPool[i];
                lastChunkPosition = chunkPool[i].transform.position;
            }
        }
    }
    private void ResetChunk(Chunk chunk)
    {
        chunk.ResetToDefault();
        chunk.gameObject.SetActive(true);
    }
    private void HideChunk(Chunk chunk)
    {
        chunk.gameObject.SetActive(false);
    }

    public IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        Spawn();
    }
    public void Spawn()
    {
        firstChunkPosition.z += firstChunk.Collider.size.z;
        Chunk firstChunkToRemove = chunkPool.TryGetFromPos(firstChunkPosition, true); 
        bool isReturned = chunkPool.ReturnToPool(firstChunkToRemove);
        //Debug.Log(isReturned);
        Chunk newChunk = chunkPool.Get();
        newChunk.transform.position = new Vector3(lastChunkPosition.x, lastChunkPosition.y, lastChunkPosition.z + lastChunk.Collider.size.z);
        lastChunkPosition.z = newChunk.transform.position.z;
    }
}

