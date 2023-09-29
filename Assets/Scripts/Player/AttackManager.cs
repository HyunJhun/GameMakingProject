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
    public List<GameObject> weapons; // 0 = sheld, 1 = shortSword, 2 = twoHandeld
    public PlayerMovementHandler player;

    [Header("Values")]
    public float cooldownTime = 2f;
    private float nextFireTime = 1f;
    public static int noOfClicks = 0;
    float lastClickedTime = 0f;
    float maxComboDelay = 1f;
    bool isAttack;
    float timer;
    public Weapon currentWeapon;
    private void Start()
    {
        playerAnimationManager = GetComponent<AnimationManager>();
        isAttack = false;
    }

    private void Update()
    {
        timer = Time.time;
        if (currentWeapon == Weapon.OneHanded)
        {// normalizedTime 은 0~1까지의 값을 가지고 있고, 1에 가까울 수록 애니메이션 클립이 재생이 완료되고있음.
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
                timer = 0f;
                noOfClicks = 0;
                if (player.GetState() != PlayerMovementHandler.PlayerState.Idle)
                {
                    Debug.Log("너임?");
                    player.setState(PlayerMovementHandler.PlayerState.Idle);
                }
            }
        }
        else if(currentWeapon == Weapon.TwoHanded)
        {
            if (playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f
                && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).IsName("2H@Attack01"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit1", false);
            }
            if (playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f
                && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).IsName("2H@Attack02"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit2", false);
            }
            if (playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f
                && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).IsName("2H@Attack04"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit3", false);
                noOfClicks = 0;
                if (player.GetState() != PlayerMovementHandler.PlayerState.Idle)
                {
                    player.setState(PlayerMovementHandler.PlayerState.Idle);
                }

            }
        }
        if(Time.time - lastClickedTime > maxComboDelay 
            && player.GetState() == PlayerMovementHandler.PlayerState.Attack) // 만약 maxComboDelay 시간동안 클릭이 없으면 콤보를 초기화
        {
            noOfClicks = 0;
            // 계속 애니메이션 막바지에 클릭을 하면 애니메이션이 끝나고 다음 애니메이션으로 넘어가지 않고 
            playerAnimationManager.getPlayerAnimator().SetBool("hit1", false);
            playerAnimationManager.getPlayerAnimator().SetBool("hit2", false);
            playerAnimationManager.getPlayerAnimator().SetBool("hit3", false);
            if (player.GetState() != PlayerMovementHandler.PlayerState.Idle)
            {
                player.setState(PlayerMovementHandler.PlayerState.Idle);
            }
        }
        if(timer > nextFireTime)
        {
            if(Input.GetButtonDown("Attack"))
            {
                OnClick();
            }
        }
      
    }
    void OnClick()
    {
        lastClickedTime = Time.time;
        noOfClicks++;
        player.setState(PlayerMovementHandler.PlayerState.Attack);
        if(noOfClicks == 1)
        {
            playerAnimationManager.getPlayerAnimator().SetBool("hit1", true);
        }
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);
        if(currentWeapon == Weapon.OneHanded)
        { 
            if(noOfClicks >=2 && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f // 콤보 공격을 이어나가기 위한 장치
            && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).IsName("RightHand@Attack01"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit1", false);
                playerAnimationManager.getPlayerAnimator().SetBool("hit2", true);
            }
            if (noOfClicks >= 3 && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f
               && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).IsName("RightHand@Attack02"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit2", false);
                playerAnimationManager.getPlayerAnimator().SetBool("hit3", true);
            }
        }
        else if(currentWeapon == Weapon.TwoHanded)
        {
            if (noOfClicks >= 2 && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f // 콤보 공격을 이어나가기 위한 장치
            && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).IsName("2H@Attack01"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit1", false);
                playerAnimationManager.getPlayerAnimator().SetBool("hit2", true);
            }
            if (noOfClicks >= 3 && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f
               && playerAnimationManager.getPlayerAnimator().GetCurrentAnimatorStateInfo(0).IsName("2H@Attack02"))
            {
                playerAnimationManager.getPlayerAnimator().SetBool("hit2", false);
                playerAnimationManager.getPlayerAnimator().SetBool("hit3", true);
            }
        }
    }
    public void moveWhenAttack()
    {
        player.getPlayerController().Move(new Vector3(0f, 0f, 1));
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
