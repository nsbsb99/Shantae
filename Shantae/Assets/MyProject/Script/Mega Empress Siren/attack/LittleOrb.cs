using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleOrb : MonoBehaviour
{

    public Transform player;
    private float spawnMax = 18f;
    private float spawnMin = 12f;
    private float lastSpawnTime = 0f;
    private float timeBetSpawn = 0f;


    public GameObject elecPrefab;
    public GameObject littleOrbPrefab;
    private int littleOrbCount = 5;
    private float littleOrbSpeed = 11.0f;
    private GameObject[] littleOrbs;
    private GameObject[] elec;

    private int poolSize1 = 15;
    private int poolSize2 = 3;
    private void Start()
    {
        timeBetSpawn = Random.Range(5, 10);

        littleOrbs = new GameObject[poolSize1];
        for (int i = 0; i < poolSize1; i++)
        {
            littleOrbs[i] = Instantiate(littleOrbPrefab, Vector3.zero, Quaternion.identity);
            littleOrbs[i].SetActive(false);
        }
        
        
        elec = new GameObject[poolSize2];
        for (int i = 0; i < poolSize2; i++)
        {
            elec[i] = Instantiate(elecPrefab, Vector3.zero, Quaternion.identity);
            elec[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject empress = GameObject.Find("Mega Empress Siren");

        lastSpawnTime += Time.deltaTime;
        if (lastSpawnTime >= timeBetSpawn - 2.0f)
        {
            GameObject elecObject = GetPooledElec();

            if (elecObject != null)
            {
                elecObject.SetActive(true);

                // Update elecObject's position to match the transform's position
                StartCoroutine(FollowTransformPosition(elecObject));

            }
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
    private GameObject GetPooledElec()
    {
        for (int i = 0; i < elec.Length; i++)
        {
            if (!elec[i].activeInHierarchy)
            {
                // elecPrefab를 풀에서 가져와 활성화
                elec[i].SetActive(true);

                // 일정 시간 후에 비활성화
                StartCoroutine(DeactivateAfterDelay(elec[i], 2.0f)); // 2.0f는 비활성화까지의 지연 시간

                return elec[i];
            }
        }
        return null;
    }
    private IEnumerator FollowTransformPosition(GameObject elecObject)
    {
        while (elecObject.activeSelf)
        {
            elecObject.transform.position = transform.position;
            yield return null;
        }
    }
    private void ReturnToPool(GameObject obj)
    {
        Destroy(obj);
    }

    private void ReturnAllToPool()
    {
        foreach (GameObject orb in littleOrbs)
        {
            ReturnToPool(orb);
        }

        foreach (GameObject elecObject in elec)
        {
            ReturnToPool(elecObject);
        }
    }

    private void OnDestroy()
    {
        // This method is called when the component is disabled or the object is returned to the pool
        ReturnAllToPool();
    }
}
