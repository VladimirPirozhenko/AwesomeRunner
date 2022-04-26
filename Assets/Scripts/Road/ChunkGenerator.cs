using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Chunk))]
public class ChunkGenerator : MonoBehaviour
{
    [SerializeField] private Obstacle obstaclePrefab;
    [SerializeField] private LaneSystem LaneSystem;
    private Vector3 chunkSize;
    private Vector3 obstacleSize;
    private void Awake()
    {
        LaneSystem = GameObject.Find("LaneSystem").GetComponent<LaneSystem>();
        //chunkSize = chunkPrefab.GetComponent<BoxCollider>().size;
        obstacleSize = obstaclePrefab.GetComponent<BoxCollider>().size;
    }
    public void Generate(Chunk chunk)
    {
        Vector3 obstaclePosition = Vector3.zero;
        obstaclePosition.x = LaneSystem.LaneWidth * UnityEngine.Random.Range(-LaneSystem.Lanes.Count / 2, LaneSystem.Lanes.Count / 2 + 1) - obstacleSize.x / 2;
        obstaclePosition.y = 0;
        obstaclePosition.z = 0;
        // Quaternion obstacleRotation = Quaternion.Euler(0, 90, 0);
        var instance = Instantiate(obstaclePrefab, new Vector3(), new Quaternion());
        instance.transform.position = chunk.transform.position + obstaclePosition;
        instance.transform.SetParent(chunk.transform, true);
    }
}
