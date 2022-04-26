using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Obstacle : MonoBehaviour
{
    public BoxCollider Collider { get; private set; }
    private void Awake()
    {
        Collider = GetComponent<BoxCollider>();
    }
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.TryGetComponent(out Player player))
        {
            if (!player.IsInvincible)
            {
                player.Lives--;
                if (player.Lives < 1)
                {
                    player.StateMachine.SetState(player.PlayerDeadState);
                }
                player.GrantInvincibility();
            } 
        }
    }
}
