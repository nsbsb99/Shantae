using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpressMove : MonoBehaviour
{
    private bool moveLeft = true;
    private float moveSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }        

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x > 10)
        { moveLeft = true; }
        else if(transform.position.x < -10)
        { moveLeft = false; }
        if(moveLeft)
        { transform.Translate(Vector3.left * moveSpeed * Time.deltaTime); }
        else if(!moveLeft)
        { transform.Translate(Vector3.right * moveSpeed * Time.deltaTime); }
    }
}
