using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Empress Siren의 패배 조건을 다루는 스크립트. "PlayerAttack" 태그에 충돌하면 
/// Empress Siren의 HP가 감소.
/// </summary>
/// 
public class EmpressController : MonoBehaviour
{
    // Empress Siren의 HP
    public static float empressHP = default;

    private void Start()
    {
        // Empress Siren 게임 오브젝트가 !null인지 확인
        Debug.Assert(this.gameObject != null);

        empressHP = 100f;
    }

    private void Update()
    {
        // Empress Siren의 패배 확인
        if(empressHP <= 0)
        {
            // EmpressMoving 코루틴 종료 메서드 추가
            StopCoroutine(EmpressMoving.instance.RandomMoving());

            PlayerWin();
        }
    }

    private void PlayerWin()
    {
        

    }

    private void NextMegaEmpress()
    {
        Debug.Log("Empress Siren과의 2차전으로 이동");
        // 로딩씬 이후 다음 보스전인 MegaEmpress Siren 로드하기. 
    }
}
