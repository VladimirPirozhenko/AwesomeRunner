using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rotator : MonoBehaviour
{
    private enum ERotationAxis
    {
        X_AXIS,
        Y_AXIS,
        Z_AXIS
    }
    private enum ERotationDirection
    {
        CLOCKWISE,
        COUNTER_CLOCKWISE
    }
    [SerializeField] private float rotationSpeed;
    [SerializeField] private ERotationDirection direction;
    [SerializeField] private ERotationAxis axis;
    private float rotationDelta;
    void Update()
    {
        switch (direction)
        {
            case ERotationDirection.CLOCKWISE:
                rotationDelta = rotationSpeed * Time.deltaTime ;
                break;
            case ERotationDirection.COUNTER_CLOCKWISE:
                rotationDelta = rotationSpeed * -Time.deltaTime ;
                break;
        }
        switch (axis)
        {
            case ERotationAxis.X_AXIS:
                transform.Rotate(rotationDelta, 0, 0, Space.Self);
                break;
            case ERotationAxis.Y_AXIS:
                transform.Rotate(0, rotationDelta, 0, Space.Self);
                break;
            case ERotationAxis.Z_AXIS:
                transform.Rotate(0, 0, rotationDelta, Space.Self);
                break;
        }
    }
}
