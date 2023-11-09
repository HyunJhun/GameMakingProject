using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [Header("Boxcast Property")]
    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask groundLayer;
    public float ShotRayForMaxHeightCheck()
    {
        // 순서대로 (레이저를 발사할 위치, 사각형의 각 좌표의 절판 크기, 발사 방향, 충돌 결과, 회전 각도, 최대 거리)
        Debug.DrawRay(transform.position, -transform.up * maxDistance, Color.red);
        Physics.Raycast(transform.position, -transform.up,out RaycastHit hit, maxDistance, groundLayer);
        return hit.distance; // ray를 쏴서 플레이어의 현재 높이를 계산하여 반환
    }
}
