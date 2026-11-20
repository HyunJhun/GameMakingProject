using UnityEngine;

public class Explode : MonoBehaviour
{
    // Start is called before the first frame update
    private Boss boss;
    private float explodeDamage = 13f;

    private void Awake()
    {
        // 보스태그변경부분
        boss = GameObject.FindWithTag("Boss").GetComponent<Boss>();
    }
    private void Start()
    {
        SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Boss.Bomb, false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!boss.player.GetComponent<Player>().b_IsHit)
            {
                if (!boss.CheckPlayerDodge())
                {
                    Debug.Log($"폭발 데미지 : {explodeDamage}");
                    boss.DamagingToPlayer(boss.transform, explodeDamage);
                }
            }
        }
    }
}
