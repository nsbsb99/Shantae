using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoralSirenMoving : MonoBehaviour
{
    public static CoralSirenMoving instance;

    private int randomAttack = default;
    public static bool fireBomb = false;
    public static bool dash = false;
    public static bool fireSpread = false;
    private Animator animator;
    public static Vector2 newBossPosition;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        StartCoroutine(RandomMoving());
    }

    // Update is called once per frame
    IEnumerator RandomMoving()
    {
        //randomAttack = Random.Range(0, 3);
        randomAttack = 2; // 임시

        if (randomAttack == 0)
        {
            Debug.Log("폭탄 발사 시작");
            // 폭탄 발사
            animator.SetBool("Fire Bomb", true);

            fireBomb = true;

            // 폭탄 발사 완료
            if (FireBomb.doneFire == true)
            {
                randomAttack = 1; // 임시
                fireBomb = false;
            }

        }

        else if (randomAttack == 1)
        {
            Debug.Log("대시 시작");
            // 대시 준비 (DashCharging)
            animator.SetBool("Ready Dash", true);
            yield return new WaitForSeconds(1.7f);
            animator.SetBool("Ready Dash", false);

            animator.SetBool("Go Dash", true);
            dash = true;
            
            //yield return new WaitForSeconds(3f);
            // 뒤 coralSiren이 위에 다시 도착한 후
            //animator.SetBool("Go Dash", false);

        }

        else if (randomAttack == 2)
        {
            Debug.Log("불 뿌리기 시작");
            // 불 뿌리기 준비 (FireSpread)
            animator.SetBool("Spread Fire Charge", true);
            
            fireSpread = true;
        }
    }
}
