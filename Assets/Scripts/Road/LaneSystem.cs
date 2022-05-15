using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneSystem : MonoBehaviour
{
    [SerializeField] private int laneCount;
    public List<int> Lanes { get; private set; }
    public float TargetX { get; set; }
    public bool isChangingLane { get; set; }

    [SerializeField] private float laneWidth;
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
    private int targetLane;
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
            if (value < Lanes[0])
            {
                return;
            }
            if (value > Lanes[Lanes.Count-1])
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
        Lanes = new List<int>(laneCount);
        for (int i = 0; i < laneCount; i++)
        {
            Lanes.Add(i);    
        }
        targetLane = laneCount/2;
        TargetX = 0;
    }
}
