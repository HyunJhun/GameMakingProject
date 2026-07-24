using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class PooledParticle : MonoBehaviour, IPoolable<PooledParticle>
{
    private IObjectPool<PooledParticle> _pool;
    private ParticleSystem[] _particleSystems;
    private Vector3 _initialLocalScale;
    private Coroutine _delayedReturn;
    private bool _isInUse;
    private bool _autoReturn;

    private void Awake()
    {
        // 수정됨: 루트와 자식에 있는 모든 ParticleSystem을 함께 관리
        _particleSystems =
            GetComponentsInChildren<ParticleSystem>(includeInactive: true);

        _initialLocalScale = transform.localScale;
    }

    private void Update()
    {
        if (!_isInUse || !_autoReturn)
        {
            return;
        }

        foreach (ParticleSystem particleSystem in _particleSystems)
        {
            if (particleSystem.IsAlive(withChildren: false))
            {
                return;
            }
        }

        ReturnToPool();
    }

    public void SetPool(IObjectPool<PooledParticle> pool)
    {
        _pool = pool;
    }

    public void Play(
        ParticleReturnMode returnMode,
        float returnDelay)
    {
        ResetParticleSystems();

        // 수정됨: Looping 여부와 무관하게 Catalog의 반환 정책을 따른다.
        _autoReturn = returnMode == ParticleReturnMode.WhenFinished;
        _isInUse = true;

        foreach (ParticleSystem particleSystem in _particleSystems)
        {
            particleSystem.Play(withChildren: false);
        }

        if (returnMode == ParticleReturnMode.AfterDelay)
        {
            ReturnToPoolAfter(returnDelay);
        }
    }

    public void ReturnToPool()
    {
        if (!_isInUse)
        {
            return;
        }

        _isInUse = false;
        _pool?.Release(this);
    }

    public void ReturnToPoolAfter(float delay)
    {
        if (_delayedReturn != null)
        {
            StopCoroutine(_delayedReturn);
        }

        // 수정됨: Destroy(obj, delay)를 대체하는 수동 지연 반환
        _delayedReturn = StartCoroutine(ReturnAfterDelay(delay));
    }

    public void PrepareForRelease()
    {
        if (_delayedReturn != null)
        {
            StopCoroutine(_delayedReturn);
            _delayedReturn = null;
        }

        _isInUse = false;
        _autoReturn = false;
        ResetParticleSystems();
    }

    private IEnumerator ReturnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _delayedReturn = null;
        ReturnToPool();
    }

    private void ResetParticleSystems()
    {
        transform.localScale = _initialLocalScale;

        foreach (ParticleSystem particleSystem in _particleSystems)
        {
            particleSystem.Stop(
                withChildren: false,
                stopBehavior:
                    ParticleSystemStopBehavior.StopEmittingAndClear);

            particleSystem.Clear(withChildren: false);
        }
    }
}
