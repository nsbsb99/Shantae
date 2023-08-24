using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrabLever : MonoBehaviour
{
    private Vector2 leverPosition = default;
    private float moveSpeed = 3f;
    private Animator animator;

    private float degreePerSecond = 100;
    private float degreePerSecond_Return = 200;
    private bool returnLever = false;
    private float originLever;

    public static bool sandActive = false;
    private Transform lever;

    private GameObject sandCloud;

    private bool pullLever = default;
    private bool backLever = default;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        lever = GameObject.Find("Lever").transform;
        Debug.Assert(lever != null);

        originLever = lever.eulerAngles.z;

        // 레버를 잡아당길 위치
        leverPosition = new Vector2(5.58f, transform.position.y);

        sandCloud = GameObject.Find("FX_Full Sand Effects");

        // 모래구름
        for (int i = 0; i < 8; i++)
        {
            sandCloud.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
            sandCloud.transform.GetChild(i).GetComponent<SandCloudEffect>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CoralSirenMoving.grabLever == true)
        {
            animator.SetBool("Fire Bomb", true);
            transform.GetComponent<SpriteRenderer>().flipX = false;

            // 레버 위치로 이동
            transform.position = Vector2.MoveTowards(transform.position, leverPosition,
                moveSpeed * Time.deltaTime);

            if (Vector2.Distance(leverPosition, transform.position) <= 0.1f)
            {
                CoralSirenMoving.grabLever = false;
                StartCoroutine(PullLever());
            }
        }

        // 레버 당기기
        if (pullLever == true && backLever == false)
        {
            //레버 rotation 삽입
            lever.Rotate(Vector3.forward * Time.deltaTime * degreePerSecond);

            // 레버가 돌아갈 각도 결정
            if (lever.rotation.eulerAngles.z > 40f)
            {
                backLever = true;
            }
        }
        else if (pullLever == true && backLever == true)
        {
            lever.Rotate(Vector3.back * Time.deltaTime * degreePerSecond_Return);

            if (Mathf.Abs(lever.eulerAngles.z - originLever) <= 3f)
            {
                lever.eulerAngles = Vector3.zero;

                pullLever = false;
                backLever = false;
            }
        }
    }

    IEnumerator PullLever()
    {
        Debug.Log("코루틴 작동 확인");

        animator.SetBool("Grab Lever", true);
        pullLever = true;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // 모래 채우기 이펙트 재생
        for (int i = 0; i < 8; i++)
        {
            sandCloud.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
            sandCloud.transform.GetChild(i).GetComponent<SandCloudEffect>().enabled = true;
        }

        sandActive = true;

        animator.SetBool("Grab Lever", false);
        animator.SetBool("Fire Bomb", false);

        yield return new WaitForSeconds(3);

        // 모래 채우기 이펙트 초기화
        for (int i = 0; i < 8; i++)
        {
            Debug.Log("모래구름 초기화");
            sandCloud.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
            sandCloud.transform.GetChild(i).GetComponent<SandCloudEffect>().enabled = false;
        }

        CoralSirenMoving.fourthPatternDone = true;

        StopCoroutine(PullLever());
    }
}
