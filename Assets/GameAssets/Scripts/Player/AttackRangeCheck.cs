using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackRangeCheck : MonoBehaviour
{
    [SerializeField]private Status triggerObjStatus;
    [SerializeField]private Boss bossTriggered;
    private int maxAttackEnemyCount = 3;
    
    private Transform playerTransform;
    private HashSet<Enemy> enemies = new HashSet<Enemy>();
    private enum Type { Player,Monster}
    private Type type;


    private void OnEnable()
    {
        enemies.Clear();
    }
    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }
    private void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other == null) { return; }// 컬리젼 되는게 없다면
        
        switch(type)
        {
            case Type.Player:
                if (other.CompareTag("Enemy"))
                {
                    if(other.GetComponent<Boss>() != null) 
                        bossTriggered = other.GetComponent<Boss>();
                    if (other.GetComponent<Enemy>() != null)
                    {
                        enemies.Add(other.GetComponent<Enemy>());
                        //preventDuplicateAdd(other.GetComponent<Enemy>());
                    }
                }
                break;
            case Type.Monster:
                if (other.CompareTag("Player"))
                {
                    triggerObjStatus = other.GetComponent<Status>();
                }
                break;
        }


    }
    private void OnTriggerExit(Collider other)
    {
        if (bossTriggered == null) return;

        bossTriggered = null;
    }

    // 몬스터 리스트를 Trigger를 통해 추가할 때, 동일한 몬스터가 추가되는 것을 방지.
   
    public List<Enemy> SelectNearEnemies()
    {
        List<Enemy> sorted = new List<Enemy>();

        sorted = enemies.Where(e => e != null && !e.b_isDie)
            .OrderBy(e => Vector3.Distance(playerTransform.position, e.transform.position))
            .Take(maxAttackEnemyCount)
            .ToList();

        return sorted;

    }  

    public void ResetBossTriggered()
    {
        if (bossTriggered != null)
            bossTriggered = null;
    }
    public void ResetTriggerObj()
    {
        if (triggerObjStatus == null) return;
        triggerObjStatus = null;
    }
    // Get
    public Status getStats()
    {
        if (triggerObjStatus != null)
            return triggerObjStatus;
        else
            return null;
    }


    public Boss GetBoss()
    {
        if (bossTriggered == null) return null;

        return bossTriggered;
    }

    
    public void SetType(int numOfType)
    {
        switch(numOfType)
        {
            case 0:
                type = Type.Player;
                break;
            case 1:
                type = Type.Monster;
                break;
        }
    }
}
