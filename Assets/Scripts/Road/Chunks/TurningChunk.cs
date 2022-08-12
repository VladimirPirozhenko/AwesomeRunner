using UnityEngine;


public class TurningChunk : Chunk
{
    [SerializeField] private bool isClockwise;
    public bool IsClockwise { get { return isClockwise; } private set { isClockwise = value; } }    
    public override void Init(ChunkSpawner spawner)
    {
        base.Init(spawner);
    }
    public override void ChangeDirectionBasedOnPreviousChunk(Chunk previousChunk)
    {

    }
}

