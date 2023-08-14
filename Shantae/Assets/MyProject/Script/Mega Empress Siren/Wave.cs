using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UIElements;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Wave : MonoBehaviour
{
    public UnityEngine.Transform centerPoint; // 회전 중심점
    public float rotationSpeed; // 회전 속도 (도/초)
    private float rotationTime = 0f; // 회전 시간
    private bool isClockwise = true; // 시계방향 회전 여부
    public float rotating;
    float speed;
    public float wait;

    private void Start()
    {
        speed = rotationSpeed;
    }
    private void Update()
    {
        rotationTime += Time.deltaTime;

        // 시계방향 회전
        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        if (isClockwise)
        {
            
            transform.RotateAround(centerPoint.position, Vector3.forward, rotationSpeed * Time.deltaTime);

            if (rotationTime > rotating)
            {
                rotationSpeed = 0;
                yield return new WaitForSeconds(wait);         
                    rotationSpeed = speed;
                rotationTime = 0f;
                isClockwise = false;
            }

        }
        // 반시계방향 회전
        else if (!isClockwise)
        {
            
            transform.RotateAround(centerPoint.position, Vector3.back, rotationSpeed * Time.deltaTime);

            if (rotationTime > rotating)
            {
                rotationSpeed = 0;
                yield return new WaitForSeconds(wait);
                rotationSpeed = speed;

                rotationTime = 0f;

                isClockwise = true;
            }

        }
    }
}
