using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Firebomb : MonoBehaviour
{
    [SerializeField] private ParticleSystem bombParticle;
    [SerializeField] private GameObject explodeArea;
    [SerializeField] private TMP_Text timerText;
    public float timer { get; set; }
    private bool collisionObstacle;

    private void Start()
    {
        timer = 7f;
        collisionObstacle = false;

    }

    private void Update()
    {
        timerText.text = ((int)timer).ToString();
        if (timer <= 5f) explodeArea.SetActive(true);
        if (collisionObstacle)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                ParticleSystem bombEffect = Instantiate(bombParticle, transform.position, Quaternion.identity);
                bombEffect.Play();
                Destroy(bombEffect.gameObject, 1f);
                Destroy(gameObject, 0.3f);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {     
        if (collision.gameObject.CompareTag("Player")) // 플레이어와 충돌했을 경우
        {
            ParticleSystem bombEffect = Instantiate(bombParticle, transform.position, Quaternion.identity);
            bombEffect.Play();
            Destroy(bombEffect.gameObject, 1f);
            Destroy(gameObject, 0.3f);
        }
        else
        {
            collisionObstacle = true;
        }
    }
}
