using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// En caso de colisión (OnTrigger) del auto del jugador con alguna de las herramientas
// suma energía

public class Arreglar : MonoBehaviour
{
    // Variables a configurar desde el editor
    [Header("Configuracion")]
    [SerializeField] float puntos = 1f;            // puntos a adicionar a la energía del jugador

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))             
        {
            Jugador jugador = other.GetComponent<Jugador>();
            if (jugador != null)                    // verifica si el componente Jugador no es null
            {
                jugador.ModificarEnergia(puntos);    // agrega los puntos a energía
                Debug.Log("REPARACIONES REALIZADAS AL JUGADOR: " + puntos);
            }
        }
    }
}