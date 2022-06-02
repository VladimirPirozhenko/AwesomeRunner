using UnityEngine;
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
    //protected Vector3 HorizontalDeltaPosition;
    //protected float VerticalDeltaPosition;
    public EInputDirection? InputDirection { get { return player.InputDirection; } } 
    public bool IsOnTargetLane(float position)
    {
        return player.LaneSystem.IsOnTargetLane(position);
    }
    public Vector3 CalculateDistanceToTargetLane()
    {
        return player.LaneSystem.CalculateDistanceToTargetLane(playerTransform.position.x) * Vector3.right;
    }
    public void IncreaseTargetLane(int amount = 1)
    {
        player.LaneSystem.TargetLane += amount;
    }
    public void DecreaseTargetLane(int amount = 1)
    {
        player.LaneSystem.TargetLane -= amount;
    }
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
    public float RightHandRigWeight { get { return player.PlayerRigging.RightHandRig.weight; } }
    public void ChangeRightHandRigWeight(float from, float to, float timeToChange) //
    {
        player.PlayerRigging.ChangeRightHandIKWeight(from, to, timeToChange);
    }
    public void ChangeRightHandRigWeight(float to)
    {
        player.PlayerRigging.ChangeRightHandIKWeight(to);
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