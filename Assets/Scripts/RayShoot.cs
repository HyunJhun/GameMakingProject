using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayShoot : MonoBehaviour
{
    // Local Var

    [Header("LayerMask")]
    [SerializeField] private LayerMask WallLayerMask;


    private enum Direction
    {
        Forward,
        Backward,
        Left,
        Right,
        ForwardLeft,
        ForwardRight,
        BackwardLeft,
        BackwardRight
    }

    private List<Direction> movableDirectionList;
    private RaycastHit hitObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShotRayEightDirection();
    }
    private void ShotRayEightDirection()
    {
        if (Physics.Raycast(transform.position, transform.forward * 7, out hitObject, 7, WallLayerMask))
        {
            Debug.Log(hitObject.transform.position);
            Debug.DrawRay(transform.position, transform.forward * 7, Color.blue);
        }
        if (Physics.Raycast(transform.position, transform.forward * -7, out hitObject, 7, WallLayerMask))
        {
            Debug.DrawRay(transform.position, transform.forward * -7, Color.blue);
        }
        if (Physics.Raycast(transform.position, transform.right * 7, out hitObject, 7, WallLayerMask))
        {
            Debug.DrawRay(transform.position, transform.right * 7, Color.blue);
        }
        if (Physics.Raycast(transform.position, transform.right * -7, out hitObject, 7, WallLayerMask))
        {
            Debug.DrawRay(transform.position, transform.right * -7, Color.blue);
        }
        if (Physics.Raycast(transform.position, (transform.forward + transform.right).normalized * 7, out hitObject, 7, WallLayerMask))
        {
            Debug.DrawRay(transform.position, (transform.forward + transform.right).normalized * 7, Color.blue);
        }
        if (Physics.Raycast(transform.position, (transform.forward + transform.right).normalized * -7, out hitObject, 7, WallLayerMask))
        {
            Debug.DrawRay(transform.position, (transform.forward + transform.right).normalized * -7, Color.blue);
        }
        if (Physics.Raycast(transform.position, (transform.forward + (-transform.right)).normalized * 7, out hitObject, 7, WallLayerMask))
        {
            Debug.DrawRay(transform.position, (transform.forward + (-transform.right)).normalized * 7, Color.blue);
        }
        if (Physics.Raycast(transform.position, (transform.forward + (-transform.right)).normalized * -7, out hitObject, 7, WallLayerMask))
        {
            Debug.DrawRay(transform.position, (transform.forward + (-transform.right)).normalized * -7, Color.blue);
        }
    }
}
