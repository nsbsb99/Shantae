using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBomb : MonoBehaviour
{
    private GameObject bombPrefab;
    private GameObject[] bombs;

    private int firstBombCount = 6;
    private int secondBombCount = 15;
    private int thirdBombCount = 20;
    private int maxBombCount = default;

    private bool readyFire = false;
    private bool doneFire = false;

    private Vector2 poolPosition_bomb = new Vector2(0f, 10f);
    private Vector2 firePosition;

    private void Start()
    {
        bombPrefab = Resources.Load<GameObject>
            ("Boss Fight_Coral Siren/Prefabs/Bomb");

        // 동시에 화면에 존재하는 폭탄의 수 (35개)
        maxBombCount = secondBombCount + thirdBombCount;
        bombs = new GameObject[maxBombCount];

        for (int i = 0; i < thirdBombCount; i++)
        {
            bombs[i] = Instantiate(bombPrefab, poolPosition_bomb, Quaternion.identity);
            Debug.Assert(bombs[i] != null);

            bombs[i].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            Debug.Assert(bombs[i].transform.GetChild(0).GetComponent<SpriteRenderer>()
                != null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CoralSirenMoving.fireBomb == true && readyFire == false)
        {
            StartCoroutine(BombsFire());
        }

        // y = 6.5f에 도달하면 Scale 확대 + 밑으로 떨어짐 + 만약 바닥의 Collider와 충돌하면
        // Animation 재생과 함께 풀로 복귀

        // 발사가 끝나면 코루틴 종료 
        if (doneFire == false)
        {
            StopCoroutine(BombsFire());
        }
    }

    IEnumerator BombsFire()
    {
        readyFire = true;

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

        doneFire = false;
    }
}
