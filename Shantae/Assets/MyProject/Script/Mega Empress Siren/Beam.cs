using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Beam : MonoBehaviour
{
    private bool isTimerStarted = false;
    private float startTime;
    private float elapsedTime;
    private float rotationSpeed = 50;
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
        if (elapsedTime >= 1f && elapsedTime < 16f)
        {
            transform.RotateAround(transform.position, Vector3.forward, -rotationSpeed * Time.deltaTime);
        }
        else if (elapsedTime >= 16f)
        {
            elapsedTime = 0f;
            isTimerStarted = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            gameObject.SetActive(false);
        }
    }
}
