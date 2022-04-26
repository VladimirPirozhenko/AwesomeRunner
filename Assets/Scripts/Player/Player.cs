using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerAnimator))]
public class Player : MonoBehaviour
{
    [SerializeField] private AnimationCurve jumpCurve;
    public int Lives { get; set; }
    public StateMachine<Player> StateMachine { get; private set; }
    public DeadState PlayerDeadState { get; private set; } 
    public JumpState PlayerJumpState { get; private set; }
    public GroundState PlayerGroundState { get; private set; }
    public InvincibleState PlayerInvincibleState { get; private set; }
   
    public bool IsInvincible { get; private set; }
    public int InvincibleTime { get; private set; } //PLAYER DATA ScriptableObject
                                                
    public EDirection? Direction { get; private set; }

    [HideInInspector]
    public Vector3 HorizontalDeltaPosition;
    public float VerticalDeltaPosition { get; set; }

    private PlayerAnimator playerAnimator;
    private CharacterController characterController; 
    private IPlayerInput input;
    public LaneSystem LaneSystem { get;  set; }
    //[SerializeField] private GameObject lanePrefab;
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
        await Task.Delay(InvincibleTime);
        IsInvincible = false;
    }
}
