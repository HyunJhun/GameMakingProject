using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackRangeCheck : MonoBehaviour
{
    private Status triggerObjStatus;
    private Boss bossTriggered;
    private int maxAttackEnemyCount = 3;
    [SerializeField] private List<Enemy> enemyTriggeredList = new List<Enemy>();

    private enum Type { Player,Monster}
    private Type type;

    private void Update()
    {
        // 죽은 적은 리스트에서 삭제
        foreach(Enemy monster in enemyTriggeredList)
        {
            if (monster == null)
            {
                enemyTriggeredList.Remove(monster);
                break;
            }
            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other == null) { return; }// 컬리젼 되는게 있다면
        
        switch(type)
        {
            case Type.Player:
                if (other.CompareTag("Enemy"))
                {
                    bossTriggered = other.GetComponent<Boss>();
                    preventDuplicateAdd(other.GetComponent<Enemy>());
                }
                break;
            case Type.Monster:
                if (other.CompareTag("Player"))
                {
                    triggerObjStatus = other.GetComponent<Status>();
                }
                break;

            default:
                triggerObjStatus = null;
                bossTriggered = null;
                break;
        }


    }

    // 몬스터 리스트를 Trigger를 통해 추가할 때, 동일한 몬스터가 추가되는 것을 방지.
    private void preventDuplicateAdd(Enemy inRangeEnemy) 
    {
        bool isDuplicated = false;
        if (enemyTriggeredList.Count == 0)
        {
            enemyTriggeredList.Add(inRangeEnemy);
            return;
        }
        for (int i = 0; i < enemyTriggeredList.Count; i++)
        {
            if (enemyTriggeredList[i].name == inRangeEnemy.name)
            {
                isDuplicated = true;
                break;
            }

        }
        if (!isDuplicated)
        {
            enemyTriggeredList.Add(inRangeEnemy);
        }
    }
    public void selectEnemyByMaxAttackCount()
    {
        List<float> distanceList = new List<float>();
        Dictionary<float, Enemy> enemyDictionary = new Dictionary<float, Enemy>();
        Transform playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        foreach(Enemy enemy in enemyTriggeredList)
        {
            float distance = Vector3.Distance(playerTransform.position, enemy.transform.position);
            distanceList.Add(distance);
            enemyDictionary.Add(distance, enemy);
        }

        distanceList.Sort();
        enemyTriggeredList.Clear();
        for(int i = 0; i < maxAttackEnemyCount; i++)
        {
            enemyTriggeredList.Add(enemyDictionary[distanceList[i]]);
        }

    }
    private void OnTriggerExit(Collider other)
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

    public List<Enemy> GetTriggeredEnemyList()
    {
        if (enemyTriggeredList == null) return null;

        return enemyTriggeredList;
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
