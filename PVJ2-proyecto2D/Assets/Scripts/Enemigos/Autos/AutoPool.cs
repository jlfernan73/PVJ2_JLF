using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AutoPool : MonoBehaviour
{
    [SerializeField] private GameObject[] autoPrefab;
    [SerializeField] protected GameObject particlesCrashEnemigo;
    [SerializeField] protected GameObject particlesHumo;
    [SerializeField] private int autoPoolSize = 5;

    private List<GameObject> pooledAutos;
    private List<GameObject> pooledCrashParticles;
    private List<GameObject> pooledHumoParticles;

    void Start()
    {
        pooledAutos = new List<GameObject>();
        pooledCrashParticles = new List<GameObject>();
        pooledHumoParticles = new List<GameObject>();

        for (int i = 0; i < autoPoolSize; i++)
        {
            int indexAleatorio = Random.Range(0, autoPrefab.Length);
            GameObject prefabAleatorio = autoPrefab[indexAleatorio];
            GameObject obj = Instantiate(prefabAleatorio);
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            pooledAutos.Add(obj);

            GameObject crash = Instantiate(particlesCrashEnemigo);
            crash.transform.SetParent(transform);
            crash.SetActive(false);
            pooledCrashParticles.Add(crash);

            GameObject humo = Instantiate(particlesHumo);
            humo.transform.SetParent(transform);
            humo.SetActive(false);
            pooledHumoParticles.Add(humo);
        }
    }

    public GameObject GetAutoPooledObject()
    {
        foreach (GameObject obj in pooledAutos)
        {
            if (!obj.activeInHierarchy) return obj;
        }
        return null;
    }
    public GameObject GetPooledCrashParticles()
    {
        foreach (GameObject obj in pooledCrashParticles)
        {
            if (!obj.activeInHierarchy) return obj;
        }
        return null;
    }
    public GameObject GetPooledHumoParticles()
    {
        foreach (GameObject obj in pooledHumoParticles)
        {
            if (!obj.activeInHierarchy) return obj;
        }
        return null;
    }
}
