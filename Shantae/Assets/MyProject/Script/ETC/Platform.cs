using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Platform : MonoBehaviour
{
    private Collider2D platform;
    private PlayerController playerController;
    private ObjectPool<GameObject> pool;
    public Vector2 movementDirection;
    public bool first = false;
   public void Initialize(ObjectPool<GameObject> objectPool)
    {
        pool = objectPool;
    }
    private void Start()
    {
        Rigidbody2D rb = transform.GetComponent<Rigidbody2D>();
        platform = GetComponent<Collider2D>(); 
        playerController = FindObjectOfType<PlayerController>();
        if (first)
        {
            rb.velocity = movementDirection * -3f;
        }
    }
    void Update()
    {
        float playerBottom = playerController.bottomY;
        float objectHeight = GetComponent<Renderer>().bounds.extents.y;
        float topY = transform.position.y + objectHeight;

        

        if( topY - 0.5 <= playerBottom)
        {
            platform.isTrigger = false;
        }
        else
        {
            platform.isTrigger = true;
        }

    }   
}
