using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneSystem : MonoBehaviour,IResettable
{
    [SerializeField] private int laneCount;
    public List<int> Lanes { get; private set; }
    public float TargetPosition { get; private set; }

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
                TargetPosition -= LaneWidth;
            }
            else 
            {
                TargetPosition += LaneWidth;
            }
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
        ResetToDefault();
    }
    public bool IsOnTargetLane(float position)
    {
        return TargetPosition == position ? true : false;
    }
    public float CalculateDistanceToTargetLane(float position)
    {
        return TargetPosition - position;
    }

    public void ResetToDefault()
    {
        targetLane = laneCount / 2;
        TargetPosition = 0;
    }
}
