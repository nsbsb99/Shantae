using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Coral Siren이 뒤에서 점프해 양옆으로 불을 발사하는 클래스
/// </summary>

public class FireSpread : MonoBehaviour
{
    #region Coral Siren의 움직임 변수
    Transform coralSiren_Back;
    Transform coralSiren_Front;
    Animator coralSiren_Back_Animator;
    Animator coralSiren_Front_Animator;

    Vector2 coralSiren_Back_OriginPosition = default;
    Vector2 coralSiren_Front_OriginPosition = default;

    float moveSpeed = 5f;
    float upSpeed = 13f;
    float fallSpeed = 16f;

    private bool moveDone = false;
    private bool firstDestination = false;
    private bool secondDestination = false;
    private bool thirdDestination = false;
    private bool alreadyChoice = false;

    public static bool allStop = false;

    private float firstFirePosition = default;
    private float secondFirePosition = default;
    private float thirdFirePosition = default;
    private float fourthFirePosition = default;
    private float whatFirePosition = default;

    private int selectedRandom = default;
    private float coralPositionX = default;
    private float coralPositionY = default;
    #endregion

    #region 불을 발사하는 변수
    private GameObject fireBallPrefab;
    private GameObject[] fireBalls;
    private GameObject fireBall_Left;
    private GameObject fireBall_Right;

    private GameObject mainHole;
    public GameObject hole1;
    public GameObject hole2;
    public GameObject hole3;
    public GameObject hole4;

    private Vector2 poolPosition_fireBalls = new Vector2(-2, 10f);

    private bool launchFireBalls = false;
    private float fireBallSpeed = 10f;

    // bool의 활성화 이전 충돌을 체크하는 것을 막기 위함.
    public static bool collisionCheck = false;
    #endregion


    private int childCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        // 뒤 Coral Siren
        coralSiren_Back = FindObjectOfType<CoralSirenMoving>().transform;
        Debug.Assert(coralSiren_Back != null);
        // 앞 Coral Siren
        coralSiren_Front = GameObject.Find("Coral Siren_Front").transform;
        Debug.Assert(coralSiren_Front != null);

        // 뒤 Coral Siren Animator
        coralSiren_Back_Animator =
            FindObjectOfType<CoralSirenMoving>().GetComponent<Animator>();
        // 앞 Coral Siren Animator
        coralSiren_Front_Animator =
            GameObject.Find("Coral Siren_Front").GetComponent<Animator>();

        // 뒤 Coral Siren의 Origin Position
        coralSiren_Back_OriginPosition = coralSiren_Back.position;
        // 앞 Coral Siren의 Origin Position
        coralSiren_Front_OriginPosition = coralSiren_Front.position;

        firstFirePosition = -5.8f;
        secondFirePosition = -1.91f;
        thirdFirePosition = 1.88f;
        fourthFirePosition = 5.79f;


        // 불꽃 공격구
        fireBalls = new GameObject[2];

        fireBallPrefab = Resources.Load<GameObject>
            ("Boss Fight_Coral Siren/Prefabs/Fire Spread Attacks");
        Debug.Assert(fireBallPrefab != null);

        for(int i = 0; i < 2; i++)
        {
            fireBalls[i] = Instantiate(fireBallPrefab, poolPosition_fireBalls, 
                Quaternion.identity);
            Debug.Assert(fireBalls[i] != null);
        }

        fireBalls[1].transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;

