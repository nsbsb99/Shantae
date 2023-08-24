using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    private AudioSource bgm;
    public static bool playing = true;            // 보스전 끝나면 보스 코드에서 건들일 bool값;
    // Start is called before the first frame update
    void Start()
    {
        bgm = GetComponent<AudioSource>();

        bgm.loop = true;

        bgm.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(!playing)
        {
            bgm.Stop();
        }
    }
}
