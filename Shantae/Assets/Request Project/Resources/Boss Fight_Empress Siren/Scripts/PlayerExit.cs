using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExit : MonoBehaviour
{
    private Vector2 playerDestination = default;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        playerDestination = new Vector2(7.47f, -2.24f);
        animator = GetComponent<Animator>();

        animator.SetBool("isGround", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (CameraShake.instance.itPlayed == true)
        {
            float moveSpeed = 3f;

            animator.SetBool("Run", true);

            transform.position = Vector2.MoveTowards
                (transform.position, playerDestination, Time.deltaTime * moveSpeed);

            // �� �� ����
            StopAllCoroutines();

            // ���� �ⱸ ��ǥ�� �ٻ�ġ�� �����ߴٸ�
            if (Mathf.Abs(transform.position.x - playerDestination.x) <= 0.01f)
            {
                AllSceneManager.instance.StartCoroutine
                    (AllSceneManager.instance.OpenLoadingScene_Third());
            }
        }
    }
}