        fireBall_Right = fireBalls[0];
        fireBall_Left = fireBalls[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (CoralSirenMoving.fireSpread == true && firstDestination == false)
        {
            if (moveDone == false)
            {
                if (alreadyChoice == false)
                {
                    selectedRandom = Random.Range(0, 4);
                    alreadyChoice = true;
                }

                // 착륙 장소 정하기 (네 개의 모래)
                if (selectedRandom == 0)
                {
                    whatFirePosition = firstFirePosition;
                    mainHole = hole1;
                }
                else if (selectedRandom == 1)
                {
                    whatFirePosition = secondFirePosition;
                    mainHole = hole2;
                }
                else if (selectedRandom == 2)
                {
                    whatFirePosition = thirdFirePosition;
                    mainHole = hole3;
                }
                else if (selectedRandom == 3)
                {
                    whatFirePosition = fourthFirePosition;
                    mainHole = hole4;
                }

                coralSiren_Back_Animator.SetBool("Fire Bomb", true);

                if(whatFirePosition < coralSiren_Back.position.x)
                {
                    coralSiren_Back.GetComponent<SpriteRenderer>().flipX = true;
                }
                else if (whatFirePosition > coralSiren_Back.position.x)
                {
                    coralSiren_Back.GetComponent<SpriteRenderer>().flipX = false;
                }

                // 발사 위치로 이동
                coralSiren_Back.position =
                Vector2.MoveTowards(coralSiren_Back.position,
                new Vector2(whatFirePosition, coralSiren_Back.position.y),
                moveSpeed * Time.deltaTime);

                coralPositionX = coralSiren_Back.position.x;
                coralPositionY = coralSiren_Back.position.y;

                // 발사 위치에 도달하면
                if (Mathf.Abs(whatFirePosition - coralPositionX) <= 0.1f)
                {
                    coralSiren_Back_Animator.SetBool("Spread Fire Charge", true);
                    coralSiren_Back_Animator.SetBool("Fire Bomb", false);

                    // 고정
                    coralSiren_Back.position =
                        new Vector2(whatFirePosition, coralSiren_Back.position.y);

                    moveDone = true;
                }
            }

            if (moveDone == true)
            {
                // 뒤 Coral Siren의 상승
                coralSiren_Back.position =
                    Vector2.MoveTowards(coralSiren_Back.position,
                    new Vector2(coralSiren_Back.position.x, 7f), upSpeed * Time.deltaTime);
            }

            if (coralSiren_Back.position.y >= 7f && firstDestination == false)
            {
                // 뒤 Coral Siren이 일정 고도에 도달하면
                coralSiren_Back_Animator.SetBool("Spread Fire Charge", false);
                coralSiren_Back_Animator.SetBool("Drop", true);

                // 앞 Coral Siren은 추락을 위해 좌표 이동
                coralSiren_Front.position =
                    new Vector2(coralSiren_Back.position.x, 13f);

                firstDestination = true;
            }
        }

        if (CoralSirenMoving.fireSpread == true && firstDestination == true
            && secondDestination == false && collisionCheck == false)
        {
            // 앞 Coral Siren이 추락
            coralSiren_Front.position = Vector2.MoveTowards(coralSiren_Front.position,
                new Vector2(coralSiren_Back.position.x, coralSiren_Front_OriginPosition.y),
                fallSpeed * Time.deltaTime);

            if (Mathf.Abs(coralSiren_Front.position.y - coralSiren_Front_OriginPosition.y) <= 0.01f)
            {
                coralSiren_Front.position = new Vector2
                    (coralSiren_Front.position.x, coralSiren_Front_OriginPosition.y);

                //if (FrontGrounded.coralSiren_Front_Sanded == true) // 모래와 충돌하면
                //{
                //    Debug.Log("모래 충격 감지");
                //    collisionCheck = true;

                //    StartCoroutine(Sanded());
                //}
                //else if (FrontGrounded.coralSiren_Front_Sanded == false) // 모래가 없으면
                //{
                //    Debug.Log("땅 충격 감지");
                //    collisionCheck = true;

                //    StartCoroutine(Grounded());
                //}
                Transform[] childTransforms = mainHole.GetComponentsInChildren<Transform>(true);

                // 첫 번째 요소는 부모 오브젝트이므로 제외하고 순회
                
                for (int i = 1; i < childTransforms.Length; i++)
                {
                    GameObject childObject = childTransforms[i].gameObject;
                    if (childObject.activeSelf)
                    {
                        childCount += 1;
                    }
                }

                if (childCount == 0) // 모래가 없으면
                {
                    Debug.Log(childCount);

                    Debug.Log("땅 충격 감지");
                    collisionCheck = true;

                    StartCoroutine(Grounded());
                }
                else// 모래와 충돌하면
                {
                    Debug.Log(childCount);

                    Debug.Log("모래 충격 감지");
                    collisionCheck = true;

                    StartCoroutine(Sanded());
                }
                childCount = 0;
                Debug.Log(childCount);

            }
        }

        // 불 공격 이동
        if (launchFireBalls == true)
        {
            fireBall_Left.transform.Translate
                (Vector2.left * Time.deltaTime * fireBallSpeed);
            fireBall_Right.transform.Translate
                (Vector2.right * Time.deltaTime * fireBallSpeed);

            if (secondDestination == true) // 앞의 Coral Siren이 다시 위로 상승하면
            {
                launchFireBalls = false;

                fireBall_Left.transform.position = poolPosition_fireBalls;
                fireBall_Right.transform.position = poolPosition_fireBalls;
            }
        }

        // 앞의 Coral Siren이 위로 올라가라는 신호를 받았다면
        if (CoralSirenMoving.fireSpread == true && secondDestination == true
            && thirdDestination == false)
        {
            coralSiren_Front.position = Vector2.MoveTowards(coralSiren_Front.position,
                new Vector2(coralSiren_Front.position.x, 13f),
                fallSpeed * Time.deltaTime);

            // 만약 앞의 Coral Siren이 일정 고도에 도달했다면
            if (coralSiren_Front.position.y >= 13f)
            {
                coralSiren_Front_Animator.SetTrigger("Fire_GoBack");

                // 풀로 복귀
                coralSiren_Front.position = coralSiren_Front_OriginPosition;

                thirdDestination = true;
            }
        }

        if (CoralSirenMoving.fireSpread == true && thirdDestination == true)
        {
            // 뒤의 Coral Siren이 땅으로 착륙
            coralSiren_Back.position = Vector2.MoveTowards(coralSiren_Back.position,
                    new Vector2(coralPositionX, coralPositionY),
                    fallSpeed * Time.deltaTime);

            // 착륙 성공
            if (Mathf.Abs(coralSiren_Back.position.y - coralPositionY) <= 0.1f)
            {
                coralSiren_Back_Animator.SetBool("Fire Spread", true);
                StartCoroutine(NextGrounded());
            }
        }

        if (allStop == true)
        {
            CoralSirenMoving.fireSpread = false;
            CoralSirenMoving.thirdPatternDone = true;

            StopCoroutine(Grounded());
            StopCoroutine(Sanded());
            StopCoroutine(NextGrounded());

            moveDone = false;
            firstDestination = false;
            secondDestination = false;
            thirdDestination = false;
            alreadyChoice = false;
            collisionCheck = false;

            allStop = false;
        }
    }

