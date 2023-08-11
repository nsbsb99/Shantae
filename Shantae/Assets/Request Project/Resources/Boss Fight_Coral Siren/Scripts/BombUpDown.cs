using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BombUpDown : MonoBehaviour
{
    private float upSpeed = 10f;
    private float downSpeed = 25.0f;
    private bool alreadyScaleUp = false;
    private Animator bombAnimator;
    private bool backThePool = false;

    private Vector2 poolPosition_bomb = new Vector2(0f, 10f);

    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        bombAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 위로 발사
        // 폭탄이 아래로 가는 오류

        transform.Translate(Vector2.up * upSpeed * Time.deltaTime);

        // 폭탄의 월드 좌표 y가 7에 도달하면
        if (transform.position.y >= 7f && alreadyScaleUp == false)
        {
            Debug.Log("도달함");
            transform.localScale = transform.localScale * 1.5f;
            alreadyScaleUp = true;
        }

        //특정 좌표에서 폭탄을 키웠다면 아래로 떨구기
        if (alreadyScaleUp == true)
        {
            transform.Translate(Vector2.down * downSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("충돌!");

        // 만약 바닥 콜라이더와 충돌하면
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 애니메이션 재생
            transform.GetComponent<Animator>().enabled = true;

            StartCoroutine(BombAnimationTimer());
        }

        // 애니메이션 재생이 끝나면 자식오브젝트(0) 렌더러 끄고 풀로 복귀
        if (backThePool == true)
        {
            Debug.Log("풀로 이동");
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            transform.position = poolPosition_bomb;

            backThePool = false;
        }
    }

    IEnumerator BombAnimationTimer()
    {
        yield return new WaitForSeconds
               (bombAnimator.GetCurrentAnimatorStateInfo(0).length);

        backThePool = true;
    }
}
