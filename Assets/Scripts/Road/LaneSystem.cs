using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneSystem : MonoBehaviour,IResettable
{
    [SerializeField] private int laneCount;
    public List<int> Lanes { get; private set; }
    public float CurrentPosition { get; private set; }
    public Transform TargetTransform;
    public float TargetPosition { get;  set; }
    public float AdditionalOffset { get;  set; }
    public float CurrentOffset { get; private set; }    
    public float DesiredDifference { get;  set; }

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

                TargetPosition -= LaneWidth ;
                CurrentOffset -= laneWidth;
                // DesiredDifference = -LaneWidth;
            }
            else 
            {
                
                TargetPosition += LaneWidth; //+ AdditionalOffset;
                CurrentOffset += laneWidth;
               // DesiredDifference = LaneWidth;
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
        AdditionalOffset = 0;
        CurrentOffset = 0;
        TargetPosition = AdditionalOffset;
    }
}
