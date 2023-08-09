using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 보스 Empress Siren의 공격을 위한 클래스
/// </summary>

public class CeilingAttack : MonoBehaviour
{
    public static CeilingAttack instance;

    #region 천장 공격 마법구
    private GameObject ceilingBallPrefab;
    private int ceilingCount = 2;
    [SerializeField] private float ceilingBallSpeed = 11.0f;
    private GameObject[] ceilingBalls;
    private Vector2 poolPosition_ceiling = new Vector2(0, -10f);

    // 양쪽 공격의 시작 위치(Empress Siren의 현 위치)
    Vector2 ceiling_OriginPosition = default;

    Transform ceiling_first = default;
    Transform ceiling_second = default;

    // 왼쪽
    Vector2 firstDestination_Left = default;
    Vector2 secondDestination_Left = default;
    Vector2 thirdDestination_Left = default;

    //오른쪽
    Vector2 firstDestination_Right = default;
    Vector2 secondDestination_Right = default;
    Vector2 thirdDestination_Right = default;

    private int ceilingMoveIndex = 0;
    private int ceilingMoveIndex_Right = 0;

    private bool leftFinish = false;
    private bool rightFinish = false;
    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(this);
            }
        }
    }

    private void Start()
    {
        ceilingBallPrefab = Resources.Load<GameObject>("Prefabs/Ceiling Attack");
        Debug.Assert(ceilingBallPrefab != null);

        ceilingBalls = new GameObject[ceilingCount];

        for (int i = 0; i < ceilingCount; i++)
        {
            // 2개의 ceilingBall을 생성 
            ceilingBalls[i] = Instantiate(ceilingBallPrefab, poolPosition_ceiling,
                Quaternion.identity);
        }

        // transform 변수 설정
        ceiling_first = ceilingBalls[0].transform;
        ceiling_second = ceilingBalls[1].transform;

        // 왼쪽 공격의 목적지 순차적 설정
        firstDestination_Left = new Vector2(-7.25f, 3.35f);
        secondDestination_Left = new Vector2(-7.25f, -3.0f);
        thirdDestination_Left = new Vector2(-0.7f, -3.0f);

        //오른쪽 공격의 목적지 순차적 설정
        firstDestination_Right = new Vector2(7.0f, 3.35f);
        secondDestination_Right = new Vector2(7.0f, -3.0f);
        thirdDestination_Right = new Vector2(0.6f, -3.0f);
    }

    private void Update()
    {
        // 한 번만 실행하도록 하기 위함. (위치 절대로 바꾸지 말 것!!!)
        if (leftFinish == true && rightFinish == true)
        {
            leftFinish = false;
            rightFinish = false;

            EmpressMoving.ceiling = false;
        }

        if (EmpressMoving.ceiling == true)
        {
            LeftCeilingAttack();
            RightCeilingAttack();
        }
    }

    void LeftCeilingAttack()
    {
        if (ceilingMoveIndex == 0 && leftFinish == false)
        {
            // 양쪽 공격이 공유하는 공격 시작 지점
            ceiling_OriginPosition = new Vector2(transform.position.x, 3.35f);

            // 오브젝트 풀에 있던 것을 맵 안으로 이동 
            ceiling_first.position = ceiling_OriginPosition;

            ceilingMoveIndex++;
        }

        else if (ceilingMoveIndex == 1)
        {
            ceiling_first.position = Vector2.MoveTowards(ceiling_first.position,
            firstDestination_Left, ceilingBallSpeed * Time.deltaTime);

            if (Vector2.Distance(ceiling_first.position, firstDestination_Left) < 0.01f)
            {
                ceilingMoveIndex++;
            }
        }

        else if (ceilingMoveIndex == 2)
        {
            ceiling_first.position = Vector2.MoveTowards(ceiling_first.position,
            secondDestination_Left, ceilingBallSpeed * Time.deltaTime);

            if (Vector2.Distance(ceiling_first.position, secondDestination_Left) < 0.01f)
            {
                ceilingMoveIndex++;
            }
        }

        else if (ceilingMoveIndex == 3)
        {
            ceiling_first.position = Vector2.MoveTowards(ceiling_first.position,
            thirdDestination_Left, ceilingBallSpeed * Time.deltaTime);

            if (Vector2.Distance(ceiling_first.position, thirdDestination_Left) < 0.01f)
            {
                ceilingMoveIndex++;
            }
        }

        // 오브젝트 풀로 복귀
        else if (ceilingMoveIndex == 4)
        {
            ceiling_first.position = poolPosition_ceiling;
            
            leftFinish = true;
            ceilingMoveIndex = 0;
        }
    }

    void RightCeilingAttack()
    {
        if (ceilingMoveIndex_Right == 0 && rightFinish == false)
        {
            // 오브젝트 풀에 있던 것을 맵 안으로 이동 
            ceiling_second.position = ceiling_OriginPosition;

            ceilingMoveIndex_Right++;
        }

        else if (ceilingMoveIndex_Right == 1)
        {
            ceiling_second.position = Vector2.MoveTowards(ceiling_second.position,
            firstDestination_Right, ceilingBallSpeed * Time.deltaTime);

            if (Vector2.Distance(ceiling_second.position, firstDestination_Right) < 0.01f)
            {
                ceilingMoveIndex_Right++;
            }
        }

        else if (ceilingMoveIndex_Right == 2)
        {
            ceiling_second.position = Vector2.MoveTowards(ceiling_second.position,
            secondDestination_Right, ceilingBallSpeed * Time.deltaTime);

            if (Vector2.Distance(ceiling_second.position, secondDestination_Right) < 0.01f)
            {
                ceilingMoveIndex_Right++;
            }
        }

        else if (ceilingMoveIndex_Right == 3)
        {
            ceiling_second.position = Vector2.MoveTowards(ceiling_second.position,
            thirdDestination_Right, ceilingBallSpeed * Time.deltaTime);

            if (Vector2.Distance(ceiling_second.position, thirdDestination_Right) < 0.01f)
            {
                ceilingMoveIndex_Right++;
            }
        }

        // 오브젝트 풀로 복귀
        else if (ceilingMoveIndex_Right == 4)
        {
            ceiling_second.position = poolPosition_ceiling;
            
            rightFinish = true;
            ceilingMoveIndex_Right = 0;
        }
    }
}
