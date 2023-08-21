using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab; // 오브젝트 풀에서 사용할 프리팹
    public int poolSize; // 풀 크기
    public string tagToPool; // 해당 오브젝트 풀의 태그

    private List<GameObject> pooledObjects = new List<GameObject>();

    // 오브젝트 풀 초기화
    public void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newObj = Instantiate(prefab);
            newObj.SetActive(false);
            pooledObjects.Add(newObj);
        }
    }

    // 오브젝트 풀에서 오브젝트 가져오기
    public GameObject GetObjectFromPool()
    {
        foreach (GameObject obj in pooledObjects)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        return null;
    }

    // 오브젝트를 풀로 돌려보내기
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}
