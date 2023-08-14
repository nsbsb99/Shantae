using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleOrb : MonoBehaviour
{
    public Transform player;

    private bool isTimerStarted = false;
    private float startTime;
    private float elapsedTime;

    private float spawnMax = 18f;
    private float spawnMin = 12f;
    private float lastSpawnTime = 0f;
    private float timeBetSpawn = 0f;


    public GameObject littleOrbPrefab;
    private int littleOrbCount = 5;
    private float littleOrbSpeed = 11.0f;
    private GameObject[] littleOrbs;
    private int poolSize = 15;
    private Vector2 poolPosition_littleOrb = new Vector2(-2.0f, -10.0f);

    // Start is called before the first frame update
    void Start()
    {
        timeBetSpawn = Random.Range(5, 10);

        littleOrbs = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            littleOrbs[i] = Instantiate(littleOrbPrefab, Vector3.zero, Quaternion.identity);
            littleOrbs[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        lastSpawnTime += Time.deltaTime;
        if (lastSpawnTime >= timeBetSpawn - 2.0f)
        {
            //지직거리는 애니메이션
        }

        if (lastSpawnTime >= timeBetSpawn)
        {
            Attack();
            timeBetSpawn = Random.Range(spawnMin, spawnMax);
            lastSpawnTime = 0f;

        }
    }

    private void Attack()
    {
        Vector3 targetDirection = player.position - transform.position;
        float angleToPlayer = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        Quaternion rotationToPlayer = Quaternion.Euler(0, 0, angleToPlayer);

        for (int i = 0; i < littleOrbCount; i++)
        {
            float angleOffset = -45f + (90f / (littleOrbCount - 1)) * i;
            Quaternion finalRotation = rotationToPlayer * Quaternion.Euler(0, 0, angleOffset);

            Vector3 spawnPosition = transform.position + (Quaternion.Euler(0, 0, angleToPlayer) * Vector3.right) * 2.0f;

            GameObject littleOrb = GetPooledOrb();
            littleOrb.transform.position = spawnPosition;
            littleOrb.transform.rotation = finalRotation;
            littleOrb.SetActive(true);

            Rigidbody2D rb = littleOrb.GetComponent<Rigidbody2D>();

            Vector3 direction = finalRotation * Vector3.right;
            rb.velocity = direction * littleOrbSpeed;

            StartCoroutine(DeactivateAfterDelay(littleOrb, 5.0f));
        }
    }
    private IEnumerator DeactivateAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
    private GameObject GetPooledOrb()
    {
        for (int i = 0; i < littleOrbs.Length; i++)
        {
            if (!littleOrbs[i].activeInHierarchy)
            {
                return littleOrbs[i];
            }
        }
        return null;
    }
}
