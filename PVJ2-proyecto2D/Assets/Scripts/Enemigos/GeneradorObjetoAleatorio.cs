using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorObjetoAleatorio : MonoBehaviour
{
    [SerializeField] private GameObject[] objetosPrefabs;

    [SerializeField]
    [Range(0.5f, 5f)]
    private float tiempoEspera;

    [SerializeField]
    [Range(0.5f, 5f)]
    private float tiempoIntervalo;

    private int contador = 0;       // lleva la cuenta de los objetos instanciados

    void Start()
    {
    }

    void GenerarObjetoAleatorio()
    {
        if (contador < 10)  // no se instanciarán más de 10
        {
            int indexAleatorio = Random.Range(0, objetosPrefabs.Length);
            GameObject prefabAleatorio = objetosPrefabs[indexAleatorio];
            Instantiate(prefabAleatorio, transform.position, Quaternion.identity);
            contador++;
        }
    }

    private void OnBecameInvisible()
    {
        Debug.Log("Aparece un spawner en la escena");
        CancelInvoke(nameof(GenerarObjetoAleatorio));
    }

    private void OnBecameVisible()
    {
        Debug.Log("Desaparece el spawner de la escena");
        InvokeRepeating(nameof(GenerarObjetoAleatorio), tiempoEspera, tiempoIntervalo);
    }
}
