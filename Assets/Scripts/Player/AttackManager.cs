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
    private int countClick = 0;
    private float lastClickedTime = 0f;
    private float maxComboDelay = 1f;
    public bool isAttack { get; set; } = false;
    public bool isEnterToAttack { get; set; } = false;
    public bool isAttackStart { get; set; } = false;
    public Weapon currentWeapon;
    private void Start()
    {
        playerAnimationManager = GetComponent<AnimationManager>();
    }

    private void Update()
    {
        Debug.Log("이즈어택 : " + isAttack);
        if (player.GetState() == PlayerMovementHandler.PlayerState.Attack)
        {
            if (currentWeapon == Weapon.OneHanded)
            {   // normalizedTime 은 0~1까지의 값을 가지고 있고, 1에 가까울 수록 애니메이션 클립이 재생이 완료되고있음.
                // isName("클립명") 은 이 애니메이션에 우선 진입했는지를 반환해주는 함수
                TurnOffPlayingAnimation("OneHanded");

            }
            else if (currentWeapon == Weapon.TwoHanded)
            {
                TurnOffPlayingAnimation("TwoHanded");
            }

            if (Time.time - lastClickedTime > maxComboDelay) // 만약 maxComboDelay 시간동안 클릭이 없으면 콤보를 초기화
            {
                if (isAttack)
                {
                    player.setState(PlayerMovementHandler.PlayerState.Idle);
                    Debug.Log("하 시발");
                }
                conditionIntialize();
                playerAnimationManager.IntializeToHitCondition();                
            }
        }
    }

    public void conditionIntialize()
    {
        isAttack = false;
        isAttackStart = false;
        isEnterToAttack = false;
        countClick = 0;
    }
    private void ChangeAttackAnimation(Weapon currentWeaponState)
    {
        if (currentWeaponState == Weapon.OneHanded)
        {
            if (countClick >= 2 && playerAnimationManager.AnimationPlayingCheck(0, 0.4f, "RightHand@Attack01"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit1", false);
                playerAnimationManager.getPlayerAnimator().SetBool("hit2", true);
            }
            if (countClick >= 3 && playerAnimationManager.AnimationPlayingCheck(0, 0.4f, "RightHand@Attack02"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit2", false);
                playerAnimationManager.getPlayerAnimator().SetBool("hit3", true);
            }
        }
        else if (currentWeaponState == Weapon.TwoHanded)
        {
            if (countClick >= 2 && playerAnimationManager.AnimationPlayingCheck(2, 0.4f, "2H@Attack01"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit1", false);
                playerAnimationManager.getPlayerAnimator().SetBool("hit2", true);
            }
            if (countClick >= 3 && playerAnimationManager.AnimationPlayingCheck(2, 0.4f, "2H@Attack02"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit2", false);
                playerAnimationManager.getPlayerAnimator().SetBool("hit3", true);
            }
        }
    }
    private void TurnOffPlayingAnimation(string currentWeaponState)
    {
        if (currentWeaponState == "OneHanded")
        {
            if (playerAnimationManager.AnimationPlayingCheck(0, 0.7f, "RightHand@Attack01"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit1", false);
            }
            if (playerAnimationManager.AnimationPlayingCheck(0, 0.7f, "RightHand@Attack02"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit2", false);
            }
            if (playerAnimationManager.AnimationPlayingCheck(0, 0.7f, "RightHand@Attack03"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit3", false);
                player.setState(PlayerMovementHandler.PlayerState.Idle);
                Debug.Log("되긴하냐?");
                conditionIntialize();
            }
        }
        else if (currentWeaponState == "TwoHanded")
        {
            if (playerAnimationManager.AnimationPlayingCheck(2, 0.7f, "2H@Attack01"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit1", false);
            }
            if (playerAnimationManager.AnimationPlayingCheck(2, 0.7f, "2H@Attack02"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit2", false);
            }
            if (playerAnimationManager.AnimationPlayingCheck(2, 0.7f, "2H@Attack03"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit3", false);
                player.setState(PlayerMovementHandler.PlayerState.Idle);
                Debug.Log("되긴하냐?");
                conditionIntialize();
            }
        }
    }
    public void OnAttack()
    {

        lastClickedTime = Time.time;
        countClick++;
        isAttack = true;
        isEnterToAttack = true;
        if (countClick == 1)
        {
            playerAnimationManager.getPlayerAnimator().SetBool("hit1", true);
        }
        Debug.Log("공격");

        if (currentWeapon == Weapon.OneHanded)
        {
            ChangeAttackAnimation(currentWeapon); // 이건 조금 더 효율적으로 줄일 수 있을듯

        }
        else if (currentWeapon == Weapon.TwoHanded)
        {
            ChangeAttackAnimation(currentWeapon);
        }
    }
    public void attack()
    {
        if (Input.GetButtonDown("Attack"))
        {  
            Debug.Log("호로롤");
            OnAttack();
        }
        
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

    public int getNumOfCountclicks()
    {
        return countClick;
    }
    // set
}
