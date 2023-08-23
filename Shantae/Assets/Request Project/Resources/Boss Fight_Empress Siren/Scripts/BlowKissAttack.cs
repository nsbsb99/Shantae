using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 보스 Empress Siren의 양쪽 벽 공격을 위한 클래스
/// </summary>

public class BlowKissAttack : MonoBehaviour
{
    #region 양쪽 벽에서의 BlowKiss 공격
    private GameObject blowKissPrefab;
    // 발사되는 수
    private int blowKissCount = 25;

    // 날아가는 속도
    private float blowKissSpeed = 15.0f;
    private float gap = 0.8f;

    private Vector2 firePosition = default;

    private GameObject[] blowKisses;
    private Vector2 poolPosition_blowKiss = new Vector2(-2.0f, -10.0f);

    private bool runCheck = false;
    private bool runCheck_right = false;
    private bool blowNow = false;

    private bool blowWait = false;
    private int nowBlowNumber = 0;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        blowKissPrefab = Resources.Load<GameObject>
            ("Boss Fight_Empress Siren/Prefabs/BlowKiss Attack");
        Debug.Assert(blowKissPrefab != null);

        blowKisses = new GameObject[blowKissCount];

        for (int i = 0; i < blowKissCount; i++)
        {
            blowKisses[i] = Instantiate(blowKissPrefab, poolPosition_blowKiss,
                Quaternion.identity);
        }
    }

    private void FixedUpdate()
    {
        #region 좌측 벽에서 공격
        if (EmpressMoving.leftWall == true && EmpressMoving.rightWall == false)
        {
            // runCheck는 발사가 전부 끝났는지 알아보기 위한 변수
            if (runCheck == false)
            {
                for (int i = 0; i < blowKissCount; i++)
                {
                    // 발사를 위한 위치 재정렬 
                    firePosition = new Vector2
                    (transform.position.x + 1.2f, transform.position.y + 0.6f);

                    blowKisses[i].transform.position = firePosition;
                }

                runCheck = true;
            }
            else if (runCheck == true)
            {
                // 재정렬이 끝났다면
                for (int i = 0; i < blowKissCount; i++)
                {
                    // 각 공격구마다 목적지 지정
                    blowKisses[i].GetComponent<BlowKissMoving>().blowKissDestination
                        = new Vector2(10.5f, 10f - (gap * i));

                    blowKisses[i].GetComponent<SpriteRenderer>().enabled = false;

                    if (blowNow == false)
                    {
                        StartCoroutine(StartAttack());
                    }
                }
            }
        }
        #endregion

        #region 우측 벽에서 공격
        if (EmpressMoving.rightWall == true && EmpressMoving.leftWall == false)
        {
            // runCheck는 발사가 전부 끝났는지 알아보기 위한 변수
            if (runCheck_right == false)
            {
                for (int i = 0; i < blowKissCount; i++)
                {
                    // 발사를 위한 위치 재정렬 
                    firePosition = new Vector2
                    (transform.position.x - 1.2f, transform.position.y + 0.6f);

                    blowKisses[i].transform.position = firePosition;
                }

                runCheck_right = true;
            }
            else if (runCheck_right == true)
            {
                // 재정렬이 끝났다면
                for (int i = 0; i < blowKissCount; i++)
                {
                    // 각 공격구마다 목적지 지정
                    blowKisses[i].GetComponent<BlowKissMoving>().blowKissDestination
                        = new Vector2(-10.5f, 10f - (gap * i));

                    blowKisses[i].GetComponent<SpriteRenderer>().enabled = false;

                    if (blowNow == false)
                    {
                        StartCoroutine(StartAttack());
                    }
                }
            }
        }
        #endregion
        
        // 동작이 끝나면 풀로 복귀
        if (EmpressMoving.rightWall == false && EmpressMoving.leftWall == false)
        {
            StopCoroutine(StartAttack());
            runCheck = false;
            runCheck_right = false;
            blowNow = false;

            // 애니메이션이 끝나면 풀로 복귀 
            for (int i = 0; i < blowKissCount; i++)
            {
                blowKisses[i].transform.position = poolPosition_blowKiss;
                blowKisses[i].GetComponent<BlowKissMoving>().enabled = false;
            }
        }

        // empressHP <= 0이면 전부 풀에 고정
        if (EmpressController.empressHP <= 0)
        {
            for (int i = 0; i < blowKissCount; i++)
            {
                blowKisses[i].transform.position = poolPosition_blowKiss;
            }
        }
    }

    IEnumerator StartAttack()
    {
        blowNow = true;

        nowBlowNumber = 0;

        while (nowBlowNumber < blowKissCount)
        {
            // 각 공격구마다 발사
            blowKisses[nowBlowNumber].GetComponent<BlowKissMoving>().enabled = true;

            yield return new WaitForSecondsRealtime(0.1f);

            nowBlowNumber++;
        }

        yield return new WaitForSecondsRealtime(1.5f);

        EmpressMoving.leftWall = false;
        EmpressMoving.rightWall = false;
    }
}
