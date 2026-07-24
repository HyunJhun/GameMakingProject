using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSpawner : PoolSpawner<Fireball>
{
    [Header("Prefab")]
    [SerializeField] private Fireball fireballPrefab;

    protected override Fireball Prefab => fireballPrefab;

    public void OnShoot(Vector3 pos, Quaternion rot, Vector3 direction)
    {
        Spawn(pos, rot);
    }
}
