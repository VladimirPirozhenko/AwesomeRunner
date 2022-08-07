using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralChunkGeneration : MonoBehaviour//,IChunkGenerationStrategy
{
    //[SerializeField] Chunk chunkPrefab;
    //[SerializeField] Obstacle obstaclePrefab;
    //[SerializeField] LaneSystem LaneSystem;
    //private Bounds chunkBounds;
    ////public LaneSystem LaneSystem { get; set; }
    ////[SerializeField] private GameObject lanePrefab;
    //private void Awake()
    //{
    //    LaneSystem = GameObject.Find("LaneSystem").GetComponent<LaneSystem>();
    //    chunkBounds = chunkPrefab.GetComponent<Renderer>().bounds;
    //    Generate(); //Generate(); Generate();
    //}
    //public void Generate()
    //{
    //    Vector3 obstaclePosition = Vector3.zero;
    //    obstaclePosition.x = LaneSystem.LaneWidth * 1;
    //    obstaclePosition.y = 0;
    //    obstaclePosition.z = chunkBounds.size.z;  
    //    var instance = Instantiate(chunkPrefab, new Vector3(), new Quaternion()); 
    //    instance.transform.localPosition = obstaclePosition;
    //    chunkPrefab.transform.SetParent(instance.transform);

    //}
}
