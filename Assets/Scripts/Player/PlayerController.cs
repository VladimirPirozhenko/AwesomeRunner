using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IResettable
{
    [SerializeField] private ChunkSpawner spawner; 
    private Health playerHealth;
    private Statistics playerStatistics;
    public CharacterController CharacterController { get; private set; }
    public float defaultHeight { get; private set; }
    public Vector3 defaultCenter { get; private set; }
    public bool isGrounded => CharacterController.isGrounded;
    private void Start()
    {
        playerHealth = GetComponent<Health>();
        playerStatistics = GetComponent<Statistics>();
        CharacterController = GetComponent<CharacterController>();
        defaultHeight = CharacterController.height;
        defaultCenter = CharacterController.center;
    }
    private void OnTriggerEnter(Collider other) //CollisionCheck
    {

        if (other.TryGetComponent(out Obstacle obstacle)) //switch..case
        {

            var damageableComponents = GetComponents<IDamageDealer>();
            if (playerHealth.IsInvincible)
                return;
            int damageAmount = 1; 
            playerHealth.TakeDamage(damageAmount);
            obstacle.Impact();
            StartCoroutine(playerHealth.GrantInvincibility());
            
        }
        else if (other.TryGetComponent(out Coin coin))
        {
            //playerStatistics.UpdateCoinCount();   
            coin.Collect();
        }     
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Chunk chunk))
        {
            //StartCoroutine(spawner.Spawn(chunk)); //EVENT
        }
    }

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
