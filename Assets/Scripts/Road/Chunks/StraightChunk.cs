using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightChunk : Chunk
{
    public override void Init(ChunkSpawner spawner)
    {
        base.Init(spawner);
        Direction = EDirection.NORTH;
    }
    public override void ChangeDirectionBasedOnPreviousChunk(Chunk previousChunk)
    {
        Direction = previousChunk.Direction;
    }
}
