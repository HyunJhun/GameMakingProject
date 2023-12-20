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

        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Player")) // 만약 지형지물 충돌했을 경우
        {
            explodeArea.SetActive(true);
            ParticleSystem bombEffect = Instantiate(bombParticle, transform.position, Quaternion.identity);
            bombEffect.Play();
            Destroy(bombEffect.gameObject, 1f);
            fractured.BreakFracturedObject();
        }

    }
}
