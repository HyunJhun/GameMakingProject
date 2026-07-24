using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; private set; }

    // 수정됨(Inspector): Player와 Boss의 ParticleManager에 동일한 Catalog를 연결
    [Header("Particle Pool")]
    [SerializeField] private ParticleCatalog catalog;

    // 수정됨(Inspector): 기존 애니메이션 이벤트 위치 계산용 참조는 그대로 유지
    [Header("Animation Event References")]
    [SerializeField] private GameObject swordHolder;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject boss;

    private static readonly ParticleType[] BasicAttackTypes =
    {
        ParticleType.PlayerBasicAttack1,
        ParticleType.PlayerBasicAttack2,
        ParticleType.PlayerBasicAttack3,
        ParticleType.PlayerTwoHandedAttack1,
        ParticleType.PlayerTwoHandedAttack2,
        ParticleType.PlayerTwoHandedAttack3
    };

    private static readonly ParticleType[] BossAttackTypes =
    {
        ParticleType.BossSpatialSection,
        ParticleType.BossRush
    };

    private readonly Dictionary<ParticleType, ParticlePool> _pools = new();
    private readonly Dictionary<ParticleType, ParticlePoolData> _poolData = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePools();
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void InitializePools()
    {
        if (catalog == null)
        {
            Debug.LogError(
                "[ParticleManager] ParticleCatalog이 연결되지 않았습니다.",
                this);
            return;
        }

        foreach (ParticlePoolData data in catalog.Entries)
        {
            if (data.Prefab == null)
            {
                Debug.LogError(
                    $"[ParticleManager] {data.Type} 프리팹이 없습니다.",
                    catalog);
                continue;
            }

            if (_pools.ContainsKey(data.Type))
            {
                Debug.LogError(
                    $"[ParticleManager] {data.Type}이 중복 등록되었습니다.",
                    catalog);
                continue;
            }

            _pools.Add(
                data.Type,
                new ParticlePool(
                    data.Prefab,
                    transform,
                    data.DefaultSize,
                    data.MaxSize));

            _poolData.Add(data.Type, data);
        }
    }

    public PooledParticle Play(
        ParticleType type,
        Vector3 position,
        Quaternion rotation)
    {
        if (!_pools.TryGetValue(type, out ParticlePool pool) ||
            !_poolData.TryGetValue(type, out ParticlePoolData data))
        {
            Debug.LogError(
                $"[ParticleManager] {type} 풀이 등록되지 않았습니다.",
                this);
            return null;
        }

        PooledParticle particle = pool.Get();
        // 수정됨: 재생 중인 월드 이펙트가 Manager 이동을 따라가지 않도록 분리
        particle.transform.SetParent(null, worldPositionStays: false);
        particle.transform.SetPositionAndRotation(position, rotation);
        // 수정됨: Particle별 Inspector 반환 정책과 지연 시간을 적용
        particle.Play(data.ReturnMode, data.ReturnDelay);
        return particle;
    }

    // 수정됨: Animation Event 호환을 유지하면서 내부 생성은 Pool로 전환
    public void BasicAttackParticleInstance(int indexOfParticle)
    {
        if (!TryGetType(BasicAttackTypes, indexOfParticle, out ParticleType type))
        {
            return;
        }

        Instance.Play(
            type,
            swordHolder.transform.position,
            player.transform.rotation);
    }

    // 수정됨: Boss Animation Event에서 기존 메서드 이름을 계속 사용
    public void BossAttackParticleInstance(int indexOfParticle)
    {
        if (!TryGetType(BossAttackTypes, indexOfParticle, out ParticleType type))
        {
            return;
        }

        Instance.Play(
            type,
            boss.transform.position,
            boss.transform.rotation);
    }

    public PooledParticle BossAttackEnterParticleInstance(int indexOfParticle)
    {
        return Instance.Play(
            ParticleType.BossMagicCircle,
            boss.transform.position,
            boss.transform.rotation);
    }

    public void SkillAttackParticleInstance(int indexOfParticle)
    {
        Instance.Play(
            ParticleType.SwordJudgment,
            swordHolder.transform.position +
            swordHolder.transform.forward * 6.5f +
            Vector3.down,
            player.transform.rotation);
    }

    public void SkillBuffParticleInstance(int indexOfParticle)
    {
        Instance.Play(
            ParticleType.Heal,
            player.transform.position,
            player.transform.rotation);
    }

    public void InstanceHitParticle(Vector3 hitPoint)
    {
        Instance.Play(
            ParticleType.PlayerHit,
            hitPoint,
            Quaternion.identity);
    }

    private static bool TryGetType(
        IReadOnlyList<ParticleType> types,
        int index,
        out ParticleType type)
    {
        if (index >= 0 && index < types.Count)
        {
            type = types[index];
            return true;
        }

        Debug.LogError(
            $"[ParticleManager] 유효하지 않은 파티클 인덱스: {index}");
        type = default;
        return false;
    }
}
