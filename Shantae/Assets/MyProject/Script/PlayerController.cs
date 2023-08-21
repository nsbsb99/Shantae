using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    public GameObject ground;
    public GameObject jump;
    public GameObject down;
    public GameObject sandCollider;

    private Vector3 originalPosition; // 원래 위치를 저장하는 변수

    private int playerHP = 50;

    public float jumpForce = 20f;
    public float fallForce;
    private bool isJumping = false;
    private bool octoJump = false;
    private bool isAir = false;
    private float jumpStartTime;
    private int jumpCount = 0;

    private Animator animator = default;

    public GameObject stepSand;
    private bool overSand = false;
    private bool inSand = false;
    private bool drillOn = false;
    private bool drillJump = false;
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
    private AudioSource playerAudio = default;
    private BoxCollider2D boxCollider;
    public float bottomY;

    private bool trigger = false;


    // Start is called before the first frame update
    void Start()
    {
        playerRigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider2D>();

        Debug.Assert(playerRigid != null);
        Debug.Assert(animator != null);
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("");

        //if(!isAir && !drillOn)
        //{
        //    isJumping = false;
        //    animator.SetBool("Jump", isJumping);

        //}
        float playerHeight = GetComponent<Renderer>().bounds.extents.y;
        bottomY = transform.position.y - playerHeight;

        animator.SetBool("isGround", !isAir);
        if (invincible && !isFlashing)
        {
            StartCoroutine(FlashPlayer(0.1f)); 
        }


        Vector2 raycastOrigin = new Vector2(transform.position.x,
            transform.position.y - GetComponent<SpriteRenderer>().bounds.extents.y); // 플레이어의 오브잭트 중앙에서 아랫쪽 끝까지의 거리 계산

        Debug.DrawRay(raycastOrigin, Vector2.down, Color.black);           // 레이케스 레이저 가시광선

        if (Physics2D.Raycast(raycastOrigin, Vector2.down, 0.1f))        //플레이어가 바닥에 있는지
        {
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, 0.1f);
            jumpCount = 0;
            if (hit.collider.CompareTag("Air") || hit.collider.CompareTag("Damage") || hit.collider.CompareTag("jewel"))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    isAir = false;
                    animator.SetBool("isGround", !isAir);

                }
                else
                {
                    jumpCount += 1;
                    isAir = true;
                    animator.SetBool("isGround", !isAir);
                    isJumping = true;
                    animator.SetBool("Jump", isJumping);
                    transform.Translate(Vector3.down * fallForce * Time.deltaTime);
                }
                
            }
            if (hit.collider.CompareTag("Ground"))
            {
                // 바닥과 충돌한 경우
                 octoJump = false;
                animator.SetBool("OctoJump", octoJump);
                isAir = false;
                animator.SetBool("isGround", !isAir);
                overSand = false;
            }
            if (hit.collider.CompareTag("SandStep"))
            {
                //Debug.Log(stepSand);
                isAir = false;
                animator.SetBool("isGround", !isAir);

                overSand = true;
                stepSand = hit.collider.gameObject;     // 밟고있는 모래의 정보를 저장
            }
            else
            {
                overSand = false;
            }
        }
        else if (!Physics2D.Raycast(raycastOrigin, Vector2.down, 0.005f))
        {
            // 바닥과 충돌하지 않은 경우
            isAir = true;
            if (!isJumping && !drillOn)
            {
                transform.Translate(Vector3.down * fallForce * Time.deltaTime);
            }
        }
        playerAnimation();          // 플레이어가 보여줄 애니메이션


        //if (Input.GetKeyDown(KeyCode.X) && !isJumping && !isAir && !isDown)      // 점프
        if (Input.GetKeyDown(KeyCode.X) && jumpCount < 3 && !isDown)      // 점프
        {
            isJumping = true;
            jumpStartTime = Time.time;
            jumpCount += 1;
            if(jumpCount == 1)
            {
                animator.SetBool("Jump", isJumping);

            }
            else if (jumpCount >= 2)
            {
                octoJump = true;
                animator.SetBool("OctoJump", octoJump);
            }
            //animator.SetBool("Jump", isJumping);
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
                        animator.SetBool("Jump", isJumping);
                    }
                    else if (jumpCount >= 2)
                    {
                        octoJump = false;
                        animator.SetBool("OctoJump", octoJump);
                    }
                }
            }
            else
            {
                isJumping = false;
                animator.SetBool("Jump", isJumping);
                if (jumpCount == 1)
                {
                }
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
                transform.Translate(Vector3.down * fallForce * Time.deltaTime);
            }
        }

        if ((!isAttack || isJumping) && !isDamage)
        {

            if (Input.GetKeyDown(KeyCode.Z))        // 공격
            {
                isAttack = true;

                if (isDown)
                {
                    originalPosition = transform.position;
                    animator.SetBool("Attack", isAttack);
                    StartCoroutine(DownAttack(0.15f));         //공격하면 

                }
                else if (!isJumping && !isDown)
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

                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);

            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
                transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
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
                    stepSand.SetActive(false);      //저장한(밟고있던)모래를 비활성화
                    boxCollider.size = new Vector2(0.7f, 0.7f);
                    boxCollider.offset = new Vector2(0f, 0f);
                    drillOn = true;
                    animator.SetBool("Drill", drillOn);
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
    #region   //공격 모션
    private IEnumerator DownAttack(float delay)
    {
        down.SetActive(true);

        originalPosition = transform.position;
        yield return new WaitForSeconds(delay); // 1초 대기

        isAttack = false;
        down.SetActive(false);

        animator.SetBool("Attack", isAttack);
    }
    private IEnumerator GroundAttack(float delay)
    {
        ground.SetActive(true);

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
                invincible = true;
                StartCoroutine(HandleInvincibleAndDamage(1.5f, 0.25f));
                //Debug.Log("123");
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
        //if (collision.tag.Equals("Air"))
        //{
        //    isAir = true;
        //    if (!isJumping && !drillOn)
        //    {
        //        transform.Translate(Vector3.down * fallForce * Time.deltaTime);
        //    }
        //}
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Sand"))
        { 
            inSand = false;
            if (trigger)
            {
                if (stepSand != null)
                {
                    stepSand.SetActive(true);
                }
                trigger = false;
            }

            //float jumptime = Time.time - jumpStartTime;

            //if (jumptime <= 0.3)
            //{
            //    isJumping = false;
            //    animator.SetBool("Jump", isJumping);
            //    transform.Translate(Vector3.up * jumpForce * Time.deltaTime);

            //}
            //else
            //{
            //    isJumping = false;
            //    animator.SetBool("Jump", isJumping);
            boxCollider.size = new Vector2(0.7f, 2.1f);
            boxCollider.offset = new Vector2(0.385f, -0.9f);
            drillOn = false;
            animator.SetBool("Drill", drillOn);

            //}
        }
    }

    private IEnumerator HandleInvincibleAndDamage(float invincibleDelay, float damageDelay)
    {
         transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        // Start Invincible coroutine
        StartCoroutine(Invincible(invincibleDelay));

        // Start Damage coroutine
        StartCoroutine(Damage(damageDelay));

        // Wait for both coroutines to finish
        yield return new WaitForSeconds(invincibleDelay);

        //originalPosition = transform.position;

        invincible = false;
    }

    private IEnumerator Damage(float delay)
    {
        isDamage = true;
        animator.SetBool("isDamage", isDamage);

        // Save the original position before the delay
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