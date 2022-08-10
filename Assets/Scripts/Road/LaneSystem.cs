using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneSystem : MonoBehaviour,IResettable
{
    static public LaneSystem Instance { get; private set; }
    [field: SerializeField] public float LaneWidth { get; private set; }

    [SerializeField] private int laneCount;
    public List<int> Lanes { get; private set; }
    public float CurrentPosition { get; private set; }
    public float TargetPosition { get;  set; }
    public float CurrentOffset { get; private set; }    
    public int TargetLane { get; private set; }
    public int CenterLane { get; private set; }

    public readonly Dictionary<int, float> LanesDict = new Dictionary<int, float>();

    private void Awake()
    {
        Instance = this;
        Lanes = new List<int>(laneCount);
        bool isLanesEven = laneCount % 2 == 0;
        if (isLanesEven)
        {
            for (int i = -laneCount / 2; i < laneCount / 2; i++)
            {
                Lanes.Add(i);
            }
        }
        else
        {
            for (int i = -laneCount / 2; i <= laneCount / 2; i++)
            {
                Lanes.Add(i);
            }
        }

        if (isLanesEven)
        {
            for (int i = -laneCount / 2; i < laneCount / 2; i++)
            {
                LanesDict.Add(i, i * LaneWidth);
            }
        }
        else
        {
            for (int i = -laneCount / 2; i <= laneCount / 2; i++)
            {
                LanesDict.Add(i, i * LaneWidth);
            }
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
        TargetLane = Lanes[laneCount / 2];
        CenterLane = Lanes[laneCount / 2];
        CurrentOffset = 0;
        CurrentPosition = 0;
        TargetPosition = 0;
    }
}
