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
        if(Input.GetKeyDown(KeyCode.Alpha1) && attackManger.currentWeapon != AttackManager.Weapon.OneHanded)
        {
            attackManger.currentWeapon = AttackManager.Weapon.OneHanded;

            attackManger.weapons[2].SetActive(false);
            attackManger.weapons[0].SetActive(true);
            attackManger.weapons[1].SetActive(true);
            Debug.Log("원핸드");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && attackManger.currentWeapon != AttackManager.Weapon.TwoHanded)
        {
            attackManger.currentWeapon = AttackManager.Weapon.TwoHanded;
            attackManger.weapons[0].SetActive(false);
            attackManger.weapons[1].SetActive(false);
            attackManger.weapons[2].SetActive(true);
            Debug.Log("투핸드");
        }
    }
}
