using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FireSpread 스크립트와 연계해, 앞의 Coral Siren이 바닥과 충돌했는지 전달하는 클래스
/// </summary>

public class FrontGrounded : MonoBehaviour
{
    public static bool coralSiren_Front_Grounded = false;
    private bool justOne = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 만약 앞의 Coral Siren이 땅과 충돌하면 
        if (collision.gameObject.CompareTag("Ground"))
        {
            coralSiren_Front_Grounded = true;
        }
    }
}
