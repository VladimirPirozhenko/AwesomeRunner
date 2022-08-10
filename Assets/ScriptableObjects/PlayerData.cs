using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/Player")]
public class PlayerData : ScriptableObject
{
    [field: SerializeField] public int InvincibilityTime {  get; private set; }
    [field: SerializeField] public float JumpHeight { get; private set; }
    [field: SerializeField] public float LaneSwitchSpeed { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }
}
