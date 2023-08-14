using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BombUpDown : MonoBehaviour
{
    private float upSpeed = 10f;
    private float downSpeed = 17.0f;
    private Animator bombAnimator;

    public bool upDone = false;
    public bool falling = false;
    public bool alreadyRun = false;
    public bool backThePool = false;

    private Vector2 poolPosition_bomb = new Vector2(0f, 10f);
    private Vector2 originScale;

    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        bombAnimator = GetComponent<Animator>();

        originScale = transform.localScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (CoralSirenMoving.fireBomb == true && upDone == false)
        {
            // 보스가 폭탄을 위로 발사
            transform.Translate(Vector2.up * upSpeed * Time.deltaTime);
        }

        // 폭탄의 월드 좌표 y가 7에 도달하면 크기 키우기
        if (CoralSirenMoving.fireBomb == true && 
            transform.position.y >= 7f && alreadyRun == false)
        {
            upDone = true;

            transform.localScale = originScale * 1.5f;
            falling = true;
        }

        // 아래로 떨구기
        if (CoralSirenMoving.fireBomb == true && falling == true)
        {
            Debug.Log("아래로");
            transform.Translate(Vector2.down * downSpeed * Time.deltaTime);
        }

        // 애니메이션 재생이 끝나면 자식오브젝트(0) 렌더러 끄고 풀로 복귀
        if (CoralSirenMoving.fireBomb == true && backThePool == true)
        {
            Debug.Log("복귀");

            // 풀로 되돌리고 크기 원래대로
            transform.position = poolPosition_bomb;
            transform.localScale = originScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 만약 바닥 콜라이더와 충돌하면
        if (other.gameObject.CompareTag("Ground"))
        {
            Debug.Log("충돌!");
            // 추락을 멈춤
            falling = false;

            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

            // 애니메이션 재생
            transform.GetComponent<Animator>().enabled = true;

            StartCoroutine(BombAnimationTimer());
        }
    }

    IEnumerator BombAnimationTimer()
    {
        // 애니메이션 재생 시간까지 기다렸다가 풀로 복귀
        yield return new WaitForSeconds
               (bombAnimator.GetCurrentAnimatorStateInfo(0).length);
        // 애니메이션 종료
        transform.GetComponent<Animator>().enabled = false;

        alreadyRun = true;
        backThePool = true;
    }
}
