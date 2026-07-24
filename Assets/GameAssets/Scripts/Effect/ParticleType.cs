// 수정됨: 파티클 목록의 숫자 인덱스 대신 사용하는 식별자
public enum ParticleType
{
    PlayerBasicAttack1 = 0,
    PlayerBasicAttack2 = 1,
    PlayerBasicAttack3 = 2,
    PlayerTwoHandedAttack1 = 3,
    PlayerTwoHandedAttack2 = 4,
    PlayerTwoHandedAttack3 = 5,
    SwordJudgment = 6,
    Heal = 7,
    PlayerHit = 8,
    BossSpatialSection = 9,
    BossRush = 10,
    BossMagicCircle = 11,
    FireballExplosion = 12,
    FirebombExplosion = 13
}

// 수정됨: Looping 파티클도 안전하게 반환할 수 있도록 반환 정책을 분리
public enum ParticleReturnMode
{
    WhenFinished = 0,
    AfterDelay = 1,
    Manual = 2
}
