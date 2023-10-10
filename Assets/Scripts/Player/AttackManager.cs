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
    private Status inRangeObjStats;
    public List<GameObject> weapons; // 0 = sheld, 1 = shortSword, 2 = twoHandeld
    public PlayerMovementHandler player;

    [Header("Values")]
    private float cooldownTime = 1f;
    private int countClick = 0;
    private float lastClickedTime = 0f;
    private float maxComboDelay = 1f;
    private bool isAttack = false;
    float timer = 0f;
    public Weapon currentWeapon;
    private void Start()
    {
        playerAnimationManager = GetComponent<AnimationManager>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        
        if (currentWeapon == Weapon.OneHanded)
        {   // normalizedTime 은 0~1까지의 값을 가지고 있고, 1에 가까울 수록 애니메이션 클립이 재생이 완료되고있음.
            // isName("클립명") 은 이 애니메이션에 우선 진입했는지를 반환해주는 함수
            if (playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f
                && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).IsName("RightHand@Attack01"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit1", false);
            }
            if (playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f
                && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).IsName("RightHand@Attack02"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit2", false);
            }
            if (playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f
                && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).IsName("RightHand@Attack03"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit3", false);
                initTimer();
                setIsAttack(false);
                countClick = 0;
            }
        }
        else if(currentWeapon == Weapon.TwoHanded)
        {
            if (playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f
                && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(2).IsName("2H@Attack01"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit1", false);
            }
            if (playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f
                && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(2).IsName("2H@Attack02"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit2", false);
            }
            if (playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f
                && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(2).IsName("2H@Attack04"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit3", false);
                initTimer();
                setIsAttack(false);
                countClick = 0;
            }
        }

        if(Time.time - lastClickedTime > maxComboDelay) // 만약 maxComboDelay 시간동안 클릭이 없으면 콤보를 초기화
        {
            countClick = 0;
            playerAnimationManager.getPlayerAnimator().SetBool("hit1", false);
            playerAnimationManager.getPlayerAnimator().SetBool("hit2", false);
            playerAnimationManager.getPlayerAnimator().SetBool("hit3", false);
            setIsAttack(false);
        }
    }
    private void initTimer()
    {
        timer = 0f; 
    }
    void OnAttack()
    { 
        lastClickedTime = Time.time;
        countClick++;
        setIsAttack(true);

        if (countClick == 1)
        {
            playerAnimationManager.getPlayerAnimator().SetBool("hit1", true);
        }

        if (currentWeapon == Weapon.OneHanded)
        {
            if (countClick >= 2 && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f 
            && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).IsName("RightHand@Attack01"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit1", false);
                playerAnimationManager.getPlayerAnimator().SetBool("hit2", true);
            }
            if (countClick >= 3 && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f
               && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).IsName("RightHand@Attack02"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit2", false);
                playerAnimationManager.getPlayerAnimator().SetBool("hit3", true);
            }
        }
        else if(currentWeapon == Weapon.TwoHanded)
        {
            if (countClick >= 2 && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f 
            && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(2).IsName("2H@Attack01"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit1", false);
                playerAnimationManager.getPlayerAnimator().SetBool("hit2", true);
            }
            if (countClick >= 3 && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f
               && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(2).IsName("2H@Attack02"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit2", false);
                playerAnimationManager.getPlayerAnimator().SetBool("hit3", true);
            }
        }
    }

    public void attack()
    {
        
        if (timer > cooldownTime)
        {
            if (Input.GetButtonDown("Attack"))
            {
                OnAttack();
            }
        }
        return;
    }


    public void ToDamage()
    {
        if (attackArrange.activeSelf == false)
        {
            attackArrange.SetActive(true);
        }
        inRangeObjStats = attackArrange.GetComponent<AttackRangeCheck>().getStats();

        if (inRangeObjStats != null)
        {
            inRangeObjStats.hpDown(inRangeObjStats.getDmg());

            attackArrange.SetActive(false);
        }


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

    // set

    public void setIsAttack(bool isAttack)
    {
        this.isAttack = isAttack;
    }
}
