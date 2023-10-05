using UnityEngine;

public class AttackRangeCheck : MonoBehaviour
{
    private Status triggerObjStatus;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other != null) // 컬리젼 되는게 있다면
        {
            if (other.CompareTag("Enemy"))
            {
                triggerObjStatus = other.GetComponent<Status>();
            }
        }
        else
            Debug.Log("아무 정보도 없음");
    }

    // Get
    public Status getStats()
    {
        return triggerObjStatus;
    }
}
