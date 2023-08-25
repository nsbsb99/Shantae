using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceesoryBreakManager : MonoBehaviour
{
    public List<GameObject> objectList;
    private int gemCount;
    private AudioSource audioSource;
    public AudioClip acceesoryBreak;

    private float originalFixedDeltaTime;
    private bool isTimeSlowed = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = acceesoryBreak;

        originalFixedDeltaTime = Time.fixedDeltaTime;

    }

    // Update is called once per frame
    void Update()
    {
        gemCount = objectList.Count;
        for (int i = objectList.Count - 1; i >= 0; i--)
        {
            if (objectList[i] == null)
            {
                // 리스트에서 파괴된 오브젝트를 제거
                objectList.RemoveAt(i);
                audioSource.Play();
                StartCoroutine(SlowDownTime());
            }
        }
    }
    private IEnumerator SlowDownTime()
    {
        Time.timeScale = 0.5f; // 슬로우모션
        yield return new WaitForSecondsRealtime(3.0f); 
        Time.timeScale = 1.0f; 
    }
}
