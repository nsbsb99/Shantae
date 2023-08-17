using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 폭탄을 발사하고 발사 간격을 조정하는 클래스
/// </summary>

public class FireBomb : MonoBehaviour
{
    private GameObject bombPrefab;
    private GameObject[] bombs;

    private int firstBombCount = 10;
    private int secondBombCount = 20;
    private int thirdBombCount = 45;
    private int maxBombCount = default; // 동작을 확인하기 위해 임시로 하나만

    private float firstInterval = 0.5f;
    private float secondInterval = 0.3f;
    private float thirdInterval = 0.1f;

    private float firstBossSpeed = 3f;
    private float secondBossSpeed = 4.5f;
    private float thirdBossSpeed = 5.5f;

    private int whatBombPattern = default;

    private bool readyFire = false;
    public static bool doneFire = false;
    public static bool wellDone = false;

    private bool firstCoroutineDone = false;
    private bool secondCoroutineDone = false;
    private bool thirdCoroutineDone = false;

    private bool bossNowMove = false;

    private Vector2 poolPosition_bomb = new Vector2(0f, 10f);
    private Vector2 firePosition;

    public static BombUpDown lastBombUpDown;
    private Animator animator;
    private Transform wherePlayer;

    private float wherePlayerX;
    private float wherePlayerY;
    private float whereBossX;
    private float whereBossY;

    private void Start()
    {
        bombPrefab = Resources.Load<GameObject>
            ("Boss Fight_Coral Siren/Prefabs/Bomb");

        // 풀에 생성할 폭탄의 수 (44개의 폭탄만 쓰인다.)
        maxBombCount = thirdBombCount;

        bombs = new GameObject[maxBombCount];

        for (int i = 0; i < maxBombCount; i++)
        {
            bombs[i] = Instantiate(bombPrefab, poolPosition_bomb, Quaternion.identity);
            Debug.Assert(bombs[i] != null);

            bombs[i].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            Debug.Assert(bombs[i].transform.GetChild(0).GetComponent<SpriteRenderer>()
                != null);
        }

        // 마지막 폭탄이 풀에 도착한 것이 확인된다면
        lastBombUpDown = bombs[maxBombCount - 1].GetComponent<BombUpDown>();
        Debug.Assert(lastBombUpDown != null);

        animator = transform.GetComponent<Animator>();

        // 발사 패턴 순서
        whatBombPattern = 0;

        // 플레이어 트랜스폼
        wherePlayer = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // 발사 시작 코루틴
        if (CoralSirenMoving.fireBomb == true && readyFire == false)
        {
            // 첫번째 코루틴 시작
            if (whatBombPattern == 0 && firstCoroutineDone == false)
            {
                StartCoroutine(FirstBombsFire());
            }

            // 두번째 코루틴 시작
            if (whatBombPattern == 1 && secondCoroutineDone == false)
            {
                StopCoroutine(FirstBombsFire());
                StartCoroutine(SecondBombsFire());
            }

            // 세번째 코루틴 시작
            if (whatBombPattern == 2 && thirdCoroutineDone == false)
            {
                StopCoroutine(SecondBombsFire());
                StartCoroutine(ThirdBombsFire());
            }
        }

        // 발사하는 동안 보스는 플레이어를 쫓음.
        if (bossNowMove == true && animator.GetBool("Fire Bomb") == true)
        {
            whereBossX = transform.position.x;

            if (Mathf.Abs(whereBossX - PlayerController.playerPosition.x) >= 0.5f)
            {
                if (PlayerController.playerPosition.x < transform.position.x)
                {
                    // 보스 좌측 이동
                    if (firstCoroutineDone == true)
                    {
                        transform.GetComponent<SpriteRenderer>().flipX = true;

                        transform.Translate(Vector2.left * firstBossSpeed * Time.deltaTime);
                    }

                    if (secondCoroutineDone == true)
                    {

                        transform.GetComponent<SpriteRenderer>().flipX = true;

                        transform.Translate(Vector2.left * secondBossSpeed * Time.deltaTime);
                    }

                    if (thirdCoroutineDone == true)
                    {

                        transform.GetComponent<SpriteRenderer>().flipX = true;

                        transform.Translate(Vector2.left * thirdBossSpeed * Time.deltaTime);
                    }
                }
                else if (PlayerController.playerPosition.x > transform.position.x)
                {
                    // 보스 우측 이동
                    if (firstCoroutineDone == true)
                    {

                        transform.GetComponent<SpriteRenderer>().flipX = false;

                        transform.Translate(Vector2.right * firstBossSpeed * Time.deltaTime);
                    }

                    if (secondCoroutineDone == true)
                    {

                        transform.GetComponent<SpriteRenderer>().flipX = false;

                        transform.Translate(Vector2.right * secondBossSpeed * Time.deltaTime);
                    }

                    if (thirdCoroutineDone == true)
                    {

                        transform.GetComponent<SpriteRenderer>().flipX = false;

                        transform.Translate(Vector2.right * thirdBossSpeed * Time.deltaTime);
                    }
                }
            }
        }

        // 전부 발사하면 발사 코루틴 종료 && 다음 공격 패턴 준비
        if (CoralSirenMoving.fireBomb == true && doneFire == true)
        {
            StopCoroutine(ThirdBombsFire());

            doneFire = false;
        }

        // 마지막 폭탄이 풀에 도착한 것이 확인되면
        if (lastBombUpDown.allBack == true)
        {
            // 시작 키워드 끄기
            CoralSirenMoving.fireBomb = false;
            CoralSirenMoving.firstPatternDone = true;

            // 모든 체크 요소 초기화
            readyFire = false;
            whatBombPattern = 0;
            firstCoroutineDone = false;
            secondCoroutineDone = false;
            thirdCoroutineDone = false;

            for (int i = 0; i < maxBombCount; i++)
            {
                // 폭탄 발사에 쓰인 모든 bool 변수 초기화
                bombs[i].GetComponent<BombUpDown>().upDone = false;
                bombs[i].GetComponent<BombUpDown>().falling = false;
                bombs[i].GetComponent<BombUpDown>().backThePool = false;
                bombs[i].GetComponent<BombUpDown>().alreadyRun = false;
                bombs[i].GetComponent<BombUpDown>().allBack = false;

                // 모든 폭탄 이동 스크립트 끄기
                bombs[i].GetComponent<BombUpDown>().enabled = false;
            }
        }
    }

