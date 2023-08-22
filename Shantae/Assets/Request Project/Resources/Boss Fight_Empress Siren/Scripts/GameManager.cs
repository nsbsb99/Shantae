using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool mageSiren;

    // 김건휘 작성
    public GameObject body;
    public Transform particle;
    public Transform platform;
    public Transform player;
    public Transform spawner;
    private bool move = false;
    // 김건휘

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(instance != this)
            {
                Destroy(this);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ESC 버튼을 누를 시 빌드한 exe 게임 파일 종료. (후에 수정 필요)
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (mageSiren)
        {
            if (body == null)
            {
                pase2_5();
            }
        }
    }

   
    // 김건휘 작성
    private void  pase2_5()
    {        

        if (player != null)
        {
            if (!move)
            {
                Vector3 newPosition = new Vector3(0f, 15f, 0f); // 새로운 위치 설정
                player.position = newPosition;
                move = true;
            }
        }
        if (particle != null)                                                    /// 떨어졌을때 넣을 코드
        {
            Vector3 newPosition = new Vector3(0f, -9f, 0f); // 새로운 위치 설정
            particle.position = newPosition;
        }
        if (platform != null)
        {
            if (!move)
            {
                Vector3 newPosition = new Vector3(0f, 11f, 0f); // 새로운 위치 설정
                platform.position = newPosition;
                move = true;
            }
        }
    }

    // 김건휘 작성

}
