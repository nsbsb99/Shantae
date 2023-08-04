using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    // 백그라운드가 흔들리는 스피드 
    private float shakeSpeed = default;

    private void Awake() // 후에 대사 추가 시 대사가 종료되면 코루틴이 시작하도록 조건 걸기 (Update)
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
        //First Background 비활성화 + Second Background로 덮기
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);

        // 이후 Camera Shaker 추가
    }

}
