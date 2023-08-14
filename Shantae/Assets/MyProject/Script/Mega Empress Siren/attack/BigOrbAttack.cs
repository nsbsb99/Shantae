using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigOrbAttack : MonoBehaviour
{
    public GameObject bigOrnPrefab;
    public float hand;

    private float spawnMax = 30f;
    private float spawnMin = 20f;
    private float lastSpawnTime = 0f;
    private float timeBetSpawn = 3f;
    private float witchHand;
    

    // Update is called once per frame
    void Update()
    {
        lastSpawnTime += Time.deltaTime;
        witchHand = (Random.Range(0, 2) == 0) ? hand : -hand;
        if (lastSpawnTime >= timeBetSpawn)
        {
            Attack();
            timeBetSpawn = Random.Range(spawnMin, spawnMax);
            lastSpawnTime = 0f;

        }

    }
    private void Attack()
    {
        Vector3 spawnPosition = new Vector3(witchHand, transform.position.y, transform.position.z);
        Instantiate(bigOrnPrefab, spawnPosition, Quaternion.identity);
    }
}
