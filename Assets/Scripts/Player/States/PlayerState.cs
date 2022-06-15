using UnityEngine;

public abstract class PlayerState : State<Player>
{
    protected PlayerStateMachine playerSM;
    protected PlayerData playerData;
    protected Transform playerTransform;

    //protected Vector3 HorizontalDeltaPosition;
    //protected float VerticalDeltaPosition;
    public PlayerState(PlayerStateMachine playerStateMachine)
    {
        playerSM = playerStateMachine;
        playerData = playerStateMachine.PlayerData;
        playerTransform = playerStateMachine.PlayerTransform;
    }
    public override void Tick()
    {}
}
