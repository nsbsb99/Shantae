using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SandCloudEffect : MonoBehaviour
{
    #region 모래구름 변수
    // 모래구름의 본래 위치
    private Vector2 originPosition = default;
    // 모래구름의 본래 색상
    private Color originColor = default;
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
    // 변경 완료를 알림.
    public bool changeFinished = false;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // 크기 변수
        originScale = new Vector2(0.65f, 0.65f);
        upScale = new Vector2(1, 1);
        // 위치 변수
        originPosition = new Vector2(transform.position.x, 0.28f);
        cloudDestination = new Vector2(transform.position.x, transform.position.y + 0.5f);
        // 컬러 변수
        originColor = new Color(1, 0.8588f, 0.6196f, 0.5607f);
        cloudOriginAlpha = 0.5607f; // RGB 0-1.0표기

        transform.localScale = originScale;

        // 모래 구름의 SpriteRenderer
        cloudColor = GetComponent<SpriteRenderer>();
        Debug.Assert(cloudColor != null);
        cloudColor.color = originColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (GrabLever.recolor == true && CoralSirenMoving.fourthPatternDone == false)
        {
            changeFinished = false;
            transform.GetComponent<SpriteRenderer>().enabled = true;

            // 지정 스케일보다 작다면
            if (transform.localScale.x < upScale.x && transform.localScale.y < upScale.y)
            {
                transform.localScale *= cloudScaleUp;
            }
            else // 지정 스케일을 만족했다면 스케일 고정
            {
                transform.localScale = upScale;
            }

            // 지정 스케일보다 크다면
            if (transform.localScale.x >= upScale.x && transform.localScale.y >= upScale.y)
            {
                transform.position = Vector2.MoveTowards
                    (transform.position, cloudDestination, Time.deltaTime * cloudSpeed);

                StartCoroutine(CloudFadeOut());
            }
        }
        else if (GrabLever.recolor == false && changeFinished == true)
        {
            transform.GetComponent<SpriteRenderer>().enabled = false;

            // 초기화
            transform.localScale = originScale;
            cloudColor.color = originColor;
        }
    }

    IEnumerator CloudFadeOut()
    {
        /// <problem> 두번째 이상 실행부터 천천히 감소하지 않는다. 
        while (cloudColor.color.a > 0)
        {
            cloudOriginAlpha -= 0.001f;
            yield return new WaitForSeconds(0.5f);
            cloudColor.color = new Color(1, 0.8588f, 0.6196f, cloudOriginAlpha);
        }

        changeFinished = true;

        yield return null;
    }
}
