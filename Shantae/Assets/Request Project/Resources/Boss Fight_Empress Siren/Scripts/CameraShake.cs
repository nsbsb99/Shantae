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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // 메인카메라 파괴 방지
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

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    public IEnumerator ShakeThisCam()
    {
        ///<summary>
        /// 처음 백그라운드 전환 시 카메라를 흔드는 코루틴. 
        ///</summary>

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
}
