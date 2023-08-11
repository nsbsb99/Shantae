using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HopBackBullet : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform targetTransform; // 다른 오브젝트의 Transform

    private void Start()
    {
        Debug.Log("이동 시작");
        // 만약 풀로 돌아간다면 해당 스크립트가 적용되지 않도록 하기. 
        targetTransform = GameObject.FindWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        // 다른 오브젝트를 향하는 방향
        Vector3 targetDirection = targetTransform.position - transform.position;
        targetDirection.Normalize();

        // 오브젝트를 다른 오브젝트를 향하는 방향으로 회전
        float targetAngle =
            Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation =
            Quaternion.RotateTowards(transform.rotation, targetRotation, 1.5f);

        // 오브젝트를 바라보는 방향으로 이동
        Vector3 forwardDirection = transform.right;
        transform.position += forwardDirection * moveSpeed * Time.deltaTime;
    }
}
