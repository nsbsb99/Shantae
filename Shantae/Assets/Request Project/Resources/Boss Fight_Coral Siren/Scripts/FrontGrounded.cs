using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// FireSpread 스크립트와 연계해, 앞의 Coral Siren이 바닥과 충돌했는지 전달하는 클래스
/// </summary>

public class FrontGrounded : MonoBehaviour
{
    public static bool coralSiren_Front_Grounded = false;
    public static bool coralSiren_Front_Sanded = false;
    private bool justOne = false;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 만약 앞의 Coral Siren이 모래와 충돌하면 
        if (collision.gameObject.CompareTag("SandStep") ||
            (collision.gameObject.CompareTag("SandPiece")) && coralSiren_Front_Grounded == false)
        {
            Debug.Log("모래와 충돌!");
            coralSiren_Front_Grounded = true;
        }

        // 만약 앞의 Coral Siren이 그냥 땅과 충돌하면
        if (collision.gameObject.CompareTag("Ground") && coralSiren_Front_Sanded == false)
        {
            Debug.Log("땅과 충돌!");
            coralSiren_Front_Sanded = true;
        }

    }
}
