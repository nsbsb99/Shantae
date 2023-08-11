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
    private int blowKissCount = 15;
    // 날아가는 속도
    private float blowKissSpeed = 15.0f;

    private Vector2 firePosition = default;

    private GameObject[] blowKisses;
    private Vector2 poolPosition_blowKiss = new Vector2(-2.0f, -10.0f);

    private bool runCheck = false;
    private bool runCheck_right = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        blowKissPrefab = Resources.Load<GameObject>("Prefabs/BlowKiss Attack");
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
                 StartCoroutine(StartRightAttack());
            }
        }

        else if (EmpressMoving.leftWall == false && EmpressMoving.rightWall == false)
        {
            StopCoroutine(StartRightAttack());
            runCheck = false;

            // 애니메이션이 끝나면 풀로 복귀 
            for (int i = 0; i < blowKissCount; i++)
            {
                blowKisses[i].transform.position = poolPosition_blowKiss;
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
                StartCoroutine(StartLeftAttack());
            }
        }

        else if (EmpressMoving.rightWall == false && EmpressMoving.leftWall == false)
        {
            StopCoroutine(StartLeftAttack());
            runCheck_right = false;

            // 애니메이션이 끝나면 풀로 복귀 
            for (int i = 0; i < blowKissCount; i++)
            {
                blowKisses[i].transform.position = poolPosition_blowKiss;
            }
        }
        #endregion
    }

    IEnumerator StartRightAttack()
    {
        for (int i = 0; i < blowKissCount; i++)
        {
            float gap = 2.5f;

            blowKisses[i].GetComponent<Renderer>().enabled = true;

            blowKisses[i].transform.position =
                Vector2.MoveTowards
                (blowKisses[i].transform.position, new Vector2(10f, 10f - (gap * i)),
                Time.deltaTime * blowKissSpeed);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator StartLeftAttack()
    {
        for (int i = 0; i < blowKissCount; i++)
        {
            float gap = 2.5f;

            blowKisses[i].GetComponent<Renderer>().enabled = true;

            blowKisses[i].transform.position =
                Vector2.MoveTowards
                (blowKisses[i].transform.position, new Vector2(-10f, 10f - (gap * i)),
                Time.deltaTime * blowKissSpeed);
            yield return new WaitForSeconds(0.1f);
        }
    }

}
