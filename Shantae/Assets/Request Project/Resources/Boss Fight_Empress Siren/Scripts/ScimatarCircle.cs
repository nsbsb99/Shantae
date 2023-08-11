using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScimatarCircle : MonoBehaviour
{
    [SerializeField] private float degreePerSecond = 3000;

    private void Update()
    {
        transform.Rotate(-(Vector3.forward) * Time.deltaTime * degreePerSecond);
    }
}
