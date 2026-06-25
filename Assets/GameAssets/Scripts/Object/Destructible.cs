using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject fractured;

    private void Start()
    {

    }

    private void Update()
    {
    }

    public void BreakFracturedObject()
    {
        GameObject destroryedObj = Instantiate(fractured, transform.position, transform.rotation);
        Destroy(gameObject,0.2f);
        Destroy(destroryedObj.gameObject, 5f);
    }

}
