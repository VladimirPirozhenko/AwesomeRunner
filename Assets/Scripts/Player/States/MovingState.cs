using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class MovingState : State<Player>
{
    private float speed; //SO
    private float laneSwitchSpeed; //SO
    protected Player player;
    protected PlayerController playerController;
    protected const float gravity = -9.8f;

    public MovingState(Player player, PlayerController controller)
    {
        this.player = player;
        speed = player.PlayerData.Speed;
        laneSwitchSpeed = player.PlayerData.LaneSwitchSpeed;
        playerController = controller;
    }
    public override void OnStateEnter(){}

    public override void OnStateExit() {}

    public override void Tick()
    {
        HandleDirection(); 
        player.HorizontalDeltaPosition.z = speed * Time.deltaTime;
        player.PlayerStatictics.IncreaseDistance(player.HorizontalDeltaPosition.z);
        SwitchLane();
        ApplyGravity();
        Vector3 deltaPosition = new Vector3(player.HorizontalDeltaPosition.x,
                                            player.VerticalDeltaPosition, 
                                            player.HorizontalDeltaPosition.z);
        playerController.Move(deltaPosition);
    }
    public void ApplyGravity()
    {   
        player.VerticalDeltaPosition += gravity * Time.deltaTime;
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
                player.PlayerStateMachine.SetState(player.PlayerJumpState);
                break;
            case EDirection.DOWN:
                player.PlayerStateMachine.SetState(player.PlayerSlideState);
                break;
            default:
                break;
        }
    }

    public void SwitchLane()
    {
        if (player.LaneSystem.IsOnTargetLane(player.transform.position.x))
        {
            player.HorizontalDeltaPosition.x = 0;
            return;
        }
        Vector3 diffX = player.LaneSystem.CalculateDistanceToTargetLane(player.transform.position.x)* Vector3.right;
        Vector3 deltaX = diffX.normalized * laneSwitchSpeed * Time.deltaTime;
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
