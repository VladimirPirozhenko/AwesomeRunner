using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDirection {  NORTH, SOUTH, EAST, WEST }
[RequireComponent(typeof(BoxCollider))]
public abstract class Chunk : MonoBehaviour, IResettable
{
    protected ChunkSpawner spawner;
    //[SerializeField] protected Coin defaultCoinPrefab;
    //[SerializeField] protected Obstacle defaultObstaclePrefab;

    [SerializeField] private Transform begin;
    [SerializeField] private Transform end;

    //[SerializeField] private float spawnCooldown;
    //private float timeSinceLastSpawn = 0;
                                            //10 < 
    
    public Transform Begin { get { return begin; } private set { begin = value; } }
    public Transform End { get { return end; } private set { end = value; } }
    public List<Coin> Coins { get; private set; }
    public List<Obstacle> Obstacles { get; private set; }
    public BoxCollider Collider { get; private set; }
    public EDirection Direction { get; protected set; }

    virtual public void Init(ChunkSpawner spawner)
    {
        this.spawner = spawner;
    }
    private void Awake()
    {
        Collider = GetComponent<BoxCollider>();
        Coins = new List<Coin>();   
        Obstacles = new List<Obstacle>();   
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
            //OnChunkPassed?.Invoke(this);
            spawner.DelayedReturnToPool(this);
            spawner.Spawn();
        }
    }
    //public bool IsOnCooldown()
    //{ 
    //   return spawnCooldown > timeSinceLastSpawn ? true : false; 
    //}
    public void ChangeTransformBasedOnPreviousChunk(Chunk previousChunk)
    {
        //ChangeDirectionBasedOnPreviousChunk(previousChunk);
        ChangePositionBasedOnPreviousChunk(previousChunk);
       // ChangeRotationBasedOnPreviousChunk(previousChunk);
    }
    abstract public void ChangeDirectionBasedOnPreviousChunk(Chunk previousChunk);
    private void ChangePositionBasedOnPreviousChunk(Chunk previousChunk)
    {
        float diffBetweenBeginAndCenter = Begin.localPosition.z;
        transform.position = previousChunk.End.position - Begin.localPosition;
        //switch (previousChunk.Direction)
        //{
        //    case EDirection.NORTH:
        //        transform.position = previousChunk.End.position - Begin.localPosition;
        //        break;
        //    case EDirection.SOUTH:
        //        transform.position = previousChunk.End.position + (diffBetweenBeginAndCenter * Vector3.forward);
        //        break;
        //    case EDirection.WEST:
        //        transform.position = previousChunk.End.position + (diffBetweenBeginAndCenter * Vector3.right);
        //        break;
        //    case EDirection.EAST:
        //        transform.position = previousChunk.End.position - (diffBetweenBeginAndCenter * Vector3.right);
        //        break;
        //}
    }
    private void ChangeRotationBasedOnPreviousChunk(Chunk previousChunk)
    {
        switch (previousChunk.Direction)
        {
            case EDirection.NORTH:
                transform.Rotate(0, 0, 0, Space.World);
                break;
            case EDirection.SOUTH:
                transform.Rotate(0, -180, 0, Space.World);
                break;
            case EDirection.WEST:
                transform.Rotate(0, -90, 0, Space.World);
                break;
            case EDirection.EAST:
                transform.Rotate(0, -270, 0, Space.World);
                break;
        }
    }
}
