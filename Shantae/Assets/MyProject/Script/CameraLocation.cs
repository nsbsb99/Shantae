using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLocation : MonoBehaviour
{
    public GameObject body;
    public Transform player;
    public float minY;
    public float maxY;

    // Update is called once per frame
    private void LateUpdate()
    {
        if (body != null)
        {
            if (player != null)
            {                
                Vector3 newPosition = transform.position;
                newPosition.y = Mathf.Clamp(player.position.y, minY, maxY);
                transform.position = newPosition;
            }
        }
        else
        {
            Vector3 newPosition = new Vector3(0f, 15f, -1f); // 새로운 위치 설정
            transform.position = newPosition;
            Camera camera = GetComponent<Camera>();

            // 카메라 크기 설정
            camera.orthographicSize = 12f;
        }

    }
}
