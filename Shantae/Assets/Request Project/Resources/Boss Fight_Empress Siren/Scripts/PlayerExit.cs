using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExit : MonoBehaviour
{
    private Vector2 playerDestination = default;

    // Start is called before the first frame update
    void Start()
    {
        playerDestination = new Vector2(7.47f, -2.24f);
    }

    // Update is called once per frame
    void Update()
    {
        if (CameraShake.instance.itPlayed == true)
        {
            float moveSpeed = 3f;

            transform.position = Vector2.MoveTowards
                (transform.position, playerDestination, Time.deltaTime * moveSpeed);

            // 만약 출구 좌표의 근사치에 접근했다면
            if (Mathf.Abs(transform.position.x - playerDestination.x) <= 0.01f)
            {
                AllSceneManager.instance.StartCoroutine
                    (AllSceneManager.instance.OpenLoadingScene_Second());
            }
        }
    }
}
