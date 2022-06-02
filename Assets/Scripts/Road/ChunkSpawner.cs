
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChunkSpawner : MonoBehaviour // TODO: ISpawner
{
    [SerializeField] private Chunk chunkPrefab;
    //[SerializeField] private Chunk turningChunkPrefab;
    [SerializeField] [Range(1, 100)] private int chunkCount;
    [SerializeField] private float spawnDelay;
    [SerializeField] private ChunkGenerator chunkGenerator;

    private ObjectPool<Chunk> chunkPool; //¬€Õ≈—“» ¬  À¿——
    private ObjectPool<Chunk> turningChunkPool;
    private Chunk lastChunk;
    private WaitForSeconds waitBeforeSpawn;


    //private EMovingDirection spawnDirection;
    private Vector3 newSpawnPosition;
    //SEND DIRECTION AS AN ACTION?
    //Action<EMovingDirection> OnDirectionChanged;
    // private event Action OnChunkReturned;
    private void Start()
    {
        waitBeforeSpawn = new WaitForSeconds(spawnDelay);
        SpawnInitialChunks();
    }
    //private void OnEnable()
    //{
    //    OnChunkReturned += Spawn;
    //}
    //private void OnDisable()
    //{
    //    OnChunkReturned -= Spawn;
    //}
    public void SpawnInitialChunks()
    {
        chunkPool = new ObjectPool<Chunk>(CreateChunk, GetChunk, ReleaseChunk, chunkCount);
       // turningChunkPool = new ObjectPool<Chunk>(CreateTurningChunk, GetChunk, ReleaseChunk, chunkCount);
        float zOffset = 0;
        lastChunk = chunkPool[0];
        for (int i = 0; i < chunkCount / 2; i++)
        {
            Chunk chunk = chunkPool.Get();
            chunk.transform.position = lastChunk.transform.position + (zOffset * Vector3.forward);
            zOffset = chunk.Collider.size.z;
            lastChunk = chunk;
        }
    }
    private Chunk CreateTurningChunk()
    {
        //Chunk chunk = Instantiate(turningChunkPrefab);
        //chunk.Init(this);
        //chunk.ResetToDefault();
        //chunk.transform.parent = this.transform;
        //chunk.gameObject.SetActive(false);
        //return chunk;
        return null;
    }
    private Chunk CreateChunk()
    {
        Chunk chunk = Instantiate(chunkPrefab);
        //chunk.gameObject.name 
        chunk.Init(this);
        //chunk.ResetToDefault();
        chunk.transform.parent = this.transform;
        chunk.gameObject.SetActive(false);
        return chunk;
    }
    private void GetChunk(Chunk chunk)
    {
        //chunk.OnChunkPassed += DelayedReturnToPool;
        //chunk.ResetToDefault();
        chunk.gameObject.SetActive(true);
        chunkGenerator.Generate(chunk);
    }
    private void ReleaseChunk(Chunk chunk)
    {
        //chunk.OnChunkPassed -= DelayedReturnToPool;
        chunk.ResetToDefault();
        foreach (Coin coin in chunk.Coins)
        {
            chunkGenerator.CoinPool.ReturnToPool(coin);
        }
        foreach (Obstacle obstacle in chunk.Obstacles)
        {
            chunkGenerator.ObstaclePool.ReturnToPool(obstacle);
        }
      
        chunk.gameObject.SetActive(false);
    }
    private static System.Random s_Generator = new System.Random();
    public void Spawn()
    {
        //Chunk newChunk = null;    
        //const double margin = 90.0 / 100.0;
        //int result = s_Generator.NextDouble() <= margin ? 1 : 0;
        //if (result == 0)
        //{
        Chunk newChunk = chunkPool.Get();
        //}
        //else
        //{
        //    newChunk = turningChunkPool.Get();
        //}
        //
        //

  
        //switch (newChunk.chunkDirection)
        //{
        //    case EChunkDirection.NORTH:
        //        newSpawnPosition = lastChunk.Collider.size.z * Vector3.forward;
        //        Debug.Log("NORTH");
        //        break;
        //    case EChunkDirection.SOUTH:
        //        //chunkToRotate.transform.Rotate(0, -180, 0, Space.World);
        //        Debug.Log("SOUTH -180");
        //        newSpawnPosition = -lastChunk.Collider.size.z * Vector3.forward;
        //        break;
        //    case EChunkDirection.WEST:
        //        //chunkToRotate.transform.Rotate(0, -90, 0, Space.World);
        //        Debug.Log("WEST -90");
        //        newSpawnPosition = -lastChunk.Collider.size.x * Vector3.right;
        //        break;
        //    case EChunkDirection.EAST:
        //        //chunkToRotate.transform.Rotate(0, 90, 0, Space.World);
        //        Debug.Log("EAST 90");
        //        newSpawnPosition = lastChunk.Collider.size.x * Vector3.right;
        //        break;
        //}
       // newChunk.transform.position = lastChunk.transform.position + newSpawnPosition;
        newChunk.transform.position = lastChunk.transform.position + lastChunk.Collider.size.z * Vector3.forward;//lastChunkPosition + newSpawnPosition;
        Debug.Log("NEW CHUNK POS: " + newChunk.transform.position + newChunk.gameObject.activeSelf);
        //for (int i = 0; i < newChunk.CoinPool.Capacity; i++)
        {
           // Debug.Log("NEW CHUNK COIN POS: " + newChunk.CoinPool[i].transform.position + " " + newChunk.CoinPool[i].gameObject.activeSelf);
            //Debug.Log("NEW CHUNK COIN LOCALPOS: " + newChunk.CoinPool[i].transform.localPosition + " " + newChunk.CoinPool[i].gameObject.activeSelf);
        }
        lastChunk = newChunk;
    }
    public void DelayedReturnToPool(Chunk chunk)
    {
        StartCoroutine(ReturnToPool(chunk));
    }
    public IEnumerator ReturnToPool(Chunk chunk) //EVENT
    {
        yield return waitBeforeSpawn;
        chunkPool.ReturnToPool(chunk);
    }
    
}

