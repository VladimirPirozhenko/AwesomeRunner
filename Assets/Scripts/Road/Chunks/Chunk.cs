using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public abstract class Chunk : PoolingObject<Chunk>, IResettable
{
    protected ChunkSpawner spawner;

    [field: SerializeField] public Transform Begin { get;  private set; } 
    [field: SerializeField] public Transform End { get; private set; }

    public List<Coin> Coins { get; private set; }
    public List<Obstacle> Obstacles { get; private set; }
    public BoxCollider Collider { get; private set; }
    public Grid Grid { get; private set; }

    virtual public void Init(ChunkSpawner spawner)
    {
        this.spawner = spawner;
        Collider = GetComponent<BoxCollider>();
        Coins = new List<Coin>();
        Obstacles = new List<Obstacle>();
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        Vector3 chunkLengthVector = (End.position - Begin.position);
        float chunkLength = chunkLengthVector.magnitude;
        Grid = new Grid(chunkLength);
    }

    public void ResetToDefault()
    {
        transform.localPosition = Vector3.zero;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            spawner.DelayedReturnToPool(this);
            spawner.Spawn();
        }
    }

    public void ChangeTransformBasedOnPreviousChunk(Chunk previousChunk)
    {
        ChangeDirectionBasedOnPreviousChunk(previousChunk);
        ChangePositionBasedOnPreviousChunk(previousChunk);
        ChangeRotationBasedOnPreviousChunk(previousChunk);
    }

    abstract public void ChangeDirectionBasedOnPreviousChunk(Chunk previousChunk);

    private void ChangePositionBasedOnPreviousChunk(Chunk previousChunk)
    {
        //float diffBetweenBeginAndCenter = Begin.localPosition.z;
        transform.position = previousChunk.End.position - Begin.localPosition;
    }
    private void ChangeRotationBasedOnPreviousChunk(Chunk previousChunk)
    {
    }
}
