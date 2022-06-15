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
        playerSM.HorizontalDeltaPosition = speed * playerSM.forwardDirection * Time.deltaTime ;
        playerSM.HorizontalDeltaPosition += playerSM.PlayerTransform.forward * speed * Time.deltaTime; //INCAPSULATE
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
        //playerSM.HorizontalDeltaPosition = playerSM.PlayerTransform.forward * speed * Time.deltaTime; //INCAPSULATE
        float sidewaysPos = 0;
        Vector3 multiplierVector = Vector3.zero;    
        if (playerSM.Direction == EDirection.NORTH)
        {
            sidewaysPos = playerTransform.localPosition.x;
            multiplierVector = Vector3.right;
        }
        if (playerSM.Direction == EDirection.SOUTH)
        {
            sidewaysPos = playerTransform.localPosition.x;
            multiplierVector = Vector3.left;
        }
        if (playerSM.Direction == EDirection.EAST)
        {
            sidewaysPos = playerTransform.localPosition.z;
            multiplierVector = Vector3.back;
        }
        if (playerSM.Direction == EDirection.WEST)
        {
            sidewaysPos = playerTransform.localPosition.z;
            multiplierVector = Vector3.forward;
        }

        float targetPos = playerSM.TargetLanePosition;
        if (playerSM.IsOnTargetLane(sidewaysPos))
        {
            playerSM.HorizontalDeltaPosition += playerSM.PlayerTransform.right * 0;
            return;
        }
        //Vector3 diffX = playerSM.CalculateDistanceToTargetLane(sidewaysPos);
        float desiredPosition =  playerSM.DesiredDifference + sidewaysPos;
        Vector3 diffX = (targetPos - sidewaysPos) * multiplierVector;
        Vector3 deltaX = diffX.normalized * laneSwitchSpeed * Time.deltaTime;
        if (deltaX.sqrMagnitude < diffX.sqrMagnitude)
        {
            if (playerSM.Direction == EDirection.NORTH || playerSM.Direction == EDirection.SOUTH)
            {
                playerSM.HorizontalDeltaPosition += playerSM.PlayerTransform.right * deltaX.x;
            }
            else
            {
                playerSM.HorizontalDeltaPosition += playerSM.PlayerTransform.right * deltaX.z;
            }
        }
        else
        {
            if (playerSM.Direction == EDirection.NORTH || playerSM.Direction == EDirection.SOUTH)
            {
                playerSM.HorizontalDeltaPosition += playerSM.PlayerTransform.right * diffX.x;
            }
            else
            {
                playerSM.HorizontalDeltaPosition += playerSM.PlayerTransform.right * diffX.z;
            }
        }
    }

}
////playerSM.HorizontalDeltaPosition = playerSM.PlayerTransform.forward * speed * Time.deltaTime; //INCAPSULATE
//float sidewaysPos = 0;
//Vector3 multiplierVector = Vector3.zero;
//if (playerSM.Direction == EDirection.NORTH || playerSM.Direction == EDirection.SOUTH)
//{
//    sidewaysPos = playerTransform.localPosition.x;
//    playerSM.HorizontalDeltaPosition.x = 0;
//    multiplierVector = Vector3.right;
//}
//else
//{
//    sidewaysPos = playerTransform.localPosition.z;
//    playerSM.HorizontalDeltaPosition.z = 0;
//    multiplierVector = Vector3.forward;
//}
//float targetPos = sidewaysPos + playerSM.TargetLanePosition;
//// if (playerSM.IsOnTargetLane(sidewaysPos))
//if (targetPos == sidewaysPos)
//{
//    // playerSM.HorizontalDeltaPosition = playerSM.PlayerTransform.right * 0;
//    return;
//}
////Vector3 diffX = playerSM.CalculateDistanceToTargetLane(sidewaysPos);
////float desiredPosition =  playerSM.DesiredDifference + sidewaysPos;
//Vector3 diffX = (playerSM.DesiredDifference - sidewaysPos) * multiplierVector;
//Vector3 deltaX = diffX.normalized * laneSwitchSpeed * Time.deltaTime;
//if (deltaX.sqrMagnitude < diffX.sqrMagnitude)
//{
//    if (playerSM.Direction == EDirection.NORTH || playerSM.Direction == EDirection.SOUTH)
//    {
//        playerSM.HorizontalDeltaPosition += playerSM.PlayerTransform.right * deltaX.x;
//    }
//    else
//    {
//        playerSM.HorizontalDeltaPosition += playerSM.PlayerTransform.right * deltaX.z;
//    }
//}
//else
//{
//    if (playerSM.Direction == EDirection.NORTH || playerSM.Direction == EDirection.SOUTH)
//    {
//        playerSM.HorizontalDeltaPosition += playerSM.PlayerTransform.right * diffX.x;
//    }
//    else
//    {
//        playerSM.HorizontalDeltaPosition += playerSM.PlayerTransform.right * diffX.z;
//    }
//}

//float sidewaysPos = 0;
//Vector3 multiplierVector = Vector3.zero;
//if (playerSM.Direction == EDirection.NORTH || playerSM.Direction == EDirection.SOUTH)
//{
//    sidewaysPos = playerTransform.localPosition.x;
//    multiplierVector = Vector3.right;
//}
//else
//{
//    sidewaysPos = playerTransform.localPosition.z;
//    multiplierVector = Vector3.forward;
//}
//float targetPos = sidewaysPos + playerSM.TargetLanePosition;
//if (playerSM.IsOnTargetLane(sidewaysPos))
//{
//    playerSM.HorizontalDeltaPosition += playerSM.PlayerTransform.right * 0;
//    return;
//}
////Vector3 diffX = playerSM.CalculateDistanceToTargetLane(sidewaysPos);
//float desiredPosition = playerSM.DesiredDifference + sidewaysPos;
//Vector3 diffX = desiredPosition * multiplierVector;

//Vector3 deltaX = diffX.normalized * laneSwitchSpeed * Time.deltaTime;
//if (deltaX.sqrMagnitude < diffX.sqrMagnitude)
//{
//    playerSM.HorizontalDeltaPosition += playerSM.PlayerTransform.right * deltaX.x;
//}
//else
//{
//    playerSM.HorizontalDeltaPosition += playerSM.PlayerTransform.right * diffX.x;
//}