using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public AttackManager attackManger;
    void Start()
    {
        attackManger.currentWeapon = AttackManager.Weapon.OneHanded;
    }

    // Update is called once per frame
    void Update()
    {
        weaponChange();
        Debug.Log(attackManger.currentWeapon);
    }

    private void weaponChange()
    {
        // 무기 교체는 이동이나 Idle일때만 가능하도록 수정 23.10.02
        if (attackManger.player.GetState() == PlayerMovementHandler.PlayerState.Idle
            || attackManger.player.GetState() == PlayerMovementHandler.PlayerState.Move)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && attackManger.currentWeapon != AttackManager.Weapon.OneHanded)
            {
                attackManger.currentWeapon = AttackManager.Weapon.OneHanded;

                attackManger.weapons[2].SetActive(false);
                attackManger.weapons[0].SetActive(true);
                attackManger.weapons[1].SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && attackManger.currentWeapon != AttackManager.Weapon.TwoHanded)
            {
                attackManger.currentWeapon = AttackManager.Weapon.TwoHanded;
                attackManger.weapons[0].SetActive(false);
                attackManger.weapons[1].SetActive(false);
                attackManger.weapons[2].SetActive(true);
            }
        } 
    }
}
