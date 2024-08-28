using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// En caso de colisión (OnCollision) del auto del jugador con otro objeto colisionador
// recibe daño (resta energía)

public class Herir : MonoBehaviour
{
    // Variables a configurar desde el editor
    [Header("Configuracion")]
    [SerializeField] float puntos = 5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Jugador jugador = collision.gameObject.GetComponent<Jugador>();

            if (jugador != null)            
            {
                jugador.ModificarEnergia(-puntos);      // resta puntos a la energía
                Debug.Log("PUNTOS DE DAÑO REALIZADOS AL JUGADOR " + puntos);
            }
        }
    }
}
