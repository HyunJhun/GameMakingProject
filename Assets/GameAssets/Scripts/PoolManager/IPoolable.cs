using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface IPoolable<T> where T : Component
{
    void SetPool(IObjectPool<T> pool);
}
