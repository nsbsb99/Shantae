using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            NextMegaEmpress();
        }
    }

    private void NextMegaEmpress()
    {
        Debug.Log("Empress Siren과의 2차전으로 이동");
    }
}
