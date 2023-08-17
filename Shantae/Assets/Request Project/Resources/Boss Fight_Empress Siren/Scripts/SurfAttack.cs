using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Empress Siren의 양손에 회전하는 검이 나타나는 공격 클래스
/// </summary>

public class SurfAttack : MonoBehaviour
{
    #region 검 발사 변수
    // 공격 시작 지점
    private Vector2 startPosition_Front;
    private Vector2 startPosition_Back;

    // 프리팹
    private GameObject frontScimatarPrefab;
    private GameObject backScimatarPrefab;

    // 실 게임오브젝트
    private GameObject frontScimatar;
    private GameObject backScimatar;

    // 오브젝트 풀 좌표
    private Vector2 poolPosition_scimartar = new Vector2(0f, 10.0f);

    // 던져진 것을 체크
    private bool runCheck = false;

    private float bladeSpeed = 3f;
    private Vector2 nowPlayerPosition;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //frontScimatarPrefab = Resources.Load<GameObject>("Prefabs/Scimatars_Front");
        frontScimatarPrefab = Resources.Load<GameObject>
            ("Boss Fight_Empress Siren/Prefabs/Blade_Front");
        backScimatarPrefab = Resources.Load<GameObject>
            ("Boss Fight_Empress Siren/Prefabs/Blade_Back");

        Debug.Assert(frontScimatarPrefab != null);
        Debug.Assert(backScimatarPrefab != null);

        // 오브젝트 풀에 생성
        frontScimatar = Instantiate
            (frontScimatarPrefab, poolPosition_scimartar, Quaternion.identity);
        backScimatar = Instantiate
            (backScimatarPrefab, poolPosition_scimartar, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (EmpressMoving.surf == true && runCheck == false)
        {
            StartCoroutine(FireBlades());
        }
        else if (EmpressMoving.surf == false)
        {
            StopCoroutine(FireBlades());

            // 애니메이션이 끝나면 풀로 복귀 (던진 후 특정한 시간이 지나는 조건 충족 시로 수정)
            frontScimatar.transform.position = poolPosition_scimartar;
            backScimatar.transform.position = poolPosition_scimartar;

            runCheck = false;
        }
    }

    IEnumerator FireBlades()
    {
        // 발사 시작 위치 
        startPosition_Front =
            new Vector2(transform.position.x + 2, transform.position.y + 0.4f);
        startPosition_Back =
            new Vector2(transform.position.x - 1.03f, transform.position.y + 0.5f);

        frontScimatar.transform.position = startPosition_Front;
        backScimatar.transform.position = startPosition_Back;

        nowPlayerPosition =
        new Vector2(GameObject.FindGameObjectWithTag("Player").transform.position.x + 1f,
        GameObject.FindGameObjectWithTag("Player").transform.position.y);

        yield return new WaitForSeconds(2.0f);

        runCheck = true;

        if (runCheck == true)
        {
            frontScimatar.transform.Translate(nowPlayerPosition * bladeSpeed
                                * Time.deltaTime);
            backScimatar.transform.Translate(nowPlayerPosition * bladeSpeed
                                * Time.deltaTime);
        }
    }
}
