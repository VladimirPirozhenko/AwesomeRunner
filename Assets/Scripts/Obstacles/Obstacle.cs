using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Renderer))]
public class Obstacle : MonoBehaviour
{
    public BoxCollider Collider { get; private set; }
    private Renderer renderer;// { get; set; } 
    private Color defaultColor;
    private void Awake()
    {
        Collider = GetComponent<BoxCollider>();
        renderer = GetComponent<Renderer>();
        defaultColor = renderer.material.color;
    }
    public void ResetToDefault()
    {
        renderer.material.color = defaultColor;
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
                renderer.material.color = Color.red;
                player.GrantInvincibility();
            } 
        }
    }
}
