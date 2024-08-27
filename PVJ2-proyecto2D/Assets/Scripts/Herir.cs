using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            // Verificar si el componente Jugador no es null
            if (jugador != null)
            {
                jugador.ModificarEnergia(-puntos);
                Debug.Log("PUNTOS DE DAÑO REALIZADOS AL JUGADOR " + puntos);
            }
        }
    }
}
