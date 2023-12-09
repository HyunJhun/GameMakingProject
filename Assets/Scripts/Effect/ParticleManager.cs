using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Particles")]
    [SerializeField] private List<ParticleSystem> oneHandedAttackParticles = new List<ParticleSystem>();

    [Header("Preference")]
    [SerializeField] private GameObject swordSlashHolder;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BasicAttackParticleInstance(int indexOfParticle)
    {
        ParticleSystem swordSlashPartice = Instantiate(oneHandedAttackParticles[indexOfParticle], swordSlashHolder.transform.position, Quaternion.identity);
        swordSlashPartice.Play();
        Destroy(swordSlashPartice, 0.3f);
        Debug.Log("Çï·Î");
    }
}
