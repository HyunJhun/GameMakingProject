using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [Header("Reference")]
    PlayerMovementHandler player;
    AttackManager attackManager;
    Animator playerAnimator;

    public bool check;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMovementHandler>();
        playerAnimator = GetComponent<Animator>();
        attackManager = GetComponent<AttackManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (check)
            playerAnimator.SetLayerWeight(0, 1);
        else
            playerAnimator.SetLayerWeight(0, 0);
    }
    public void PlayerMoveAnimation(Vector3 direction)
    {
        if (attackManager.currentWeapon == AttackManager.Weapon.OneHanded)
        {
            if (player.getIsLockOn() == true)
            {
                // 기존 원핸드 움직임에 관한 Animation Layer들을 꺼주고 투핸드에 관한 Animation Layer들을 껴주는 작업

                // 투핸드 종료
                playerAnimator.SetLayerWeight(2, 0);
                playerAnimator.SetLayerWeight(3, 0);

                // 원핸드 시작
                playerAnimator.SetLayerWeight(0, 1);
                playerAnimator.SetLayerWeight(1, 1);

                // 방향값
                playerAnimator.SetFloat("speedX", direction.x);
                playerAnimator.SetFloat("speedY", direction.z);

            }
            else if (player.getIsLockOn() == false)
            {
                // 기존 원핸드 움직임에 관한 Animation Layer들을 꺼주고 투핸드에 관한 Animation Layer들을 껴주는 작업

                // 투핸드 종료
                playerAnimator.SetLayerWeight(2, 0);
                playerAnimator.SetLayerWeight(3, 0);

                // 원핸드 시작
                playerAnimator.SetLayerWeight(0, 1);
                playerAnimator.SetLayerWeight(1, 0);

                // 방향값
                playerAnimator.SetFloat("speedY", direction.magnitude);

            }
        }
        else if (attackManager.currentWeapon == AttackManager.Weapon.TwoHanded)
        {
            if (player.getIsLockOn() == true)
            {
                // 기존 원핸드 움직임에 관한 Animation Layer들을 꺼주고 투핸드에 관한 Animation Layer들을 껴주는 작업

                // 원핸드 종료
                playerAnimator.SetLayerWeight(0, 0);
                playerAnimator.SetLayerWeight(1, 0);

                // 투핸드 시작
                playerAnimator.SetLayerWeight(2, 1);
                playerAnimator.SetLayerWeight(3, 1);

                // 방향값
                playerAnimator.SetFloat("speedX", direction.x);
                playerAnimator.SetFloat("speedY", direction.z);
            }
            else if (player.getIsLockOn() == false)
            {
                // 기존 원핸드 움직임에 관한 Animation Layer들을 꺼주고 투핸드에 관한 Animation Layer들을 껴주는 작업

                // 원핸드 종료
                playerAnimator.SetLayerWeight(0, 0);
                playerAnimator.SetLayerWeight(1, 0);
                // 투핸드 시작
                playerAnimator.SetLayerWeight(2, 1);
                playerAnimator.SetLayerWeight(3, 0);

                // 방향값
                playerAnimator.SetFloat("speedY", direction.magnitude);
            }
        }
    }

    public void PlayerDodgeAnimation()
    {
        playerAnimator.SetBool("isDodge", player.getIsDodge());
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f
            &&playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("human"))
        {
            playerAnimator.SetBool("isDodge",player.getIsDodge());
        }

    }
    public Animator getPlayerAnimator()
    {
        return playerAnimator;
    }
}
