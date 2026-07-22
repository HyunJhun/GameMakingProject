using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private Fireball fireballPrefab;
    private ObjectPoolComponent<Fireball> _pool;
    private void Start()
    {
        _pool = new ObjectPoolComponent<Fireball>(fireballPrefab, null);
        PoolManager.instanmce.Register(_pool);
    }

    public void OnShoot(Vector3 pos, Quaternion rot, Vector3 direction)
    {
        Fireball fireball = PoolManager.instanmce.Get<Fireball>().Get();
        fireball.transform.SetPositionAndRotation(pos, rot);
    }
}
