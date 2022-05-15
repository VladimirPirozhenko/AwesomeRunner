using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IObstacle
{
	//public void Stumble(Player player);
}

interface IResettable
{
	public void ResetToDefault();
}
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Renderer))]
public class Obstacle : MonoBehaviour,IObstacle,IResettable
{
    public BoxCollider Collider { get; private set; }
    
    private Renderer defaultRenderer;
    private Renderer obstacleRenderer;
	//public ObstacleData {get;private set;} //DATA SO
    private void Awake()
    {
        Collider = GetComponent<BoxCollider>();
       // obstacleRenderer = GetComponent<Renderer>();
        defaultRenderer = GetComponent<Renderer>();
        obstacleRenderer = defaultRenderer;
    }
    public void ResetToDefault()
    {
        if (obstacleRenderer.material.color != defaultRenderer.material.color)
             obstacleRenderer.material.color = defaultRenderer.material.color;
    }
    public void Impact()
    {
        obstacleRenderer.material.color = Color.red;
        gameObject.SetActive(false);
    }
    //private void OnTriggerEnter(Collider other)
    //{      
    //    if (other.TryGetComponent(out Player player))
    //    {
    //        if (!player.IsInvincible)
    //        {
    //            player.Lives--;
    //            if (player.Lives < 1)
    //            {
    //                player.StateMachine.SetState(player.PlayerDeadState);
    //            }
               
    //            player.GrantInvincibility();
    //        } 
    //    }
    //}
}

public class Turret : MonoBehaviour,IObstacle
{
	
}