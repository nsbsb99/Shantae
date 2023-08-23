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
    private BoxCollider2D empressHitBox;

    #region 움직임 패턴 결정
    // 보스의 패턴 결정_벽 or 천장
    private float randomValue = default;
    // 보스 패턴 결정_바닥에서
    private float randomValue_Ground = default;
    // 애니메이터
    private Animator animator;
    // 플립을 위한 Sprite Renderer
    private SpriteRenderer spriteRenderer;
    // 텔레포트 효과를 위한 게임오브젝트 (애니메이션)
    private GameObject teleportAni;
    private Animator teleportAniTime = default;
    // 텔레포트 효과를 위한 파티클
    private ParticleSystem teleportParticle;
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

        teleportAni = transform.GetChild(0).gameObject;
        Debug.Assert(teleportAni!= null);
        teleportAniTime = teleportAni.GetComponent<Animator>();

        teleportParticle = transform.GetChild(1).GetComponent<ParticleSystem>();
        Debug.Assert(teleportParticle != null);
    }

    private void Start()
    {
        empressHitBox = transform.GetComponent<BoxCollider2D>();

        StartCoroutine(RandomMoving());
    }

    public IEnumerator RandomMoving()
    {
        ///<summary>
        /// 보스의 랜덤 이동과 그에 맞는 공격을 결정하는 메서드
        /// </summary>
        
        /// <point> Empress Siren의 시작 유예 시간. CameraShake와 같음. (컷신 종료 후 실행)
        yield return new WaitForSeconds(3.5f);

        spriteRenderer.flipX = false;

        // 만약 Empress Siren의 체력이 0보다 크다면 전투 지속 
        while (EmpressController.empressHP > 0)
        {
            leftWall = false;
            rightWall = false;

            randomValue = Random.Range(0, 3);
            randomValue = 2; // 임시

            if (randomValue == 0)
            {
                // 텔레포트 애니메이션 재생시간 동안 대기
                yield return new WaitForSeconds
                    (teleportAniTime.GetCurrentAnimatorStateInfo(0).length - 0.2f);
                teleportAni.SetActive(false);

                // 콜라이더 사이즈 조정
                empressHitBox.size = new Vector2(5, 4.2f);
                // 콜라이더 위치 조정
                empressHitBox.offset = new Vector2(0.8f, -0.2f);

                // 왼쪽 벽
                transform.position = new Vector2(-6.6f, 1.54f);
                animator.Play("Float and Kiss");

                yield return new WaitForSeconds(2.1f);

                leftWall = true;

                yield return new WaitForSeconds
                    (animator.GetCurrentAnimatorStateInfo(0).length - 2.1f);

                teleportAni.SetActive(true);
                teleportParticle.Play();
            }
            else if (randomValue == 1)
            {
                // 텔레포트 애니메이션 재생시간 동안 대기
                yield return new WaitForSeconds
                    (teleportAniTime.GetCurrentAnimatorStateInfo(0).length - 0.2f);
                teleportAni.SetActive(false);

                // 콜라이더 사이즈 조정
                empressHitBox.size = new Vector2(5, 4.2f);
                // 콜라이더 위치 조정
                empressHitBox.offset = new Vector2(0.8f, -0.2f);

                // 오른쪽 벽
                transform.position = new Vector2(6.43f, 1.54f);
                spriteRenderer.flipX = true;
                animator.Play("Float and Kiss");

                yield return new WaitForSeconds(2.1f);

                rightWall = true;

                yield return new WaitForSeconds
                    (animator.GetCurrentAnimatorStateInfo(0).length - 2.1f);

                teleportAni.SetActive(true);
                teleportParticle.Play();
            }
            else if (randomValue == 2)
            {
                // 텔레포트 애니메이션 재생시간 동안 대기
                yield return new WaitForSeconds
                    (teleportAniTime.GetCurrentAnimatorStateInfo(0).length - 0.2f);
                teleportAni.SetActive(false);

                // 콜라이더 사이즈 조정
                empressHitBox.size = new Vector2(3.2f, 3);
                // 콜라이더 위치 조정
                empressHitBox.offset = new Vector2(0.3f, 0.2f);

                // 천장
                transform.position = new Vector2(-0.29f, 2.18f);

                animator.Play("Ceiling");

                yield return new WaitForSeconds(1.3f);

                ceiling = true;
                CameraShake.instance.StartCoroutine(CameraShake.instance.CeilingShake());

                yield return new WaitForSeconds
                    (animator.GetCurrentAnimatorStateInfo(0).length - 1.3f);

                teleportAni.SetActive(true);
                teleportParticle.Play();
            }

            randomValue_Ground = Random.Range(0, 2);
            randomValue_Ground = 0; // 임시

            // x축으로 뒤집힌 보스를 원상태로 되돌리기
            if (spriteRenderer.flipX == true)
            {
                spriteRenderer.flipX = false;
            }

            if (randomValue_Ground == 0)
            {
                // 텔레포트 애니메이션 재생시간 동안 대기
                yield return new WaitForSeconds
                    (teleportAniTime.GetCurrentAnimatorStateInfo(0).length - 0.2f);
                teleportAni.SetActive(false);

                // 콜라이더 사이즈 조정
                empressHitBox.size = new Vector2(6.5f, 4.2f);
                // 콜라이더 위치 조정
                empressHitBox.offset = new Vector2(0.5f, -0.2f);

                // 바닥 Surf
                transform.position = new Vector2(-4.07f, -1.72f);
                animator.Play("Surf");

                yield return new WaitForSeconds(0.6f);

                surf = true;
                    
                yield return new WaitForSeconds
                    (animator.GetCurrentAnimatorStateInfo(0).length - 0.6f);

                teleportAni.SetActive(true);
                teleportParticle.Play();
            }
            else if (randomValue_Ground == 1)
            {
                // 텔레포트 애니메이션 재생시간 동안 대기
                yield return new WaitForSeconds
                    (teleportAniTime.GetCurrentAnimatorStateInfo(0).length - 0.2f);
                teleportAni.SetActive(false);

                // 콜라이더 사이즈 조정
                empressHitBox.size = new Vector2(4, 4.2f);
                // 콜라이더 위치 조정
                empressHitBox.offset = new Vector2(-0.2f, -0.2f);

                // 바닥 Hop
                transform.position = new Vector2(-4.07f, -1.44f);
                animator.Play("Hopback");

                yield return new WaitForSeconds(1.0f);

                hopBack = true;

                yield return new WaitForSeconds
                    (animator.GetCurrentAnimatorStateInfo(0).length - 1.0f);

                teleportAni.SetActive(true);
                teleportParticle.Play();
            }

            surf = false;
            hopBack = false;
        }
    }
}
