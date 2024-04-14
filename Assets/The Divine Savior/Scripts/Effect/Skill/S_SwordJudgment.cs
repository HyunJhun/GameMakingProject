using System.Collections.Generic;
using UnityEngine;

public class S_SwordJudgment : MonoBehaviour
{
    private bool isCollide;
    [SerializeField]private List<Enemy> collisionEnemies = new List<Enemy>();
    private Boss collisionBoss;
    // Start is called before the first frame update
    void Start()
    {
        isCollide = false;
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
    private void OnDestroy()
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
            enemy.b_isCollide = false;
        }
        if (collisionBoss == null) return;
        collisionBoss.isParticleCollision = false;
    }
}
