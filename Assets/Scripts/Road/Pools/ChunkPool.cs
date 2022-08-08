using System;
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(ChunkSpawner))]
public class ChunkPool : BasePool<Chunk>
{  
    [SerializeField] private ChunkSpawner spawner;

    protected override Chunk CreateAction() 
    {
        Chunk chunk = base.CreateAction();
        chunk.Init(spawner);
        return chunk;
    }

    protected override void ReturnAction(Chunk chunk)
    {
        base.ReturnAction(chunk);
        //foreach (Coin coin in chunk.Coins)
        //{
        //    generator.CoinPool.ReturnToPool(coin);
        //}
        //foreach (Obstacle obstacle in chunk.Obstacles)
        //{
        //    generator.ObstaclePool.ReturnToPool(obstacle);
        //}
        chunk.ResetToDefault();
        chunk.Coins.Clear();
        chunk.Obstacles.Clear();
    }
}
