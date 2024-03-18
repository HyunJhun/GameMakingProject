using UnityEngine;

public class EnemyIdle : EnemyState
{
    public EnemyIdle(Enemy enemy, Status stats, EnemyStateMachine enemyStateMachine) : base(enemy, stats, enemyStateMachine)
    { }

    // Local Var
    private float timer;
    private float waitingTime; 

    public override void Enter()
    {
        enemy.GetAnimator().SetBool("isIdle", true);
        timer = 0f;
        selectWaitingTime();
    }

    public override void StateActionUpdate()
    {
        if (timer > waitingTime)
        {
            enemyStateMachine.ChangeState(enemy.patrolState);
            return;
        }

        timer += Time.deltaTime;


    }

    public override void Exit()
    {
        enemy.GetAnimator().SetBool("isIdle", false);
    }

    private void selectWaitingTime()
    {
        waitingTime = Random.Range(0, 2f);

    }
    
}
