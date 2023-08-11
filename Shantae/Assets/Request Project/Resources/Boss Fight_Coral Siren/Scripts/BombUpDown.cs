using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombUpDown : MonoBehaviour
{
    private float upSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * upSpeed * Time.deltaTime);
    }
}
