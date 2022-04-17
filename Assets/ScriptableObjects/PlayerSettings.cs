using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Player Settings")]
public class PlayerSettings : ScriptableObject
{ 
    [SerializeField] private float speed;
    public float Speed { get; }
}
