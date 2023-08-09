using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowKissAttack : MonoBehaviour
{

    #region 양쪽 벽에서의 BlowKiss 공격
    private GameObject blowKissPrefab;
    private int blowKissCount = 20;
    [SerializeField] private float blowKissSpeed = 11.0f;
    private GameObject[] blowKisses;
    private Vector2 poolPosition_blowKiss = new Vector2(-2.0f, -10.0f);
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
