using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform player;
    public float minX;
    public float maxX;

    // Update is called once per frame
    private void LateUpdate()
    {

        if (player != null)
        {
            Vector3 newPosition = transform.position;
            newPosition.x = Mathf.Clamp(player.position.x, minX, maxX);
            transform.position = newPosition;
        }

    }
}