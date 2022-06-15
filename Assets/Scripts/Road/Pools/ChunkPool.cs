using System;
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(ChunkSpawner))]
public class ChunkPool : MonoBehaviour
{
    private ObjectPool<Chunk> pool;
    private ChunkSpawner spawner;
    [SerializeField] private ChunkGenerator generator;
    private void Awake()
    {
        spawner = GetComponent<ChunkSpawner>();
    }
    public void CreateChunkPool(Chunk chunkPrefab) 
    {
        Func<Chunk> createChunk = () =>
        {
            Chunk chunk = Instantiate(chunkPrefab);
            chunk.Init(spawner);
            chunk.gameObject.SetActive(false);
            chunk.transform.SetParent(gameObject.transform, false);
            return chunk;
        };
        Action<Chunk> getChunk = (Chunk chunk) =>
        {
            //Debug.LogError("COIN_POS_GET: " + coin.transform.position);
            //chunk.transform.position = spawner.lastChunk.transform.position + spawner.lastChunk.Collider.size.z * Vector3.forward;
            chunk.gameObject.SetActive(true);
            generator.Generate(chunk);

        };
        Action<Chunk> releaseChunk = (Chunk chunk) =>
        {
            //Debug.LogError("COIN_POS_RELEASE: " + coin.transform.position);
            foreach (Coin coin in chunk.Coins)
            {
                generator.CoinPool.ReturnToPool(coin);
            }
            foreach (Obstacle obstacle in chunk.Obstacles)
            {
                generator.ObstaclePool.ReturnToPool(obstacle);
            }
            chunk.ResetToDefault();
            chunk.Coins.Clear();
            chunk.Obstacles.Clear();
            chunk.gameObject.SetActive(false);

        };
        pool = new ObjectPool<Chunk>(createChunk, getChunk, releaseChunk, 100);
    }
    public Chunk GetFromPool()
    {
        return pool.Get();
    }
    public void ReturnToPool(Chunk coin)
    {
        pool.ReturnToPool(coin);
    }
}
