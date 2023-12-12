using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Particles")]
    [SerializeField] private List<GameObject> basicAttackParticles = new List<GameObject>();
    [SerializeField] private List<GameObject> bossAttackParticles = new List<GameObject>();
    [Header("Preference")]
    [SerializeField] private GameObject swordHolder;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject boss;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void BasicAttackParticleInstance(int indexOfParticle)
    {
        GameObject swordSlashPartice = Instantiate(basicAttackParticles[indexOfParticle],
            swordHolder.transform.position,
            player.transform.rotation);
        
        swordSlashPartice.GetComponentInChildren<ParticleSystem>().Play();
        Destroy(swordSlashPartice.gameObject, 0.3f);
        Debug.Log("Çï·Î");
    }

    public void BossAttackParticleInstance(int indexOfParticle)
    {
        GameObject bossAttackPartice = Instantiate(bossAttackParticles[indexOfParticle],
            boss.transform.position,
            boss.transform.rotation);

        bossAttackPartice.GetComponent<ParticleSystem>().Play();
        Destroy(bossAttackPartice.gameObject, 0.3f);
    }
}
