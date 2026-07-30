using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();

    public event Action OnAllEnemiesDied;

    private readonly HashSet<Enemy> aliveEnemies = new HashSet<Enemy>();
    private bool hasNotifiedAllEnemiesDied;

    public int AliveEnemyCount => aliveEnemies.Count;

    private void Awake()
    {
        if (enemies.Count == 0)
            enemies.AddRange(FindObjectsOfType<Enemy>());

        foreach (Enemy enemy in enemies)
            Register(enemy);
    }

    private void Start()
    {
        CheckAllEnemiesDied();
    }

    private void OnDestroy()
    {
        foreach (Enemy enemy in aliveEnemies)
        {
            if (enemy != null)
                enemy.OnDied -= HandleEnemyDied;
        }
    }

    public void Register(Enemy enemy)
    {
        if (enemy == null || enemy.b_isDie || !aliveEnemies.Add(enemy))
            return;

        enemy.OnDied += HandleEnemyDied;
        hasNotifiedAllEnemiesDied = false;
    }

    private void HandleEnemyDied(Enemy enemy)
    {
        if (enemy == null || !aliveEnemies.Remove(enemy))
            return;

        enemy.OnDied -= HandleEnemyDied;
        CheckAllEnemiesDied();
    }

    private void CheckAllEnemiesDied()
    {
        if (hasNotifiedAllEnemiesDied || aliveEnemies.Count > 0)
            return;

        hasNotifiedAllEnemiesDied = true;
        OnAllEnemiesDied?.Invoke();
    }
}
