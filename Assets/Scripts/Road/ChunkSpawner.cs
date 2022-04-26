
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ADD TURNS
public class ChunkSpawner : MonoBehaviour
{   
    [SerializeField] public float ChunkWidth { get; private set; }
    //[SerializeField] private List<Chunk> chunkPrefabs;
    [SerializeField] private Chunk chunkPrefab;
    [SerializeField] [Range(5,100)] private int chunkCount;

    private ObjectPool<Chunk> chunkPool;
    [SerializeField] private float spawnDelay;

    private Vector3 firstChunkPosition;
    private Vector3 lastChunkPosition;

    private Chunk firstChunk;
    private Chunk lastChunk;
    [SerializeField] private Obstacle obstaclePrefab;
    [SerializeField] private LaneSystem LaneSystem;
    private Vector3 chunkSize;
    private Vector3 obstacleSize;

    ChunkGenerator chunkGenerator;

    public void Generate(Chunk chunk)
    {
        Vector3 obstaclePosition = Vector3.zero;
        obstaclePosition.x = LaneSystem.LaneWidth * UnityEngine.Random.Range(-LaneSystem.Lanes.Count/2, LaneSystem.Lanes.Count/2+1) - obstacleSize.x/2;
        obstaclePosition.y = 0;
        obstaclePosition.z = chunkSize.z;
       // Quaternion obstacleRotation = Quaternion.Euler(0, 90, 0);
        var instance = Instantiate(obstaclePrefab, new Vector3(), new Quaternion());
        instance.transform.position = chunk.transform.position + obstaclePosition;
        instance.transform.SetParent(chunk.transform, true);
    }
    private void Awake()
    {

        chunkPool = new ObjectPool<Chunk>( CreateChunk, ResetChunk, HideChunk, chunkCount); // MAKE INACTIVE AND SHOW ONLY HALF
        LaneSystem = GameObject.Find("LaneSystem").GetComponent<LaneSystem>();
        chunkGenerator = GameObject.Find("ChunkGenerator").GetComponent<ChunkGenerator>();
        chunkSize = chunkPrefab.GetComponent<BoxCollider>().size;
        obstacleSize = obstaclePrefab.GetComponent<BoxCollider>().size;

    }
    void Start()
    {  
        Vector3 initialPosition = Vector3.zero;
        BoxCollider previousCollider = chunkPool[0].Collider;
        firstChunk = chunkPool[0];
        firstChunkPosition = chunkPool[0].transform.position;
        ChunkWidth = previousCollider.size.x;
        //int iteration = 0;
        for (int i = 0; i < chunkCount; i++)
        {
            chunkPool[i].transform.parent = this.transform;
            //iteration++;
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
            Generate(chunkPool[i]);
        }
        //foreach (Chunk chunk in chunkPool)
        //{
        //    chunk.transform.parent = this.transform;
        //    iteration++;
        //    if (iteration > chunkCount/2)
        //    {
        //        chunk.gameObject.SetActive(false);
        //    }    
        //    Vector3 newPosition = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z + previousCollider.size.z);
        //    chunk.transform.position = newPosition;
        //    previousCollider = chunk.Collider;
        //    initialPosition.z = newPosition.z; 
        //    if (chunk.gameObject.activeInHierarchy)
        //    {
        //        lastChunk = chunk;
        //        lastChunkPosition = chunk.transform.position;
        //    }
        //    Generate(chunk);
        //}

    }
    private Chunk CreateChunk()
    {
        //return chunkGenerator.Generate();
        return chunkPrefab;
    }
    private void ResetChunk(Chunk chunk)
    {
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

