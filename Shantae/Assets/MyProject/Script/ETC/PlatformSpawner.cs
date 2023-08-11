using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public float speedSlow;
    public float speedFast;
    private float selectSpeed;

    private GameObject[] platforms;
    private int poolSize = 50;
    private int currentPlatformIndex = 0;

    private float spawnMax = 2f;
    private float spawnMin = 1f;
    private float lastSpawnTime = 0f;
    private float timeBetSpawn = 0f;

    private float xPointMax = 15f;       // 맵 좌우 끝으로 정해야됨
    private float xPointMin = -15f;
    public float yPoint;        //위에서 나오는거, 아래에서 나오는거
    private float xPoint;
    public Vector2 movementDirection;

    private void Start()
    {
        platforms = new GameObject[poolSize];
        for ( int i = 0; i < poolSize; i++ )
        {
            platforms[i] = Instantiate(platformPrefab, Vector3.zero, Quaternion.identity);
            platforms[i].SetActive(false);
        }
    }

    private void Update()
    {
        lastSpawnTime += Time.deltaTime;
        selectSpeed = (Random.Range(0, 3) == 0) ? speedFast : speedSlow;
        
        if (lastSpawnTime >= timeBetSpawn)
        {
            SpawnPlatform();
            timeBetSpawn = Random.Range(spawnMin, spawnMax);
            lastSpawnTime = 0f;
        }
        
    }
    private void SpawnPlatform()
    {
        xPoint = Random.Range(xPointMin, xPointMax);
        Vector2 spawnPosition = new Vector2(xPoint, yPoint);

        GameObject newPlatform = platforms[currentPlatformIndex];
        currentPlatformIndex = (currentPlatformIndex + 1) % poolSize;

        newPlatform.transform.position = spawnPosition;
        newPlatform.SetActive(true);




        Rigidbody2D rb = newPlatform.GetComponent<Rigidbody2D>();
        if(rb != null )
        {
            rb.velocity = movementDirection * selectSpeed;
            rb.gravityScale = 0f;
            rb.isKinematic = true;
        }
    }
}
