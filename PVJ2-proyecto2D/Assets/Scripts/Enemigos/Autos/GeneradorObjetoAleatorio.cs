using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GeneradorObjetoAleatorio : MonoBehaviour
{
    [SerializeField]
    [Range(0.5f, 5f)]
    private float tiempoEspera;

    [SerializeField]
    [Range(0.5f, 5f)]
    private float tiempoIntervalo;

    private AutoPool autoPool;

    private void Awake()
    {
        autoPool = GetComponent<AutoPool>();
    }

    private void OnBecameVisible()
    {
        InvokeRepeating(nameof(GenerarObjetoLoop), tiempoEspera, tiempoIntervalo);
    }
    private void OnBecameInvisible()
    {
        CancelInvoke(nameof(GenerarObjetoLoop));
    }

    void GenerarObjetoLoop()
    {
        GameObject pooledAuto = autoPool.GetAutoPooledObject();
        GameObject pooledCrashParticles = autoPool.GetPooledCrashParticles();
        GameObject pooledHumoParticles = autoPool.GetPooledHumoParticles();
        AutoEnemigo auto;

        if (pooledAuto != null)
        {
            pooledAuto.transform.position = transform.position;
            pooledAuto.transform.rotation = transform.rotation;
            pooledAuto.SetActive(true);
            auto = pooledAuto.GetComponent<AutoEnemigo>();

            if (pooledCrashParticles != null)
            {
                pooledCrashParticles.transform.position = transform.position;
                pooledCrashParticles.transform.rotation = transform.rotation;
                pooledCrashParticles.SetActive(true);
                auto.AsignarCrash(pooledCrashParticles);
            }
            if (pooledHumoParticles != null)
            {
                pooledHumoParticles.transform.position = transform.position;
                pooledHumoParticles.transform.rotation = transform.rotation;
                pooledHumoParticles.SetActive(true);
                auto.AsignarHumo(pooledHumoParticles);
            }
        }
    }
 }
