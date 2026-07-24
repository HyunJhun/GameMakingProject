using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private GameObject explodeArea;

    private Destructible fractured;

    private void Start()
    {
        fractured = GetComponent<Destructible>();
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Ground")) // 만약 지형지물 충돌했을 경우
        {
            explodeArea.SetActive(true);
            SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Boss.Bomb, false);
            // 수정됨: 개별 Spawner 대신 중앙 ParticleManager의 타입별 풀을 사용
            ParticleManager.Instance.Play(
                ParticleType.FireballExplosion,
                transform.position,
                Quaternion.identity);
            fractured.BreakFracturedObject();
        }

    }
}
