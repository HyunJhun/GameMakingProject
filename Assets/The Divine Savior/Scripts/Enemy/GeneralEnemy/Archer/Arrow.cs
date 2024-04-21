using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("Arrow")]
    [SerializeField] private float arrowAttackDamage = 10f;
    [SerializeField] private AttackRangeCheck attackRangeCheck;
    [SerializeField] Vector3 direction;
    void Start()
    {
        attackRangeCheck = GetComponent<AttackRangeCheck>();
        attackRangeCheck.SetType(1);
        Vector3 playerPos = GameObject.Find("Player").GetComponent<Player>().transform.position + new Vector3(0f, 1f, 0f);
        direction = (playerPos - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * 0.15f;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (!other.GetComponent<Player>().b_IsDodege)
            {
                other.GetComponent<Player>().b_IsHit = true;
                attackRangeCheck.GetComponent<AttackRangeCheck>().getStats().hpDown(arrowAttackDamage - other.GetComponent<Status>().GetArmor());
            }
            Destroy(gameObject, 0.3f);
        }
        else if(other.CompareTag("Obstacle") || other.CompareTag("Ground"))
        {
            Destroy(gameObject, 0.1f);
        }
    }
}
