using UnityEngine;

/// <summary>
/// 타입별 풀 생성, 등록, 기본 Spawn 처리를 담당하는 공통 Spawner.
/// </summary>
public abstract class PoolSpawner<T> : MonoBehaviour where T : Component
{
    protected ObjectPoolComponent<T> Pool { get; private set; }

    protected abstract T Prefab { get; }

    protected virtual Transform PoolParent => null;

    protected virtual void Start()
    {
        Pool = new ObjectPoolComponent<T>(Prefab, PoolParent);
        PoolManager.instanmce.Register(Pool);
    }

    protected T Spawn(Vector3 position, Quaternion rotation)
    {
        T instance = Pool.Get();
        instance.transform.SetPositionAndRotation(position, rotation);
        return instance;
    }
}
