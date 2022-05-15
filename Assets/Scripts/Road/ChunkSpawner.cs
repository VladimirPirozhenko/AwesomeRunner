
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: TURNS
public enum EMovingDirection
{
    NORTH,
    SOUTH,
    WEST,
    EAST
}
public class ChunkSpawner : MonoBehaviour // TODO: ISpawner
{
    [SerializeField] public float ChunkWidth { get; private set; }
    [SerializeField] [Range(1, 100)] private int chunkCount;

    private ObjectPool<Chunk> chunkPool;
    [SerializeField] private float spawnDelay;

    private Chunk firstChunk;
    private Chunk lastChunk;
    private Vector3 firstChunkPosition;
    private Vector3 lastChunkPosition;

    [SerializeField] private ChunkGenerator chunkGenerator;
    private EMovingDirection spawnDirection;
    private Vector3 newSpawnPosition;
    //SEND DIRECTION AS AN ACTION?
    Action<EMovingDirection> OnDirectionChanged;
    WaitForSeconds waitCo;

    private void Start()
    {
        waitCo = new WaitForSeconds(spawnDelay);
        chunkPool = new ObjectPool<Chunk>(CreateChunk, ResetChunk, HideChunk, chunkCount);
        BoxCollider previousCollider = chunkPool[0].Collider; 
        Vector3 previousPosition = Vector3.zero;
        Chunk previousChunk = chunkPool[0];
        firstChunk = chunkPool[0];
        lastChunk = chunkPool[0];
        firstChunkPosition = chunkPool[0].transform.position;

        for (int i = 0; i < chunkCount / 2; i++)
        {
            chunkPool[i].gameObject.SetActive(true);
            Vector3 newPosition = new Vector3(previousPosition.x, previousPosition.y, previousPosition.z + previousCollider.size.z);
            RotateChunk(chunkPool[i]);
            previousCollider = chunkPool[i].Collider;
            previousChunk = chunkPool[i];
            previousPosition.z = newPosition.z;
            lastChunk = chunkPool[i];
            chunkPool[i].transform.position = lastChunkPosition + newSpawnPosition;
            lastChunkPosition = chunkPool[i].transform.position;
        }
    }

    private Chunk CreateChunk()
	{
		Chunk chunk = chunkGenerator.Generate();
        chunk.Init(this);
        chunk.transform.parent = this.transform;
        chunk.gameObject.SetActive(false);
        return chunk;
	}
    private void ResetChunk(Chunk chunk)
    {
        chunk.ResetToDefault();
        chunk.gameObject.SetActive(true);
    }
    private void HideChunk(Chunk chunk)
    {
        chunk.gameObject.SetActive(false);
    }

    public IEnumerator DelayedSpawn(Chunk chunkToDespawn)
    {
        yield return new WaitForSeconds(spawnDelay);
        InternalSpawn(chunkToDespawn);
    }
    private void InternalSpawn(Chunk chunkToDespawn)
    {
        bool isReturned = chunkPool.ReturnToPool(chunkToDespawn);
        Chunk newChunk = chunkPool.Get();
        RotateChunk(newChunk);
        newChunk.transform.position = lastChunkPosition + newSpawnPosition;

        lastChunkPosition = newChunk.transform.position;
        lastChunk = newChunk;
    }
    private void NextDirection(bool IsClockwise)
    {
        if (!IsClockwise)
        {
            switch (spawnDirection)
            {
                case EMovingDirection.NORTH:
                    spawnDirection = EMovingDirection.WEST;
                    break;
                case EMovingDirection.SOUTH:
                    spawnDirection = EMovingDirection.EAST;
                    break;
                case EMovingDirection.WEST:
                    spawnDirection = EMovingDirection.SOUTH;
                    break;
                case EMovingDirection.EAST:
                    spawnDirection = EMovingDirection.NORTH;
                    break;
            }
        }
        else
        {
            switch (spawnDirection)
            {
                case EMovingDirection.NORTH:
                    spawnDirection = EMovingDirection.EAST;
                    break;
                case EMovingDirection.SOUTH:
                    spawnDirection = EMovingDirection.WEST;
                    break;
                case EMovingDirection.WEST:
                    spawnDirection = EMovingDirection.NORTH;
                    break;
                case EMovingDirection.EAST:
                    spawnDirection = EMovingDirection.SOUTH;
                    break;
            }
        }
        OnDirectionChanged.Invoke(spawnDirection);
    }
    void RotateChunk(Chunk chunkToRotate)
    {
        switch (spawnDirection)
        {
            case EMovingDirection.NORTH:
                newSpawnPosition = lastChunk.Collider.size.z * Vector3.forward;
                Debug.Log("NORTH");
                break;
            case EMovingDirection.SOUTH:
                chunkToRotate.transform.Rotate(0, -180, 0, Space.World);
                Debug.Log("SOUTH -180");
                newSpawnPosition = -lastChunk.Collider.size.z * Vector3.forward;
                break;
            case EMovingDirection.WEST:
                chunkToRotate.transform.Rotate(0, -90, 0, Space.World);
                Debug.Log("WEST -90");
                newSpawnPosition = -lastChunk.Collider.size.x * Vector3.right;
                break;
            case EMovingDirection.EAST:
                chunkToRotate.transform.Rotate(0, 90, 0, Space.World);
                Debug.Log("EAST 90");
                newSpawnPosition = lastChunk.Collider.size.x * Vector3.right;
                break;
        }
        if (chunkToRotate.chunkType == EChunkType.LEFT_CHUNK)
        {
            NextDirection(false);
            Debug.Log("CHANGING!");
        }
    }
}

