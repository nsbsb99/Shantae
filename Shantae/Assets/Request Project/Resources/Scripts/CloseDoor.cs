using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 클래스: 대사 패스 등 조건 충족 시 문이 닫히는 효과
/// </summary>

public class CloseDoor : MonoBehaviour
{
    private void Awake() // 후에 대사 추가 시 대사가 종료되면 코루틴이 시작하도록 조건 걸기 (Update로 변경)
    {
        StartCoroutine(DoorTimer());
    }

    IEnumerator DoorTimer()
    {
        // 조건 입력 몇 초 후 문 닫는 메서드로 이동
        yield return new WaitForSeconds(3.0f);
        CloseTwoDoors();
    }

    void CloseTwoDoors()
    {
        /// <summary>
        /// 문이 열려있는 백그라운드에서 문이 닫힌 백그라운드로 교체 
        /// </summary>>
        
        // First Background 비활성화 + Second Background로 덮기
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);

        // Camera Shaker 이동
        StartCoroutine(CameraShake.instance.ShakeThisCam());
    }
}
