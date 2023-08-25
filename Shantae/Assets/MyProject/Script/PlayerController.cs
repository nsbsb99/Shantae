using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    public GameObject ground;
    public GameObject jump;
    public GameObject down;
    public GameObject sandCollider;
    public float fadeDuration = 1.0f;

    private Vector3 originalPosition; // 원래 위치를 저장하는 변수

    public static int playerHP = 50;

    public float jumpForce = 20f;
    public float fallForce;
    private bool isJumping = false;
    private bool octoJump = false;
    private bool isAir = false;
    private float jumpStartTime;
    private int jumpCount = 0;

    private Animator animator = default;

    private SpriteRenderer spriteRenderer;

    public GameObject stepSand;
    private bool overSand = false;
    private bool inSand = false;
    private bool drillOn = false;
    private bool drillJump = false;
    private Vector3 headDirection = Vector3.up;
    private List<GameObject> breakSand = new List<GameObject>();            // 부숴진 모래들의 리스트 (레버 당기면 활성화)
    public List<GameObject> deactivatedParents = new List<GameObject>();

    private bool isAttack = false;
    private bool isRun;
    private bool isDown;
    private bool isDownAndRun;
    private bool isDamage = false;
    private bool invincible = false;
    private bool isFlashing = false;

    private Rigidbody2D playerRigid = default;
    private AudioSource playerAudio;
    public AudioClip hurt;
    public AudioClip hair;
    public AudioClip jumping;
    public AudioClip drill;
    public AudioClip octo;
    public AudioClip doubleJump;
    public AudioClip dieVioce;
    public bool isEmpress;
    private int maxJump = 3;

    private BoxCollider2D boxCollider;
    public float bottomY;

    // === (노솔빈 수정)
    public static Vector2 playerPosition = default;
    public static bool gotDamage = false;
    // ===

    private bool trigger = false;
    private float count = 0;
    public GameObject deathParticle;
    private bool triggerDie1;
    private bool triggerDie2;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        playerRigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider2D>();

        Debug.Assert(playerRigid != null);
        Debug.Assert(animator != null);

        if (!isEmpress)
        {
            maxJump = 1;
        }
        // === (노솔빈 수정)
        if (transform.GetComponent<PlayerEntry>() != null)
        {
            transform.GetComponent<PlayerEntry>().enabled = false;
        }
        // ===
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(playerHP);
        if (playerHP <= 0)
        {
            //Debug.Log("!");
            Die();
        }
        // === (노솔빈 수정) 플레이어의 좌표를 실시간으로 뿌림.
        playerPosition = transform.position;
        // ===

        float playerHeight = GetComponent<Renderer>().bounds.extents.y;
        bottomY = transform.position.y - playerHeight;

        animator.SetBool("isGround", !isAir);
        if (invincible && !isFlashing)
        {
            StartCoroutine(FlashPlayer(0.1f));
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            isJumping = false;

            animator.SetBool("Jump", isJumping);
        }

        if (!(playerHP <= 0))
        {
            Vector2 raycastOrigin = new Vector2(transform.position.x,
            transform.position.y - GetComponent<SpriteRenderer>().bounds.extents.y); // 플레이어의 오브잭트 중앙에서 아랫쪽 끝까지의 거리 계산

            Debug.DrawRay(raycastOrigin, Vector2.down, Color.black);           // 레이케스트 레이저 가시광선
            RaycastHit2D[] hits = Physics2D.RaycastAll(raycastOrigin, Vector2.down, 0.1f);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {

                    //if (hit.collider.CompareTag("Damage") || hit.collider.CompareTag("jewel"))
                    //{
                    //     Debug.Log("플레이어의 위치: " + hit.collider);
                    //    if (hit.collider.CompareTag("Ground"))
                    //    {
                    //        jumpCount = 0;
                    //        isAir = false;
                    //        animator.SetBool("isGround", !isAir);
                    //        isJumping = false;
                    //        animator.SetBool("Jump", isJumping);
                    //    }
                    //    else
                    //    {
                    //        jumpCount += 1;
                    //        isAir = true;
                    //        animator.SetBool("isGround", !isAir);
                    //        isJumping = true;
                    //        animator.SetBool("Jump", isJumping);
                    //        if (playerHP > 0)
                    //        {
                    //            transform.Translate(Vector3.down * fallForce * Time.deltaTime);
                    //        }
                    //    }
                    //}

                    if (hit.collider.CompareTag("Ground"))
                    {
                        jumpCount = 0;

                        // 바닥과 충돌한 경우
                        octoJump = false;
                        animator.SetBool("OctoJump", octoJump);
                        isAir = false;
                        animator.SetBool("isGround", !isAir);
                        overSand = false;
                        playerRigid.gravityScale = 1;
                    }
                    else if (hit.collider.CompareTag("SandStep"))
                    {
                        jumpCount = 0;

                        isAir = false;
                        animator.SetBool("isGround", !isAir);

                        overSand = true;
                        stepSand = hit.collider.gameObject;     // 밟고있는 모래의 정보를 저장
                        playerRigid.gravityScale = 1;
                    }
                    else
                    {
                        overSand = false;
                        if (playerHP > 0)
                        {
                            transform.Translate(Vector3.down * fallForce * Time.deltaTime);
                        }
                    }
                }
            }

            if (hits.Length == 0)
            {
                // 바닥과 충돌하지 않은 경우
                if (!drillOn)
                {
                    playerRigid.gravityScale = 0;
                }

                isAir = true;
                if (!isJumping && !drillOn)
                {
                    if (jumpCount == 0)
                    {
                        jumpCount += 1;
                    }

                    if (playerHP > 0)
                    {
                        transform.Translate(Vector3.down * fallForce * Time.deltaTime);
                    }
                }
            }
            playerAnimation();          // 플레이어가 보여줄 애니메이션


            if (Input.GetKeyDown(KeyCode.X) && jumpCount < maxJump && !isDown)      // 점프
            {
                isJumping = true;
                jumpStartTime = Time.time;
                jumpCount += 1;
                if (jumpCount == 1)
                {
                    animator.SetBool("Jump", isJumping);
                    playerAudio.clip = jumping;
                    playerAudio.Play();
                }
                else if (jumpCount >= 2 && isEmpress)
                {
                    if (jumpCount == 2)
                    {
                        playerAudio.clip = octo;
                        playerAudio.Play();
                    }
                    else if (jumpCount == 3)
                    {
                        playerAudio.clip = doubleJump;
                        playerAudio.Play();
                    }
                    octoJump = true;
                    animator.SetBool("OctoJump", octoJump);
                }
                if (Input.GetKeyDown(KeyCode.Z))        // 공격
                {
                    animator.SetBool("Attack", isAttack);

                    StartCoroutine(AirAttack(0.15f));
                }
            }

            if (isJumping)
            {
                float jumptime = Time.time - jumpStartTime;

                if (jumptime <= 0.3)
                {
                    transform.Translate(Vector3.up * jumpForce * Time.deltaTime);
                    if (Input.GetKeyUp(KeyCode.X))
                    {
                        isJumping = false;
                        if (jumpCount == 1)
                        {
                            Debug.Log(jumpCount);

                            animator.SetBool("Jump", isJumping);
                        }
                        else if (jumpCount >= 2 && isEmpress)
                        {

                            octoJump = false;
                            animator.SetBool("OctoJump", octoJump);
                            Debug.Log(jumpCount);

                        }
                    }
                }
                else
                {
                    isJumping = false;
                    animator.SetBool("Jump", isJumping);
                    if (jumpCount == 1)
                    { }
                    else
                    {
                        octoJump = false;
                        animator.SetBool("OctoJump", octoJump);
                    }
                }
            }
            else
            {
                if (jumpCount > 0)
                {
                    if (playerHP > 0)
                    {

                        transform.Translate(Vector3.down * fallForce * Time.deltaTime);

                    }
                }

            }

            if ((!isAttack || isJumping) && !isDamage)
            {
                if (Input.GetKeyDown(KeyCode.Z) && !drillOn)        // 공격
                {
                    isAttack = true;
                    playerAudio.clip = hair;
                    if (isDown)
                    {
                        originalPosition = transform.position;
                        animator.SetBool("Attack", isAttack);
                        StartCoroutine(DownAttack(0.15f));         //공격하면 

                    }
                    else if (!isAir && !isDown)
                    {
                        originalPosition = transform.position;
                        animator.SetBool("Attack", isAttack);

                        StartCoroutine(GroundAttack(0.15f));         //공격하면 
                    }
                    else if (isAir)
                    {
                        animator.SetBool("Attack", isAttack);

                        StartCoroutine(AirAttack(0.15f));
                    }
                }

                if (Input.GetKey(KeyCode.RightArrow))
                {
                    if (drillOn)
                    {
                        transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);

                        animator.SetTrigger("DrillRight");
                        animator.ResetTrigger("DrillLeft");
                    }
                    else
                    {
                        transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
                    }
                    transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    if (drillOn)
                    {
                        transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
                        animator.SetTrigger("DrillLeft");
                        animator.ResetTrigger("DrillRight");
                    }
                    else
                    {
                        transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
                    }
                    transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.DownArrow) && !isAir)
                {
                    if (!overSand && !drillOn)
                    {
                        boxCollider.size = new Vector2(1.5f, 0.9f);
                        boxCollider.offset = new Vector2(0f, -1.45f);
                        isDown = true;
                    }
                    else if (overSand)
                    {
                        playerAudio.clip = drill;
                        playerAudio.Play();
                        playerRigid.gravityScale = 1;
                        stepSand.SetActive(false);      //저장한(밟고있던)모래를 비활성화
                        boxCollider.size = new Vector2(0.7f, 0.7f);
                        boxCollider.offset = new Vector2(0f, 0f);
                        drillOn = true;
                        animator.SetBool("Drill", drillOn);
                        animator.SetTrigger("DrillDown");
                        animator.ResetTrigger("DrillRight");
                        animator.ResetTrigger("DrillLeft");

                    }
                }
                if (Input.GetKeyUp(KeyCode.DownArrow) && !drillOn)
                {

                    isDown = false;
                    isDownAndRun = false;
                    boxCollider.size = new Vector2(0.7f, 2f);
                    boxCollider.offset = new Vector2(0.385f, -0.9f);
                }
            }
        }
    }

    private void playerAnimation()      // 플레이어 지상 애니메이션
    {
        if (!isDown)
        {
            moveSpeed = 10f;
            if (Input.GetKey(KeyCode.RightArrow))
            {
                isRun = true;


                animator.SetBool("Run", isRun);
                Debug.Assert(animator != null);
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    boxCollider.size = new Vector2(1.3f, 0.9f);
                    boxCollider.offset = new Vector2(0f, -0.7f);

                    isRun = false;
                    animator.SetBool("Run", isRun);

                    isDownAndRun = true;
                    animator.SetBool("DownRun", isDownAndRun);

                    isDown = true;
                    animator.SetBool("Down", isDown);
                }
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                isRun = true;

                animator.SetBool("Run", isRun);
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    boxCollider.size = new Vector2(1.3f, 0.9f);
                    boxCollider.offset = new Vector2(0f, -0.7f);

                    isRun = false;
                    animator.SetBool("Run", isRun);

                    isDownAndRun = true;
                    animator.SetBool("DownRun", isDownAndRun);

                    isDown = true;
                    animator.SetBool("Down", isDown);

                }
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                isDown = true;

            }
            else if (!Input.anyKey)
            {
                isRun = false;
                isDown = false;
                animator.SetBool("Run", isRun);
                animator.SetBool("Down", isDown);

            }
        }
        else
        {
            moveSpeed = 5f;
            if (Input.GetKey(KeyCode.RightArrow))
            {
                isDownAndRun = true;


                animator.SetBool("DownRun", isDownAndRun);
                Debug.Assert(animator != null);
                if (Input.GetKeyUp(KeyCode.DownArrow))
                {
                    isDown = false;
                    animator.SetBool("Down", isDown);

                    isDownAndRun = false;
                    animator.SetBool("DownRun", isDownAndRun);

                    isRun = true;
                    animator.SetBool("Run", isRun);
                }

            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                isDownAndRun = false;
                animator.SetBool("DownRun", isDownAndRun);

            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                isDownAndRun = true;

                animator.SetBool("DownRun", isDownAndRun);
                Debug.Assert(animator != null);
                if (Input.GetKeyUp(KeyCode.DownArrow))
                {
                    isDown = false;
                    animator.SetBool("Down", isDown);


                    isDownAndRun = false;
                    animator.SetBool("DownRun", isDownAndRun);

                    isRun = true;
                    animator.SetBool("Run", isRun);
                }
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                isDownAndRun = false;
                animator.SetBool("DownRun", isDownAndRun);

            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                animator.SetBool("Down", isDown);
                Debug.Assert(animator != null);
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                isDown = false;
                isDownAndRun = false;
                animator.SetBool("Down", isDown);
                animator.SetBool("DownRun", isDownAndRun);
                if (!drillOn)
                {
                    boxCollider.size = new Vector2(0.7f, 2.1f);
                    boxCollider.offset = new Vector2(0.385f, -0.9f);
                }
            }
            else if (!Input.anyKey)
            {
                isDownAndRun = false;
                isDown = false;
                animator.SetBool("Down", isDown);
                animator.SetBool("DownRun", isDownAndRun);
            }
        }

    }
    private void Die()
    {
        //Debug.Log(count);
        if (playerHP <= 0)
        {
            count += Time.deltaTime;
        }
        animator.SetTrigger("Die");
        if (!triggerDie1)
        {
            Color newColor = Color.Lerp(spriteRenderer.color, Color.black, 1 * Time.deltaTime);
            spriteRenderer.color = newColor;
        }
        if (count > 3 && count < 7)
        {
            if (!triggerDie2)
            {
                triggerDie1 = true;
                deathParticle.SetActive(true);
                Debug.Log("3초 지남");
                playerAudio.clip = dieVioce;
                playerAudio.Play();
                Color newColor2 = spriteRenderer.color;
                newColor2.a = 0.0f;
                spriteRenderer.color = newColor2;
                triggerDie2 = true;
            }

        }
        else if (count >= 7)
        {
            if (triggerDie2) // 7초가 지난 후에만 실행되도록 확인
            {

                // 원래대로 돌리는 부분
                Color originalColor = spriteRenderer.color;
                spriteRenderer.color = originalColor;

                deathParticle.SetActive(false);
                SceneManager.LoadScene("Lobby");/*, LoadSceneMode.Single);*/
                playerHP = 50;
                triggerDie2 = false; // 원래대로 돌아간 후 다시 false로 설정
                triggerDie1 = false;
                //Vector3 newPosition = new Vector3(-11f, 0f, 0f); // 새로운 위치 설정
                //transform.position = newPosition;
            }

        }


    }
    #region   //공격 모션
    private IEnumerator DownAttack(float delay)
    {
        down.SetActive(true);
        playerAudio.Play();

        originalPosition = transform.position;
        yield return new WaitForSeconds(delay); // 1초 대기

        isAttack = false;
        down.SetActive(false);

        animator.SetBool("Attack", isAttack);
    }
    private IEnumerator GroundAttack(float delay)
    {
        ground.SetActive(true);
        playerAudio.Play();

        originalPosition = transform.position;
        yield return new WaitForSeconds(delay); // 1초 대기

        isAttack = false;
        ground.SetActive(false);

        animator.SetBool("Attack", isAttack);
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            isDown = false;
            isDownAndRun = false;
            boxCollider.size = new Vector2(0.7f, 2f);
            boxCollider.offset = new Vector2(0.385f, -0.9f);
        }
    }
    private IEnumerator AirAttack(float delay)
    {
        jump.SetActive(true);
        playerAudio.Play();

        yield return new WaitForSeconds(delay); // 1초 대기

        isAttack = false;
        jump.SetActive(false);
        octoJump = false;

        animator.SetBool("OctoJump", octoJump);
        animator.SetBool("Attack", isAttack);
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag.Equals("Damage"))
        {
            if (!invincible)        // 무적시간
            {
                playerHP -= 1;
                if (playerHP > 1)
                {
                    playerAudio.clip = hurt;
                    playerAudio.Play();

                    invincible = true;
                    StartCoroutine(HandleInvincibleAndDamage(1.5f, 0.25f));
                }

                // === (노솔빈 수정) 플레이어의 데미지를 게임매니저에 전달
                // 무적시간 동안 추가 피격 없도록 수정이 필요. 
                gotDamage = true;
                // ===
            }
        }
        if (collision.tag.Equals("Sand"))
        {
            inSand = true;
            trigger = true;
        }

        if (collision.tag.Equals("SandPiece") && drillOn)
        {
            Transform parentTransform = collision.transform.parent;

            if (parentTransform != null)
            {
                GameObject sandPiece = collision.gameObject;
                sandPiece.SetActive(false); // 부모 오브젝트 비활성화

                deactivatedParents.Add(sandPiece); // 비활성화된 부모 오브젝트를 리스트에 추가
            }
        }
        if (collision.tag.Equals("end"))
        {
            playerHP = 0;
            Die();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Sand"))
        {
            jumpCount = 3;
            inSand = false;
            if (trigger)
            {
                if (stepSand != null)
                {
                    stepSand.SetActive(true);
                }
                trigger = false;
            }

            boxCollider.size = new Vector2(0.7f, 2.1f);
            boxCollider.offset = new Vector2(0.385f, -0.9f);
            drillOn = false;
            animator.SetBool("Drill", drillOn);
            animator.ResetTrigger("DrillRight");
            animator.ResetTrigger("DrillLeft");
        }
    }

    private IEnumerator HandleInvincibleAndDamage(float invincibleDelay, float damageDelay)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        StartCoroutine(Invincible(invincibleDelay));

        StartCoroutine(Damage(damageDelay));

        yield return new WaitForSeconds(invincibleDelay);


        invincible = false;
    }

    private IEnumerator Damage(float delay)
    {
        isDamage = true;
        animator.SetBool("isDamage", isDamage);

        originalPosition = transform.position;

        yield return new WaitForSeconds(delay);

        isDamage = false;
        animator.SetBool("isDamage", isDamage);
    }

    private IEnumerator Invincible(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
    private IEnumerator FlashPlayer(float interval)
    {
        isFlashing = true;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color originalColor = spriteRenderer.color;
        Color transparentColor = originalColor;
        transparentColor.a = 0.5f;

        float blinkTime = 0;

        while (invincible)
        {
            spriteRenderer.color = transparentColor;
            yield return new WaitForSeconds(interval);

            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(interval);
            blinkTime += interval * 2;
        }

        spriteRenderer.color = originalColor;
        isFlashing = false;
    }
}