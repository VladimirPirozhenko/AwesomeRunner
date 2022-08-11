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
    public MovingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
        this.playerSM = playerStateMachine;
        speed = playerData.Speed;
        laneSwitchSpeed = playerData.LaneSwitchSpeed;
    }
    public override void OnStateEnter(){}

    public override void OnStateExit() {}

    public override void Tick()
    {
        //HandleDirection();
        playerSM.HorizontalDeltaPosition = speed * playerSM.PlayerTransform.forward * Time.deltaTime ;
        playerSM.HorizontalDeltaPosition += playerSM.PlayerTransform.forward * speed * Time.deltaTime; 
        playerSM.UpdateDistance(playerSM.HorizontalDeltaPosition.z); //вынести в контроллер
        SwitchLane();
        ApplyGravity();
        Vector3 deltaPosition = new Vector3(playerSM.HorizontalDeltaPosition.x,
                                            playerSM.VerticalDeltaPosition,
                                            playerSM.HorizontalDeltaPosition.z);
        playerSM.Move(deltaPosition);
    }
    public void ApplyGravity()
    {
        playerSM.VerticalDeltaPosition += gravity * Time.deltaTime;
    }


    //private void HandleDirection()
    //{
    //    switch (playerSM.InputDirection)
    //    {
    //        case EInputDirection.RIGHT:
    //            playerSM.IncreaseTargetLane();
    //            break;
    //        case EInputDirection.LEFT:
    //            playerSM.DecreaseTargetLane();
    //            break;
    //        case EInputDirection.UP:
    //            playerSM.SetState(playerSM.PlayerJumpState);
    //            break;
    //        case EInputDirection.DOWN:
    //            playerSM.SetState(playerSM.PlayerSlideState);
    //            break;
    //        default:
    //            break;
    //    }
    //}

    public void SwitchLane()
    {
        float sidewaysPos = playerTransform.localPosition.x;
        Vector3 playerDirection = playerTransform.right;
        
        float targetPos = playerSM.TargetPosition;
        if (playerSM.IsOnTargetLane(sidewaysPos))
        {
            return;
        }
        Vector3 diffX = (targetPos - sidewaysPos) * playerDirection;
        Debug.DrawLine(playerTransform.position, diffX, Color.green);
        Vector3 deltaX = diffX.normalized * laneSwitchSpeed * Time.deltaTime;
        Debug.DrawLine(playerTransform.position, deltaX, Color.red);
        if (deltaX.sqrMagnitude < diffX.sqrMagnitude)
        {
            playerSM.HorizontalDeltaPosition += playerSM.PlayerTransform.right * deltaX.x;
        }
        else
        {
            playerSM.HorizontalDeltaPosition += playerSM.PlayerTransform.right * diffX.x;
        }
    }


    
}


