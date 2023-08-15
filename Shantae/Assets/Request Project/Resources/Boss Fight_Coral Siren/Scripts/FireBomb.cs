using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 폭탄을 발사하고 발사 간격을 조정하는 클래스
/// </summary>

public class FireBomb : MonoBehaviour
{
    private GameObject bombPrefab;
    private GameObject[] bombs;

    private int firstBombCount = 6;
    private int secondBombCount = 15;
    private int thirdBombCount = 20;
    private int maxBombCount = default; // 동작을 확인하기 위해 임시로 하나만

    private bool readyFire = false;
    public static bool doneFire = false;
    public static bool wellDone = false;

    private Vector2 poolPosition_bomb = new Vector2(0f, 10f);
    private Vector2 firePosition;

    public static BombUpDown lastBombUpDown;
    private Animator animator;

    private void Start()
    {
        bombPrefab = Resources.Load<GameObject>
            ("Boss Fight_Coral Siren/Prefabs/Bomb");

        // 동시에 화면에 존재하는 폭탄의 수 (35개)
        //maxBombCount = secondBombCount + thirdBombCount;
        maxBombCount = 5; //임시

        bombs = new GameObject[maxBombCount];

        for (int i = 0; i < maxBombCount; i++)
        {
            bombs[i] = Instantiate(bombPrefab, poolPosition_bomb, Quaternion.identity);
            Debug.Assert(bombs[i] != null);

            bombs[i].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            Debug.Assert(bombs[i].transform.GetChild(0).GetComponent<SpriteRenderer>()
                != null);
        }

        lastBombUpDown = bombs[maxBombCount - 1].GetComponent<BombUpDown>();
        animator = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 발사 시작 코루틴
        if (CoralSirenMoving.fireBomb == true && readyFire == false)
        {
            StartCoroutine(BombsFire());
        }

        // 전부 발사하면 발사 코루틴 종료 && 다음 동작 준비
        if (CoralSirenMoving.fireBomb == true && doneFire == true)
        {
            StopCoroutine(BombsFire());

            doneFire = false;
        }

        // 마지막 폭탄이 풀에 도착한 것이 확인되면
        if (lastBombUpDown.allBack == true)
        {
            readyFire = false;

            for (int i = 0; i < maxBombCount; i++)
            {
                // 폭탄 발사에 쓰인 모든 bool 변수 초기화
                bombs[i].GetComponent<BombUpDown>().upDone = false;
                bombs[i].GetComponent<BombUpDown>().falling = false;
                bombs[i].GetComponent<BombUpDown>().backThePool = false;
                bombs[i].GetComponent<BombUpDown>().alreadyRun = false;
                bombs[i].GetComponent<BombUpDown>().allBack = false;

                // 모든 폭탄 이동 스크립트 끄기
                bombs[i].GetComponent<BombUpDown>().enabled = false;
            }

            // 시작 키워드 끄기
            CoralSirenMoving.fireBomb = false;
        }
    }

    IEnumerator BombsFire()
    {
        readyFire = true;

        // 애니메이션 싱크
        yield return new WaitForSeconds(1.2f);

        for (int i = 0; i < maxBombCount; i++)
        {
            // 발사 위치로
            firePosition = new Vector2
                (transform.position.x, transform.position.y + 1.5f);
            bombs[i].transform.position = firePosition;

            // 발사 시작
            bombs[i].GetComponent<BombUpDown>().enabled = true;

            yield return new WaitForSeconds(0.5f);
        }

        // 전부 발사함
        doneFire = true;

        // 애니메이션 기본 상태로
        animator.SetBool("Fire Bomb", false);
    }
}
