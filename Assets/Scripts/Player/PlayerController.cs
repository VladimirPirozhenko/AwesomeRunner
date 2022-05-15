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
    private CharacterController characterController;
    public float defaultHeight { get; private set; }
    public Vector3 defaultCenter { get; private set; }
    public bool isGrounded => characterController.isGrounded;
    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerStatistics = GetComponent<PlayerStatistics>();
        characterController = GetComponent<CharacterController>();
        defaultHeight = characterController.height;
        defaultCenter = characterController.center;
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
        characterController.Move(deltaPosition);
    }
    public void ChangeColliderHeight(float newHeight)
    {
        characterController.height = newHeight;
    }
    public void ChangeColliderCenter(Vector3 newCenter)
    {
        characterController.center = newCenter;
    }
    public void ResetToDefault()
    {
        characterController.height = defaultHeight;
        characterController.center = defaultCenter;
    }

}
