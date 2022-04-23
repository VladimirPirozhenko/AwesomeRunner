using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : MovingState
{
    private Player player;
    public GroundState(Player player,CharacterController controller) : base(player,controller)
    {
        this.player = player;
    }
    public override void OnStateEnter()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnStateExit()
    {
       // throw new System.NotImplementedException();
    }

    public override void Tick()
    {
      base.Tick();
      Move();
    }

}
