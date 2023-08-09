using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLocation : MonoBehaviour
{
    public Transform player;
    public float minY;
    public float maxY;

    // Update is called once per frame
    private void LateUpdate()
    {
        if (player != null )
        {
            Vector3 newPosition = transform.position;
            newPosition.y = Mathf.Clamp(player.position.y, minY, maxY);
            transform.position = newPosition;
        }
    }
}
