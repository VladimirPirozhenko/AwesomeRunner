
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChunkSpawner : MonoBehaviour // TODO: ISpawner
{
    [SerializeField] private StraightChunk chunkPrefab;
    //[SerializeField] private TurningChunk turningChunkPrefab;
    [SerializeField] [Range(1, 100)] private int chunkCount;
    [SerializeField] private float spawnDelay;
    [SerializeField] private ChunkGenerator chunkGenerator;

   // private ChunkPool chunkPool;
    private ObjectPool<Chunk> chunkPool; //¬€Õ≈—“» ¬  À¿——
    private ObjectPool<Chunk> turningChunkPool;
    private Chunk lastChunk;
    private WaitForSeconds waitBeforeSpawn;

    float defaultTurningChunkSpawnCooldown = 10;
    float turningChunkSpawnCooldownTimeLeft = 0;
    bool isTurningChunksOnCooldown = false;

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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            Spawn();
    }
    public void SpawnInitialChunks()
    {
        //chunkPool.CreateChunkPool(chunkPrefab);
        chunkPool = new ObjectPool<Chunk>(CreateChunk, GetChunk, ReleaseChunk, chunkCount);
       // turningChunkPool = new ObjectPool<Chunk>(CreateTurningChunk, GetChunk, ReleaseChunk, chunkCount);
        float zOffset = 0;
        lastChunk = chunkPool[0];
        for (int i = 0; i < chunkCount / 2; i++)
        {
            Chunk chunk = chunkPool.Get();
            //chunk.transform.position = lastChunk.transform.position + (zOffset * Vector3.forward);
            chunk.ChangeTransformBasedOnPreviousChunk(lastChunk);
            //zOffset = chunk.Collider.size.z;
            lastChunk = chunk;
        }
    }
    //private Chunk CreateTurningChunk()
    //{
        //Chunk chunk = Instantiate(turningChunkPrefab);
        //chunk.Init(this);
        //chunk.transform.parent = this.transform;
        //chunk.gameObject.SetActive(false);
        //return chunk;
    //}
    private Chunk CreateChunk()
    {
        Chunk chunk = Instantiate(chunkPrefab);
        chunk.Init(this);
        chunk.transform.parent = this.transform;
        chunk.gameObject.SetActive(false);
        return chunk;
    }
    private void GetChunk(Chunk chunk)
    {
        //chunk.OnChunkPassed += DelayedReturnToPool;
        //chunk.ResetToDefault();
       //lastChunk.Collider.size.z * Vector3.forward;
        chunk.gameObject.SetActive(true);
        //chunkGenerator.Generate(chunk);
    }
    private void ReleaseChunk(Chunk chunk)
    {
        //chunk.OnChunkPassed -= DelayedReturnToPool;
      
        foreach (Coin coin in chunk.Coins)
        {
            chunkGenerator.CoinPool.ReturnToPool(coin);
        }
        foreach (Obstacle obstacle in chunk.Obstacles)
        {
            chunkGenerator.ObstaclePool.ReturnToPool(obstacle);
        }
        chunk.ResetToDefault();
        chunk.Coins.Clear();
        chunk.Obstacles.Clear();
        chunk.gameObject.SetActive(false);
    }
   

    private IEnumerator PutTurningChunksOnCooldown()
    {
        isTurningChunksOnCooldown = true;
        while (turningChunkSpawnCooldownTimeLeft > 0)
        {
            turningChunkSpawnCooldownTimeLeft -= Time.deltaTime;
            yield return null;
        }
        turningChunkSpawnCooldownTimeLeft = defaultTurningChunkSpawnCooldown;
        isTurningChunksOnCooldown = false;
    } 
    private System.Random spawnChanceRandomGenerator = new System.Random();
    public void Spawn()
    {
        Chunk newChunk = null;    
        const double margin = 90.0 / 100.0;
        //int chunkSpawnChance = spawnChanceRandomGenerator.NextDouble() <= margin ? 1 : 0;
        //if (chunkSpawnChance == 1 || isTurningChunksOnCooldown)
        //{
        //     newChunk = chunkPool.Get();
        //}
        //else
        //{         
        //    StartCoroutine(PutTurningChunksOnCooldown());
        //    newChunk = turningChunkPool.Get();  
        //} 
        newChunk = chunkPool.Get();
        //newChunk.transform.position = lastChunk.transform.position + newSpawnPosition;
        newChunk.ChangeTransformBasedOnPreviousChunk(lastChunk);
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

