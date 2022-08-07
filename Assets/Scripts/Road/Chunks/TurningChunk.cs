using System.Collections;
using UnityEngine;


public class TurningChunk : Chunk
{
    [SerializeField] private bool isClockwise;
    public bool IsClockwise { get { return isClockwise; } private set { isClockwise = value; } }    
    public override void Init(ChunkSpawner spawner)
    {
        base.Init(spawner);
        Direction = EDirection.WEST;
    }
    public override void ChangeDirectionBasedOnPreviousChunk(Chunk previousChunk)
    {
        if (!isClockwise)
        {
            switch (previousChunk.Direction)
            {
                case EDirection.NORTH:
                    Direction = EDirection.WEST;
                    break;
                case EDirection.SOUTH:
                    Direction = EDirection.EAST;
                    break;
                case EDirection.WEST:
                    Direction = EDirection.SOUTH;
                    break;
                case EDirection.EAST:
                    Direction = EDirection.NORTH;
                    break;
            }
        }
        else
        {
            switch (previousChunk.Direction)
            {
                case EDirection.NORTH:
                    Direction = EDirection.EAST;
                    break;
                case EDirection.SOUTH:
                    Direction = EDirection.WEST;
                    break;
                case EDirection.WEST:
                    Direction = EDirection.NORTH;
                    break;
                case EDirection.EAST:
                    Direction = EDirection.SOUTH;
                    break;
            }
        }
    }
}

