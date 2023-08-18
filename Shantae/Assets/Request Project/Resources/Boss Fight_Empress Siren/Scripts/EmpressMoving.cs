using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

/// <summary>
/// 클래스: 보스 Empress Siren의 움직임을 구현 
/// 분석: 보스는 양쪽 벽, 천장, 바닥에서의 네 가지 패턴을 가짐. 
/// 바닥을 제외한 한 가지 패턴을 실행했다면
/// 다음 패턴은 무조건 바닥에서 진행함.
/// </summary>

public class EmpressMoving : MonoBehaviour
{
    public static EmpressMoving instance;

    #region 움직임 패턴 결정
    // 보스의 패턴 결정_벽 or 천장
    private float randomValue = default;
    // 보스 패턴 결정_바닥에서
    private float randomValue_Ground = default;
    // 애니메이터
    private Animator animator;
    // 플립을 위한 Sprite Renderer
    private SpriteRenderer spriteRenderer;
    #endregion

    #region 공격 패턴 결정
    public static bool ceiling = false;
    public static bool leftWall = false;
    public static bool rightWall = false;
    public static bool surf = false;
    public static bool hopBack = false;
    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        StartCoroutine(RandomMoving());
    }

    public IEnumerator RandomMoving()
    {
        ///<summary>
        /// 보스의 랜덤 이동과 그에 맞는 공격을 결정하는 메서드
        /// </summary>
        
        // 3.5초 후 보스가 행동을 시작하도록 지정.(후에 컷신 종료 시 시작되도록 변경 필요)
        yield return new WaitForSeconds(3.5f);

        // 만약 Empress Siren의 체력이 0보다 크다면 전투 지속 
        while (EmpressController.empressHP > 0)
        {
            randomValue = Random.Range(0, 3);
            randomValue_Ground = Random.Range(0, 2);

            randomValue = 0; // 임시

            if (randomValue == 0)
            { 
                // 왼쪽 벽
                transform.position = new Vector2(-6.6f, 1.54f);
                animator.Play("Float and Kiss");

                yield return new WaitForSeconds(2.1f);

                leftWall = true;
            }
            else if (randomValue == 1)
            {
                // 오른쪽 벽
                transform.position = new Vector2(6.43f, 1.54f);
                spriteRenderer.flipX = true;
                animator.Play("Float and Kiss");

                yield return new WaitForSeconds(2.1f);

                rightWall = true;
            }
            else if (randomValue == 2)
            {
                // 천장
                transform.position = new Vector2(-0.29f, 2.18f);

                animator.Play("Ceiling");

                yield return new WaitForSeconds(1.3f);

                ceiling = true;
            }

            yield return new WaitForSeconds(1.0f);
            yield return new WaitForSeconds
                (animator.GetCurrentAnimatorStateInfo(0).length);

            leftWall = false;
            rightWall = false;

            // x축으로 뒤집힌 보스를 원상태로 되돌리기
            if (spriteRenderer.flipX == true)
            {
                spriteRenderer.flipX = false;
            }

            if (randomValue_Ground == 0)
            {
                // 바닥 Surf
                transform.position = new Vector2(-4.07f, -1.72f);
                animator.Play("Surf");

                yield return new WaitForSeconds(0.6f);

                surf = true;
            }
            else if (randomValue_Ground == 1)
            {
                // 바닥 Hop
                transform.position = new Vector2(-4.07f, -1.44f);
                animator.Play("Hopback");

                yield return new WaitForSeconds(1.0f);

                hopBack = true;
            }

            yield return new WaitForSeconds(1.0f);
            yield return new WaitForSeconds
                (animator.GetCurrentAnimatorStateInfo(0).length);

            surf = false;
            hopBack = false;
        }
    }
}
