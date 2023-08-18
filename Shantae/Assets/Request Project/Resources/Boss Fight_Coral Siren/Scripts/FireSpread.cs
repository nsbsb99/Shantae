using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Coral Siren�� �ڿ��� ������ �翷���� ���� �߻��ϴ� Ŭ����
/// </summary>

public class FireSpread : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        // �� Coral Siren
        coralSiren_Back = FindObjectOfType<CoralSirenMoving>().transform;
        Debug.Assert(coralSiren_Back != null);
        // �� Coral Siren
        coralSiren_Front = GameObject.Find("Coral Siren_Front").transform;
        Debug.Assert(coralSiren_Front != null);

        // �� Coral Siren Animator
        coralSiren_Back_Animator =
            FindObjectOfType<CoralSirenMoving>().GetComponent<Animator>();
        // �� Coral Siren Animator
        coralSiren_Front_Animator =
            GameObject.Find("Coral Siren_Front").GetComponent<Animator>();

        // �� Coral Siren�� Origin Position
        coralSiren_Back_OriginPosition = coralSiren_Back.position;
        // �� Coral Siren�� Origin Position
        coralSiren_Front_OriginPosition = coralSiren_Front.position;

        firstFirePosition = -5.8f;
        secondFirePosition = -1.91f;
        thirdFirePosition = 1.88f;
        fourthFirePosition = 5.79f;
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

                if (selectedRandom == 0)
                {
                    whatFirePosition = firstFirePosition;
                }
                else if (selectedRandom == 1)
                {
                    whatFirePosition = secondFirePosition;
                }
                else if (selectedRandom == 2)
                {
                    whatFirePosition = thirdFirePosition;
                }
                else if (selectedRandom == 3)
                {
                    whatFirePosition = fourthFirePosition;
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

                // �߻� ��ġ�� �̵�
                coralSiren_Back.position =
                Vector2.MoveTowards(coralSiren_Back.position,
                new Vector2(whatFirePosition, coralSiren_Back.position.y),
                moveSpeed * Time.deltaTime);

                coralPositionX = coralSiren_Back.position.x;
                coralPositionY = coralSiren_Back.position.y;

                // �߻� ��ġ�� �����ϸ�
                if (Mathf.Abs(whatFirePosition - coralPositionX) <= 0.1f)
                {
                    coralSiren_Back_Animator.SetBool("Spread Fire Charge", true);
                    coralSiren_Back_Animator.SetBool("Fire Bomb", false);

                    // ����
                    coralSiren_Back.position =
                        new Vector2(whatFirePosition, coralSiren_Back.position.y);

                    moveDone = true;
                }
            }

            if (moveDone == true)
            {
                // �� Coral Siren�� ���
                coralSiren_Back.position =
                    Vector2.MoveTowards(coralSiren_Back.position,
                    new Vector2(coralSiren_Back.position.x, 7f), upSpeed * Time.deltaTime);
            }

            if (coralSiren_Back.position.y >= 7f && firstDestination == false)
            {
                // �� Coral Siren�� ���� ������ �����ϸ�
                coralSiren_Back_Animator.SetBool("Spread Fire Charge", false);
                coralSiren_Back_Animator.SetBool("Drop", true);

                // �� Coral Siren�� �߶��� ���� ��ǥ �̵�
                coralSiren_Front.position =
                    new Vector2(coralSiren_Back.position.x, 13f);

                firstDestination = true;
            }
        }

        if (CoralSirenMoving.fireSpread == true && firstDestination == true
            && secondDestination == false)
        {
            // �� Coral Siren�� �߶�
            coralSiren_Front.position = Vector2.MoveTowards(coralSiren_Front.position,
                new Vector2(coralSiren_Back.position.x, coralSiren_Front_OriginPosition.y),
                fallSpeed * Time.deltaTime);
        }

        /// <point> ���⿡ sand = null�� �� üũ.
        // ���� Coral Siren�� ���� �浹�ϸ� 
        if (CoralSirenMoving.fireSpread == true &&
            FrontGrounded.coralSiren_Front_Grounded == true)
        {
            StartCoroutine(Grounded());

            FrontGrounded.coralSiren_Front_Grounded = false;
        }
        else if (CoralSirenMoving.fireSpread == true &&
            FrontGrounded.coralSiren_Front_Sanded == true)
        {
            StartCoroutine(Sanded());

            FrontGrounded.coralSiren_Front_Sanded = false;
        }

        // ���� Coral Siren�� ���� �ö󰡶�� ��ȣ�� �޾Ҵٸ�
        if (CoralSirenMoving.fireSpread == true && secondDestination == true
            && thirdDestination == false)
        {
            coralSiren_Front.position = Vector2.MoveTowards(coralSiren_Front.position,
                new Vector2(coralSiren_Front.position.x, 13f),
                fallSpeed * Time.deltaTime);

            // ���� ���� Coral Siren�� ���� ������ �����ߴٸ�
            if (coralSiren_Front.position.y >= 13f)
            {
                coralSiren_Front_Animator.SetBool("Go Back", false);

                // Ǯ�� ����
                coralSiren_Front.position = coralSiren_Front_OriginPosition;

                thirdDestination = true;
            }
        }

        if (CoralSirenMoving.fireSpread == true && thirdDestination == true)
        {
            // ���� Coral Siren�� ������ ����
            coralSiren_Back.position = Vector2.MoveTowards(coralSiren_Back.position,
                    new Vector2(coralPositionX, coralPositionY),
                    fallSpeed * Time.deltaTime);

            // ���� ����
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

            allStop = false;
        }
    }

    IEnumerator Grounded()
    {
        // �� ���� ���� 
        coralSiren_Front_Animator.SetBool("Fire", true);
        yield return new WaitForSeconds(4.0f);

        // ���� Coral Siren�� ���� �÷�������
        coralSiren_Front_Animator.SetBool("Fire", false);
        coralSiren_Front_Animator.SetBool("Go Back", true);

        secondDestination = true;
    }

    IEnumerator Sanded()
    {
        coralSiren_Front_Animator.SetBool("Fire_Frail", true);
        // 3�ʰ� ���հŸ�
        yield return new WaitForSeconds(3.0f);

        coralSiren_Front_Animator.SetBool("Fire_Frail", false);
        // Ż�� ��� ���� ���
        yield return new WaitForSeconds
            (coralSiren_Front_Animator.GetCurrentAnimatorStateInfo(0).length);

        coralSiren_Front_Animator.SetBool("Go Back", true);

        secondDestination = true;
    }

    IEnumerator NextGrounded()
    {
        coralSiren_Back_Animator.SetBool("Drop", false);

        // �� Coral Siren�� ���� ��� ���� �ð� 
        yield return new WaitForSeconds(1.5f);

        coralSiren_Back_Animator.SetBool("Spread Fire Charge", false);
        coralSiren_Back_Animator.SetBool("Fire Spread", false);

        allStop = true;
    }
}