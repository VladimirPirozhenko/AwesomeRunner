using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class InvincibleState : State<Player>
{
    private Player player;
    private BoxCollider playerBoxCollider;
    private CharacterController controller;

    private int invincibleTime = 3000;
    public InvincibleState(Player player, CharacterController controller)
    {
        this.player = player;
        player.TryGetComponent(out playerBoxCollider);
        this.controller = controller;
    }
    public override void OnStateEnter()
    {
        Debug.Log("EnterInvincibleState");
        if (player.Lives < 1)
        {
            player.StateMachine.SetState(player.PlayerDeadState);
        }
        GrantInvincibility();
        //REVERT STATE HEHE
        //player.StateMachine.RevertState();
    }
    public override void OnStateExit()
    {
        Debug.Log("ExitInvincibleState");
    
    }
    public override void Tick()
    {
       // throw new System.NotImplementedException();
    }

    public async void GrantInvincibility()
    {
        playerBoxCollider.enabled = false;
        controller.enabled = false;
        await Task.Delay(invincibleTime);
        playerBoxCollider.enabled = true;
        controller.enabled = true;
    }
}
