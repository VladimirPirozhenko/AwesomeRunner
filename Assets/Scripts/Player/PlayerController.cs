using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerState
{
    Jump,
    Slide,

}
public class PlayerController : MonoBehaviour
{
    //Vector3 kRowGap = Vector3.right * 3.5f;
    const float kLaneGap = 2.5f;
    [SerializeField] private float speed;
    [SerializeField] private AnimationCurve jumpCurve;

    private CharacterController characterController;
    protected IPlayerInput input; 
    public int[] rows = new int[] { -1, 0, 1 };
    private int targetRow;
    private Vector3 targetPosition;
    EDirection? direction = null;

    Animator animator;
    Vector3 locationAfterChangingLane = Vector3.zero;
    Vector3 moveDirection = Vector3.zero;
    private bool isChangingLane = false;
    private float gravity = -9.8f;
    private float duration = 1;
    private float expiredTime = 0;
    private float height = 5;
    private void Awake()
    {
        input = new ArrowKeysInput();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }
    private void Start()
    {
        targetRow = 1;
        transform.position = new Vector3(rows[targetRow], 0, 100);
    }
    private bool isJumping = false;
    private void Update()
    { 
        //targetPosition.y = transform.position.y;
        //targetPosition.z = transform.position.z;
        //DetectInput();
        //MoveForward();
        //if (!characterController.isGrounded && !isJumping)
        //{
        //    moveDirection.y = gravity * Time.deltaTime;
        //}
        //if (isChangingLane)
        //{
        //    if (targetPosition.x == transform.position.x)
        //        isChangingLane = false;
        //    Vector3 moveDiff = (targetPosition.x - transform.position.x) * Vector3.right;
        //    Vector3 moveDir = moveDiff.normalized * 15f * Time.deltaTime;
        //    if (moveDir.sqrMagnitude < moveDiff.sqrMagnitude)
        //    {
        //        moveDirection.x = moveDir.x;
        //    }
        //    else
        //    {
        //        moveDirection.x = moveDiff.x;
        //    }
        //}

        //characterController.Move(moveDirection);
    }
    public void Rotate()
    {

    }
  

    public IEnumerator Jump()
    {
        float jumpProgress = 0f;
        isJumping = true;
        while (jumpProgress < 1)
        {
            expiredTime += Time.deltaTime;
            if (expiredTime > duration)
            {
                expiredTime = 0;
                isJumping = false;
                moveDirection.y = 0;
                yield break;
            }
            jumpProgress = expiredTime / duration;
            Vector3 jumpDirection = jumpCurve.Evaluate(jumpProgress) * Vector3.up*height;
            jumpDirection = transform.TransformDirection(jumpDirection);
            Vector3 diff = (jumpDirection.y - transform.position.y) * Vector3.up;
            if (jumpDirection.sqrMagnitude < diff.sqrMagnitude)
            {
                moveDirection.y = jumpDirection.y * Time.deltaTime;
            }
            else
            {
                moveDirection.y = diff.y;
            }
            yield return null;
        }
     
    }
    public void Slide()
    {

    }
    public void MoveForward()
    {
        //moveDirection.z += Vector3.forward * speed ;
        moveDirection.z = speed * Time.deltaTime;
        //characterController.Move(Vector3.forward * speed * Time.deltaTime);
    }
    private void DetectInput()
    {
        direction = input.ScanDirection();
        switch (direction)
        {
            case EDirection.RIGHT:
               // targetRow = 1;
                targetPosition -= Vector3.left * kLaneGap;
                direction = null;
                isChangingLane = true;
                break;
            case EDirection.LEFT:
               // targetRow = -1;
                targetPosition += Vector3.left * kLaneGap;
                //StartCoroutine(SwitchLane());
               // locationAfterChangingLane.x = transform.position.x + targetPosition.x;
                isChangingLane = true;
                direction = null;
                break;
            case EDirection.UP:
                StartCoroutine(Jump());
                break;
            case EDirection.DOWN:
                //targetPosition += Vector3.down * 10;
                break;
        }     
    }
    public IEnumerator SwitchLane()
    {
        //if (direction == EDirection.RIGHT)
        //    targetPosition -= Vector3.left * kLaneGap;
        //if (direction == EDirection.LEFT)
        //    targetPosition += Vector3.left * kLaneGap;
        if (targetPosition.x == transform.position.x)
        {
            moveDirection.x = 0;
            yield break;
        }
            
        Vector3 moveDiff = (targetPosition.x - transform.position.x) * Vector3.right;
        Vector3 moveDir = moveDiff.normalized * 15f * Time.deltaTime;
        if (moveDir.sqrMagnitude < moveDiff.sqrMagnitude)
        {
            moveDirection.x = moveDir.x;
        }
        else
        {
            moveDirection.x = moveDiff.x;
           // yield break;
        }
        yield return null;
    }
}
