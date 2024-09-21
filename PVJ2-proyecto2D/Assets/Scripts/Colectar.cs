using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// En caso de colisión (OnTrigger) del auto del jugador con el ítem diamante
// suma una unidad recolectada

public class Colectar : MonoBehaviour
{
    [SerializeField] private AudioClip diamanteSFX; // para asociar el clip del sonido de levantar diamante
    private AudioSource audioDiamante;
    private SpriteRenderer miDiamante;              // se va a acceder al spriteRenderer del diamante 

    private void OnEnable()
    {
        audioDiamante = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (miDiamante != null && !miDiamante.enabled && !audioDiamante.isPlaying)     //si se ha desactivado el spriteRenderer del diamante y la música ya terminó
        {
            gameObject.SetActive(false);                        // se borra el diamante
            miDiamante = null;
        }
    }

        private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Jugador jugador = collision.gameObject.GetComponent<Jugador>();
            miDiamante = gameObject.GetComponent<SpriteRenderer>();
            if (jugador != null)                                // verifica si el componente Jugador no es null
            {
                audioDiamante.PlayOneShot(diamanteSFX);         // se ejecuta el sonido de recolección de diamante
                jugador.AgregarItem();                          // suma uno a los ítems recolectados
                Debug.Log("ITEM COLECTADO");
                miDiamante.enabled = false;                     // se desactiva el spriteRenderer del diamante (sin borrarlo)
            }
        }
    }
}
