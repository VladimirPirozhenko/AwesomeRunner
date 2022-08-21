using Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    [SerializeField] private LaneSystem LaneSystem;
    public CoinPool CoinPool { get; private set; }
    [field: SerializeField] public List<ObstaclePool> ObstaclePools { get; private set; }

    public Chunk Generate(Chunk chunkToFill)
    {
        if (ObstaclePools.IsEmpty())
            return chunkToFill;
        var obstaclePool = ObstaclePools.GetRandomElement();
        var obstacle = obstaclePool.Spawn();
        chunkToFill.Obstacles.Add(obstacle);
        obstacle.transform.SetParent(chunkToFill.transform, true);
        obstacle.transform.localPosition = chunkToFill.Grid.GetRandomPosition();
        if (obstacle.IsOnAllLanes)
        {
            obstacle.transform.localPosition = new Vector3(LaneSystem.CenterLane * LaneSystem.LaneWidth, transform.localPosition.y, transform.localPosition.z);
        }
        return chunkToFill;
    }
}
