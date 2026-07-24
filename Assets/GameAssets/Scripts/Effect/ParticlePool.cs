using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 기존 GameObject 파티클 프리팹을 수정하지 않고 런타임에 풀링 래퍼를 붙인다.
/// </summary>
public sealed class ParticlePool
{
    private readonly GameObject _prefab;
    private readonly Transform _parent;
    private readonly IObjectPool<PooledParticle> _pool;

    public ParticlePool(
        GameObject prefab,
        Transform parent,
        int defaultSize,
        int maxSize)
    {
        _prefab = prefab;
        _parent = parent;

        _pool = new ObjectPool<PooledParticle>(
            CreateItem,
            OnGet,
            OnRelease,
            OnDestroy,
            collectionCheck: true,
            defaultCapacity: defaultSize,
            maxSize: maxSize);
    }

    public PooledParticle Get()
    {
        return _pool.Get();
    }

    private PooledParticle CreateItem()
    {
        GameObject instance = Object.Instantiate(_prefab, _parent);

        // 수정됨: 기존 파티클 프리팹에도 런타임에 풀링 기능을 자동 적용
        PooledParticle pooledParticle =
            instance.GetComponent<PooledParticle>();

        if (pooledParticle == null)
        {
            pooledParticle = instance.AddComponent<PooledParticle>();
        }

        pooledParticle.SetPool(_pool);
        return pooledParticle;
    }

    private static void OnGet(PooledParticle particle)
    {
        particle.gameObject.SetActive(true);
    }

    private void OnRelease(PooledParticle particle)
    {
        particle.PrepareForRelease();
        particle.transform.SetParent(_parent, worldPositionStays: false);
        particle.gameObject.SetActive(false);
    }

    private static void OnDestroy(PooledParticle particle)
    {
        Object.Destroy(particle.gameObject);
    }
}
