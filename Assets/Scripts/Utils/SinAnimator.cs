using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinAnimator : MonoBehaviour
{
    private Vector3 startPosition;
    [SerializeField] private float amplitude;
    [SerializeField] private Vector3 animationVelocity;
    void Start()
    {
        startPosition = transform.localPosition;
    }
    void Update()
    {
        transform.localPosition = startPosition + new Vector3(Mathf.Sin(Time.time * animationVelocity.x), 
                                                         Mathf.Sin(Time.time * animationVelocity.y),
                                                         Mathf.Sin(Time.time * animationVelocity.z)) * amplitude;
    }
}
