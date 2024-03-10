using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyState
{
    // Start is called before the first frame update
    public EnemyAttack(Enemy enemy, Status stats, EnemyStateMachine enemyStateMachine) : base(enemy, stats, enemyStateMachine)
    { }
    private float timer;
    private bool b_inRound = false;
    private float radius;
    private float theta = 0.0f;
    private Vector3 playerPostion;

    public override void Enter()
    {
        enemy.b_isAttack = true;
        enemy.GetEnemyNavMeshAgent().enabled = false;
        radius = Vector3.Distance(enemy.transform.position, enemy.GetPlayer().transform.position);
        playerPostion = enemy.GetPlayer().transform.position;
    }

    public override void StateActionUpdate()
    {
        //if (b_inRound) timer += Time.deltaTime;
        OnCircularMove();
        
    }
    public override void StateActionFixedUpdate()
    {
    
    }

    public override void Exit()
    {
        base.Exit();
    }

    public void ToDamage(int numOfAttack)
    {

    }

    private void OnAttack()
    {

    }
    private void OnCircularMove()
    {
        float x = playerPostion.x + Mathf.Cos(theta * Mathf.Deg2Rad) * radius;
        float y = enemy.transform.position.y;
        float z = playerPostion.z - Mathf.Sin(theta * Mathf.Deg2Rad) * radius;

        Vector3 enemyNewPosition = new Vector3(x, y, z);

        enemy.transform.position = Vector3.Lerp(enemy.transform.position, enemyNewPosition, theta * Time.deltaTime);
        enemy.transform.LookAt(enemy.GetPlayer().transform);    
        theta += enemy.f_attackRoundSpeed * Time.deltaTime;
        Debug.Log("THETA IS : " + theta);
        // 360도를 넘어가면 다시 0으로 초기화
        if (theta >= 360.0f)
        {
            theta = 0;
        }
       


    }
}
