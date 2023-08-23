using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntry : MonoBehaviour
{
    private Vector2 destination = default;
    private float moveSpeed = 2f;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        destination = new Vector2(-4.81f, -2.24f);
        animator = GetComponent<Animator>();

        animator.SetBool("isGround", true);
        animator.SetBool("Run", true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards
            (transform.position, destination, Time.deltaTime * moveSpeed);

        if (Mathf.Abs(transform.position.x - destination.x) <= 0.01f)
        {
            animator.SetBool("Run", false);
            transform.GetComponent<PlayerController>().enabled = true;
        }
    }
}
