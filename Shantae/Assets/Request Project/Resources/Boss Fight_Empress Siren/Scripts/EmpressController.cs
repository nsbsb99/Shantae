using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Empress Siren의 패배 조건을 다루는 스크립트. "PlayerAttack" 태그에 충돌하면 
/// Empress Siren의 HP가 감소.
/// </summary>

public class EmpressController : MonoBehaviour
{
    // Empress Siren의 HP
    public static float empressHP = default;
    // 플레이어 데미지 판정 (Empress 피격 판정)
    private float getDamage = default;
    // 애니메이터
    private Animator animator;
    // Empress Siren의 텔레포트 
    private GameObject teleportAnimator = default;
    private GameObject teleportParticle = default;
    // 피격 시 색상 교체를 위함
    private Color originColor = default;
    private Color transparentColor = default;
    // 플레이어 포지션
    private Vector2 playerPosition = default;
    // Empress Siren의 포지션
    private Vector2 empressPosition = default;

    private void Start()
    {
        // Empress Siren 게임 오브젝트가 !null인지 확인
        Debug.Assert(this.gameObject != null);

        animator = GetComponent<Animator>();
        teleportAnimator = transform.GetChild(0).gameObject;
        teleportParticle = transform.GetChild(1).gameObject;

        empressHP = 15f; // 임시 조정

        originColor = transform.GetComponent<SpriteRenderer>().color;
        transparentColor = originColor;
        transparentColor.a = 0.5f;
    }

    private void Update()
    {
        // Empress Siren의 패배 확인
        if (empressHP <= 0)
        {
            // EmpressMoving 코루틴 종료 메서드 추가
            StopCoroutine(EmpressMoving.instance.RandomMoving());

            StartCoroutine(PlayerWin());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Empress Siren의 피격 확인
        if (collision.CompareTag("PlayerAttack") && empressHP > 0)
        {
            // 플레이어가 입힌 데미지는 4부터 9까지의 정수
            getDamage = Random.Range(4, 10);
            // Empress HP 깎기. 
            empressHP -= getDamage;

            StartCoroutine(FlashEmpress());
        }
    }

    private IEnumerator FlashEmpress()
    {
        float blinkTime = 1;
        float nowTime = 0;

        while (nowTime < blinkTime)
        {
            nowTime += Time.deltaTime * 30;

            transform.GetComponent<SpriteRenderer>().color = transparentColor;
            yield return new WaitForSeconds(0.1f);

            transform.GetComponent<SpriteRenderer>().color = originColor;
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("루프 탈출");

        transform.GetComponent<SpriteRenderer>().color = originColor;

        StopCoroutine(FlashEmpress());
    }

    private IEnumerator PlayerWin()
    {
        playerPosition = GameObject.FindWithTag("Player").transform.position;
        empressPosition = transform.position;

        GameObject.FindWithTag("Player").GetComponent<PlayerExit>().enabled = true;

        // 패배 시 플레이어가 Empress Siren의 왼쪽에 위치
        if (playerPosition.x < empressPosition.x)
        {
            transform.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (playerPosition.x > empressPosition.x) // Empress Siren의 오른쪽에 위치
        {
            transform.GetComponent<SpriteRenderer>().flipX = false;
        }

        animator.SetTrigger("Empress Lose");
        yield return new WaitForSeconds(3);

        teleportAnimator.SetActive(true);
        yield return new WaitForSeconds
            (teleportAnimator.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
        teleportAnimator.SetActive(false);

        transform.GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(2f);

        CameraShake.instance.StartCoroutine(CameraShake.instance.OpenTheDoor());

        // 자기자신(Empress Siren 종료)
        gameObject.SetActive(false);
    }
}
