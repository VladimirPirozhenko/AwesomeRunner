using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State<Player>
{
    private Player player;
    public DeadState(Player player) 
    {
        this.player = player;
    }
    public override void OnStateEnter()
    {
        Debug.Log("DEAD");
    }

    public  override void OnStateExit()
    {
        //throw new System.NotImplementedException();
    }

    public override void Tick()
    {
        
    }
}
