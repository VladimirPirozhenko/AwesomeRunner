using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneSystem : MonoBehaviour
{
    [SerializeField] private float laneWidth;
    [SerializeField] private int laneCount;
    public float TargetX { get; set; }
    public bool isChangingLane { get; set; }
    private int targetLane; 
    private List<int> lanes;
    public float LaneWidth 
    {
        get
        {
            return laneWidth;
        }

        private set
        {
            laneWidth = value;
        }
    }
    public int TargetLane 
    { 
        get
        {
            return targetLane;
        }
        set
        {
            if (value == targetLane)
                return;
            if (value < lanes[0])
            {
                return;
            }
            if (value > lanes[lanes.Count-1])
            {
                return;
            }
            
            if (value < targetLane)
            {
                TargetX -= LaneWidth;
            }
            else 
            {
                TargetX += LaneWidth;
            }
            isChangingLane = true;
            targetLane = value;
        }
    }

    private void Awake()
    {
        lanes = new List<int>(laneCount);
        for (int i = 0; i < laneCount; i++)
        {
            lanes.Add(i);    
        }
        targetLane = laneCount/2;
        TargetX = 0;
    }
}
