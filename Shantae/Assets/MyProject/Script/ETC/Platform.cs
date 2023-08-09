using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private Collider2D platform;
    private PlayerController playerController;
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("end"))
        {
            Destroy(gameObject);
        }
    }
}
