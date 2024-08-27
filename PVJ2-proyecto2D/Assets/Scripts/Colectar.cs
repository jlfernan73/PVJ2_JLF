using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colectar : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Jugador jugador = collision.gameObject.GetComponent<Jugador>();
            // Verificar si el componente Jugador no es null
            if (jugador != null)
            {
                jugador.AgregarItem();
                Debug.Log("ITEM COLECTADO");
                gameObject.SetActive(false);
            }
        }
    }
}
