using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Pooling 시스템에 등록하기 위한 큰 틀
public class PoolManager : MonoBehaviour
{
    public static PoolManager instanmce { get;private set; }

    // System.Type으로 파싱한 이유는 string으로 파싱 시 오타등의 단순한 실수로 인해
    // 자료를 못찾거나 런타임 에러가 발생할 수도 있기 때문이다.
    // 그에 비해 Type은 언제 어디서든 항상 같은 값을 반환하기 때문에 안정적이므로
    // Type을 key로 사용한다. 또한, object 를 value로 사용한 이유는 매니징 단계에선
    // T 라는 제네릭 클래스를 사용할 수가 없기 때문에 여러 타입을 관리하기 위한
    // 수단으로 object를 사용하였다.
    private readonly Dictionary<System.Type, object> _pools = new();

    private void Awake()
    {
        if (instanmce == null)
        {
            instanmce = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Register<T>(ObjectPoolComponent<T> pool) where T : Component
    {
        _pools[typeof(T)] = pool;
    }

    // 위에 주석에 따라 object타입으로 받아온 값을 다시 넘겨줘야 할 떄에는 
    // 다시 타입 캐스팅을 통해 ObjectPoolComponent<T>로 변환하여 반환한다.
    public ObjectPoolComponent<T> Get<T>() where T : Component
    {
        if (_pools.TryGetValue(typeof(T), out var pool))
        {
            return pool as ObjectPoolComponent<T>;
        }
        else
        {
            Debug.LogError($"Pool for type {typeof(T)} not found.");
            return null;
        }
    }
}
