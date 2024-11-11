using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] protected GameObject particlesChispas;
    [SerializeField] private int poolSize = 5;

    private List<GameObject> pooledObjects;
    private List<GameObject> pooledChispasParticles;

    void Start()
    {
        pooledObjects = new List<GameObject>();
        pooledChispasParticles = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(objectPrefab);
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            pooledObjects.Add(obj);

            GameObject chispas = Instantiate(particlesChispas);
            chispas.transform.SetParent(transform);
            chispas.SetActive(false);
            pooledChispasParticles.Add(chispas);
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
    public GameObject GetPooledChispasParticles()
    {
        foreach (GameObject obj in pooledChispasParticles)
        {
            if (!obj.activeInHierarchy) return obj;
        }
        return null;
    }

}
