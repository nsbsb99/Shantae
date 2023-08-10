using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acceesorybreak : MonoBehaviour
{
    public UnityEngine.Transform centerPoint; // 회전 중심점
    public float rotationSpeed; // 회전 속도 (도/초)
    public bool isClockwise; // 시계방향 회전 여부

    private float floatSpeed = 200.0f; // 오브젝트가 위로 올라가는 속도

    private Rigidbody2D rb;

    private bool isTimerStarted = false;
    private float startTime;
    private float elapsedTime;
    public GameObject unBreak;
    //private Acceesory acceesory;
    private void Start()
    {
        //acceesory = FindObjectOfType<Acceesory>();

        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 7.0f);
        
       
    }

    private void Update()
    {
        if (unBreak == null && !isTimerStarted)
        {
            isTimerStarted = true;
            startTime = Time.time;
            Debug.Log("Timer started!");
        }

        if (isTimerStarted)
        {
            elapsedTime = Time.time - startTime;
            Debug.Log("Elapsed time: " + elapsedTime.ToString("F2") + " seconds");
        }
        if (elapsedTime < 0.1f)
        {
            Vector2 newPosition = rb.position + Vector2.up * floatSpeed * Time.deltaTime;
            rb.MovePosition(newPosition);
        }
        else
        {
            rb.gravityScale = 0.5f;
        }

        if (isClockwise)
        {
            transform.RotateAround(centerPoint.position, Vector3.forward, rotationSpeed * Time.deltaTime);


        }
        else if (!isClockwise)
        {
            transform.RotateAround(centerPoint.position, Vector3.back, rotationSpeed * Time.deltaTime);
        }
        
    }   
}
