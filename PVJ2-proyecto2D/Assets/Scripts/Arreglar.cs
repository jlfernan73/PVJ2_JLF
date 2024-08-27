using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arreglar : MonoBehaviour
{
    // Variables a configurar desde el editor
    [Header("Configuracion")]
    [SerializeField] float puntos = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Jugador jugador = collision.gameObject.GetComponent<Jugador>();
            // Verificar si el componente Jugador no es null
            if (jugador != null)
            {
                jugador.ModificarEnergia(puntos);
                Debug.Log("REPARACIONES REALIZADAS AL JUGADOR " + puntos);
                gameObject.SetActive(false);
            }
        }
    }
}