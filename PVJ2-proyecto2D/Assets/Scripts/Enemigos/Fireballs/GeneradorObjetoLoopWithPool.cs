using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorObjetoLoopWithPool : MonoBehaviour
{
    [SerializeField]
    [Range(0.2f, 2f)]
    private float tiempoEspera;
    [SerializeField]
    [Range(0.2f, 2f)]
    private float tiempoIntervalo;

    private ObjectPool objectPool;

    private void Awake()
    {
        objectPool = GetComponent<ObjectPool>();
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
        GameObject pooledObject = objectPool.GetPooledObject();
        GameObject pooledChispasParticles = objectPool.GetPooledChispasParticles();
        Fireball fireball;
        if (pooledObject != null )
        {
            pooledObject.transform.position = transform.position;
            pooledObject.transform.rotation = transform.rotation;
            pooledObject.SetActive(true);

            fireball = pooledObject.GetComponent<Fireball>();

            if (pooledChispasParticles != null)
            {
                pooledChispasParticles.transform.position = transform.position;
                pooledChispasParticles.transform.rotation = transform.rotation;
                pooledChispasParticles.SetActive(true);
                fireball.AsignarChispas(pooledChispasParticles);
            }
        }
    }

}
