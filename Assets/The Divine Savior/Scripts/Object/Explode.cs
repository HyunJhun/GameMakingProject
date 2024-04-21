using UnityEngine;

public class Explode : MonoBehaviour
{
    // Start is called before the first frame update
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
            if (!boss.player.GetComponent<Player>().b_IsHit)
            {
                if (!boss.CheckPlayerDodge())
                {
                    Debug.Log($"Æø¹ß µ¥¹ÌÁö : {explodeDamage}");
                    boss.DamagingToPlayer(boss.transform, explodeDamage);
                }
            }
        }
    }
}
