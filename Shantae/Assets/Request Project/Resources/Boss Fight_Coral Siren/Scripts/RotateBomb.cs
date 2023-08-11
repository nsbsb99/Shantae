using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBomb : MonoBehaviour
{
    [SerializeField] private float degreePerSecond = 600;

    private void Update()
    {
        transform.Rotate(-(Vector3.forward) * Time.deltaTime * degreePerSecond);
    }
}
