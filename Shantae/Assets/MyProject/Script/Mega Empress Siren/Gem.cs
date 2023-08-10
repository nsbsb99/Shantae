using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    //public bool startTimer = false;
    public GameObject gemBreakPrefab;
    private int gemHP = 1;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (gemHP <= 0)
        {
            //startTimer = true;
            //gemBreakPrefab.SetActive(true);
            Vector3 spawnPosition = transform.position;
            Quaternion spawnRotation = transform.rotation;

            // 프리팹을 소환
            Instantiate(gemBreakPrefab, spawnPosition, spawnRotation);

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("PlayerAttack"))
        {
            gemHP -= 1;
            Debug.Log(gemHP);
        }
    }
}
