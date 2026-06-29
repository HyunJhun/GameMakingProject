using UnityEngine;

public class PlayerMoving : PlayerState
{
    public PlayerMoving(Player player, Status stats, PlayerStateMachine playerStateMachine) : base(player, stats, playerStateMachine)
    { }

    // Local Var
    private Transform playerTransform;
    private float playerRotationSpeed;
    private float playerWalkSpeed;

    public override void Enter()
    {
        OnInitialize();
        SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Player.Walk, true);
    }
    public override void StateActionUpdate()
    {
        if (Input.GetButtonDown("Dodge"))
        {
            playerStateMachine.ChangeState(player.dodgeState);
            return;
        }
        else if (Input.GetButtonDown("Sprint"))
        {
            playerStateMachine.ChangeState(player.sprintState);
            return;
        }
        else if (Input.GetButtonDown("Block"))
        {
            playerStateMachine.ChangeState(player.defenseState);
            return;
        }
        else if (Input.GetButtonDown("Attack"))
        {
            playerStateMachine.ChangeState(player.offenseState);
            return;
        }
        else if (player.GetKeyInputManager().CheckSkillKeyInput())
        {
            playerStateMachine.ChangeState(player.spellCastingState);
            return;
        }
        else
            OnMove(playerWalkSpeed);
    }

    public override void Exit()
    {
        SoundManager.soundManagerInstacne.StopSfx(SoundManager.SFX_Player.Walk);
    }

    private void OnInitialize()
    {
        playerTransform = player.transform;
        playerRotationSpeed = player.f_PlayerRotationSpeed;
        playerWalkSpeed = player.f_PlayerWalkSpeed;
    }

    public void OnMove(float playerMoveSpeed)
    {
        Vector3 moveDir = player.GetMoveToDirection();

        if (moveDir == Vector3.zero)
        {
            playerStateMachine.ChangeState(player.idleState);
            return;
        }

        // 캐릭터의 회전을 부드럽게 해주는 작업. Slerp를 사용해 구면 회전을 이용하였음
        Vector3 forward = Vector3.Slerp(player.transform.forward, moveDir,
        playerRotationSpeed * Time.deltaTime / Vector3.Angle(playerTransform.forward, moveDir));
        playerTransform.LookAt(playerTransform.position + forward);

        player.tempMoveSpeed = (moveDir * playerMoveSpeed * Time.deltaTime).magnitude;
        player.GetPlayerController().Move(moveDir * playerMoveSpeed * Time.deltaTime);

    }


    // Get Function
    //public Vector3 GetLastPlayerMoveDirection() { return lastPlayerMoveDirection; }
}
