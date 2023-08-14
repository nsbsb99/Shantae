using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    Transform coralSiren_Back;
    Transform coralSiren_Front;
    Vector2 coralSiren_Back_OriginPosition = default;
    Vector2 coralSiren_Back_RightDestination = default;
    Vector2 coralSiren_Front_LeftDestination = default;

    float moveSpeed = 15f;
    float getDistance = default;

    Animator coralSiren_Back_Animator;
    Animator coralSiren_Front_Animator;

    private bool getFirstDestination = false;
    private bool getSecondDestination = false;

    // Start is called before the first frame update
    void Start()
    {
        // 뒤에 위치한 Coral Siren
        coralSiren_Back = FindObjectOfType<CoralSirenMoving>().transform;
        Debug.Assert(coralSiren_Back != null);
        // 앞에 위치한 Coral Siren
        coralSiren_Front = GameObject.Find("Coral Siren_Front").transform;
        Debug.Assert(coralSiren_Front != null);

        // 뒤 Coral Siren의 원래 위치
        coralSiren_Back_OriginPosition = coralSiren_Back.position;

        // 뒤 Coral Siren의 목적지
        coralSiren_Back_RightDestination =
            new Vector2(11f, coralSiren_Back.position.y);
        // 앞 Coral Siren의 목적지
        coralSiren_Front_LeftDestination =
            new Vector2(-12f, coralSiren_Front.position.y);

        // 뒤 Coral Siren의 애니메이터
        coralSiren_Back_Animator =
            FindObjectOfType<CoralSirenMoving>().GetComponent<Animator>();

        // 앞 Coral Siren의 애니메이터
        coralSiren_Front_Animator =
            GameObject.Find("Coral Siren_Front").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CoralSirenMoving.dash == true && getFirstDestination == false)
        {
            // 뒤에 위치한 보스가 오른쪽으로 이동
            coralSiren_Back.position = Vector2.MoveTowards
                (coralSiren_Back.position, coralSiren_Back_RightDestination,
                moveSpeed * Time.deltaTime);

            // 뒤에 위치한 보스가 이동을 완료하면
            if (Vector2.Distance
                (coralSiren_Back.position, coralSiren_Back_RightDestination) <= 0.1f)
            {
                getFirstDestination = true;

                // 뒤의 보스는 다음 출발지로 이동
                coralSiren_Back.position =
                    new Vector2(-11f, coralSiren_Back.transform.position.y);

                coralSiren_Front.GetComponent<SpriteRenderer>().flipX = true;
                coralSiren_Front_Animator.SetBool("Front Dash", true);

                getSecondDestination = true;
            }
        }

        if (CoralSirenMoving.dash == true && getSecondDestination == true)
        {
            // 앞에 위치한 보스가 왼쪽으로 이동
            coralSiren_Front.position = Vector2.MoveTowards
                (coralSiren_Front.position, coralSiren_Front_LeftDestination,
                moveSpeed * Time.deltaTime);
        }

        // 앞에 위치한 보스가 이동을 완료하면
        if (CoralSirenMoving.dash == true && Vector2.Distance
            (coralSiren_Front.position, coralSiren_Front_LeftDestination) <= 0.1f)
        {
            //getSecondDestination = false;

            coralSiren_Front_Animator.SetBool("Front Dash", false);

            coralSiren_Back.position = Vector2.MoveTowards
            (coralSiren_Back.position, coralSiren_Back_OriginPosition,
            moveSpeed * Time.deltaTime);

            if (Vector2.Distance
            (coralSiren_Back.position, coralSiren_Back_OriginPosition) <= 0.1f)
            {
                coralSiren_Back_Animator.SetBool("Go Dash", false);
            }
        }
    }
}
