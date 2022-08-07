using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerStateMachine : StateMachine<Player>
{
    private Player player;
    private PlayerData playerData;
    private Transform playerTransform;
    public Transform PlayerTransform { get { return playerTransform; } }
    public PlayerData PlayerData { get { return playerData; } }
    public PlayerStateMachine(Player player)
    {
        this.player = player;
        playerData = player.PlayerData;
        playerTransform = player.transform;
        InitStates();
    }
    #region States
    public DeadState PlayerDeadState { get; private set; }
    public JumpState PlayerJumpState { get; private set; }
    public GroundState PlayerGroundState { get; private set; }
    public SlideState PlayerSlideState { get; private set; }
    public StartingIdleState PlayerStartingIdleState { get; private set; }
    private void InitStates()
    {
        PlayerDeadState = new DeadState(this);
        PlayerSlideState = new SlideState(this);
        PlayerGroundState = new GroundState(this);
        PlayerJumpState = new JumpState(this, player.JumpDeltaYCurve);
        PlayerStartingIdleState = new StartingIdleState(this);
    }
    #endregion
    #region Movement
    public Vector3 HorizontalDeltaPosition;
    public float VerticalDeltaPosition;

    public Vector3 DeltaPosition;
    public Vector3 forwardDirection;
    public Vector3 sidewaysDirection;
    //public EForwardDirection? InputDirection { get { return player.InputDirection; } }
    public EDirection Direction => player.Direction;
    public EInputDirection? InputDirection { get { return player.InputDirection; } }
    public bool IsOnTargetLane(float position)
    {
        return player.LaneSystem.IsOnTargetLane(position);
    }
    public float DesiredDifference { get { return player.LaneSystem.DesiredDifference; } }
    public float TargetLanePosition { get { return player.LaneSystem.TargetPosition; } }
    public float CalculateDistanceToTargetLane(float position)
    {
        //
        return player.LaneSystem.CalculateDistanceToTargetLane(position) ;

        //return player.LaneSystem.CalculateDistanceToTargetLane(playerTransform.position.z) * playerTransform.right;
       // return player.LaneSystem.CalculateDistanceToTargetLane(playerTransform.position.x) * playerTransform.right;
    }
    public void IncreaseTargetLane(int amount = 1)
    {
        if (player.IsTurning)
        {
            player.IsTurning = false;
            return;
        }
        if (Direction == EDirection.NORTH || Direction == EDirection.WEST)
        {
            player.LaneSystem.TargetLane += amount;
        }
        else
        {
            player.LaneSystem.TargetLane -= amount;
        }
        
    }
    public void DecreaseTargetLane(int amount = 1)
    {
        if (player.IsTurning)
        {
            player.IsTurning = false;
            return;
        }       
        if (Direction == EDirection.NORTH || Direction == EDirection.WEST)
        {
            player.LaneSystem.TargetLane -= amount;
        }
        else
        {
            player.LaneSystem.TargetLane += amount;
        }
    }
    public bool IsTurning { get { return player.IsTurning; } set { player.IsTurning = value; } }
    public Vector3 start => player.startPoint;
    public Vector3 end => player.endPoint;
    //public void ChangeHorizontalDelta()
    //{
    //    switch (playerSM.Direction)
    //    {
    //        case EDirection.NORTH:
    //            playerSM.HorizontalDeltaPosition.z = speed * Time.deltaTime; //INCAPSULATE
    //            break;
    //        case EDirection.SOUTH:
    //            playerSM.HorizontalDeltaPosition.z = -speed * Time.deltaTime; //INCAPSULATE
    //            break;
    //        case EDirection.WEST:
    //            playerSM.HorizontalDeltaPosition.x = -speed * Time.deltaTime; //INCAPSULATE
    //            break;
    //        case EDirection.EAST:
    //            playerSM.HorizontalDeltaPosition.x = speed * Time.deltaTime; //INCAPSULATE
    //            break;
    //    }
    //}

    public void Move(Vector3 deltaPosition)
        {
            player.CharacterController.Move(deltaPosition);
            //player.Move(deltaPosition);
        }
        #endregion
    #region Animation
    public void PlayDeadAnimation(bool isPlaying)
    {
        player.PlayerAnimator.SetDeadState(isPlaying);
    }
    public void PlayIdleAnimation(bool isPlaying)
    {
        player.PlayerAnimator.SetIdleState(isPlaying);
    }
    public void PlayRunningAnimation(bool isPlaying)
    {
        player.PlayerAnimator.SetRunState(isPlaying);
    }
    public void PlayJumpingAnimation(bool isPlaying) // NAMING
    {
        player.PlayerAnimator.SetJumpState(isPlaying);
    }
    public void SetAnimatorSlideState(bool isPlaying)
    {
        player.PlayerAnimator.SetSlideState(isPlaying);
    }
    #endregion
    #region Rigging
    //public float RightHandRigWeight { get { return player.PlayerRigging.RightHandRig.weight; } }
    //public Rig RightHandRig { get { return player.PlayerRigging.RightHandRig; } }
    public void ChangeRigWeight(Rig rig,float from, float to, float timeToChange) //
    {
        rig.ChangeWeightOverTime(from, to, timeToChange);//ChaChangeRigWeight(rig,from, to, timeToChange);
}
    public void ChangeRigWeight(Rig rig, float to)
    {
        rig.ChangeWeight(to);
    }
    #endregion
    #region Statistics
    public void UpdateDistance(float amount)
    {
        player.PlayerStatictics.UpdateDistance(amount);
    }
    //ADD CALCULATE SCORE
    #endregion
    #region Collider
    public float DefaultColliderHeight { get { return player.playerCollider.defaultHeight; } } // =>
    public Vector3 DefaultColliderCenter { get { return player.playerCollider.defaultCenter; } }
    public void ChangeColliderHeight(float newHeight)
    {
        player.playerCollider.ChangeColliderHeight(newHeight);
    }
    public void ChangeColliderCenter(Vector3 newCenter)
    {
        player.playerCollider.ChangeColliderCenter(newCenter);
    }
    public void ResetColliderToDefault()
    {
        player.playerCollider.ResetToDefault(); 
    }
    #endregion
}