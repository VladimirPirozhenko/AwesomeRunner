using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]  
public class FollowingCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 defaultDistance = new Vector3(0f,3f,-1f);
    [SerializeField] private float distanceDamp = 0.3f;

    private Transform camTransform;
    private Vector3 velocity = Vector3.one;
    private Vector3 defaultTargetPosition;
    private Quaternion defaultTargetRotation;
    private Camera cam;
    private void Awake()
    {
        camTransform = transform;
        cam = GetComponent<Camera>();
    }

    private void Start()
    {
        defaultTargetPosition = transform.position;
        defaultTargetRotation = transform.rotation;  
    }

    private void LateUpdate()
    {
        SmoothFollow();
    }
    void SmoothFollow()
    {

        Vector3 moveVector = new Vector3(target.position.x, target.position.y, target.position.z);
        //Quaternion rotationVector = Quaternion.Euler(target.rotation.y, target.rotation.y, defaultTargetRotation.z);
        //float curveX = GameSession.Instance.Curver.CurveStrengthX;
        camTransform.rotation = target.rotation;
        Vector3 toPos = moveVector + (target.rotation * defaultDistance);
        //Vector3 toPos = target.position + (target.rotation * defaultDistance);
        Vector3 curPos = Vector3.SmoothDamp(camTransform.position, toPos, ref velocity, distanceDamp);
        camTransform.position = curPos;
        camTransform.LookAt(target, target.up);
    }
}
