using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Firebomb : MonoBehaviour
{
    [SerializeField] private ParticleSystem bombParticle;
    [SerializeField] private GameObject explodeArea;
    [SerializeField] private TMP_Text timerText;

    private Destructible fractured;
    public float timer { get; set; }
    private bool collisionObstacle;
    private bool isBoom;

    private void Start()
    {
        timer = 7f;
        collisionObstacle = false;
        isBoom = false;
        fractured = GetComponent<Destructible>();
    }

    private void Update()
    {
        timerText.text = ((int)timer).ToString();
        if (collisionObstacle)
        {
            timer -= Time.deltaTime;
            if (timer <= 0.1f) explodeArea.SetActive(true);
            if (timer <= 0f&& !isBoom)
            {
                isBoom = true;
                ParticleSystem bombEffect = Instantiate(bombParticle, transform.position, Quaternion.identity);
                bombEffect.Play();
                Destroy(bombEffect.gameObject, 1f);
                fractured.BreakFracturedObject();
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {     
        if (collision.gameObject.CompareTag("Player")) // 플레이어와 충돌했을 경우
        {
            //SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Boss.Bomb, false);
            ParticleSystem bombEffect = Instantiate(bombParticle, transform.position, Quaternion.identity);
            bombEffect.Play();
            Destroy(bombEffect.gameObject, 1f);
            Destroy(gameObject, 0.1f);
        }
        else
        {
            collisionObstacle = true;
        }
    }
}
