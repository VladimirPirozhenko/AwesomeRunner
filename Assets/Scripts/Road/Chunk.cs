using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EChunkType { STRAIGHT, TURNING }
public enum EChunkDirection {  NORTH, SOUTH, EAST, WEST }
[RequireComponent(typeof(BoxCollider))]
public class Chunk : MonoBehaviour, IResettable
{
    private ChunkSpawner spawner;
    [SerializeField] private Coin defaultCoinPrefab;
    [SerializeField] private Obstacle defaultObstaclePrefab;
    public ObjectPool<Obstacle> ObstaclePool { get; private set; } //¬€Õ≈—“» ¬ Œ“ƒ≈À‹Õ€…  À¿——, «ƒ≈—‹ ’–¿Õ»“‹ —œ»— »  
    public ObjectPool<Coin> CoinPool { get; private set; }

    public List<Coin> Coins { get; private set; }
    public List<Obstacle> Obstacles { get; private set; }
    public BoxCollider Collider { get; private set; }
    public EChunkType chunkType { get; private set; }
    public EChunkDirection chunkDirection { get; private set; }
    public void Init(ChunkSpawner spawner)
    {
        this.spawner = spawner; 
        chunkDirection = EChunkDirection.NORTH; 
    }
    private void Awake()
    {
        Collider = GetComponent<BoxCollider>();
        Coins = new List<Coin>();   
        Obstacles = new List<Obstacle>();   

        //CreateObstaclePool(defaultObstaclePrefab);
        //CreateCoinPool(defaultCoinPrefab);
    }
    private void OnEnable()
    {
        //Coin.OnCoinDissapeared += CoinPool.ReturnToPool;
    }
    private void OnDisable()
    {
        //Coin.OnCoinDissapeared -= CoinPool.ReturnToPool;
    }
    public void CreateObstaclePool(Obstacle obstaclePrefab) // ¬€Õ≈—“» ¬  À¿——€
    {
        Func<Obstacle> createObstacle = () =>
        {
            Obstacle obstacle = Instantiate(obstaclePrefab);
            obstacle.gameObject.SetActive(false);
            obstacle.transform.SetParent(gameObject.transform, false);
            return obstacle;
        };
        Action<Obstacle> getObstacle = (Obstacle obstacle) =>
        {
            obstacle.gameObject.SetActive(true);
        };
        Action<Obstacle> releaseObstacle = (Obstacle obstacle) =>
        {
            obstacle.gameObject.SetActive(false);
        };
        ObstaclePool = new ObjectPool<Obstacle>(createObstacle, getObstacle, releaseObstacle, 5);
    }
    public void CreateCoinPool(Coin coinPrefab)
    {
        Func<Coin> createCoin = () =>
        {
            Coin coin = Instantiate(coinPrefab);
            coin.gameObject.SetActive(false);
            coin.transform.SetParent(gameObject.transform, false);
            return coin;
        };
        Action<Coin> getCoin = (Coin coin) =>
        {
            coin.gameObject.SetActive(true);
        };
        Action<Coin> releaseCoin = (Coin coin) =>
        {
            //coin.ResetToDefault();
            coin.transform.localPosition = Vector3.zero;
            coin.transform.position = Vector3.zero;
            coin.transform.rotation = Quaternion.identity;
            coin.gameObject.SetActive(false);
        };
        CoinPool = new ObjectPool<Coin>(createCoin, getCoin, releaseCoin, 5);
    }
    public void ResetToDefault()
    {
        //var activeObstacles = ObstaclePool.GetActiveElements();
        //foreach (var obstacle in ObstaclePool)
        //{
        //    //obstacle.ResetToDefault();
        //    ObstaclePool.ReturnToPool(obstacle);
        //}
        ////var activeCoins = CoinPool.GetActiveElements();
        //foreach (var coin in CoinPool)  
        //{
        //   // coin.ResetToDefault();
        //    CoinPool.ReturnToPool(coin);
        //}
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            //OnChunkPassed?.Invoke(this);
            spawner.DelayedReturnToPool(this);
            spawner.Spawn();
        }
    }

    void RotateChunk()
    {
        switch (chunkDirection)
        {
            case EChunkDirection.NORTH:
                //newSpawnPosition = lastChunk.Collider.size.z * Vector3.forward;
                Debug.Log("NORTH");
                break;
            case EChunkDirection.SOUTH:
                transform.Rotate(0, -180, 0, Space.World);
                Debug.Log("SOUTH -180");
                //newSpawnPosition = -lastChunk.Collider.size.z * Vector3.forward;
                break;
            case EChunkDirection.WEST:
                transform.Rotate(0, -90, 0, Space.World);
                Debug.Log("WEST -90");
                //newSpawnPosition = -lastChunk.Collider.size.x * Vector3.right;
                break;
            case EChunkDirection.EAST:
                transform.Rotate(0, 90, 0, Space.World);
                Debug.Log("EAST 90");
                //newSpawnPosition = lastChunk.Collider.size.x * Vector3.right;
                break;
        }
        //if (chunkToRotate.chunkType == EChunkType.LEFT_CHUNK)
        //{
        //    NextDirection(false);
        //    Debug.Log("CHANGING!");
        //}
    }
    }
