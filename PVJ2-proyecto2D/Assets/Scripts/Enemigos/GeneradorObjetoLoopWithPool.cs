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
        if (pooledObject != null )
        {
            pooledObject.transform.position = transform.position;
            pooledObject.transform.rotation = transform.rotation;
            pooledObject.SetActive(true);
        }
    }

}