    IEnumerator Grounded() // 버둥거림
    {
        coralSiren_Front_Animator.SetBool("Fire_Frail", true);
        // 3초간 버둥거림
        yield return new WaitForSeconds(3.0f);
        coralSiren_Front_Animator.SetBool("Fire_Frail", false);

        // 탈출 모션 동안 대기
        yield return new WaitForSeconds
            (coralSiren_Front_Animator.GetCurrentAnimatorStateInfo(0).length);

        coralSiren_Front_Animator.SetBool("Go Back", true);

        secondDestination = true;
    }

    IEnumerator Sanded() // 불 공격
    {
        FrontGrounded.coralSiren_Front_Sanded = false;

        // 불 공격 진입 
        coralSiren_Front_Animator.SetBool("Fire", true);

        fireBall_Right.transform.position = new Vector2
            (coralSiren_Front.position.x + 3f, coralSiren_Front.position.y - 1.45f);
        fireBall_Left.transform.position = new Vector2
            (coralSiren_Front.position.x - 3f, coralSiren_Front.position.y - 1.45f);

        launchFireBalls = true;

        yield return new WaitForSeconds
            (coralSiren_Front_Animator.GetCurrentAnimatorClipInfo(0).Length);

        coralSiren_Front_Animator.SetBool("Fire", false);
        secondDestination = true;
    }

    IEnumerator NextGrounded()
    {
        coralSiren_Back_Animator.SetBool("Drop", false);

        // 뒤 Coral Siren의 착륙 모션 진행 시간 
        yield return new WaitForSeconds(1.5f);

        coralSiren_Back_Animator.SetBool("Spread Fire Charge", false);
        coralSiren_Back_Animator.SetBool("Fire Spread", false);

        allStop = true;
    }
}
