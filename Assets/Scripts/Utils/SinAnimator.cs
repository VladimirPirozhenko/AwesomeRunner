using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinAnimator : MonoBehaviour
{
    private Vector3 startPosition;
    [SerializeField] private float amplitude;
    [SerializeField] private Vector3 animationSpeed;
    void Start()
    {
        startPosition = transform.position;
    }
    void Update()
    {
        transform.position = startPosition + new Vector3(Mathf.Sin(Time.time * animationSpeed.x), 
                                                         Mathf.Sin(Time.time * animationSpeed.y),
                                                         Mathf.Sin(Time.time * animationSpeed.z)) * amplitude;
    }
}
