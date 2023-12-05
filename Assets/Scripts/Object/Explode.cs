using UnityEngine;

public class Explode : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Firebomb fireBomb;
    private Boss boss;
    private float explodeDamage = 13f;

    private void Awake()
    {
        boss = GameObject.FindWithTag("Enemy").GetComponent<Boss>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!boss.player.GetComponent<PlayerMovementHandler>().isDamaged)
            {
                if (!boss.CheckPlayerDodge())
                {
                    boss.DamagingToPlayer(boss.transform, explodeDamage);
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (fireBomb.timer <= 0.3f)
            {
                if (!boss.player.GetComponent<PlayerMovementHandler>().isDamaged)
                {
                    if (!boss.CheckPlayerDodge())
                    {
                        boss.DamagingToPlayer(boss.transform, explodeDamage);
                    }
                }
            }
        }
    }
}