    IEnumerator FirstBombsFire()
    {
        firstCoroutineDone = true;

        animator.SetBool("Fire Bomb", true);

        // 애니메이션 싱크
        yield return new WaitForSeconds(1f);

        bossNowMove = true;

        /// <caution> 폭탄을 74개 생성해 아래와 같이 지정해봤지만 정작 쓰이는 것은 세번째 발사에 
        /// 쓰이는 폭탄 최댓값인 45개 뿐이다.

        for (int i = 0; i < firstBombCount; i++)
        {
            // 발사 위치로
            firePosition = new Vector2
                (transform.position.x, transform.position.y + 1.5f);
            bombs[i].transform.position = firePosition;

            // 발사 시작
            bombs[i].GetComponent<BombUpDown>().enabled = true;

            yield return new WaitForSeconds(firstInterval);
        }

        // 애니메이션 기본 상태로
        animator.SetBool("Fire Bomb", false);
        bossNowMove = false;

        // 다음 발사까지 대기
        yield return new WaitForSeconds(0.7f);

        whatBombPattern = 1;
    }

    IEnumerator SecondBombsFire()
    {
        secondCoroutineDone = true;

        animator.SetBool("Fire Bomb", true);

        // 애니메이션 싱크
        yield return new WaitForSeconds(1f);
        bossNowMove = true;

        for (int i = firstBombCount; i < secondBombCount; i++)
        {
            // 발사 위치로
            firePosition = new Vector2
                (transform.position.x, transform.position.y + 1.5f);
            bombs[i].transform.position = firePosition;

            // 발사 시작
            bombs[i].GetComponent<BombUpDown>().enabled = true;

            yield return new WaitForSeconds(secondInterval);
        }

        // 애니메이션 기본 상태로
        animator.SetBool("Fire Bomb", false);
        bossNowMove = false;

        // 다음 발사까지 대기
        yield return new WaitForSeconds(0.5f);

        whatBombPattern = 2;
    }

    IEnumerator ThirdBombsFire()
    {
        thirdCoroutineDone = true;

        readyFire = true;

        animator.SetBool("Fire Bomb", true);

        // 애니메이션 싱크
        yield return new WaitForSeconds(1f);
        bossNowMove = true;

        for (int i = secondBombCount; i < thirdBombCount; i++)
        {
            // 발사 위치로
            firePosition = new Vector2
                (transform.position.x, transform.position.y + 1.5f);
            bombs[i].transform.position = firePosition;

            // 발사 시작
            bombs[i].GetComponent<BombUpDown>().enabled = true;

            yield return new WaitForSeconds(thirdInterval);
        }

        whatBombPattern = 3;

        // 애니메이션 기본 상태로
        animator.SetBool("Fire Bomb", false);
        bossNowMove = false;

        doneFire = true;
    }
}
