using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid 
{
    public readonly List<Vector3> GridPositions = new List<Vector3>();
    private int gridRowCount;

    public Grid(float gridLength)
    {
        gridRowCount = 1;
        float rowLength = gridLength / gridRowCount;
        foreach (var lane in LaneSystem.Instance.Lanes)
        {
            float lanePosition = lane * LaneSystem.Instance.LaneWidth;
            for (int i = 0; i < gridRowCount; i++)
            {
                Vector3 gridPosition = new Vector3(lanePosition, 0, i * rowLength);
                GridPositions.Add(gridPosition);
            }
        }
    }
    public Vector3 GetRandomPosition()
    {
       return GridPositions.GetRandomElement();   
    }
}
