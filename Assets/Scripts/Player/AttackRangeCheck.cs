using UnityEngine;

public class AttackRangeCheck : MonoBehaviour
{
    private Status triggerObjStatus;
    private Boss bossTriggered;
    private Enemy enemyTriggered;
    private enum Type { Player,Monster}
    private Type type;
    private void OnTriggerEnter(Collider other)
    {
        if (other == null) { return; }// 컬리젼 되는게 있다면
        
        switch(type)
        {
            case Type.Player:
                if (other.CompareTag("Enemy"))
                {
                    triggerObjStatus = other.GetComponent<Status>();
                    bossTriggered = other.GetComponent<Boss>();
                    enemyTriggered = other.GetComponent<Enemy>();
                    Debug.Log("name is : " + triggerObjStatus.name);
                }
                break;
            case Type.Monster:
                if (other.CompareTag("Player"))
                {
                    triggerObjStatus = other.GetComponent<Status>();
                    Debug.Log("name is : " + triggerObjStatus.name);
                }
                break;

            default:
                triggerObjStatus = null;
                bossTriggered = null;
                enemyTriggered = null;
                break;
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

    public Enemy GetEnemy()
    {
        if (enemyTriggered == null) return null;

        return enemyTriggered;
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
