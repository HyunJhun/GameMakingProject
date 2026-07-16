using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// 타입 별로 오브젝트 풀을 하는 클래스
// 기본적인 풀링 기능은 이쪽에서 구현
public class ObjectPoolComponent<T> where T : Component
{
    private readonly IObjectPool<T> _pool;
    private readonly T _prefab;
    private readonly Transform _parent;
    // 풀에 있는 활성화된 오브젝트 수
    public int CountInactive => _pool.CountInactive;

    public ObjectPoolComponent(T prefab, Transform parent, int defaultSize = 10, int maxSize = 30)
    {
        _prefab = prefab;
        _parent = parent;
        _pool = new ObjectPool<T>(
        CreateItme, OnGet, OnRelease, OnDestroy, true, defaultSize, maxSize);
    }

    private T CreateItme()
    {
        T item = Object.Instantiate(_prefab, _parent);

        // 이 부분이 빠져있으면 Arrow._pool은 영원히 null!
        if (item is IPoolable<T> poolable)
            poolable.SetPool(_pool);

        Debug.Log("[Pool] 새 오브젝트 Instantiate 발생!");
        return item;
    }

    private void OnGet(T obj) => obj.gameObject.SetActive(true);
    private void OnRelease(T obj) => obj.gameObject.SetActive(false);
    private void OnDestroy(T obj) => Object.Destroy(obj.gameObject);

    public T Get()
    {
        Debug.Log($"[Pool] Get 호출 전 대기 재고: {_pool.CountInactive}");
        return _pool.Get();
    }
    public void Release(T obj)
    {
        _pool.Release(obj);
        Debug.Log($"[Pool] Release 완료. 현재 대기 재고: {_pool.CountInactive}");
    }
    

}





