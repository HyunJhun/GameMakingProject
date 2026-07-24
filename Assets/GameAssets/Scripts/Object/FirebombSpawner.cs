using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebombSpawner : PoolSpawner<Firebomb>
{
    [Header("Prefab")]
    [SerializeField] private Firebomb firebombPrefab;

    protected override Firebomb Prefab => firebombPrefab;

    public void OnShoot(Vector3 pos, Quaternion rot, Vector3 direction)
    {
        Spawn(pos, rot);
    }
}
