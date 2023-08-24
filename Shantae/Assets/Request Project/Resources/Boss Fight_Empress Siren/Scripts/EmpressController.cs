using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Empress Siren의 패배 조건을 다루는 스크립트. "PlayerAttack" 태그에 충돌하면 
/// Empress Siren의 HP가 감소.
/// </summary>

public class EmpressController : MonoBehaviour
{
    #region Empress Siren의 피격, 패배 여부 확인 변수
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

    private bool alreadyRun = false;
    #endregion

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
        if (empressHP <= 0 && alreadyRun == false)
        {
            // EmpressMoving 종료 메서드 추가
            transform.GetComponent<EmpressMoving>().enabled = false;

            alreadyRun = true;
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
        GameObject.FindWithTag("Player").transform.GetChild(0).
            GetComponent<PlayerExit>().enabled = true;

        yield return new WaitForSeconds(0.5f);

        playerPosition = GameObject.FindWithTag("Player").transform.position;

        // 패배 시 플레이어가 Empress Siren의 왼쪽에 위치
        if (playerPosition.x < 0)
        {
            Debug.Log("왼쪽 보기");
            transform.position = new Vector2(playerPosition.x + 4f, -1.44f);
            transform.GetComponent<SpriteRenderer>().flipX = true;
            animator.SetTrigger("Empress Lose");
        }
        else if (playerPosition.x >= 0) // Empress Siren의 오른쪽에 위치
        {
            Debug.Log("오른쪽 보기");
            transform.position = new Vector2(playerPosition.x - 4f, -1.44f);
            transform.GetComponent<SpriteRenderer>().flipX = false;

            animator.SetTrigger("Empress Lose");
        }

        yield return new WaitForSeconds(3.5f);

        teleportAnimator.SetActive(true);
        transform.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds
            (teleportAnimator.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
        teleportAnimator.SetActive(false);

        yield return new WaitForSeconds(3f);

        GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = false;
        CameraShake.instance.StartCoroutine(CameraShake.instance.OpenTheDoor());

        StopCoroutine(PlayerWin());

        // 자기자신(Empress Siren 종료)
        gameObject.SetActive(false);
    }
}
