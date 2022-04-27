using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerAnimator))]
public class Player : MonoBehaviour
{
    
    #region StateMachine
    public StateMachine<Player> StateMachine { get; private set; }
    public DeadState PlayerDeadState { get; private set; } 
    public JumpState PlayerJumpState { get; private set; }
    public GroundState PlayerGroundState { get; private set; }
    //public InvincibleState PlayerInvincibleState { get; private set; }
    #endregion

    #region Animation
    private PlayerAnimator playerAnimator;
    [SerializeField] private AnimationCurve jumpCurve;
    #endregion

    #region PlayerProperties
    public int Lives { get; set; }
    public bool IsInvincible { get; private set; }
    public int InvincibilityTime { get; private set; } //PLAYER DATA ScriptableObject
    #endregion

    #region MovementControl
    private IPlayerInput input;
    private CharacterController characterController;                                   
    public EDirection? Direction { get; private set; }
    [HideInInspector] public Vector3 HorizontalDeltaPosition;
    public float VerticalDeltaPosition { get; set; }
    public LaneSystem LaneSystem { get;  set; }
    #endregion
    private void Awake()
    {
        input = new ArrowKeysInput(); 
        LaneSystem = GameObject.Find("LaneSystem").GetComponent<LaneSystem>();
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<PlayerAnimator>();
        StateMachine = new StateMachine<Player>();
        InitStates();
    }
    private void Start()
    {
        StateMachine.SetState(PlayerGroundState);
        transform.position = new Vector3(0, 0, 100);
        HorizontalDeltaPosition = Vector3.zero;
        VerticalDeltaPosition = 0;
        Lives = 3;
        InvincibilityTime = 3000;
    }
    private void Update()
    {
        Direction = input.ScanDirection();
        StateMachine.Tick();   
    }
    private void FixedUpdate()
    {
        StateMachine.FixedTick();
    }
    private void InitStates()
    {
        PlayerDeadState = new DeadState(this);
        PlayerGroundState = new GroundState(this, characterController, playerAnimator);
        PlayerJumpState = new JumpState(this, characterController, jumpCurve, playerAnimator);
    }
    public async void GrantInvincibility()
    {
        IsInvincible = true;
        await Task.Delay(InvincibilityTime);
        IsInvincible = false;
    }
}
