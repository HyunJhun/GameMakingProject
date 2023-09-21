using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [Header("References")]
    AnimationManager playerAnimationManager;
    public GameObject attackArrange;
    bool isAttack;
    private void Start()
    {
        playerAnimationManager = GetComponent<AnimationManager>();
        isAttack = false;
    }

    public void attack()
    {
        Debug.Log("공격 여부의 불값은 : " + isAttack);
        isAttack = true;
        Debug.Log("공격 시작");
        Invoke("attackOut",1f);
    }

    private void attackOut()
    {
        Debug.Log("공격 중지의 불값은 : " + isAttack);
        Debug.Log("공격 중지");
        isAttack = false;
        playerAnimationManager.getPlayerAnimator().SetBool("isAttack", isAttack);
    }

    public void ToDamage()
    {
        return;
    }

    public void TakeDamage()
    {
        return;
    }

    // get function
    public bool getIsAttack()
    {
        return isAttack;
    }

}
