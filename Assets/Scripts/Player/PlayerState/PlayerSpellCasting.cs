using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellCasting : PlayerState
{
    public PlayerSpellCasting(Player player,Status stats,PlayerStateMachine playerStateMachine) : base(player,stats,playerStateMachine)
    {

    }
    // 1. z,x,c 중 어떤 키가 입력되었는지 체크
    // 2. 각 키를 기반으로 어떤 스킬이 재생될 것인지 체크
    // 3. 스킬
    private List<string> skillNameList = new List<string>();
    private bool b_isSkillExecuting = false;
    public override void Enter()
    {
        SelectSkillByKeyInput();
    }

    public override void StateActionUpdate()
    {
        checkSKillExecute();
    }
    public override void Exit()
    {
        b_isSkillExecuting = false;
    }
    private void onInitialize()
    {
        skillNameList.Add("S_SwordJudgment");
        skillNameList.Add("S_Heal");
    }

    private void checkSKillExecute()
    {   
        if (player.GetPlayerAnimationManager().CheckCurrentAnimationName("S_SwordJudgment") ||
            player.GetPlayerAnimationManager().CheckCurrentAnimationName("S_Heal"))
        {
            if (player.GetPlayerAnimationManager().GetPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
            {
                playerStateMachine.ChangeState(player.idleState);
                return;
            }
        }
    }

    public void SelectSkillByKeyInput()
    {       
        string currentKey = player.GetKeyInputManager().GetCurrentInputKey().ToString();
        Debug.Log(currentKey);

        switch(currentKey)
        {
            case "Z":
                if (!checkPlayerCanUseSkillByMpUsage(0))
                {
                    playerStateMachine.ChangeState(player.idleState);
                    break;
                }
                player.GetSkillManger().SwordJudgment();
                break;
            case "X":
                if (!checkPlayerCanUseSkillByMpUsage(1))
                {
                    playerStateMachine.ChangeState(player.idleState);
                    break;
                }
                player.GetSkillManger().Heal();
                break;
            case "C":
                break;

        }
    }

    private bool checkPlayerCanUseSkillByMpUsage(int indexOfSkill)
    {
        if (stats.GetCurrentMp() < stats.GetSkillMpUsage(indexOfSkill))
        {
            return false;
        }
        return true;
    }
}
