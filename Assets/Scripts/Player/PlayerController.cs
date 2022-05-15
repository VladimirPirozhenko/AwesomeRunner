using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IResettable
{
   // [SerializeField] private ChunkSpawner spawner; 
    private PlayerHealth playerHealth;
    private PlayerStatistics playerStatistics;
    public CharacterController CharacterController { get; private set; }
    public float defaultHeight { get; private set; }
    public Vector3 defaultCenter { get; private set; }
    public bool isGrounded => CharacterController.isGrounded;
    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerStatistics = GetComponent<PlayerStatistics>();
        CharacterController = GetComponent<CharacterController>();
        defaultHeight = CharacterController.height;
        defaultCenter = CharacterController.center;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Obstacle obstacle))
        {
            if (playerHealth.IsInvincible)
                return;
            playerHealth.TakeDamage();
            obstacle.Impact();
            StartCoroutine(playerHealth.GrantInvincibility());
        }
        if (other.TryGetComponent(out Coin coin))
        {
            playerStatistics.IncreaseCoinCount();   
            coin.Collect();
        }     
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.TryGetComponent(out Chunk chunk))
    //    {
    //        spawner.DelayedSpawn(chunk);
    //    }
    //}
    
    public void Move(Vector3 deltaPosition)
    {
        CharacterController.Move(deltaPosition);
    }
    public void ChangeColliderHeight(float newHeight)
    {
        CharacterController.height = newHeight;
    }
    public void ChangeColliderCenter(Vector3 newCenter)
    {
        CharacterController.center = newCenter;
    }
    public void ResetToDefault()
    {
        CharacterController.height = defaultHeight;
        CharacterController.center = defaultCenter;
    }

}
