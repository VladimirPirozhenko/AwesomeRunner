using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    [SerializeField] private List<Chunk> chunkPrefabs;
    [SerializeField] private Coin coinPrefab;
    [SerializeField] private Obstacle obstaclePrefab;
    [SerializeField] private LaneSystem LaneSystem;

    private Vector3 obstacleSize;
    private float coinElevation;
    private int coinsOnLane = 5;
    private void Awake()
    {
        obstacleSize = obstaclePrefab.GetComponent<BoxCollider>().size;
    }
    public Chunk Generate()
    {
        int randomChunkPrefab = Random.Range(0, chunkPrefabs.Count);
        Chunk chunk = Instantiate(chunkPrefabs[randomChunkPrefab], new Vector3(), new Quaternion());
       // var values = Enum.GetValues(typeof(SomeEnum));
       // SomeEnum randomValue = (SomeEnum)values[Random.Range(0, values.Length)];
        Obstacle obstacle = Instantiate(obstaclePrefab, new Vector3(), new Quaternion());
        Vector3 obstaclePosition = Vector3.zero;
        int randomObstacleLane = Random.Range(-LaneSystem.Lanes.Count / 2, LaneSystem.Lanes.Count / 2 + 1);
        obstaclePosition.x = LaneSystem.LaneWidth * randomObstacleLane;
        obstaclePosition.y = 0;
        obstaclePosition.z = 0;
        obstacle.transform.position = chunk.transform.position + obstaclePosition;
        obstacle.transform.SetParent(chunk.transform, false);
        chunk.Obstacles.Add(obstacle);

        coinElevation = obstacle.Collider.size.y * 2;
        coinsOnLane = Random.Range(0, 7);
        int randomCoinsLane = Random.Range(-LaneSystem.Lanes.Count / 2, LaneSystem.Lanes.Count / 2 + 1);
        for (int i = 0; i < coinsOnLane; i++)
        { 
            Coin coin = Instantiate(coinPrefab, new Vector3(), new Quaternion());
            Vector3 coinPosition = Vector3.zero;
            coinPosition.z = i * obstacleSize.z * 2;
            coinPosition.x = LaneSystem.LaneWidth * randomCoinsLane;
            if (randomCoinsLane == randomObstacleLane)
            {
                //y = Ax2 + Bx + C
                //where x is the horizontal location of a given point, 
                //y is the vertical location, 
                //and A, B, and C are constants.A cannot be zero.
   
            
                //coinPosition.y = 
                coinPosition.y = coinElevation;
            }
            else
            {
                coinPosition.y = coin.Renderer.bounds.size.y;
            }
           
            coin.transform.position = chunk.transform.position + coinPosition;
            coin.transform.SetParent(chunk.transform, false);
            chunk.Coins.Add(coin);
        }
        return chunk;
    }
    //private List<Obstacle> GenerateObstacles(Chunk chunk)
    //{
    //    Obstacle obstacle = Instantiate(obstaclePrefab, new Vector3(), new Quaternion());
    //    obstacle.transform.position = chunk.transform.position + obstaclePosition;
    //    obstacle.transform.SetParent(chunk.transform, false);

    //    return 
    //}

    private void GenerateCoins()
    {

    }
}
