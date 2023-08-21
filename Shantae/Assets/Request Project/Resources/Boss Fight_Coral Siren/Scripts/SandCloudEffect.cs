using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SandCloudEffect : MonoBehaviour
{
    #region 모래구름 변수
    // 모래구름의 본래 위치
    private Vector2 originPosition = default;
    // 모래구름의 목적 위치
    private Vector2 cloudDestination = default;
    // 모래구름의 상승 속도
    private float cloudSpeed = 0.3f;
    // 모래구름이 커지는 정도
    private float cloudScaleUp = 1.1f;
    private Vector2 originScale = default;
    private Vector2 upScale = default;
    // 모래구름의 컬러 (Sprite Renderer)
    private SpriteRenderer cloudColor = default;
    // 모래구름의  원래 알파값
    private float cloudOriginAlpha = default;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        originScale = new Vector2(0.5f, 0.5f);
        upScale = new Vector2(1, 1);
        originPosition = new Vector2(transform.position.x, 0.28f);
        cloudDestination = new Vector2(transform.position.x, transform.position.y + 0.5f);

        cloudColor = GetComponent<SpriteRenderer>();
        Debug.Assert(cloudColor != null);

        cloudOriginAlpha = 0.5607f; // RGB 0-1.0표기
    }

    // Update is called once per frame
    void Update()
    {
        // 지정 스케일보다 작다면
        if (transform.localScale.x < upScale.x && transform.localScale.y < upScale.y)
        {
            transform.localScale *= cloudScaleUp;
        }
        else // 지정 스케일을 만족했다면 스케일 고정
            transform.localScale = upScale;

        // 지정 스케일보다 크다면
        if (transform.localScale.x >= upScale.x && transform.localScale.y >= upScale.y)
        {
            transform.position = Vector2.MoveTowards
                (transform.position, cloudDestination, Time.deltaTime * cloudSpeed);

            StartCoroutine(CloudFadeOut());
        }
    }

    IEnumerator CloudFadeOut()
    {
        while (cloudColor.color.a > 0)
        {
            cloudOriginAlpha -= 0.001f;
            yield return new WaitForSeconds(0.5f);
            cloudColor.color = new Color(1, 0.8588f, 0.6196f, cloudOriginAlpha);
        }
    }
}
