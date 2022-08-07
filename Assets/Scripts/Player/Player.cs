
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Statistics))]
//[RequireComponent(typeof(WeaponController))] //NULL OBJECT PATTERN?
//[RequireComponent(typeof(RigController))]
public class Player : MonoBehaviour, IResettable
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
    //public WeaponController PlayerWeaponController { get; private set; }
    //public RigController PlayerRigging { get; private set; }
    public PlayerData PlayerData { get { return playerData; } }
   
    #endregion
    #region MovementControl
    private IPlayerInput input;                          
    public EInputDirection? InputDirection { get; private set; }
    [SerializeField] private LaneSystem laneSystem;
    public LaneSystem LaneSystem { get { return laneSystem; } private set { laneSystem = value; } }
    public CharacterController CharacterController { get; private set; }
    public PlayerCollider playerCollider { get; private set; }
    //public bool IsTurned { get; private set; }
    #endregion
    public bool IsInvincible { get; private set; }
    public float InvincibilityTime { get; private set; } //PLAYER DATA ScriptableObject
    public EDirection Direction { get; private set; }
    public EDirection PendingDirection { get; private set; }
    public bool IsTurning { get;  set; }
    private void Awake()
    {
        input = new ArrowKeysInput();
        animator = GetComponent<Animator>();
        if (animator)
            PlayerAnimator = new PlayerAnimator(animator);
        CharacterController = GetComponent<CharacterController>();
        playerCollider = new PlayerCollider(CharacterController);   
        PlayerHealth = GetComponent<IDamageable>();
        PlayerStatictics = GetComponent<Statistics>();
        //PlayerWeaponController = GetComponent<WeaponController>();
        //PlayerRigging = GetComponent<RigController>();
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
        //IsTurned = false;
        IsTurning = false;
    }
    private void Update()
    {
        //if (CurrentSession.IsSessionPaused())
        //    return;
        InputDirection = input.ScanDirection();
        //if (input.IsShooting())
            //PlayerWeaponController.PerfomShoot();
        if (IsTurning)
        {
            if (InputDirection == EInputDirection.LEFT)
            {
                transform.Rotate(0, -90, 0, Space.Self);
                Direction = PendingDirection;
                LaneSystem.AdditionalOffset = PendingAdditionalOffset;
                LaneSystem.TargetPosition = LaneSystem.AdditionalOffset + LaneSystem.CurrentOffset;
                Debug.Log(Direction);
                Debug.Log(LaneSystem.TargetLane);
                //IsTurning = false;
            }
            if (InputDirection == EInputDirection.RIGHT)
            {
                transform.Rotate(0, 90, 0, Space.Self);
                Direction = PendingDirection;
                LaneSystem.AdditionalOffset = PendingAdditionalOffset;
                LaneSystem.TargetPosition = LaneSystem.AdditionalOffset + LaneSystem.CurrentOffset; ;
                Debug.Log(Direction);
                Debug.Log(LaneSystem.TargetLane);
                //IsTurning = false;
            }
    }
        PlayerStateMachine.Tick();   
    }
    private void FixedUpdate()
    {
        PlayerStateMachine.FixedTick();
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.TryGetComponent(out TurningChunk chunk)) //switch..case
    //    {
    //        if (Input.GetKeyDown(KeyCode.LeftArrow))
    //        {
    //            if (Direction == chunk.Direction)
    //                return;
    //            Direction = chunk.Direction;
    //            if (chunk.IsClockwise)
    //            {
    //                transform.Rotate(0, 90, 0, Space.Self);
    //                LaneSystem.TargetLane = 0;
    //            }
    //            else
    //            {
    //                transform.Rotate(0, -90, 0, Space.Self);
    //                LaneSystem.TargetLane = 0;
    //            }
    //        }
    //    }
    //}
    public Vector3 startPoint = Vector3.zero;
    public Vector3 endPoint = Vector3.zero; 
    public float PendingAdditionalOffset { get; private set; }
    private void OnTriggerEnter(Collider other) 
    {
        if (other.TryGetComponent(out TurningChunk chunk)) //switch..case
        {
            IsTurning = true;
            startPoint = chunk.Begin.transform.position;
            endPoint = chunk.End.transform.position;
            PendingDirection = chunk.Direction;
            if (chunk.Direction == EDirection.NORTH || chunk.Direction == EDirection.SOUTH)
            {
                PendingAdditionalOffset = chunk.End.transform.position.x;
                Debug.Log(PendingAdditionalOffset);
            }
            else
            {
                PendingAdditionalOffset = chunk.End.transform.position.z;
                Debug.Log(PendingAdditionalOffset);
            }
        }
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
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out TurningChunk chunk)) //switch..case
        {
            IsTurning = false;

        }
    }

    private void Die()
    {
        PlayerStateMachine.SetState(PlayerStateMachine.PlayerDeadState);    
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
    public void RestartSession()
    { 
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
        ResetToDefault();
    }
    public void GoToMainMenu()
    {     
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        ResetToDefault();
    }
}
