using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayShoot : MonoBehaviour
{
    LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitObject;
        
        Debug.DrawRay(new Vector3(0f,1f,0f), transform.forward * 7, Color.red);
        if (Physics.Raycast(transform.position, transform.forward, out hitObject, 7, layerMask))
        {
            Debug.DrawRay(transform.position, transform.forward * 7, Color.blue);
        }
    }
}
