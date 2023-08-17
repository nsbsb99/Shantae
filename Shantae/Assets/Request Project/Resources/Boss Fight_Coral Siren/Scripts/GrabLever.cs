using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        lever = GameObject.Find("Lever").transform;
        originLever = lever.eulerAngles.z;

        // 레버를 잡아당길 위치
        leverPosition = new Vector2(5.58f, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (CoralSirenMoving.grabLever == true)
        {
            animator.SetBool("Fire Bomb", true);
            // 레버 위치로 이동
            transform.position = Vector2.MoveTowards(transform.position, leverPosition,
                moveSpeed * Time.deltaTime);

            if (Vector2.Distance(leverPosition, transform.position) <= 0.1f)
            {
                StartCoroutine(PullLever());
            }
        }
    }

    IEnumerator PullLever()
    {
        animator.SetBool("Grab Lever", true);
        //레버 rotation 삽입
        lever.Rotate(Vector3.forward * Time.deltaTime * degreePerSecond);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.SetBool("Grab Lever", false);
        animator.SetBool("Fire Bomb", false);
        if (Mathf.Abs(lever.eulerAngles.z - originLever) > 3f)
        {
            lever.Rotate(-(Vector3.forward) * Time.deltaTime * degreePerSecond_Return);
        }
        else if (Mathf.Abs(lever.eulerAngles.z - originLever) <= 3f)
        {
            lever.eulerAngles = Vector3.zero;
        }

        sandActive = true;
    }
}
