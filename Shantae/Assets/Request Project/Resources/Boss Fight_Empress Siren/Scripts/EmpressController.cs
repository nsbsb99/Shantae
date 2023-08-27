using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Empress Siren의 패배 조건을 다루는 스크립트. "PlayerAttack" 태그에 충돌하면 
/// Empress Siren의 HP가 감소.
/// </summary>

public class EmpressController : MonoBehaviour
{

    public Transform targetTransform; // 다른 오브젝트의 Transform
    private string player = "Player";
    private GameObject playerTransform;

    private bool playerControllerOff = false;

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
        playerTransform = GameObject.Find(player);
        // 만약 풀로 돌아간다면 해당 스크립트가 적용되지 않도록 하기. 
        targetTransform = playerTransform.transform;

        // Empress Siren 게임 오브젝트가 !null인지 확인
        Debug.Assert(this.gameObject != null);

        animator = GetComponent<Animator>();
        teleportAnimator = transform.GetChild(0).gameObject;
        teleportParticle = transform.GetChild(1).gameObject;

        empressHP = 10f;

        originColor = transform.GetComponent<SpriteRenderer>().color;
        transparentColor = originColor;
        transparentColor.a = 0.5f;
    }

    private void Update()
    {
        // Empress Siren의 패배 확인
        if (empressHP <= 0 && alreadyRun == false)
        {
            alreadyRun = true;

            // EmpressMoving 종료 메서드 추가
            transform.GetComponent<EmpressMoving>().enabled = false;
            transform.GetComponent<EmpressMoving>().StopAllCoroutines();

            transform.GetComponent<CeilingAttack>().enabled = false;
            transform.GetComponent<CeilingAttack>().StopAllCoroutines();

            transform.GetComponent<BlowKissAttack>().enabled = false;
            transform.GetComponent<BlowKissAttack>().StopAllCoroutines();

            transform.GetComponent<SurfAttack>().enabled = false;
            transform.GetComponent<SurfAttack>().StopAllCoroutines();

            transform.GetComponent<HopBackAttack>().enabled = false;
            transform.GetComponent<HopBackAttack>().StopAllCoroutines();

            // 피격 중일 때를 고려
            transform.GetComponent<SpriteRenderer>().color = originColor;

            StartCoroutine(PlayerWin());
        }

        /// <problem> 왜 시작부터 true?
        Debug.Log(transform.GetComponent<SpriteRenderer>().flipX);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Empress Siren의 피격 확인
        if (collision.CompareTag("PlayerAttack") && empressHP > 0)
        {
            // 플레이어가 입힌 데미지
            getDamage = 1;
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
            transform.GetComponent<PlayerExit>().enabled = true;

        yield return new WaitForSeconds(1f);

        playerPosition = playerTransform.transform.position;

        // 패배 시 플레이어가 Empress Siren의 왼쪽에 위치 (Empress Siren의 시선)
        if (playerPosition.x < 0)
        {
            Debug.Log("왼쪽 보기");
            transform.position = new Vector2(4.89f, -1.44f);

            transform.GetComponent<SpriteRenderer>().flipX = true;

            animator.SetTrigger("Empress Lose");

            playerControllerOff = true;
        }
        else if (playerPosition.x >= 0) // Empress Siren의 오른쪽에 위치
        {
            Debug.Log("오른쪽 보기");
            transform.position = new Vector2(-4.89f, -1.44f);

            transform.GetComponent<SpriteRenderer>().flipX = false;

            animator.SetTrigger("Empress Lose");

            playerControllerOff = true;
        }

        yield return new WaitForSeconds(3.5f);

        // Empress Siren이 사라지는 효과
        teleportAnimator.SetActive(true);
        transform.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds
            (teleportAnimator.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
        teleportAnimator.SetActive(false);

        Debug.Log(targetTransform.GetComponent<SpriteRenderer>().flipX);

        /// <problem> 플레이어 스프라이트의 방향을 고정하지 못하는 문제 
        SpriteRenderer forFlipX = GameObject.Find(player).transform.GetComponent<SpriteRenderer>();

        // 플레이어의 이동을 금지하기 전 스프라이트 방향 고정
        if (forFlipX.flipX == true) // 왼쪽을 보고 있다면 오른쪽을 보도록
        {
            transform.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (forFlipX.flipX == false) // 오른쪽을 보고 있더라도 오른쪽을 보도록
        {
            transform.GetComponent<SpriteRenderer>().flipX = false;
        }

        // 플레이어의 이동 막기
        targetTransform.GetComponent<PlayerController>().enabled = false;

        yield return new WaitForSeconds(3f);

        CameraShake.instance.StartCoroutine(CameraShake.instance.OpenTheDoor());

        StopCoroutine(PlayerWin());

        // 자기자신(Empress Siren 종료)
        gameObject.SetActive(false);
    }
}
