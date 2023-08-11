using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceesoryCount : MonoBehaviour
{
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 자식 오브잭트 카운트
        int activeObjectCount = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject childObject = transform.GetChild(i).gameObject;
            if (childObject.activeSelf) 
            {
                activeObjectCount++;
            }
        }
        // 자식 오브잭트 카운트

        if (activeObjectCount == 0)
        {
            Destroy(gameObject);
        }
    }
}
