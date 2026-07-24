using UnityEngine;

public class ArrowSpawner : PoolSpawner<Arrow>
{
    [Header("Prefab")]
    [SerializeField] private Arrow arrowPrefab;

    protected override Arrow Prefab => arrowPrefab;

    public void OnShoot(Vector3 pos, Quaternion rot, Vector3 direction)
    {
        Arrow arrow = Spawn(pos, rot);
        arrow.OnSpawned(direction);
    }
}
