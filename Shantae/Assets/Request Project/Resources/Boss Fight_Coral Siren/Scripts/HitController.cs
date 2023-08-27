using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitController : MonoBehaviour
{
    private GameObject coralSiren_Parent;
    public static bool coralDamaged = false;
    private int coralSirenHP = 30;

    private void Awake()
    {
        // �θ� ������Ʈ Coral Siren�� ���� ������Ʈ
        coralSiren_Parent = gameObject.transform.parent.gameObject;
    }
    private void Update()
    {
        if(coralSirenHP <= 0)
        {
            CoralSirenController.die = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            coralDamaged = true;
            coralSirenHP -= 1;
        }
    }
}
