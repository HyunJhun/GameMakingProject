using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public enum Weapon
    {
        OneHanded,
        TwoHanded
    }
    AnimationManager playerAnimationManager;
    private Status inRangeObjStats;
    public List<GameObject> weapons; // 0 = sheld, 1 = shortSword, 2 = twoHandeld
    public PlayerMovementHandler player;
    [SerializeField]private List<GameObject> attackCollisionBox; // 0 = OneHanded , 1 = TwoHanded

    [Header("Values")]
    private int countClick = 0;
    private float lastClickedTime = 0f;
    private float maxComboDelay = 1f;

    


    public bool isAttack { get; set; } = false;
    public bool isOneHandedAttack { get; set; }
    public bool isAttackStart { get; set; } = false;
    public Weapon currentWeapon;
    private void Start()
    {
        playerAnimationManager = GetComponent<AnimationManager>();
        GetAttackRangeBoxByCurentWeapon().SetActive(false);
    }

    private void Update()
    {
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
        GetAttackRangeBoxByCurentWeapon().SetActive(false);
        countClick = 0;
    }
    private void ChangeAttackAnimation(Weapon currentWeaponState)
    {
        if (currentWeaponState == Weapon.OneHanded)
        {
            if (countClick == 1)
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit1", true);
                playerAnimationManager.getPlayerAnimator().SetBool("isOneHandedAttack", isOneHandedAttack);
            }
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
            if (countClick == 1)
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit1", true);
                playerAnimationManager.getPlayerAnimator().SetBool("isOneHandedAttack", isOneHandedAttack);
            }
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
                conditionIntialize();
            }
        }
    }
    public void OnAttack()
    {

        lastClickedTime = Time.time;
        countClick++;
        isAttack = true;

        if (currentWeapon == Weapon.OneHanded)
        {
            isOneHandedAttack = true;
            ChangeAttackAnimation(currentWeapon);

        }
        else if (currentWeapon == Weapon.TwoHanded)
        {
            isOneHandedAttack = false;
            ChangeAttackAnimation(currentWeapon);
        }
    }
    public void attack()
    {
        GetAttackRangeBoxByCurentWeapon().SetActive(true); // 공격 상태로 넘어가면 무기 collider가 활성화
        if (Input.GetButtonDown("Attack"))
        {  
            OnAttack();
        }
        
    }


    public void ToDamage(int idx)
    {
        player.GetStats().staminaDown(player.GetStats().GetAttackStamina(idx)); // 일단 공격 모션이 진행되면 공격을 한다는 판정이므로 스태미너 감소는 진행
        if (GetAttackRangeBoxByCurentWeapon().GetComponent<AttackRangeCheck>().getStats() != null) // 스태미너가 감소된 이후 상대방이 닿았는지 체크
        {
            inRangeObjStats = GetAttackRangeBoxByCurentWeapon().GetComponent<AttackRangeCheck>().getStats();
            inRangeObjStats.hpDown(player.GetStats().GetAttackDamage(idx));
        }       
    }
    private GameObject GetAttackRangeBoxByCurentWeapon() // 무기별로 가지고 있는 공격범위가 다르기때문에 그걸 반환해주는 역할
    {
        if(currentWeapon == Weapon.OneHanded)
        {
            return attackCollisionBox[0];
        }
        else
        {
            return attackCollisionBox[1];
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
