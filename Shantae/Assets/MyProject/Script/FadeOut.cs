using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    // Start is called before the first frame update
    public Image fadeImage;
    public float fadeDuration = 1.0f;

    private float timer = 0f;

    private void Start()
    {
    }
    private void Update()
    {
        if(EndBoss.finish == true)
        {
            BGMManager.playing = false;
            timer += Time.deltaTime;
            if( timer > 2f )
            {
                StartCoroutine(DoFadeOut());

            }
        }
    }

    private IEnumerator DoFadeOut()
    {
        Color originalColor = fadeImage.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1.0f);

        float startTime = Time.time;
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime = Time.time - startTime;
            float percentage = elapsedTime / fadeDuration;

            fadeImage.color = Color.Lerp(originalColor, targetColor, percentage);

            yield return null;
        }

    }
}
