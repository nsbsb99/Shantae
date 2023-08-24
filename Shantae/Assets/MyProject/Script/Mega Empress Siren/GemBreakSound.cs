using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemBreakSound : MonoBehaviour
{
    public List<GameObject> objectList;
    private int gemCount;
    private AudioSource audioSource;
    public AudioClip gemBreak;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = gemBreak;
    }

    // Update is called once per frame
    void Update()
    {
        gemCount = objectList.Count;
        Debug.Log(gemCount);
        for (int i = objectList.Count - 1; i >= 0; i--)
        {
            if (objectList[i] == null)
            {
                // 리스트에서 파괴된 오브젝트를 제거합니다.
                objectList.RemoveAt(i);
                audioSource.Play();
            }
        }
    }
}
