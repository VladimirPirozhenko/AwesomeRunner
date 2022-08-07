
using UnityEngine;

public class LaneMovementController : MonoBehaviour
{
    public EInputDirection? MovingDirection { get; private set; }
    [HideInInspector] public Vector3 HorizontalDeltaPosition;
    public float VerticalDeltaPosition { get; set; }
    [SerializeField] private LaneSystem laneSystem;
    public LaneSystem LaneSystem { get { return laneSystem; } private set { laneSystem = value; } }

    public void ChangeLane(int lanesCount)
    {
        LaneSystem.TargetLane += lanesCount;
    }

}
