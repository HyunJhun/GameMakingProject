using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ParticlePoolData
{
    // 수정됨(Inspector): 파티클 종류와 프리팹, 풀 크기를 카탈로그에서 설정
    [SerializeField] private ParticleType type;
    [SerializeField] private GameObject prefab;
    [SerializeField, Min(0)] private int defaultSize = 5;
    [SerializeField, Min(1)] private int maxSize = 20;
    // 수정됨(Inspector): Looping 파티클은 AfterDelay, 외부 제어 효과는 Manual로 설정
    [SerializeField] private ParticleReturnMode returnMode;
    [SerializeField, Min(0f)] private float returnDelay;

    public ParticleType Type => type;
    public GameObject Prefab => prefab;
    public int DefaultSize => defaultSize;
    public int MaxSize => maxSize;
    public ParticleReturnMode ReturnMode => returnMode;
    public float ReturnDelay => returnDelay;
}

[CreateAssetMenu(
    fileName = "ParticleCatalog",
    menuName = "Game/Particle Catalog")]
public class ParticleCatalog : ScriptableObject
{
    // 수정됨(Inspector): 모든 ParticleManager가 공유하는 파티클 풀 설정
    [SerializeField] private List<ParticlePoolData> entries = new();

    public IReadOnlyList<ParticlePoolData> Entries => entries;
}
