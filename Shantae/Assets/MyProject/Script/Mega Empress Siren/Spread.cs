using System.Collections;
using System.Collections.Generic;
// === (노솔빈 수정) 빌드 테스트 중 오류 발생으로 삭제.
//using Unity.VisualScripting;
// ===
using UnityEngine;
// === (노솔빈 수정) 빌드 테스트 중 오류 발생으로 삭제.
//using static Unity.VisualScripting.Metadata;
// ===

public class Spread : MonoBehaviour
{
    
    private float rotationSpeed = 100; // 회전 속도 
    private bool isClockwise; // 시계방향 회전 여부

    private bool isTimerStarted = false;
    private float startTime;
    private float elapsedTime;
    private Transform[] children;

    private void Start()
    {
        // 자식 오브젝트들 가져오기
            children = GetComponentsInChildren<Transform>();
        isClockwise = Random.value > 0.5f;

        foreach (Transform child in children)
        {
            if (child != transform) // 부모 오브젝트는 제외
            {
                Rigidbody2D rb = child.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.gravityScale = 0.0f; // 중력 미적용
                }
            }
        }
    }

    private void Update()
    {

        if (!isTimerStarted)
        {
            isTimerStarted = true;
            startTime = Time.time;
        }

        if (isTimerStarted)
        {
            elapsedTime = Time.time - startTime;
        }

        if (elapsedTime < 0.5f)
        {
            foreach (Transform child in children)
            {
                if (child != transform)
                {
                    Rigidbody2D rb = child.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        Vector2 randomForce = new Vector2(Random.Range(-0.5f, 0.5f), 0.001f);
                        rb.AddForce(randomForce, ForceMode2D.Impulse);
                    }
                }
            }
        }
        else
        {
            // 자식 오브젝트들에 중력 적용
            Transform[] children = GetComponentsInChildren<Transform>();

            foreach (Transform child in children)
            {
                if (child != transform) // 부모 오브젝트는 제외
                {
                    Rigidbody2D rb = child.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.gravityScale = 1f;
                    }
                }
            }
        }
        Transform centerPoint = transform;
        // 회전 처리
        foreach (Transform child in children)
        {
            if (child != transform && child != null) // 부모 오브젝트는 제외
            {
                
                Transform centerPointChild = child; // 자식 오브젝트를 중심으로 회전
                if (isClockwise)
                {
                    child.RotateAround(centerPointChild.position, Vector3.forward, rotationSpeed * Time.deltaTime);
                }
                else
                {
                    child.RotateAround(centerPointChild.position, Vector3.back, rotationSpeed * Time.deltaTime);
                }
            }
        }
    }
}
