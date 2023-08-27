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
    private float originLever;

    public static bool sandActive = false;
    private Transform lever;

    private GameObject sandCloud;

    private bool pullLever = default;
    private bool backLever = default;

    public static bool recolor = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        lever = GameObject.Find("Lever").transform;
        Debug.Assert(lever != null);

        originLever = lever.eulerAngles.z;

        // ������ ��ƴ�� ��ġ
        leverPosition = new Vector2(5.58f, transform.position.y);

        sandCloud = GameObject.Find("FX_Full Sand Effects");

        // �𷡱���
        for (int i = 0; i < 8; i++)
        {
            sandCloud.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CoralSirenMoving.grabLever == true)
        {
            animator.SetBool("Fire Bomb", true);
            transform.GetComponent<SpriteRenderer>().flipX = false;

            // ���� ��ġ�� �̵�
            transform.position = Vector2.MoveTowards(transform.position, leverPosition,
                moveSpeed * Time.deltaTime);

            if (Vector2.Distance(leverPosition, transform.position) <= 0.1f)
            {
                CoralSirenMoving.grabLever = false;
                StartCoroutine(PullLever());
            }
        }

        // ���� ����
        if (pullLever == true && backLever == false)
        {
            //���� rotation ����
            lever.Rotate(Vector3.forward * Time.deltaTime * degreePerSecond);

            // ������ ���ư� ���� ����
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
        animator.SetBool("Grab Lever", true);
        pullLever = true;
        recolor = true;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        sandActive = true;

        animator.SetBool("Grab Lever", false);
        animator.SetBool("Fire Bomb", false);

        yield return new WaitForSeconds(3);

        // ����/ũ�� �ʱ�ȭ
        recolor = false;

        CoralSirenMoving.fourthPatternDone = true;

        StopCoroutine(PullLever());
    }
}
