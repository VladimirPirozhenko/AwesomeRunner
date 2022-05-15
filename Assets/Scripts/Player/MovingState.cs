using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class MovingState : State<Player>
{
    private float speed; //SO
    private float laneSwitchSpeed; //SO
    protected Player player;
    protected PlayerController playerCollider;
    protected const float gravity = -9.8f;

    public bool canTurn = false;
    public MovingState(Player player, PlayerController collider)
    {
        this.player = player;
        speed = player.PlayerData.Speed;
        laneSwitchSpeed = player.PlayerData.LaneSwitchSpeed;
        playerCollider = collider;
    }
    public override void OnStateEnter(){}

    public override void OnStateExit() {}

    public override void Tick()
    {
        HandleDirection(); 
        player.HorizontalDeltaPosition.z = speed * Time.deltaTime;
        player.PlayerStatictics.IncreaseDistance(player.HorizontalDeltaPosition.z);
        if (player.LaneSystem.isChangingLane)
        {
            SwitchLane();
        }
        ApplyGravity();
        Vector3 deltaPosition = new Vector3(player.HorizontalDeltaPosition.x,
                                            player.VerticalDeltaPosition, 
                                            player.HorizontalDeltaPosition.z);
       // Debug.Log("IsGrounded: " + characterController.isGrounded);
        //Debug.Log("isChangingLane: " + player.LaneSystem.isChangingLane);
        //Debug.Log("deltaPosition: " + deltaPosition);
        playerCollider.Move(deltaPosition);
    }
    public void ApplyGravity()
    {
        {
            player.VerticalDeltaPosition += gravity * Time.deltaTime;
        }
        //else
        //{
        //    player.VerticalDeltaPosition = 0;
        //}
        //if (player.PlayerStateMachine.CurrentState == player.PlayerGroundState ||
        //    player.PlayerStateMachine.CurrentState == player.PlayerSlideState)//characterController.isGrounded
        //{
        //    player.VerticalDeltaPosition = 0;
        //}
        //else
        //{
        //    //player.VerticalDeltaPosition = 0;    
        //}
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
        if (player.LaneSystem.TargetX == player.transform.position.x)//player.TargetPosX
        {
            player.HorizontalDeltaPosition.x = 0;
            player.LaneSystem.isChangingLane = false;
            return;
        }
        Vector3 diffX = (player.LaneSystem.TargetX - player.transform.position.x) * Vector3.right;//player.TargetPosX
        Vector3 deltaX = diffX.normalized * laneSwitchSpeed * Time.deltaTime;
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
