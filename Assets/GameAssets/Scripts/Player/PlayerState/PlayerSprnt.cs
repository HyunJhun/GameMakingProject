using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprnt : PlayerState
{
    public PlayerSprnt(Player player, Status stats, PlayerStateMachine playerStateMachine) : base(player, stats, playerStateMachine)
    { }

    public override void Enter()
    {
        if (player.GetPlayerStatus().getStamina() < 1)
        {
            playerStateMachine.ChangeState(player.idleState);
            return;
        }

        player.b_IsSprint = true;
        player.GetPlayerStatus().InvokeRepeating("staminaDown_Sprint", 1f, 1f);
        SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Player.Sprint, true);

    }
    public override void StateActionUpdate()
    {

        if (Input.GetButtonDown("Attack"))
        {
            playerStateMachine.ChangeState(player.offenseState);
            return;
        }
        else if (Input.GetButtonDown("Block"))
        {
            playerStateMachine.ChangeState(player.defenseState);
            return;
        }
        else if (Input.GetButtonDown("Dodge"))
        {
            playerStateMachine.ChangeState(player.dodgeState);
            return;
        }
        else if (player.GetKeyInputManager().CheckSkillKeyInput())
        {
            playerStateMachine.ChangeState(player.spellCastingState);
            return;
        }
        else if (Input.GetButton("Sprint"))
        {
            OnMove(player.f_PlayerSprintSpeed);
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            playerStateMachine.ChangeState(player.idleState);
            return;
        }
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
        player.f_PlayerRotationSpeed * Time.deltaTime / Vector3.Angle(player.transform.forward, moveDir));
        player.transform.LookAt(player.transform.position + forward);

        player.tempMoveSpeed = (moveDir * playerMoveSpeed * Time.deltaTime).magnitude;
        player.GetPlayerController().Move(moveDir * playerMoveSpeed * Time.deltaTime);

    }
    public override void Exit()
    {
        player.b_IsSprint = false;
        player.GetPlayerStatus().CancelInvoke("staminaDown_Sprint");
        SoundManager.soundManagerInstacne.StopSfx(SoundManager.SFX_Player.Sprint);
    }
}
