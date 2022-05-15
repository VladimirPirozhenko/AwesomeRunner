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
    public StartingIdleState PlayerStartingIdleState { get; private set; }
    #endregion

    #region Animation
    [SerializeField] private AnimationCurve jumpDeltaYCurve;
    private PlayerAnimator playerAnimator;   
    #endregion

    #region PlayerComponents
    [SerializeField] private PlayerData playerData;
    public PlayerHealth PlayerHealth { get; private set; }
    public PlayerController PlayerController { get; private set; }
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
        PlayerController = GetComponent<PlayerController>();
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
        PlayerStateMachine.SetState(PlayerStartingIdleState);
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
    }
    private void InitStates()
    {
        PlayerDeadState = new DeadState(this, playerAnimator);
        PlayerSlideState = new SlideState(this, PlayerController, playerAnimator);
        PlayerGroundState = new GroundState(this, PlayerController, playerAnimator);
        PlayerJumpState = new JumpState(this, PlayerController, jumpDeltaYCurve, playerAnimator);
        PlayerStartingIdleState = new StartingIdleState(this, playerAnimator);
    }
    
    public void RestartSession()
    {
        PlayerStateMachine.SetState(PlayerStartingIdleState);
        PlayerStatictics.ResetToDefault();
        PlayerHealth.ResetToDefault();
        LaneSystem.ResetToDefault();
        Physics.SyncTransforms();
    }
}
