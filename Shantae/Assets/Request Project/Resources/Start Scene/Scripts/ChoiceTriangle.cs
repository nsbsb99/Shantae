using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiceTriangle : MonoBehaviour
{
    // ���� �ﰢ���� �����ϱ� ���� UI Canvas
    private GameObject canvas_UI;
    // ���� �� ��ġ�� �ﰢ��
    private GameObject firstTriangle;
    // ���� �� ��ġ�� �ﰢ�� 
    private GameObject secondTriangle;
    // ����/���Ḧ ��Ÿ��
    private bool wantStart = default;
    // ȭ��ǥ�� �����̸� �︱ ��ƽ��
    private AudioSource tiktok = default;
    // ��� ����
    private AudioSource bgm = default;

    // Start is called before the first frame update
    void Start()
    {
        // ó���� '����'�� ��ġ�ϵ���
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
            // �ﰢ�� ��ġ
            firstTriangle.SetActive(true);
            secondTriangle.SetActive(false);

            // ���⼭ ���� �Ʒ� ����Ű�� ������ wantStart = false;
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                wantStart = false;
                tiktok.Play();
            }

            // Enter �Է� �� ���� ������.
            if (Input.GetKeyDown(KeyCode.Return))
            {
                bgm.Stop();

                AllSceneManager.instance.StartCoroutine
                    (AllSceneManager.instance.OpenLoadingScene());
            }
        }    
        else if (wantStart == false)
        {
            // �ﰢ�� ��ġ
            firstTriangle.SetActive(false);
            secondTriangle.SetActive(true);

            // ���⼭ ���� �Ʒ� ����Ű�� ������ wantStart = true;
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                wantStart = true;
                tiktok.Play();
            }

            // Enter �Է� �� ���� ����
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Application.Quit();
            }
        }
    }
}
