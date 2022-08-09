using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    //[SerializeField] private Coin coinPrefab;
    [SerializeField] private Obstacle obstaclePrefab;
    [SerializeField] private LaneSystem LaneSystem;
    [SerializeField] private int gridColumns;
    public CoinPool CoinPool { get; private set; }
    [field: SerializeField] public List<ObstaclePool> ObstaclePools { get; private set; }
    
    private void Awake()
    {
        //obstacleSize = obstaclePrefab.GetComponent<BoxCollider>().size;
        //coinSize = coinPrefab.GetComponent<BoxCollider>().size;
        //CoinPool = GetComponent<CoinPool>();
        //ObstaclePool = GetComponent<ObstaclePool>();
        //CoinPool.CreateCoinPool(coinPrefab);
        //ObstaclePool.CreateObstaclePool(obstaclePrefab);
    }
    public Chunk Generate(Chunk chunkToFill)
    {
        //var obstacle = Instantiate(obstaclePrefab);
        var obstaclePool = ObstaclePools.GetRandomElement();
        var obstacle = obstaclePool.GetFromPool();
        chunkToFill.Obstacles.Add(obstacle);    
        obstacle.transform.SetParent(chunkToFill.transform, true);
        obstacle.transform.localPosition = chunkToFill.GridPositions.GetRandomElement();
        ////  int randomChunkPrefab = Random.Range(0, chunkPrefabs.Count);
        ////Chunk chunk = Instantiate(chunkPrefabs[randomChunkPrefab], new Vector3(), new Quaternion());
        //Obstacle obstacle = ObstaclePool.GetFromPool();//chunkToFill.ObstaclePool.Get();
        //obstacle.transform.rotation = Quaternion.identity;
        //Vector3 obstaclePosition = Vector3.zero;
        //int randomObstacleLane = Random.Range(-LaneSystem.Lanes.Count / 2, LaneSystem.Lanes.Count / 2 + 1);
        //obstaclePosition.x = LaneSystem.LaneWidth * randomObstacleLane;
        //obstaclePosition.y = 0;
        //obstaclePosition.z = obstacle.Collider.size.z;
        //obstacle.transform.position = chunkToFill.transform.position + obstaclePosition;
        //obstacle.transform.SetParent(chunkToFill.transform, true);        
        //coinElevation = obstacle.Collider.size.y * 2;
        //obstacle.gameObject.SetActive(true);
        //int randomCoinsLane = Random.Range(-LaneSystem.Lanes.Count / 2, LaneSystem.Lanes.Count / 2 + 1);
        //coinsOnLane = Random.Range(0, maxCoinsOnLane + 1);
        //chunkToFill.Obstacles.Add(obstacle);
        //for (int i = 0; i < coinsOnLane; i++)
        //{
        //    Coin coin = CoinPool.GetFromPool();//chunkToFill.CoinPool.Get();
        //    //coin.transform.SetParent(null);
        //    //coin.transform.localPosition = Vector3.zero;
        //    //coin.transform.position = Vector3.zero;
        //    //coin.transform.rotation = Quaternion.identity;
        //    Vector3 coinPosition = Vector3.zero;
        //    coinPosition.z = i * coinSize.z * 20 * coin.transform.localScale.z; 
        //    coinPosition.x = LaneSystem.LaneWidth * randomCoinsLane;
        //    //if (randomCoinsLane == randomObstacleLane)
        //    //{
        //    coinPosition.y = coinElevation;
        //    //}
        //    //else
        //    //{
        //    //    coinPosition.y = coin.Collider.size.y * coin.transform.localScale.y; //*scale
        //    //}
        //    //coin.gameObject.SetActive(true);
        //    //coin.transform.position = Vector3.zero;
        //    //coin.transform.localPosition = coinPosition;
        //    coin.transform.position = chunkToFill.transform.position + coinPosition;//coinPosition;
        //                                                                            //coin.transform.SetParent(chunkToFill.transform, true);
        //    coin.UpdateStartPositionForSinAnimator();


        //   // Debug.Log("CoinPosition: " + coin.transform.position);
        //    chunkToFill.Coins.Add(coin);
        //}
        //Debug.Log("ChunkPosition: " + chunkToFill.transform.position);
        //Debug.LogError("Chunk: " + chunkToFill.gameObject.name + " " + chunkToFill.CoinPool.Capacity);   
        return chunkToFill;
    }
    private void GenerateCoins()
    {

    }
}
