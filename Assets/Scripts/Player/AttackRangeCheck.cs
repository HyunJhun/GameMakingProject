using UnityEngine;

public class AttackRangeCheck : MonoBehaviour
{
    private Status triggerObjStatus;
    private void OnTriggerEnter(Collider other)
    {
        if (other != null) // 컬리젼 되는게 있다면
        {
            if (other.CompareTag("Enemy"))
            {
                triggerObjStatus = other.GetComponent<Status>();
                Debug.Log("name is : " + triggerObjStatus.name);
            }
            else
            {
                triggerObjStatus = null;
            }
        }
    }
    // Get
    public Status getStats()
    {
        if (triggerObjStatus != null)
            return triggerObjStatus;
        else
            return null;
    }
}
