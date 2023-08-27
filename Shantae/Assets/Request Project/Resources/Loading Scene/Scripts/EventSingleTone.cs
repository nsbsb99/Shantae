using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSingleTone : MonoBehaviour
{

    public static EventSingleTone instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this);
            }
        }
    }

}
