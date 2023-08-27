using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiceTriangle : MonoBehaviour
{
    // 선택 삼각형에 접근하기 위한 UI Canvas
    private GameObject canvas_UI;
    // 시작 시 위치할 삼각형
    private GameObject firstTriangle;
    // 종료 시 위치할 삼각형 
    private GameObject secondTriangle;
    // 시작/종료를 나타냄
    private bool wantStart = default;
    // 화살표를 움직이면 울릴 햅틱음
    private AudioSource tiktok = default;
    // 배경 사운드
    private AudioSource bgm = default;

    // Start is called before the first frame update
    void Start()
    {
        // 처음엔 '시작'에 위치하도록
        wantStart = true;

        canvas_UI = GameObject.Find("UI Canvas");
        firstTriangle = canvas_UI.transform.GetChild(3).gameObject;
        Debug.Assert(firstTriangle != null);
        secondTriangle = canvas_UI.transform.GetChild(4).gameObject;
        Debug.Assert(secondTriangle != null);

        bgm = GameObject.Find("Sound Controller").GetComponent<AudioSource>();

        tiktok = GetComponent<AudioSource>();
        Debug.Assert(tiktok != null);
    }

    // Update is called once per frame
    void Update()
    {
        if (wantStart == true)
        {
            // 삼각형 위치
            firstTriangle.SetActive(true);
            secondTriangle.SetActive(false);

            // 여기서 위나 아래 방향키를 누르면 wantStart = false;
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                wantStart = false;
                tiktok.Play();
            }

            // Enter 입력 시 다음 씬으로.
            if (Input.GetKeyDown(KeyCode.Return))
            {
                bgm.Stop();

                AllSceneManager.instance.StartCoroutine
                    (AllSceneManager.instance.OpenLoadingScene());
            }
        }    
        else if (wantStart == false)
        {
            // 삼각형 위치
            firstTriangle.SetActive(false);
            secondTriangle.SetActive(true);

            // 여기서 위나 아래 방향키를 누르면 wantStart = true;
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                wantStart = true;
                tiktok.Play();
            }

            // Enter 입력 시 게임 종료
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Application.Quit();
            }
        }
    }
}
