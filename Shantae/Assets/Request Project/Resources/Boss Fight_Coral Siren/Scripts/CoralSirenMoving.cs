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
    private bool patternFinished = false;

    private GameObject sandGroup;
    private GameObject firstSand;
    private GameObject secondSand;
    private GameObject thirdSand;
    private GameObject fourthSand;

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

        sandGroup = GameObject.Find("Sands");
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
            firstSand.SetActive(true);
            secondSand.SetActive(true);
            thirdSand.SetActive(true);
            fourthSand.SetActive(true);

            grabLever = false;
            GrabLever.sandActive = false;
        }

        // 패턴 연속 실행 코드
        if (firstPatternDone == true || secondPatternDone == true 
            || thirdPatternDone == true || fourthPatternDone == true)
        {
            if (patternFinished == false)
            {
                StopCoroutine(RandomMoving());
                StartCoroutine(RandomMoving());
                patternFinished = true;
            }

            firstPatternDone = false;
            secondPatternDone = false;
            thirdPatternDone = false;
            fourthPatternDone = false;

            patternFinished = false;
        }
    }

    // Update is called once per frame
    IEnumerator RandomMoving()
    {
        // 발동 조건 체크
        if (firstSand.activeSelf == false || secondSand.activeSelf == false
                || thirdSand == false || fourthSand == false)
        {
            // 모래 중 하나라도 비어있다면 바로 모래 채우는 패턴 실행
            randomAttack = 3;
        }
        else
        {
            randomAttack = Random.Range(0, 3);
        }

        yield return new WaitForSeconds(3f);

        if (randomAttack == 0)
        {
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
        else if (randomAttack == 1)
        {
            // 대시 준비 (DashCharging)
            dash = true;
           
        }
        else if (randomAttack == 2)
        {
            // 불 뿌리기 준비 (FireSpread)
            fireSpread = true;
        }
        else if (randomAttack == 3)
        {
            // 모래 채우기 준비
           grabLever = true;
        }
    }
}
