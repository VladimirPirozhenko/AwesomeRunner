using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Statistics))]

public class Player : MonoBehaviour,IResettable, ICommandTranslator
{
    #region StateMachine

    public PlayerStateMachine PlayerStateMachine { get; private set; }

    #endregion

    #region Animation

    private Animator animator;
    [SerializeField] private AnimationCurve jumpDeltaYCurve;
    public AnimationCurve JumpDeltaYCurve { get { return jumpDeltaYCurve; } }
    public PlayerAnimator PlayerAnimator { get; private set; }  

    #endregion

    #region PlayerComponents

    [SerializeField] private PlayerData playerData;
    public IDamageable PlayerHealth { get; private set; }
    public Statistics PlayerStatictics { get; private set; }
    public PlayerData PlayerData { get { return playerData; } }
   
    #endregion

    #region MovementControl

    [SerializeField] private LaneSystem laneSystem;
    public LaneSystem LaneSystem { get { return laneSystem; } private set { laneSystem = value; } }
    public CharacterController CharacterController { get; private set; }
    public PlayerCollider PlayerCollider { get; private set; }

    #endregion

    public bool IsInvincible { get; private set; }
    public float InvincibilityTime { get; private set; } //PLAYER DATA ScriptableObject

    private void Awake()
    {
        GameSession.Instance.AddCommandTranslator(this);
        animator = GetComponent<Animator>();
        if (animator)
            PlayerAnimator = new PlayerAnimator(animator);
        CharacterController = GetComponent<CharacterController>();
        PlayerCollider = new PlayerCollider(CharacterController);   
        PlayerHealth = GetComponent<IDamageable>();
        PlayerStatictics = GetComponent<Statistics>();
        PlayerStateMachine = new PlayerStateMachine(this);
        InvincibilityTime = playerData.InvincibilityTime;
    }  
    private void OnEnable()
    {
        PlayerHealth.OnOutOfHealth += Die;
    }
    private void OnDisable()
    {
        PlayerHealth.OnOutOfHealth -= Die;
    }
    private void Start()
    {
        PlayerStateMachine.SetState(PlayerStateMachine.PlayerStartingIdleState);
    }


    private void Update()
    {
        PlayerStateMachine.Tick();   
    }
    private void FixedUpdate()
    {
        PlayerStateMachine.FixedTick();
    } 
    public float PendingAdditionalOffset { get; private set; }
    private void OnTriggerEnter(Collider other) 
    {
        if (other.TryGetComponent(out IDamageDealer damageDealer)) //switch..case
        {
            if (IsInvincible)
                return;
            int damageAmount = 1;       
            var damageableComponents = GetComponents<IDamageable>();
            foreach (var component in damageableComponents)
            {
                damageDealer.DealDamage(component, damageAmount);
            }
            StartCoroutine(GrantInvincibility());
        }
        if (other.TryGetComponent(out IObstacle obstacle)) //switch..case
        {
            obstacle.Impact();
        }
        else if (other.TryGetComponent(out ICollectable collectable))
        {
            collectable.Collect();
        }
    }

    private void Die()
    {
        PlayerStateMachine.SetState(PlayerStateMachine.PlayerDeadState);
        GameSession.Instance.UpdateScoreboard(new ScoreboardEntry(name,PlayerStatictics.Score));
    }

    public IEnumerator GrantInvincibility()
    {
        IsInvincible = true;
        yield return new WaitForSeconds(InvincibilityTime);
        IsInvincible = false;
    }
    private void ReloadAnimator()
    {
        if (animator)
            PlayerAnimator = new PlayerAnimator(animator);
    }
    public void ResetToDefault()
    {
        PlayerStateMachine.SetState(null);
        PlayerStatictics.ResetToDefault();
        LaneSystem.ResetToDefault();
        Physics.SyncTransforms();
        ReloadAnimator();
    }

    public void TranslateCommand(ECommand command, PressedState state)
    {
        if (state.IsPressed)
        {
            switch (command)
            {
                case ECommand.RIGHT:
                    PlayerStateMachine.IncreaseTargetLane();
                    break;
                case ECommand.LEFT:
                    PlayerStateMachine.DecreaseTargetLane();
                    break;
                case ECommand.UP:
                    PlayerStateMachine.SetState(PlayerStateMachine.PlayerJumpState);
                    break;
                case ECommand.DOWN:
                    PlayerStateMachine.SetState(PlayerStateMachine.PlayerSlideState);
                    break;
                default:
                    break;
            }
        }
    }
}
