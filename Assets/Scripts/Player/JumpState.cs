using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class JumpState : MovingState
{
    private AnimationCurve curve;
    Animator animator;
    private float duration = 0.5f;
    private float expiredTime = 0;
    private float height = 5;
    
    public JumpState(Player player,CharacterController controller, AnimationCurve curve,Animator animator) : base(player,controller)
    {
        this.animator = animator;
        this.curve = curve;
    }
    public override void OnStateEnter()
    {
        directionY = 1f;
        //animator.SetTrigger("Jump");
        Jump();
        //StartCoroutine(Jump());
        // Jump();
        //throw new System.NotImplementedException();
    }

    public override void OnStateExit()
    {
        //throw new System.NotImplementedException();
    }
    public override void Tick()
    {
        base.Tick();
       // JumpCo();
        Move();
    }
    float directionY;
    public void JumpCo()
    {
        directionY += Time.deltaTime;
       // moveDirection = player.transform.TransformDirection(moveDirection);
        moveDirection.y = directionY;

        //expiredTime += Time.deltaTime;
        //if (expiredTime > duration)
        //{
        //    expiredTime = 0;
        //    //moveDirection.y = 0;
        //    player.StateMachine.SetState(player.GroundState);
        //    return;
        //} 
        //float jumpProgress = expiredTime / duration;
        //Vector3 jumpDirection = curve.Evaluate(jumpProgress) * Vector3.up * height;
        ////jumpDirection = player.transform.TransformDirection(jumpDirection);
        //Vector3 diff = (jumpDirection.y - player.transform.position.y) * Vector3.up;
        //player.transform.position = new Vector3(player.transform.position.x,jumpDirection.y, player.transform.position.z);
        //if (jumpDirection.sqrMagnitude < diff.sqrMagnitude)
        // {
        // moveDirection.y = jumpDirection.y * speed;
        //}
        //else
        //{
        //    moveDirection.y = diff.y ;
        //}
        //characterController.center = jumpDirection;
        if (characterController.isGrounded)
        {
            player.StateMachine.SetState(player.GroundState);
            return;
        }
    }
    public async void Jump()
    {
        float jumpProgress = 0f;
        //isJumping = true;
        while (jumpProgress < 1)
        {
            expiredTime += Time.deltaTime;
            if (expiredTime > duration)
            {
                expiredTime = 0;
                // isJumping = false;
                moveDirection.y = 0;
                player.StateMachine.SetState(player.GroundState);
                return;
                //yield break;
            }
            jumpProgress = expiredTime / duration;
            Vector3 jumpDirection = curve.Evaluate(jumpProgress) * Vector3.up * height;
            jumpDirection = player.transform.TransformDirection(jumpDirection);
            Vector3 diff = (jumpDirection.y - player.transform.position.y) * Vector3.up;
            if (jumpDirection.sqrMagnitude < diff.sqrMagnitude)
            {
                moveDirection.y = jumpDirection.y * Time.deltaTime;
            }
            else
            {
                moveDirection.y = diff.y;
            }
            await Task.Yield();
        }
        //    float jumpProgress = 0f;
        //while (jumpProgress < 1)
        //{
        //    expiredTime += Time.deltaTime;
        //    if (expiredTime > duration)
        //    {
        //        expiredTime = 0;
        //        // isJumping = false;
        //        moveDirection.y = 0;
        //        player.StateController.SetState(player.GroundState);
        //        return;
        //        //yield break;
        //    }
        //}
        //await Task.Yield();
           // yield return null;
        //}
    }
}
