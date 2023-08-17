using System.Collections;
using System.Collections.Generic;
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
    private Animator animator;
    public static Vector2 newBossPosition;

    public static bool firstPatternDone = false;
    public static bool secondPatternDone = false;
    public static bool thirdPatternDone = false;
    private bool patternFinished = false;

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

        /// <problem> 폭탄 패턴의 초기화가 이루어지지 않는다. allBack 변수의 문제
        // 패턴 연속 실행 코드
        if (firstPatternDone == true || secondPatternDone == true 
            || thirdPatternDone == true)
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

            patternFinished = false;
        }
    }

    // Update is called once per frame
    IEnumerator RandomMoving()
    {
        randomAttack = Random.Range(0, 3);

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
    }
}
