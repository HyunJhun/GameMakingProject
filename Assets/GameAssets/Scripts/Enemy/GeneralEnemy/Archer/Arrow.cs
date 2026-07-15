using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;

public class Arrow : MonoBehaviour, IPoolable<Arrow>
{
    [Header("Arrow")]
    [SerializeField] private float arrowAttackDamage = 10f;
    [SerializeField] private AttackRangeCheck attackRangeCheck;
    [SerializeField] Vector3 _direction;

    // Pooling
    private IObjectPool<Arrow> _pool;
    public void SetPool(IObjectPool<Arrow> pool)
    {
        _pool = pool;
    }

    public void OnSpawned(Vector3 dir)
    {
        _direction = dir;

        attackRangeCheck = GetComponent<AttackRangeCheck>();

        if(attackRangeCheck == null)
        {
            Debug.LogError("AttackRangeCheck component is missing on the Arrow object.");
            return;
        }

        attackRangeCheck.SetType(1);
        //Vector3 playerPos = GameObject.Find("Player").GetComponent<Player>().transform.position + new Vector3(0f, 1f, 0f);
        //direction = (playerPos - transform.position).normalized;
    }

    public void ReturnToPool() => _pool.Release(this);

    //void Start()
    //{
    //    attackRangeCheck = GetComponent<AttackRangeCheck>();
    //    attackRangeCheck.SetType(1);
    //    Vector3 playerPos = GameObject.Find("Player").GetComponent<Player>().transform.position + new Vector3(0f, 1f, 0f);
    //    direction = (playerPos - transform.position).normalized;
    //}

    // Update is called once per frame
    void Update()
    {
        transform.position += _direction * 1f * Time.deltaTime;
    }

    private async UniTaskVoid ReturnToPoolDelayed(float delayTime,CancellationToken tk = default)
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(delayTime));
        ReturnToPool();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (!other.GetComponent<Player>().b_IsDodge)
            {
                other.GetComponent<Player>().b_IsHit = true;
                attackRangeCheck.GetComponent<AttackRangeCheck>().getStats().hpDown(arrowAttackDamage - other.GetComponent<Status>().GetArmor());
            }
            ReturnToPoolDelayed(0.1f).Forget();
        }
        else if(other.CompareTag("Obstacle") || other.CompareTag("Ground"))
        {
            Destroy(gameObject, 0.1f);
        }
    }
}
