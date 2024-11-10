using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private int poolSize = 5;

    private List<GameObject> pooledObjects;

    void Start()
    {
        pooledObjects = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(objectPrefab);
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in pooledObjects)
        {
            if (!obj.activeInHierarchy) return obj;
        }
        return null;
    }

}
