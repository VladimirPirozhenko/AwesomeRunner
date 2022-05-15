using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnState : MovingState
{
    Player player;
    public TurnState(Player player, PlayerController collider) : base(player, collider)
    {
        this.player = player;
    }

    public override void Tick()
    {

        base.Tick();
    }
    public void Turn()
    {
        if (canTurn)
        {
            switch (player.Direction)
            {
                case EDirection.RIGHT:
                    // player.
                    player.transform.Rotate(0, 90, 0);
                    break;
                case EDirection.LEFT:
                    player.transform.Rotate(0, -90, 0);
                    break;
                default:
                    break;
            }
        }
    }
}
