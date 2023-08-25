using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoralSirenController : MonoBehaviour
{
    // 피격 시 색상 교체를 위함
    private Color originColor = default;
    private Color transparentColor = default;
    // CoralSiren의 HP
    private float coralSirenHP = 15f;
    // Coral Siren_Back
    private GameObject coralSiren_Back;
    // Coral Siren의 공격 매니저
    private GameObject bossAttackManager;
    // Player
    private GameObject player;

    private bool already_1 = false;
    private bool already_2 = false;
    private bool leftFalling = false;
    private bool rightFalling = false;

    // Coral Siren의 패배를 전달
    public static bool coralDefeated = false;

    // Start is called before the first frame update
    void Start()
    {
        originColor = transform.GetComponent<SpriteRenderer>().color;
        transparentColor = originColor;
        transparentColor.a = 0.5f;

        coralSiren_Back = GameObject.Find("Coral Siren_Back");
        player = GameObject.Find("Player");

        bossAttackManager = GameObject.Find("Boss Attack Manager");
    }

    // Update is called once per frame
    void Update()
    {
        if (HitController.coralDamaged == true)
        {
            // 플레이어가 입힌 데미지는 4부터 9까지의 정수
            int getDamage = Random.Range(4, 10);
            // Empress HP 깎기. 
            coralSirenHP -= getDamage;

            StartCoroutine(FlashCoral());
        }

        if (coralSirenHP <= 0 && already_1 == false) // 패배 시 즉시 적용
        {
            if (player.transform.position.x < 0)
            {
                transform.position = new Vector2(4, 9f);

                rightFalling = true;
            }
            else if (player.transform.position.x > 0)
            {
                transform.position = new Vector2(-4, 9f);

                leftFalling = true;
            }
        }

        if (rightFalling == true && already_2 == false|| leftFalling == true && already_2 == false)
        { 
            already_2 = true;

            // 기존에 진행되던 스크립트와 코루틴 전부 종료하기_CoralSiren_Back
            coralSiren_Back.GetComponent<CoralSirenMoving>().StopAllCoroutines();
            coralSiren_Back.GetComponent<FireBomb>().StopAllCoroutines();
            coralSiren_Back.GetComponent<GrabLever>().StopAllCoroutines();
            coralSiren_Back.GetComponent<CoralSirenMoving>().enabled = false;
            coralSiren_Back.GetComponent<FireBomb>().enabled = false;
            coralSiren_Back.GetComponent<GrabLever>().enabled = false;

            // boss attack manager
            bossAttackManager.GetComponent<Dash>().enabled = false;
            bossAttackManager.GetComponent<FireSpread>().enabled = false;

            // 기존에 진행되던 스크립트와 코루틴 전부 종료하기_CoralSiren_Front
            transform.GetComponent<FrontGrounded>().enabled = false;
            transform.GetComponent<FrontGrounded>().StopAllCoroutines();

            transform.GetComponent<Animator>().SetBool("Fire_Frail", true);

            // 위치를 재정렬하고 이동 관련 스크립트를 모두 꺼야 다음 동작을 진행하도록. 
            already_1 = true;
        }

        if (coralSirenHP <= 0) // coralSiren_Back의 즉시 위치 이동을 위함.
        {
            coralSiren_Back.transform.position = new Vector2(10f, 10f);
        }

        /// 아래 수정하기
        // PlayWin() 코루틴에서 추락 신호를 전달받으면
        if (rightFalling == true)
        {
            Debug.Log("추락 신호 전달");

            Vector2 destination = new Vector2(4, -1.5f);
            transform.position = Vector2.MoveTowards(transform.position, destination,
                Time.deltaTime * 20f);

            if (Vector2.Distance(transform.position, destination) <= 0.01f)
            {
                rightFalling = false;

                transform.position = destination;
                transform.GetComponent<Animator>().Play("Fire_Frail");
            }
        }
        else if (leftFalling == true)
        {
            Debug.Log("추락 신호 전달");

            Vector2 destination = new Vector2(-4, -1.5f);
            transform.position = Vector2.MoveTowards(transform.position, destination,
                Time.deltaTime * 20f);

            if (Vector2.Distance(transform.position, destination) <= 0.01f)
            {
                leftFalling = false;

                transform.position = destination;
                transform.GetComponent<Animator>().Play("Fire_Frail");
            }
        }
    }

    private IEnumerator FlashCoral()
    {
        float blinkTime = 1;
        float nowTime = 0;

        while (nowTime < blinkTime)
        {
            nowTime += Time.deltaTime * 30;

            transform.GetComponent<SpriteRenderer>().color = transparentColor;
            yield return new WaitForSeconds(0.1f);

            transform.GetComponent<SpriteRenderer>().color = originColor;
            yield return new WaitForSeconds(0.1f);
        }

        transform.GetComponent<SpriteRenderer>().color = originColor;
        HitController.coralDamaged = false;

        StopCoroutine(FlashCoral());
    }

    private IEnumerator PlayerWin() // Coral Siren 패배
    {
        yield return new WaitForSeconds(1.5f);

        transform.GetComponent<Animator>().Play("Fire_Drop");

        GameObject player = GameObject.Find("Player");

        yield return null;
    }
}
