using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 클래스: 게임 오브젝트 충격 시 카메라가 흔들리는 효과
/// </summary>

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    // ShakeThisCam
    private const float firstShakeTime = 0.15f;
    private const float secondShakeTime = 0.1f;
    private const float shakeSpeed = 10.0f;
    private const float firstShakeAmount = 0.1f;
    private const float secondShakeAmount = 0.05f;

    // CeilingShake
    private float ceilingShakeAmount = 0.07f;
    private float ceilingShakeTime = 0.5f;
    private float ceilingShakeSpeed = 20.0f;

    private Vector3 originalPosition;

    // Empress Siren 패배 시 백그라운드 교체
    private GameObject backgrounds;
    public bool itPlayed = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(this);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;

        backgrounds = GameObject.Find("Backgrounds");

        StartCoroutine(ShakeThisCam());
    }

    public IEnumerator ShakeThisCam()
    {
        ///<summary>
        /// 처음 백그라운드 전환 시 카메라를 흔드는 코루틴. 
        ///</summary>

        Vector3 originPosition = transform.position;

        float elapsed = 0.0f;

        /// <point> Empress Siren의 시작 유예 시간. (컷신 종료 후 실행되도록 수정 필요)
        yield return new WaitForSeconds(3f);

        while (elapsed < firstShakeTime)
        {
            float yOffset = Mathf.Sin(Time.time * shakeSpeed) * firstShakeAmount;
            transform.position = new Vector3
                (originPosition.x, originPosition.y - yOffset, -10f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 본래 좌표로 복귀
        transform.position = originalPosition;

        elapsed = 0.0f;

        while (elapsed < secondShakeTime)
        {
            float yOffset = Mathf.Sin(Time.time * shakeSpeed) * secondShakeAmount;
            transform.position = new Vector3
                (originPosition.x, originPosition.y - yOffset, -10f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 본래 좌표로 복귀
        transform.position = originalPosition;

        StopCoroutine(ShakeThisCam());
    }

    public IEnumerator CeilingShake()
    {
        ///<summary>
        /// Empress Siren이 천장에서 공격 시 카메라를 흔드는 코루틴. (상하좌우)
        ///</summary>

        Vector3 originPosition = transform.position;

        float elapsed = 0.0f;

        while (elapsed < ceilingShakeTime)
        {
            elapsed += Time.deltaTime;

            float xOffset = Mathf.Sin(Time.time * shakeSpeed) * ceilingShakeAmount;
            float yOffset = Mathf.Sin(Time.time * shakeSpeed) * ceilingShakeAmount;
            transform.position = new Vector3
                (originPosition.x - xOffset, originPosition.y + yOffset, -10f);

            yield return null;
        }

        elapsed = 0.0f;

        while (elapsed < ceilingShakeTime)
        {
            elapsed += Time.deltaTime;

            float xOffset = Mathf.Sin(Time.time * ceilingShakeSpeed) * ceilingShakeAmount;
            float yOffset = Mathf.Sin(Time.time * ceilingShakeSpeed) * ceilingShakeAmount;
            transform.position = new Vector3
                (originPosition.x + xOffset, originPosition.y - yOffset, -10f);

            yield return null;
        }

        // 본래 좌표로 복귀
        transform.position = originalPosition;

        StopCoroutine(CeilingShake());
    }

    public IEnumerator OpenTheDoor()
    {
        Vector3 originPosition = transform.position;

        float elapsed = 0.0f;

        while (elapsed < firstShakeTime)
        {
            float yOffset = Mathf.Sin(Time.time * shakeSpeed) * firstShakeAmount;
            transform.position = new Vector3
                (originPosition.x, originPosition.y - yOffset, -10f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 본래 좌표로 복귀
        transform.position = originalPosition;

        elapsed = 0.0f;

        while (elapsed < secondShakeTime)
        {
            float yOffset = Mathf.Sin(Time.time * shakeSpeed) * secondShakeAmount;
            transform.position = new Vector3
                (originPosition.x, originPosition.y - yOffset, -10f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 백그라운드 교체 
        backgrounds.transform.GetChild(2).gameObject.SetActive(false);
        backgrounds.transform.GetChild(1).gameObject.SetActive(true);

        // 본래 좌표로 복귀
        transform.position = originalPosition;

        // PlayerExit 활성화
        itPlayed = true;

        StopCoroutine(OpenTheDoor());
    }    
}
