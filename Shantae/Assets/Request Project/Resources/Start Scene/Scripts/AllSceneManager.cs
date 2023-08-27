using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllSceneManager : MonoBehaviour
{
    public static AllSceneManager instance;
    private bool alreadyEmpressScene = false;

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
        SceneManager.LoadScene("Loading Scene", LoadSceneMode.Additive);
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
        SceneManager.UnloadScene("Loading Scene");
    }

    public IEnumerator OpenLoadingScene_Second()
    {
        if (alreadyEmpressScene == false)
        {
            alreadyEmpressScene = true;

            SceneManager.LoadScene("Loading Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.5f);

            SceneManager.LoadScene("Boss Fight_Empress Siren", LoadSceneMode.Single);
        }
    }

    public IEnumerator OpenLoadingScene_Third() 
    {
        SceneManager.LoadScene("Loading Scene", LoadSceneMode.Single);
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("Boss Fight Mega_Empress Siren", LoadSceneMode.Single);
    }
}
