using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 폭탄의 위 아래 이동을 실행하고, 애니메이션을 실행하는 클래스
/// </summary>

public class BombUpDown : MonoBehaviour
{
    private float upSpeed = 10f;
    private float downSpeed = 17.0f;
    private Animator bombAnimator;

    public bool upDone = false;
    public bool falling = false;
    public bool alreadyRun = false;
    public bool backThePool = false;
    public bool allBack = false;

    private Vector2 poolPosition_bomb = new Vector2(0f, 10f);
    private Vector2 originScale;

    private Animator animator;
    private SpriteRenderer sparkRenderer;

    // Start is called before the first frame update
    void Start()
    {
        bombAnimator = GetComponent<Animator>();

        // 폭탄 확대 전 원래 크기
        originScale = transform.localScale;

        animator = GetComponent<Animator>();

        // 스파크가 표시되는 렌더러
        sparkRenderer = transform.GetChild(0).
            transform.GetChild(0).transform.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (CoralSirenMoving.fireBomb == true && upDone == false)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;

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
            transform.Translate(Vector2.down * downSpeed * Time.deltaTime);
        }

        // 애니메이션 재생이 끝나면 자식오브젝트(0) 렌더러 끄고 풀로 복귀
        if (CoralSirenMoving.fireBomb == true && backThePool == true)
        {
            // 풀로 되돌리고 크기 원래대로
            transform.position = poolPosition_bomb;
            transform.localScale = originScale;

            allBack = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 만약 바닥 콜라이더와 충돌하면
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Sand"))
        { 
            // 스파크 렌더러 끄기
            sparkRenderer.enabled = false;

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
        // 스파크 렌더러 복귀
        sparkRenderer.enabled = true;

        animator.Rebind();
        

        alreadyRun = true;
        backThePool = true;
    }
}
