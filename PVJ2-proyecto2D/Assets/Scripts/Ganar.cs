using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ganar : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Jugador jugador = collision.gameObject.GetComponent<Jugador>();
            // Verificar si el componente Jugador no es null
            if (jugador != null)
            {
                Debug.Log("LLEGASTE A LA META, GANASTE!!");
                gameObject.SetActive(false);
            }
        }
    }
}
