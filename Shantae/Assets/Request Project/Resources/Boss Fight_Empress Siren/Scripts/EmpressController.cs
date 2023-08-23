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
    // 백그라운드 교체를 위함
    private GameObject background = default;

    private void Start()
    {
        // Empress Siren 게임 오브젝트가 !null인지 확인
        Debug.Assert(this.gameObject != null);

        animator = GetComponent<Animator>();
        teleportAnimator = transform.GetChild(0).gameObject;
        teleportParticle = transform.GetChild(1).gameObject;

        background = GameObject.Find("Backgrounds");

        empressHP = 100f;
    }

    private void Update()
    {
        // Empress Siren의 패배 확인
        if (empressHP <= 0)
        {
            // 플레이어가 입힌 데미지는 4부터 9까지의 정수
            getDamage = Random.Range(4, 10);
            // Empress HP 깎기. 
            empressHP -= getDamage;

            // EmpressMoving 코루틴 종료 메서드 추가
            StopCoroutine(EmpressMoving.instance.RandomMoving());

            StartCoroutine(PlayerWin());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /// <problem> 플레이어의 공격이 잘 들어가지 않는다. 밑 코루틴까지 같이 보기. 
        if (collision.CompareTag("PlayerAttack"))
        {
            StartCoroutine(FlashEmpress());
        }
    }

    private IEnumerator FlashEmpress()
    {
        Debug.Log("적 피격 코루틴 진입");

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color originalColor = spriteRenderer.color;
        Color transparentColor = originalColor;
        transparentColor.a = 0.5f;

        float blinkTime = 1;
        float nowTime = 0;

        while (nowTime < blinkTime)
        {
            spriteRenderer.color = transparentColor;
            yield return new WaitForSeconds(0.2f);

            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(0.2f);
            blinkTime += Time.deltaTime;
        }

        spriteRenderer.color = originalColor;

        StopCoroutine(FlashEmpress());
    }

    private IEnumerator PlayerWin()
    {
        if (empressHP <= 0)
        {
            animator.SetTrigger("Empress Lose");
            yield return new WaitForSeconds(3);

            teleportAnimator.SetActive(true);
            teleportParticle.GetComponent<ParticleSystem>().Play();

            background.transform.GetChild(1).gameObject.SetActive(true);
            background.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("end"))
        {
            AllSceneManager.instance.StartCoroutine(AllSceneManager.instance.OpenLoadingScene_Second());
        }
    }
}
