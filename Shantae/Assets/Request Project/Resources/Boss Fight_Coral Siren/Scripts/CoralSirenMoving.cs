using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoralSirenMoving : MonoBehaviour
{
    public static CoralSirenMoving instance;

    private int randomAttack = default;
    public static bool fireBomb = false;
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
    }

    // Update is called once per frame
    void Update()
    {
        //randomAttack = Random.Range(0, 3);
        randomAttack = 0;

        if (randomAttack == 0)
        {
            animator.SetBool("Fire Bomb", true);
            newBossPosition = transform.position;

            fireBomb = true;
        }
    }
}
