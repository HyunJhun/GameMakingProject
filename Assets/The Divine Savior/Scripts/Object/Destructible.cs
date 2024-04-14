using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject fractured;
    private Boss boss;
    private int triggerHp;

    private void Start()
    {
        boss = GameObject.Find("Boss").GetComponent<Boss>();
        triggerHp = (int)boss.GetStatus().GetMaxHP() / 3;
    }

    private void Update()
    {
        if (Input.GetKeyDown("g"))
            BreakFracturedObject();

        if (boss.GetStatus().getHp() <= triggerHp)
            BreakFracturedObject();
    }

    public void BreakFracturedObject()
    {
        GameObject destroryedObj = Instantiate(fractured, transform.position, transform.rotation);
        Destroy(gameObject,0.2f);
        Destroy(destroryedObj.gameObject, 5f);
    }

}
