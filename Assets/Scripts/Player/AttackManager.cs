using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public enum Weapon
    {
        OneHanded,
        TwoHanded
    }
    [Header("References")]
    AnimationManager playerAnimationManager;
    public GameObject attackArrange;

    [Header("Values")]
    public float cooldownTime = 2f;
    private float nextFireTime = 0.3f;
    public static int noOfClicks = 0;
    float lastClickedTime = 0f;
    float maxComboDelay = 1f;
    bool isAttack;
    Weapon currentWeapon;
    private void Start()
    {
        playerAnimationManager = GetComponent<AnimationManager>();
        isAttack = false;
        currentWeapon = Weapon.OneHanded;
    }

    private void Update()
    {
        if (currentWeapon == Weapon.OneHanded)
        {// normalizedTime 은 0~1까지의 값을 가지고 있고, 1에 가까울 수록 애니메이션 클립이 재생이 완료되고있음.
         // isName("클립명") 은 이 애니메이션에 우선 진입했는지를 반환해주는 함수
            if (playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime < 0.7f
                && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).IsName("RightHand@Attack01"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit1", false);
            }
            if (playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime < 0.7f
                && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).IsName("RightHand@Attack02"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit2", false);
            }
            if (playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime < 0.7f
                && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).IsName("RightHand@Attack03"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit3", false);
                noOfClicks = 0;
            }
        }
        else if(currentWeapon == Weapon.TwoHanded)
        {
            if (playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime < 0.7f
                && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).IsName("RightHand@Attack01"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit1", false);
            }
            if (playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime < 0.7f
                && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).IsName("RightHand@Attack02"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit2", false);
            }
            if (playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime < 0.7f
                && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).IsName("RightHand@Attack03"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit3", false);
                noOfClicks = 0;
            }
        }
        if(Time.time - lastClickedTime > maxComboDelay) // 만약 maxComboDelay 시간동안 클릭이 없으면 콤보를 초기화
        {
            noOfClicks = 0;
        }
        if(Time.time > nextFireTime)
        {
            if(Input.GetMouseButtonDown(0))
            {
                OnClick();
            }
        }

        Debug.Log( "클릭수는 " + noOfClicks);
      
    }
    void OnClick()
    {
        lastClickedTime = Time.time;
        noOfClicks++;
        if(noOfClicks == 1)
        {
            playerAnimationManager.getPlayerAnimator().SetBool("hit1", true);
            Debug.Log("공격 1트");
        }
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);
        Debug.Log("반환된 클릭 값 : " + noOfClicks);
        if(noOfClicks >=2 && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f
            && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).IsName("RightHand@Attack01"))
        {
            playerAnimationManager.getPlayerAnimator().SetBool("hit1", false);
            playerAnimationManager.getPlayerAnimator().SetBool("hit2", true);
            Debug.Log("공격 2트");
        }
        if (noOfClicks >= 3 && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f
           && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).IsName("RightHand@Attack02"))
        {
            playerAnimationManager.getPlayerAnimator().SetBool("hit2", false);
            playerAnimationManager.getPlayerAnimator().SetBool("hit3", true);
            Debug.Log("공격 3트");
        }
    }
    public void attack()
    {
        isAttack = true;
        Invoke("attackOut",1f);
    }

    private void attackOut()
    {
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
