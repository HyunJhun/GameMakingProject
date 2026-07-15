using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class ArrowSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private Arrow arrowPrefab;
    private ObjectPoolComponent<Arrow> _pool;
    private void Start()
    {
        _pool = new ObjectPoolComponent<Arrow>(arrowPrefab, transform);
        PoolManager.instanmce.Register(_pool);
    }

    public void OnShoot(Vector3 pos, Quaternion rot, Vector3 direction)
    {
        Arrow arrow = PoolManager.instanmce.Get<Arrow>().Get();
        arrow.transform.SetPositionAndRotation(pos, rot);
        arrow.OnSpawned(direction);
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }
}
