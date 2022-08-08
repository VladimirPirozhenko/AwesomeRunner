using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneSystem : MonoBehaviour,IResettable
{
    [field: SerializeField] public float LaneWidth { get; private set; }

    [SerializeField] private int laneCount;
    public List<int> Lanes { get; private set; }
    public float CurrentPosition { get; private set; }
    public float TargetPosition { get;  set; }
    public float CurrentOffset { get; private set; }    
    public int TargetLane { get; private set; }

    //public int TargetLane 
    //{ 
    //    get
    //    {
    //        return targetLane;
    //    }
    //    set
    //    {
    //        if (value == targetLane)
    //            return;
    //        if (value < Lanes[0])
    //        {
    //            return;
    //        }
    //        if (value > Lanes[Lanes.Count-1])
    //        {
    //            return;
    //        }         
    //        if (value < targetLane)
    //        {

    //            TargetPosition -= LaneWidth ;
    //            CurrentOffset -= LaneWidth;
    //            // DesiredDifference = -LaneWidth;
    //        }
    //        else 
    //        {
                
    //            TargetPosition += LaneWidth; //+ AdditionalOffset;
    //            CurrentOffset += LaneWidth;
    //           // DesiredDifference = LaneWidth;
    //        }
    //        targetLane = value;
    //    }
    //}

    private void Awake()
    {
        Lanes = new List<int>(laneCount);
        for (int i = 0; i < laneCount; i++)
        {
            Lanes.Add(i);    
        }
        ResetToDefault();
    }
    public void IncreaseTargetLane(int amount)
    {
        TargetLane += amount;
        if (TargetLane > Lanes[Lanes.Count - 1])
        {
            TargetLane -= amount;
            return;
        }
        TargetPosition += LaneWidth;
        CurrentOffset += LaneWidth;
    }

    public void DecreaseTargetLane(int amount)
    {
        TargetLane -= amount;
        if (TargetLane < Lanes[0])
        {
            TargetLane += amount;
            return;
        }
        TargetPosition -= LaneWidth;
        CurrentOffset -= LaneWidth;
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
        TargetLane = laneCount / 2;
        CurrentOffset = 0;
        CurrentPosition = 0;
        TargetPosition = 0;
    }
}
