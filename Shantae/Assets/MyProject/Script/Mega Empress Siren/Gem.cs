using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    //public bool startTimer = false;
    public GameObject gemBreakPrefab;
    private int gemHP = 1;
    private EmpressAnimation empressAnimation;
    private EmpressAnimation[] empressAnimations;
    // Start is called before the first frame update
    void Start()
    {
        empressAnimation = FindObjectOfType<EmpressAnimation>();
        empressAnimations = FindObjectsOfType<EmpressAnimation>();

        
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
            foreach (EmpressAnimation empressAnimation in empressAnimations)
            {
                empressAnimation.EmpressDamage();
            }
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
