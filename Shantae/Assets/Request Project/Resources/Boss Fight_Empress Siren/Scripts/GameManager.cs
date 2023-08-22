using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 전체 UI와 ESC 반응 등을 총괄. 이후 Lobby로 이동. 
/// </summary>

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    // 캔버스와 하트
    private GameObject canvas_UI;
    private Transform firstHeart_UI;
    private Transform secondHeart_UI;
    private Transform thirdHeart_UI;
    private Transform fourthHeart_UI;

    // 플레이어 허용 피격 횟수
    private int fullLife = 16;

    // 김건휘 작성
    public GameObject body;
    public Transform platform;
    public Transform player;
    public Transform spawner;
    private bool move = false;
    public bool pase2Camera = false;
    // 김건휘

    private void Awake()
    {
        if(instance == null)
        {
            /// <point> GameManager는 이후 Lobby로 옮기도록 함. 
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

    private void Start()
    {
        // UI 캔버스
        canvas_UI = GameObject.Find("UI Canvas");
        DontDestroyOnLoad(canvas_UI);

        firstHeart_UI = canvas_UI.transform.GetChild(0);
        secondHeart_UI = canvas_UI.transform.GetChild(1);
        thirdHeart_UI = canvas_UI.transform.GetChild(2);
        fourthHeart_UI = canvas_UI.transform.GetChild(3);
    }

    // Update is called once per frame
    void Update()
    {
        // ESC 버튼을 누를 시 빌드한 exe 게임 파일 종료. (후에 수정 필요)
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }


        if(body == null)
        {
            pase2_5();
        }

        if (PlayerController.gotDamage == true)
        {
            /// <problem> 무적시간에도 플레이어가 피해를 입는 문제 처리 필요. 

            if (fullLife <= 16 && fullLife >= 13) // 네번째 하트
            {
                if (fullLife == 16)
                {
                    PlayerController.gotDamage = false;

                    fourthHeart_UI.GetChild(4).gameObject.SetActive(false);
                    fullLife--;
                }
                else if (fullLife == 15)
                {
                    PlayerController.gotDamage = false;

                    fourthHeart_UI.GetChild(3).gameObject.SetActive(false);
                    fullLife--;
                }
                else if (fullLife == 14)
                {
                    PlayerController.gotDamage = false;

                    fourthHeart_UI.GetChild(2).gameObject.SetActive(false);
                    fullLife--;
                }
                else if (fullLife == 13)
                {
                    PlayerController.gotDamage = false;

                    fourthHeart_UI.GetChild(1).gameObject.SetActive(false);
                    fullLife--;

                    /// <point> 빈 하트는 남겨두어야 하니 GetChild(0)은 패스.
                }
            }

            if (fullLife <= 12 && fullLife >= 9) // 세번째 하트
            {
                if (fullLife == 12)
                {
                    PlayerController.gotDamage = false;

                    thirdHeart_UI.GetChild(4).gameObject.SetActive(false);
                    fullLife--;
                }
                else if (fullLife == 11)
                {
                    PlayerController.gotDamage = false;

                    thirdHeart_UI.GetChild(3).gameObject.SetActive(false);
                    fullLife--;
                }
                else if (fullLife == 10)
                {
                    PlayerController.gotDamage = false;

                    thirdHeart_UI.GetChild(2).gameObject.SetActive(false);
                    fullLife--;
                }
                else if (fullLife == 9)
                {
                    PlayerController.gotDamage = false;

                    thirdHeart_UI.GetChild(1).gameObject.SetActive(false);
                    fullLife--;
                }
            }

            if (fullLife <= 8 && fullLife >= 5) // 두번째 하트
            {
                if (fullLife == 8)
                {
                    PlayerController.gotDamage = false;

                    secondHeart_UI.GetChild(4).gameObject.SetActive(false);
                    fullLife--;
                }
                else if (fullLife == 7)
                {
                    PlayerController.gotDamage = false;

                    secondHeart_UI.GetChild(3).gameObject.SetActive(false);
                    fullLife--;
                }
                else if (fullLife == 6)
                {
                    PlayerController.gotDamage = false;

                    secondHeart_UI.GetChild(2).gameObject.SetActive(false);
                    fullLife--;
                }
                else if (fullLife == 5)
                {
                    PlayerController.gotDamage = false;

                    secondHeart_UI.GetChild(1).gameObject.SetActive(false);
                    fullLife--;
                }
            }

            if (fullLife <= 4 && fullLife >= 1) // 첫번째 하트
            {
                if (fullLife == 4)
                {
                    PlayerController.gotDamage = false;

                    firstHeart_UI.GetChild(4).gameObject.SetActive(false);
                    fullLife--;
                }
                else if (fullLife == 3)
                {
                    PlayerController.gotDamage = false;

                    firstHeart_UI.GetChild(3).gameObject.SetActive(false);
                    fullLife--;
                }
                else if (fullLife == 2)
                {
                    PlayerController.gotDamage = false;

                    firstHeart_UI.GetChild(2).gameObject.SetActive(false);
                    fullLife--;
                }
                else if (fullLife == 1)
                {
                    PlayerController.gotDamage = false;

                    firstHeart_UI.GetChild(1).gameObject.SetActive(false);
                    fullLife--;
                }
            }

            if (fullLife == 0) // 플레이어 죽음 처리
            {
                Debug.Log("플레이어 죽음");
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
        //if (spawner != null)                                                    /// 떨어졌을때 넣을 코드
        //{
        //    Vector3 newPosition = new Vector3(10f, 0f, 0f); // 새로운 위치 설정
        //    spawner.position = newPosition;
        //}
        if(platform != null)
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
