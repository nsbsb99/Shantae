using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiara_Gem : MonoBehaviour
{
    public GameObject beam;

    private bool isTimerStarted = false;
    private float startTime;
    private float elapsedTime = 15;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTimerStarted)
        {
            isTimerStarted = true;
            startTime = Time.time;
        }

        if (isTimerStarted)
        {
            elapsedTime = Time.time - startTime;
        }
        if (elapsedTime >= 20f)
        {
            beam.SetActive(true);
            elapsedTime = 0;
            isTimerStarted = false;
        }
    }
}
