using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowKissMoving : MonoBehaviour
{
    public Vector2 blowKissDestination = default;
    private float blowKissSpeed = 15f;
    private Transform empress;

    private void Start()
    {
        empress = GameObject.Find("Empress Siren").transform;
        Debug.Assert(empress != null);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, blowKissDestination,
            blowKissSpeed * Time.deltaTime);

        if (Vector2.Distance(empress.position, transform.position) >= 0.5f)
        {
            transform.GetComponent<SpriteRenderer>().enabled = true;
        }

    }
}
