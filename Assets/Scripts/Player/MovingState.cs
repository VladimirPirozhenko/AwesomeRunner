using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingState : State<Player>
{
    protected float speed = 15; 
    protected Player player;
    protected Vector3 moveDirection = Vector3.zero;
    protected CharacterController characterController;
    protected float gravity = -9.8f;   

    private Vector3 targetPosition = Vector3.zero;
    private EDirection? direction = null;
    private const float kLaneGap = 2.5f;
    private bool isChangingLane = false;
    private int targetRow = 0; 
    //private int[] rows = new int[] { -1, 0, 1 } ;
    public MovingState(Player player, CharacterController controller)
    {
        this.player = player;
        characterController = controller;
    }
    public override void OnStateEnter(){}

    public override void OnStateExit() {}

    public override void Tick()
    {
        DetectInput();
        MoveForward();
        if (isChangingLane)
        {
            SwitchLane();
        }
        if (!characterController.isGrounded && player.StateMachine.CurrentState != player.JumpState)
        {
            moveDirection.y = gravity * Time.deltaTime;
        }
    }
    private void DetectInput()
    {
        direction = player.input.ScanDirection();
        switch (direction)
        {
            case EDirection.RIGHT:
                targetRow++;
                if (targetRow > 1)
                {
                    targetRow = 1;
                }
                else
                {
                    targetPosition.x += kLaneGap;
                    isChangingLane = true;
                }
                break;
            case EDirection.LEFT:
                targetRow--;
                if (targetRow < -1)
                {
                    targetRow = -1;
                }
                else
                {
                    targetPosition.x -= kLaneGap;
                    isChangingLane = true;
                }     
                break;
            case EDirection.UP:
                player.StateMachine.SetState(player.JumpState);
                break;
            case EDirection.DOWN:
                break;
        }
    }
    public void SwitchLane()
    {
        if (targetPosition.x == player.transform.position.x)
        {
            moveDirection.x = 0;
            isChangingLane = false;
            direction = null;
            Debug.Log(player.transform.position.x);
        }
            
        Vector3 moveDiff = (targetPosition.x - player.transform.position.x) * Vector3.right;
        Vector3 moveDir = moveDiff.normalized * speed * Time.deltaTime;
        if (moveDir.sqrMagnitude < moveDiff.sqrMagnitude)
        {
            moveDirection.x = moveDir.x;
        }
        else
        {
            moveDirection.x = moveDiff.x;
        }
    }
    public void MoveForward()
    {
        moveDirection.z = speed * Time.deltaTime;
    }
    public void Move()
    {
        characterController.Move(moveDirection);
    }
}
