using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitController : MonoBehaviour
{
    private GameObject coralSiren_Parent;
    public static bool coralDamaged = false;

    private void Awake()
    {
        // 부모 오브젝트 Coral Siren의 게임 오브젝트
        coralSiren_Parent = gameObject.transform.parent.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            Debug.Log("보스가 공격 받음!");

            coralDamaged = true;
        }
    }
}
