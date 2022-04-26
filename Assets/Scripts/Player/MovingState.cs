using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class MovingState : State<Player>
{
    protected float speed = 15; //SO
    protected Player player;
    protected CharacterController characterController;
    protected const float gravity = -9.8f;

    public MovingState(Player player, CharacterController controller)
    {
        this.player = player;
        characterController = controller;
    }
    public override void OnStateEnter(){}

    public override void OnStateExit() {}

    public override void Tick()
    {
        HandleDirection(); 
        player.HorizontalDeltaPosition.z = speed * Time.deltaTime;
        if (player.LaneSystem.isChangingLane)
        {
            SwitchLane();
        }
        HandleGravity();
        Vector3 deltaPosition = new Vector3(player.HorizontalDeltaPosition.x,
                                            player.VerticalDeltaPosition, 
                                            player.HorizontalDeltaPosition.z);
        characterController.Move(deltaPosition);
    }
    public void HandleGravity()
    {
        if (characterController.isGrounded)
        {
            player.VerticalDeltaPosition = 0;
        }
        else
        {
            player.VerticalDeltaPosition += gravity * Time.deltaTime;
        }
    }
    private void HandleDirection()
    {
        switch (player.Direction)
        {
            case EDirection.RIGHT:
                player.LaneSystem.TargetLane++;
                break;
            case EDirection.LEFT:
                player.LaneSystem.TargetLane--;
                break;
            case EDirection.UP:
                player.StateMachine.SetState(player.PlayerJumpState);
                break;
            case EDirection.DOWN:
                break;
        }
    }
    public void SwitchLane()
    {
        if (player.LaneSystem.TargetX == player.transform.position.x)//player.TargetPosX
        {
            player.HorizontalDeltaPosition.x = 0;
            player.LaneSystem.isChangingLane = false;
            return;
        }
        Vector3 diffX = (player.LaneSystem.TargetX - player.transform.position.x) * Vector3.right;//player.TargetPosX
        Vector3 deltaX = diffX.normalized * speed * Time.deltaTime;
        //horizontalDeltaPosition.x = Mathf.Min(deltaX.sqrMagnitude, diffX.sqrMagnitude) * Mathf.Sign(diffX.x);
        //horizontalDeltaPosition.x = Mathf.Min(Mathf.Abs(deltaX + player.transform.position.x),
        //    Mathf.Abs(targetPos.x)) * Mathf.Sign(deltaX + player.transform.position.x)) // ¬ Õ”À≈ Õ≈ –¿¡Œ“¿≈“
        //if (deltaX.sqrMagnitude < diffX.sqrMagnitude)
        if (deltaX.sqrMagnitude < diffX.sqrMagnitude)
        {
            player.HorizontalDeltaPosition.x = deltaX.x;
        }
        else
        {
            player.HorizontalDeltaPosition.x = diffX.x;
        }
    }
}
