using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// En caso de colisión (OnTrigger) del auto del jugador con el ítem diamante
// suma una unidad recolectada

public class Colectar : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Jugador jugador = collision.gameObject.GetComponent<Jugador>();

            if (jugador != null)                    // verifica si el componente Jugador no es null
            {
                jugador.AgregarItem();              // suma uno a los ítems recolectados
                Debug.Log("ITEM COLECTADO");
                gameObject.SetActive(false);        // desactiva el objeto
            }
        }
    }
}
