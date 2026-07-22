using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebombSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private Firebomb firebombPrefab;
    private ObjectPoolComponent<Firebomb> _pool;
    private void Start()
    {
        _pool = new ObjectPoolComponent<Firebomb>(firebombPrefab, null);
        PoolManager.instanmce.Register(_pool);
    }

    public void OnShoot(Vector3 pos, Quaternion rot, Vector3 direction)
    {
        Firebomb firebomb = PoolManager.instanmce.Get<Firebomb>().Get();
        firebomb.transform.SetPositionAndRotation(pos, rot);
    }
}
