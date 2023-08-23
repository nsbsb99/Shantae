using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllSceneManager : MonoBehaviour
{
    public static AllSceneManager instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 일시정지 화면 필요
        }
    }

    public IEnumerator OpenLoadingScene()
    {
        SceneManager.LoadScene("Loading Scene", LoadSceneMode.Single);

        yield return new WaitForSeconds(3);

        /// <point> 추후 완성 시 'Lobby'로 교체
        SceneManager.LoadScene("Boss Fight_Empress Siren", LoadSceneMode.Single);
    }

    public IEnumerator OpenLoadingScene_Second()
    {
        Debug.Log("코루틴 진입!");

        SceneManager.LoadScene("Loading Scene", LoadSceneMode.Single);

        yield return new WaitForSeconds(3);

        /// <point> 추후 완성 시 'Mega Empress Siren'으로 교체
        SceneManager.LoadScene("Boss Fight_Coral Siren", LoadSceneMode.Single);
    }
}
