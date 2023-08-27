using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoralSirenController : MonoBehaviour
{
    // 피격 시 색상 교체를 위함
    private Color originColor = default;
    private Color transparentColor = default;
    // CoralSiren의 HP
    private float coralSirenHP = 13f;
    // Coral Siren_Back
    private GameObject coralSiren_Back;
    // Coral Siren의 공격 매니저
    private GameObject bossAttackManager;
    // Player
    private GameObject player;
    // Damage 태그가 달린 모든 게임 오브젝트를 삭제할 것
    private GameObject[] damageObjects;
    // 폭발 게임오브젝트
    private GameObject explosionPrefab;
    private GameObject[] explosions;

    private bool already_1 = false;
    private bool already_2 = false;
    private bool leftFalling = false;
    private bool rightFalling = false;
    private bool ending = false;

    // Coral Siren의 패배를 전달
    public static bool coralDefeated = false;

    public static bool die = false;


    // Start is called before the first frame update
    void Start()
    {
        originColor = transform.GetComponent<SpriteRenderer>().color;
        transparentColor = originColor;
        transparentColor.a = 0.5f;

        coralSiren_Back = GameObject.Find("Coral Siren_Back");
        player = GameObject.Find("Player");

        bossAttackManager = GameObject.Find("Boss Attack Manager");

        explosionPrefab = Resources.Load<GameObject>("Boss Fight_Coral Siren/Prefabs/blow");

        for (int i = 0; i < 15; i++)
        {
            // 밑에 수정하기
            //explosions[i] = Instantiate(explosionPrefab, )
        }
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
            // 공격 오브젝트들 카메라 밖으로 이동
            damageObjects = GameObject.FindGameObjectsWithTag("Damage");

            foreach (GameObject damageObject in damageObjects)
            {
                damageObject.transform.position = new Vector2(0, -10f);
            }

            // 보스의 이동 포지션 설정
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
            coralSiren_Back.GetComponent<CoralSirenMoving>().enabled = false;
            coralSiren_Back.GetComponent<FireBomb>().enabled = false;
            coralSiren_Back.GetComponent<GrabLever>().enabled = false;
            coralSiren_Back.GetComponent<CoralSirenMoving>().StopAllCoroutines();
            coralSiren_Back.GetComponent<FireBomb>().StopAllCoroutines();
            coralSiren_Back.GetComponent<GrabLever>().StopAllCoroutines();

            // boss attack manager
            bossAttackManager.GetComponent<Dash>().enabled = false;
            bossAttackManager.GetComponent<FireSpread>().enabled = false;

            // 기존에 진행되던 스크립트와 코루틴 전부 종료하기_CoralSiren_Front
            transform.GetComponent<FrontGrounded>().enabled = false;
            transform.GetComponent<FrontGrounded>().StopAllCoroutines();

            // 깜빡거리는 거 막기
            transform.GetComponent<SpriteRenderer>().color = originColor;

            for (int i = 0; i < 3; i++) // Coral Siren_Front의 파티클 등 효과 종료
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            transform.GetComponent<Animator>().SetBool("Fire_Frail", true);

            // 위치를 재정렬하고 이동 관련 스크립트를 모두 꺼야 다음 동작을 진행하도록. 
            already_1 = true;
        }

        if (coralSirenHP <= 0) // coralSiren_Back의 즉시 위치 이동을 위함.
        {
            coralSiren_Back.transform.position = new Vector2(10f, 10f);
        }

        // PlayWin() 코루틴에서 추락 신호를 전달받으면
        if (rightFalling == true) // 오른쪽으로 추락
        {
            Debug.Log("추락 신호 전달");

            // 이동 효과 삽입
            transform.GetChild(3).gameObject.SetActive(true);

            /// <point> 해당 좌표에서 효과 재생이 끝나면 이동하도록.

            Vector2 destination = new Vector2(4, -1.5f);
            transform.position = Vector2.MoveTowards(transform.position, destination,
                Time.deltaTime * 20f);

            if (Vector2.Distance(transform.position, destination) <= 0.01f)
            {
                rightFalling = false;

                transform.position = destination;
                transform.GetComponent<Animator>().Play("Fire_Frail");

                ending = true;
            }
        }
        else if (leftFalling == true) // 왼쪽으로 추락
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

                ending = true;
            }
        }

        if (ending == true)
        {
            // 폭탄 효과 수정 
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
