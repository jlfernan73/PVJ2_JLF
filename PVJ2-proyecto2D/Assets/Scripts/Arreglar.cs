using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// En caso de colisi�n (OnTrigger) del auto del jugador con el �tem de herramientas
// suma energ�a

public class Arreglar : MonoBehaviour
{
    // Variables a configurar desde el editor
    [Header("Configuracion")]
    [SerializeField] float puntos = 30f;            // puntos a adicionar a la energ�a del jugador

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Jugador jugador = collision.gameObject.GetComponent<Jugador>();
            if (jugador != null)                    // verifica si el componente Jugador no es null
            {
                jugador.ModificarEnergia(puntos);    // agrega los puntos a energ�a
                Debug.Log("REPARACIONES REALIZADAS AL JUGADOR: " + puntos);
                gameObject.SetActive(false);        // desactiva el objeto
            }
        }
    }
}