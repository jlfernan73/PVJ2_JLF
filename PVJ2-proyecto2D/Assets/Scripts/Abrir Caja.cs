using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// En caso de colisión (OnTrigger) del auto del jugador con la caja de herramientas
// activa el sistema de partículas

public class AbrirCaja : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private new ParticleSystem particleSystem;    

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Jugador jugador = collision.gameObject.GetComponent<Jugador>();
            if (jugador != null)                    // verifica si el componente Jugador no es null
            {
                gameObject.SetActive(false);        // desactiva el objeto
                particleSystem.Play();
            }
        }
    }
}
