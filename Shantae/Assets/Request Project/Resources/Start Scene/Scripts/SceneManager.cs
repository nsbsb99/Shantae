using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;

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
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Pause Scene");
        }
    }

    public IEnumerator OpenLoadingScene()
    {
        UnityEngine.SceneManagement.
            SceneManager.LoadScene("Loading Scene", LoadSceneMode.Single);

        yield return new WaitForSeconds(3);

        /// <point> 추후 완성 시 'Lobby'로 교체
        UnityEngine.SceneManagement.
            SceneManager.LoadScene("Boss Fight_Empress Siren", LoadSceneMode.Single);
    }
}
