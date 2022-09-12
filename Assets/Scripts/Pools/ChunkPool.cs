using UnityEngine;

public class ChunkPool : BasePool<Chunk>
{
    [SerializeField] private ChunkSpawner spawner;

    protected override Chunk CreateAction()
    {
        Chunk chunk = base.CreateAction();
        chunk.Init(spawner);
        chunk.transform.position = new Vector3(0, 0, 0);
        return chunk;
    }

    protected override void ReturnAction(Chunk chunk)
    {
        base.ReturnAction(chunk);
        chunk.ResetToDefault();
        chunk.Coins.Clear();
        foreach (var obstacle in chunk.Obstacles)
        {
            obstacle.ResetToDefault();
        }
        chunk.Obstacles.Clear();
    }
}