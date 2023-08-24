using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigOrbAttack : MonoBehaviour
{
    public GameObject body;
    public GameObject bigOrnPrefab;
    public float hand;

    private float spawnMax = 30f;
    private float spawnMin = 20f;
    private float lastSpawnTime = 0f;
    private float timeBetSpawn = 3f;
    private float witchHand;

    private AudioSource audioSource;
    public AudioClip charge;
    // Update is called once per frame

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }
    void Update()
    {
        if (body != null)
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
    }
    private void Attack()
    {
        Vector3 spawnPosition = new Vector3(witchHand, transform.position.y, transform.position.z);
        Instantiate(bigOrnPrefab, spawnPosition, Quaternion.identity);
        audioSource.clip = charge;
        audioSource.Play();
    }
}
