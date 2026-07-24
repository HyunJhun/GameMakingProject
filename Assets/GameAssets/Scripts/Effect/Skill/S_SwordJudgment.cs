using System.Collections.Generic;
using UnityEngine;

public class S_SwordJudgment : MonoBehaviour
{
    [SerializeField]private List<Enemy> collisionEnemies = new List<Enemy>();
    private Boss collisionBoss;
    // Start is called before the first frame update
    void Start()
    {
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<Enemy>() != null)
            {
                preventDuplicateAdd(other.GetComponent<Enemy>());
            }
            if (other.GetComponent<Boss>() != null)
            {
                collisionBoss = other.GetComponent<Boss>();
                collisionBoss.isGetHit = true;
            }

        }
    }
    // 수정됨: 풀 반환은 Destroy가 아니라 비활성화이므로 OnDisable에서 상태 정리
    private void OnDisable()
    {
        DetectAllEnemyAndInitialize();
    }
    private void preventDuplicateAdd(Enemy inRangeEnemy)
    {
        bool isDuplicated = false;

        if (collisionEnemies.Count == 0)
        {
            collisionEnemies.Add(inRangeEnemy);
            return;
        }

        for (int i = 0; i < collisionEnemies.Count; i++)
        {
            if (collisionEnemies[i].name == inRangeEnemy.name)
            {
                isDuplicated = true;
                break;
            }
        }
        if (!isDuplicated) collisionEnemies.Add(inRangeEnemy);
    }
    private void DetectAllEnemyAndInitialize()
    {
        foreach (Enemy enemy in collisionEnemies)
        {
            if (enemy != null)
            {
                enemy.b_isCollide = false;
            }
        }

        collisionEnemies.Clear();

        if (collisionBoss == null) return;
        collisionBoss.isParticleCollision = false;
        collisionBoss = null;
    }
}
