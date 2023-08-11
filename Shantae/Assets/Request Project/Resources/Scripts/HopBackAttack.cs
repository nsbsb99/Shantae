using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HopBackAttack : MonoBehaviour
{
    private GameObject hopBackPrefab;
    private GameObject[] hopBacks;
    private int hopBackCount = 8;
    private Vector2 poolPosition_hopBack = new Vector2(3f, 10.0f);
    private Vector2 firePosition;
    private bool readyRun = false;
    private bool stopRun = false;

    // Start is called before the first frame update
    void Start()
    {
        hopBackPrefab = Resources.Load<GameObject>("Prefabs/HopBack Attack");
        hopBacks = new GameObject[hopBackCount];

        for (int i = 0; i < hopBackCount; i++)
        {
            hopBacks[i] = Instantiate
                (hopBackPrefab, poolPosition_hopBack, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (EmpressMoving.hopBack == true && readyRun == false)
        {
            // 발사 위치로 이동
            firePosition = new Vector2(transform.position.x + 2.67f,
                transform.position.y + 0.64f);

            StartCoroutine(LocateFirePosition());
        }

        // 유도탄이 동작할 특정 시간이 지났다 && 애니메이션 실행 상태가 아니다
        if (stopRun == true && EmpressMoving.hopBack == false)
        {
            StopCoroutine(startMovingTimer());

            for (int i = 0; i < hopBackCount; i++)
            {
                // 오브젝트 풀로 복귀
                hopBacks[i].transform.position = poolPosition_hopBack;
                // 이동 스크립트 끄기
                hopBacks[i].GetComponent<HopBackBullet>().enabled = false;
            }

            readyRun = false;
        }
    }

    IEnumerator LocateFirePosition()
    {
        // 코루틴을 시작하면 발사 위치 이동 코드가 시작되지 않도록 함. 
        readyRun = true;

        for (int i = 0; i < hopBackCount; i++)
        {
            // 특정 시점마다 발사 위치로 이동
            hopBacks[i].transform.position = firePosition;

            // 이동 시작
            hopBacks[i].GetComponent<HopBackBullet>().enabled = true;

            yield return new WaitForSeconds(0.15f);
        }

        // 전체 유도탄 발사 완료, 유도탄 동작 타이머 시작
        StartCoroutine(startMovingTimer());
    }

    IEnumerator startMovingTimer()
    {
        StopCoroutine(LocateFirePosition());

        // 5초 뒤에 유도탄의 동작을 멈춰라
        yield return new WaitForSeconds(5.0f);
        stopRun = true;
    }
}
