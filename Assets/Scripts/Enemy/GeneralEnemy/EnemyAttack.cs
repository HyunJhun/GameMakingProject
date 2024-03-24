public class EnemyAttack : EnemyState
{
    // Start is called before the first frame update
    public EnemyAttack(Enemy enemy, Status stats, EnemyStateMachine enemyStateMachine) : base(enemy, stats, enemyStateMachine)
    { }
    public override void Enter()
    {
        enemy.GetAttackRangeBox().SetActive(true);
        enemy.GetAnimator().SetTrigger("Attack");
    }

    public override void StateActionUpdate()
    {
        if (animationPlayingCheck())
        {
            enemyStateMachine.ChangeState(enemy.chaseState);
            return;
        }
    }
    public override void Exit()
    {
        enemy.GetAttackRangeBox().SetActive(false);
        enemy.GetEnemyNavMeshAgent().enabled = true;
    }
    private bool animationPlayingCheck()
    {
        return enemy.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("Attack") && enemy.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f;
    }
}
