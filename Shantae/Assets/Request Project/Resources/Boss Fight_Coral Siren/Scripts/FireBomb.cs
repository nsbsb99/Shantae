using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    private static bool stopRun = false;

    private Vector2 poolPosition_bomb = new Vector2(0f, 10f);
    private Vector2 firePosition;

    private BombUpDown lastBombUpDown;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (CoralSirenMoving.fireBomb == true && readyFire == false)
        {
            StartCoroutine(BombsFire());
        }

        // 전부 발사하면 발사 코루틴 종료 && 다음 동작 준비
        if (CoralSirenMoving.fireBomb == false && doneFire == true)
        {
            StopCoroutine(BombsFire());

            CoralSirenMoving.fireBomb = false;

            readyFire = false;
            doneFire = false;
        }

        // 마지막 폭탄이 풀에 도착한 것이 확인되면
        if (CoralSirenMoving.fireBomb == false && lastBombUpDown.backThePool == true)
        {
            lastBombUpDown.upDone = false;
            lastBombUpDown.falling = false;
            lastBombUpDown.backThePool = false;
            lastBombUpDown.alreadyRun = false;
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

            Debug.Log("발사 시작");

            // 발사 시작
            bombs[i].GetComponent<BombUpDown>().enabled = true;

            yield return new WaitForSeconds(0.5f);
        }

        // 보스는 폭탄을 전부 발사한 후 몇 초 뒤 다음 동작 시작
        yield return new WaitForSeconds(3.0f);
        // 전부 발사함
        doneFire = true;
    }
}
