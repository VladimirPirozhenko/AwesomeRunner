using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDirection {  NORTH, SOUTH, EAST, WEST }
[RequireComponent(typeof(BoxCollider))]
public abstract class Chunk : MonoBehaviour, IResettable,IPoolable<Chunk>
{
    protected ChunkSpawner spawner;

    [field: SerializeField] public Transform Begin { get;  private set; } 
    [field: SerializeField] public Transform End { get; private set; }

    [SerializeField] private int gridRowCount;
    public List<Coin> Coins { get; private set; }
    public List<Obstacle> Obstacles { get; private set; }
    public BoxCollider Collider { get; private set; }
    public EDirection Direction { get; protected set; }
    public BasePool<Chunk> OwningPool { private get;  set; }

    public readonly List<Vector3> GridPositions = new List<Vector3>();
    virtual public void Init(ChunkSpawner spawner)
    {
        this.spawner = spawner;
    }
    private void Awake()
    {
        Collider = GetComponent<BoxCollider>();
        Coins = new List<Coin>();   
        Obstacles = new List<Obstacle>();

        InitializeGrid();
    }

    private void InitializeGrid()
    {
        Vector3 chunkLengthVector = (End.position - Begin.position);
        float chunkLength = chunkLengthVector.magnitude;
        float rowLength = chunkLength / gridRowCount;
        Debug.DrawLine(Vector3.zero, new Vector3(0, 5, 0), Color.red);
        foreach (var lane in LaneSystem.Instance.Lanes)
        {
            float lanePosition = lane * LaneSystem.Instance.LaneWidth;
            for (int i = 0; i < gridRowCount; i++)
            {
                Vector3 gridPosition = new Vector3(lanePosition, 0, i * rowLength);
                Debug.DrawLine(gridPosition, Vector3.up* 100,Color.red,500);    
                GridPositions.Add(gridPosition);
            }
        }
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
        float diffBetweenBeginAndCenter = Begin.localPosition.z;
        //transform.position = previousChunk.End.position - Begin.localPosition;
        switch (previousChunk.Direction)
        {
            case EDirection.NORTH:
                transform.position = previousChunk.End.position - Begin.localPosition;
                break;
            case EDirection.SOUTH:
                transform.position = previousChunk.End.position + (diffBetweenBeginAndCenter * Vector3.forward);
                break;
            case EDirection.WEST:
                transform.position = previousChunk.End.position - (Begin.localPosition.x * Vector3.right);
                break;
            case EDirection.EAST:
                transform.position = previousChunk.End.position - (diffBetweenBeginAndCenter * Vector3.right);
                break;
        }
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

    public void ReturnToPool()
    {
        OwningPool.ReturnToPool(this);
    }
}
