using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Chunk))]
public class ChunkGenerator : MonoBehaviour
{
    [SerializeField] private Chunk chunkPrefab;
    [SerializeField] private Obstacle obstaclePrefab;
    [SerializeField] private LaneSystem LaneSystem;
    private Vector3 chunkSize;
    private Vector3 obstacleSize;
    private void Awake()
    {
        LaneSystem = GameObject.Find("LaneSystem").GetComponent<LaneSystem>();
        obstacleSize = obstaclePrefab.GetComponent<BoxCollider>().size;
    }
    public Chunk Generate()
    {
        Vector3 obstaclePosition = Vector3.zero;
        obstaclePosition.x = LaneSystem.LaneWidth * UnityEngine.Random.Range(-LaneSystem.Lanes.Count / 2, LaneSystem.Lanes.Count / 2 + 1) - obstacleSize.x / 2;
        obstaclePosition.y = 0;
        obstaclePosition.z = 0;
        Chunk chunk = Instantiate(chunkPrefab, new Vector3(), new Quaternion());
        Obstacle obstacle = Instantiate(obstaclePrefab, new Vector3(), new Quaternion());
        obstacle.transform.position = chunk.transform.position + obstaclePosition;
        obstacle.transform.SetParent(chunk.transform, false);
        chunk.Obstacles.Add(obstacle);
        return chunk;
    }
}
