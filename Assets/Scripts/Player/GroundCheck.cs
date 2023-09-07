using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [Header("Boxcast Property")]
    [SerializeField] private Vector3 boxSize;
    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask groundLayer;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);
    }

    public bool IsGrounded()
    {
        // 순서대로 (레이저를 발사할 위치, 사각형의 각 좌표의 절판 크기, 발사 방향, 충돌 결과, 회전 각도, 최대 거리)
        bool isHit = Physics.BoxCast(transform.position, boxSize, -transform.up,out RaycastHit hit, transform.rotation, maxDistance, groundLayer);

        return isHit;
        
    }
}
