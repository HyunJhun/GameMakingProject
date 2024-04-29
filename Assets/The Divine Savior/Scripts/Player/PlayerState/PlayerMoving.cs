using UnityEngine;

public class PlayerMoving : PlayerState
{
    public PlayerMoving(Player player, Status stats, PlayerStateMachine playerStateMachine) : base(player, stats, playerStateMachine)
    { }

    // Local Var
    private Transform playerTransform;
    private float playerRotationSpeed;
    private float playerWalkSpeed;

    private Vector3 lastPlayerMoveDirection;

    public override void Enter()
    {
        OnInitialize();
        SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Player.Walk,true);
    }
    public override void StateActionUpdate()
    {
        if (Input.GetButtonDown("Dodge"))
        {           
            playerStateMachine.ChangeState(player.dodgeState);
            return;
        }
        if (Input.GetButtonDown("Sprint"))
        {
            playerStateMachine.ChangeState(player.sprintState);
            return;
        }
        if (Input.GetButtonDown("Block"))
        {
            playerStateMachine.ChangeState(player.defenseState);
            return;
        }
        if (Input.GetButtonDown("Attack"))
        {
            playerStateMachine.ChangeState(player.offenseState);
            return;
        }
        if (player.GetKeyInputManager().CheckSkillKeyInput())
        {
            playerStateMachine.ChangeState(player.spellCastingState);
            return;
        }
    }
    public override void StateActionFixedUpdate()
    {
        OnMove(playerWalkSpeed);
    }
    public override void Exit()
    {
        SoundManager.soundManagerInstacne.StopSfx(SoundManager.SFX_Player.Walk);
    }

    private Vector3 GetMoveToDirection()
    {
        Vector3 camForward = player.GetPlayerCamera().forward;
        Vector3 camRight = player.GetPlayerCamera().right;
        camForward.y = 0;
        camRight.y = 0;
        Vector3 forwardRelatvie = Input.GetAxisRaw("Horizontal") * camRight;
        Vector3 rightRelatvie = Input.GetAxisRaw("Vertical") * camForward;
   
        Vector3 moveDir = (forwardRelatvie + rightRelatvie).normalized; // normalized을 통해 대각선으로 움직여도 값을 1로 맞추어 이동속도가 달라지는 일이 없게 함

        return moveDir;
    }

    private void OnInitialize()
    {
        playerTransform = player.transform;
        playerRotationSpeed = player.f_PlayerRotationSpeed;
        playerWalkSpeed = player.f_PlayerWalkSpeed;
    }

    public void OnMove(float playerMoveSpeed)
    {
        Vector3 moveDir = GetMoveToDirection();
        lastPlayerMoveDirection = moveDir;
        if (moveDir.magnitude == 0)
        {
            playerStateMachine.ChangeState(player.idleState);
            return;
        }

        if (moveDir != Vector3.zero)
        {
            // 캐릭터의 회전을 부드럽게 해주는 작업. Slerp를 사용해 구면 회전을 이용하였음
            Vector3 forward = Vector3.Slerp(player.transform.forward, moveDir,
            playerRotationSpeed * Time.deltaTime / Vector3.Angle(playerTransform.forward, moveDir));
            playerTransform.LookAt(playerTransform.position + forward);
        }

        player.GetPlayerController().Move(moveDir * playerMoveSpeed * Time.fixedDeltaTime);
    }


    // Get Function
    public Vector3 GetLastPlayerMoveDirection() { return lastPlayerMoveDirection; }
}
