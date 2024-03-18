public class EnemyAttack : EnemyState
{
    // Start is called before the first frame update
    public EnemyAttack(Enemy enemy, Status stats, EnemyStateMachine enemyStateMachine) : base(enemy, stats, enemyStateMachine)
    { }

    private bool b_isAttack;

    public override void Enter()
    {
        enemy.GetAttackRangeBox().SetActive(true);
        b_isAttack = true;
        onAttack();

    }

    public override void StateActionUpdate()
    {
        if (animationPlayingCheck())
        {
            enemyStateMachine.ChangeState(enemy.chaseState);
            return;
        }


    }
    public override void StateActionFixedUpdate()
    {

    }

    public override void Exit()
    {
        enemy.GetAttackRangeBox().SetActive(false);
        enemy.GetEnemyNavMeshAgent().enabled = true;
    }
    private void onAttack()
    {
        enemy.GetAnimator().SetTrigger("Attack");
    }

    private bool animationPlayingCheck()
    {
        return enemy.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("Attack") && enemy.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f;
    }

    public bool GetIsAttack() { return b_isAttack; }
}
