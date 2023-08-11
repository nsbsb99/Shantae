using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Platform : MonoBehaviour
{
    private Collider2D platform;
    private PlayerController playerController;
    private ObjectPool<GameObject> pool;

   public void Initialize(ObjectPool<GameObject> objectPool)
    {
        pool = objectPool;
    }
    private void Start()
    {
        platform = GetComponent<Collider2D>(); 
        playerController = FindObjectOfType<PlayerController>();
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
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("end"))
    //    {
    //        if (pool  != null)
    //        {
    //            pool.Release(gameObject);
    //        }
    //    }
    //}
}
