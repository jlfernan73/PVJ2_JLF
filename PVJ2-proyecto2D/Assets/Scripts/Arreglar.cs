using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// En caso de colisi�n (OnTrigger) del auto del jugador con alguna de las herramientas
// suma energ�a

public class Arreglar : MonoBehaviour
{
    // Variables a configurar desde el editor
    [Header("Configuracion")]
    [SerializeField] float puntos = 1f;            // puntos a adicionar a la energ�a del jugador

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))             
        {
            Jugador jugador = other.GetComponent<Jugador>();
            if (jugador != null)                    // verifica si el componente Jugador no es null
            {
                jugador.ModificarEnergia(puntos);    // agrega los puntos a energ�a
                Debug.Log("REPARACIONES REALIZADAS AL JUGADOR: " + puntos);
            }
        }
    }
}