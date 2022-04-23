using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private Animator jumpAnimator;
    private CharacterController characterController;
    public StateMachine<Player> StateMachine { get; private set; }
    public DeadState DeadState { get; private set; } 
    public JumpState JumpState { get; private set; }
    public GroundState GroundState { get; private set; }
    public EDirection? Direction { get; private set; }

    public IPlayerInput input { get; private set; }

    private void Awake()
    {
        input = new ArrowKeysInput();
        characterController = GetComponent<CharacterController>();
        jumpAnimator = GetComponent<Animator>();

        StateMachine = new StateMachine<Player>();
        DeadState =  new DeadState(this);
        GroundState = new GroundState(this, characterController);
        JumpState = new JumpState(this, characterController, jumpCurve, jumpAnimator);   
    }
    private void Start()
    {
        input.ScanDirection();
        StateMachine.SetState(GroundState);
    }
    private void Update()
    {
        StateMachine.CurrentState.Tick();   
    }
}
