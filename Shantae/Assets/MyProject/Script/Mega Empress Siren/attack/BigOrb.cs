using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BigOrb : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private float moveSpeed = 5f;

    private bool isTimerStarted = false;
    private float startTime;
    private float elapsedTime;
    private float originalPositionX;

   
    private void Start()
    {
        

        originalPositionX = transform.position.x;

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        // Set the initial transparency
        Color transparentColor = originalColor;
        transparentColor.a = 0.0f; // 0.0f means fully transparent
        spriteRenderer.color = transparentColor;

        // Start the coroutine to restore the original color
        StartCoroutine(RestoreOriginalColorAfterDelay(3.0f));

    }
    private void Update()
    {
        GameObject player = GameObject.Find("Player");
        GameObject empress = GameObject.Find("Mega Empress Siren");


        bool trigger = true;
        if (player != null && empress != null)
        {
            // Player의 좌표값 가져오기
            Vector3 playerPosition = player.transform.position;

            // Empress의 Transform 컴포넌트를 사용하여 x값 가져오기
            //float empressX = empress.transform.position.x;

        }

        if (!isTimerStarted)
        {
            isTimerStarted = true;
            startTime = Time.time;
        }

        if (isTimerStarted)
        {
            elapsedTime = Time.time - startTime;
        }
        if(elapsedTime<= 5f)
        {
            if(trigger)
            {
                //audioSource.Play();
                trigger = false;
            }
            Vector3 newPosition = transform.position;
            newPosition.x = empress.transform.position.x + originalPositionX;
            transform.position = newPosition;
        }
        else if (elapsedTime > 5f && elapsedTime < 10f)
        {
            bool move = false;
            if (!move)
            {
                Vector3 targetPosition = player.transform.position;
                Vector3 objectPosition = transform.position;
                Vector3 direction = targetPosition - objectPosition;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = rotation;
                move = true;
            }
                // 플레이어 위치
                Vector3 targetDirection = player.transform.position - transform.position;
                targetDirection.Normalize();

                // 플레이어 방향 회전
                float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 0.00001f);

                // 플레이어에게 이동
                Vector3 forwardDirection = transform.right;
                transform.position += forwardDirection * moveSpeed * Time.deltaTime;
            
        }
        else if (elapsedTime >= 10f && elapsedTime <= 25f)
        {

            bool move = false;
            if (!move)
            {
                if (transform.position.x > 0f)
                {
                    Vector3 newPosition = transform.position + new Vector3(moveSpeed/500, 0, 0);

                    // 새로운 위치로 오브젝트 이동
                    transform.position = newPosition;
                    //transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                    move = true;
                }
                else if (transform.position.x <= 0f)
                {
                    Vector3 newPosition = transform.position + new Vector3(- moveSpeed/500, 0, 0);

                    // 새로운 위치로 오브젝트 이동
                    transform.position = newPosition;
                    //transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
                    move = true;
                }
            }
        }
        else if(elapsedTime >25f)
        {
            Destroy(gameObject);
        }
       
    }
    private IEnumerator RestoreOriginalColorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Restore the original color
        spriteRenderer.color = originalColor;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Player"))
        {
            Destroy(gameObject);
        }
    }
}
