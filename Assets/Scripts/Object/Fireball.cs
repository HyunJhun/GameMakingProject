using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private ParticleSystem bombParticle;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Obstacle")) // 만약 지형지물 충돌했을 경우
        {
            ParticleSystem bombEffect = Instantiate(bombParticle, transform.position, Quaternion.identity);
            bombEffect.Play();
            Destroy(bombEffect.gameObject, 1f);
            Destroy(gameObject, 0.3f);
        }
        else if(collision.gameObject.CompareTag("Player")) // 플레이어와 충돌했을 경우
        {
            ParticleSystem bombEffect = Instantiate(bombParticle, transform.position, Quaternion.identity);
            bombEffect.Play();
            Destroy(bombEffect.gameObject, 1f);
            Destroy(gameObject, 0.3f);
        }
    }
}
