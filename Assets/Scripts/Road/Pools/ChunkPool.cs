using UnityEngine;

public class ChunkPool : BasePool<Chunk>
{  
    [SerializeField] private ChunkSpawner spawner;

    protected override Chunk CreateAction() 
    {
        Chunk chunk = base.CreateAction();
        chunk.Init(spawner);
        chunk.transform.position = new Vector3(0,0,0);    
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
        foreach (var obstacle in chunk.Obstacles)
        {
            obstacle.transform.SetParent(null);
            obstacle.ResetToDefault();
        }
        chunk.Obstacles.Clear();
    }
}
