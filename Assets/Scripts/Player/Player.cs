using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


//[RequireComponent(typeof(CharacterController))]
//[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerStatistics))]
[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{

    #region StateMachine
    public StateMachine<Player> PlayerStateMachine { get; private set; }
    public DeadState PlayerDeadState { get; private set; } 
    public JumpState PlayerJumpState { get; private set; }
    public GroundState PlayerGroundState { get; private set; }
    public SlideState PlayerSlideState { get; private set; }
    public TurnState PlayerTurnState { get; private set; }
    #endregion

    #region Animation
    private PlayerAnimator playerAnimator;
    [SerializeField] private AnimationCurve jumpCurve;
    #endregion

    #region PlayerComponents
    [SerializeField] private PlayerData playerData;
    public PlayerHealth PlayerHealth { get; private set; }
    public PlayerController PlayerCollider { get; private set; }
    public PlayerStatistics PlayerStatictics { get; private set; }
    
    public PlayerData PlayerData { get { return playerData; } }
    #endregion

    #region MovementControl
    private IPlayerInput input;                          
    public EDirection? Direction { get; private set; }
    [HideInInspector] public Vector3 HorizontalDeltaPosition;
    public float VerticalDeltaPosition { get; set; }
    [SerializeField] private LaneSystem laneSystem;
    public LaneSystem LaneSystem { get { return laneSystem; } private set { laneSystem = value; } }
    #endregion


    private void Awake()
    {
        input = new ArrowKeysInput();       
        playerAnimator = new PlayerAnimator(GetComponent<Animator>());   
        PlayerHealth = GetComponent<PlayerHealth>();
        PlayerCollider = GetComponent<PlayerController>();
        PlayerStatictics = GetComponent<PlayerStatistics>();
        PlayerStateMachine = new StateMachine<Player>();
        InitStates();
    }  
    private void OnEnable()
    {
        PlayerHealth.OnDied += Die;
    }
    private void OnDisable()
    {
        PlayerHealth.OnDied -= Die;
    }
    private void Start()
    {
        PlayerStateMachine.SetState(PlayerGroundState);
        transform.position = new Vector3(0, 0, 30);
        HorizontalDeltaPosition = Vector3.zero;
        VerticalDeltaPosition = 0;
    }
    private void Update()
    {
        Direction = input.ScanDirection();
        PlayerStateMachine.Tick();   
    }
    private void FixedUpdate()
    {
        PlayerStateMachine.FixedTick();
    }
    private void Die()
    {
        PlayerStateMachine.SetState(PlayerDeadState);
        PlayerStatictics.ShowGameOverPopUp(true);
    }
    private void InitStates()
    {
        PlayerDeadState = new DeadState(this, playerAnimator);
        PlayerSlideState = new SlideState(this, PlayerCollider, playerAnimator);
        PlayerGroundState = new GroundState(this, PlayerCollider, playerAnimator);
        PlayerTurnState = new TurnState(this, PlayerCollider);
        PlayerJumpState = new JumpState(this, PlayerCollider, jumpCurve, playerAnimator);
    }

    public void RestartSession()
    {
        PlayerStateMachine.SetState(PlayerGroundState);
        transform.position = new Vector3(0, 0, 0);
        PlayerStatictics.ShowGameOverPopUp(false);
        PlayerStatictics.ResetToDefault();
        PlayerHealth.ResetToDefault();
    }
    //void HandleDirection()
    //{
    //    switch (Direction)
    //    {
    //        case EDirection.RIGHT:
    //            LaneSystem.TargetLane++;
    //            break;
    //        case EDirection.LEFT:
    //            LaneSystem.TargetLane--;
    //            break;
    //        case EDirection.UP:
    //            PlayerStateMachine.SetState(PlayerJumpState);
    //            break;
    //        case EDirection.DOWN:
    //            PlayerStateMachine.SetState(PlayerSlideState);
    //            break;
    //        default:
    //            break;
    //    }
    //}
    //PlayerCollisionHandler
    //private void OnTriggerEnter(Collider other)
    //   {      
    //       if (other.TryGetComponent(out Obstacle obstacle))
    //       {
    //		if (IsInvincible)
    //			return;
    //           TakeDamage();
    //           obstacle.Impact(); 
    //		StartCoroutine(GrantInvincibility());
    //       }
    //       if (other.TryGetComponent(out Coin coin))
    //       {
    //           coin.gameObject.SetActive(false);
    //       }
    //   }

    //   private void OnTriggerExit(Collider other)
    //   {
    //       if (other.TryGetComponent(out Chunk chunk))
    //       {
    //           //OnChunkExited.Invoke();
    //       }
    //   }

    //   public void TakeDamage()
    //{
    //	Lives--;
    //	if (Lives < 1)
    //	{
    //		StateMachine.SetState(PlayerDeadState);
    //	}
    //}
    //public IEnumerator GrantInvincibility()
    //{
    //	IsInvincible = true;
    //	yield return new WaitForSeconds(InvincibilityTime);
    //       IsInvincible = false;
    //}
}
