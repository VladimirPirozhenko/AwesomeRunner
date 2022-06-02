using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class MovingState : PlayerState
{
    private float speed; //SO
    private float laneSwitchSpeed; //SO
    protected const float gravity = -9.8f;
    private float invincibilityTime => playerSM.PlayerData.InvincibilityTime; 
    public MovingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)//, PlayerController controller
    {
        this.playerSM = playerStateMachine;
        speed = playerData.Speed;
        laneSwitchSpeed = playerData.LaneSwitchSpeed;
    }
    public override void OnStateEnter(){}

    public override void OnStateExit() {}

    public override void Tick()
    {
        HandleDirection(); 
        HorizontalDeltaPosition.z = speed * Time.deltaTime; //INCAPSULATE
        playerSM.UpdateDistance(HorizontalDeltaPosition.z); //вынести в контроллер
        SwitchLane();
        ApplyGravity();
        Vector3 deltaPosition = new Vector3(HorizontalDeltaPosition.x,
                                            VerticalDeltaPosition,
                                            HorizontalDeltaPosition.z);
        playerSM.Move(deltaPosition);
    }
    public void ApplyGravity()
    {   
        VerticalDeltaPosition += gravity * Time.deltaTime;
    }
    private void HandleDirection()
    {
        switch (playerSM.InputDirection)
        {
            case EInputDirection.RIGHT:
                playerSM.IncreaseTargetLane();
                break;
            case EInputDirection.LEFT:
                playerSM.DecreaseTargetLane();
                break;
            case EInputDirection.UP:
                playerSM.SetState(playerSM.PlayerJumpState);
                break;
            case EInputDirection.DOWN:
                playerSM.SetState(playerSM.PlayerSlideState);
                break;
            default:
                break;
        }
    }

    public void SwitchLane()
    {
        if (playerSM.IsOnTargetLane(playerTransform.position.x))
        {
            HorizontalDeltaPosition.x = 0;
            return;
        }
        Vector3 diffX = playerSM.CalculateDistanceToTargetLane();
        Vector3 deltaX = diffX.normalized * laneSwitchSpeed * Time.deltaTime;
        if (deltaX.sqrMagnitude < diffX.sqrMagnitude)
        {
            HorizontalDeltaPosition.x = deltaX.x;
        }
        else
        {
            HorizontalDeltaPosition.x = diffX.x;
        }
    }

}
