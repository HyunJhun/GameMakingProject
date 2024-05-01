using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private ParticleSystem bombParticle;
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
            ParticleSystem bombEffect = Instantiate(bombParticle, transform.position, Quaternion.identity);
            bombEffect.Play();
            Destroy(bombEffect.gameObject, 1f);
            fractured.BreakFracturedObject();
        }

    }
}
