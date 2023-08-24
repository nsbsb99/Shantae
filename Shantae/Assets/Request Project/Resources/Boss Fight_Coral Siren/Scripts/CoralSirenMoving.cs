using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 보스 Coral Siren의 공격 패턴을 정하는 클래스. 
/// </summary>

public class CoralSirenMoving : MonoBehaviour
{
    public static CoralSirenMoving instance;

    private int randomAttack = default;
    public static bool fireBomb = false;
    public static bool dash = false;
    public static bool fireSpread = false;
    public static bool grabLever = false;
    private Animator animator;
    public static Vector2 newBossPosition;

    public static bool firstPatternDone = false;
    public static bool secondPatternDone = false;
    public static bool thirdPatternDone = false;
    public static bool fourthPatternDone = false;
    private bool patternPlay = false;

    private GameObject sandGroup;
    private GameObject firstSand;
    private GameObject secondSand;
    private GameObject thirdSand;
    private GameObject fourthSand;

    // 중복 실행 버그를 막기 위함. 
    private bool notDuplication = false;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        sandGroup = GameObject.Find("Sand");
        Debug.Assert(sandGroup != null);

        firstSand = sandGroup.transform.GetChild(0).gameObject;
        secondSand = sandGroup.transform.GetChild(1).gameObject;
        thirdSand = sandGroup.transform.GetChild(2).gameObject;
        fourthSand = sandGroup.transform.GetChild(3).gameObject;

        // 패턴 시작
        StartCoroutine(RandomMoving());
    }

    private void Update()
    {
        // 불 뿌리기가 끝나면 초기화 신호 뿌리기
        if (FireSpread.allStop == true)
        {
            fireSpread = false;
        }

        // 레버를 당기는 액션을 취했다면 모래를 채우고 초기화 신호 뿌리기
        if (GrabLever.sandActive == true)
        {
            for (int i = 0; i < 7; i++)
            {
                firstSand.transform.GetChild(i).gameObject.SetActive(true);
                secondSand.transform.GetChild(i).gameObject.SetActive(true);
                thirdSand.transform.GetChild(i).gameObject.SetActive(true);
                fourthSand.transform.GetChild(i).gameObject.SetActive(true);
            }
            GrabLever.sandActive = false;
        }

        // 네 패턴 중 하나라도 완전히 끝났다면
        if ((firstPatternDone == true || secondPatternDone == true
            || thirdPatternDone == true || fourthPatternDone == true))
        {
            StopCoroutine(RandomMoving());

            // 전부 초기화하고 패턴 코루틴 다시 실행 
            firstPatternDone = false;
            secondPatternDone = false;
            thirdPatternDone = false;
            fourthPatternDone = false;

            patternPlay = false;

            StartCoroutine(RandomMoving());
        }
    }

    // Update is called once per frame
    IEnumerator RandomMoving()
    {
        // 모래 채우기 발동 조건 체크
        for (int i = 0; i < 7; i++)
        {
            if (firstSand.transform.GetChild(i).gameObject.activeSelf == false ||
                secondSand.transform.GetChild(i).gameObject.activeSelf == false ||
                thirdSand.transform.GetChild(i).gameObject.activeSelf == false ||
                fourthSand.transform.GetChild(i).gameObject.activeSelf == false)
            {
                // 모래 중 하나라도 비어있다면 바로 모래 채우는 패턴 실행
                randomAttack = 3;
            }
            else
            {
                randomAttack = Random.Range(0, 3);
            }   
        }
       
        // 다음 패턴 재생 대기 시간
        yield return new WaitForSeconds(1.5f);

        if (randomAttack == 0 && patternPlay == false)
        {
            patternPlay = true;

            Debug.Log("1. 폭탄 발사");

            // 폭탄 발사
            animator.SetBool("Fire Bomb", true);

            fireBomb = true;

            // 폭탄 발사 완료
            if (FireBomb.doneFire == true)
            {
                animator.SetBool("Fire Bomb", false);

                fireBomb = false;
            }
        }
        else if (randomAttack == 1 && patternPlay == false)
        {
            patternPlay = true;

            Debug.Log("2. 대시");

            // 대시 준비 (DashCharging)
            dash = true;
        }
        else if (randomAttack == 2 && patternPlay == false)
        {
            patternPlay = true;

            Debug.Log("3. 불 뿌리기");

            // 불 뿌리기 준비 (FireSpread)
            fireSpread = true;
        }
        else if (randomAttack == 3 && patternPlay == false)
        {
            patternPlay = true;

            Debug.Log("4. 모래 채우기");

            // 모래 채우기 준비
            grabLever = true;
        }
    }
}
