using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// En caso de colisión (OnTrigger) del auto del jugador con el ítem diamante
// suma una unidad recolectada

public class Colectar : MonoBehaviour
{
    [SerializeField] private AudioClip diamanteSFX;
    private AudioSource audioDiamante;
    private SpriteRenderer miDiamante;

    private void OnEnable()
    {
        audioDiamante = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (miDiamante != null && !audioDiamante.isPlaying)
        {
            gameObject.SetActive(false);
            miDiamante = null;
        }
    }

        private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Jugador jugador = collision.gameObject.GetComponent<Jugador>();
            miDiamante = gameObject.GetComponent<SpriteRenderer>();
            if (jugador != null)                    // verifica si el componente Jugador no es null
            {
                audioDiamante.PlayOneShot(diamanteSFX);
                jugador.AgregarItem();              // suma uno a los ítems recolectados
                Debug.Log("ITEM COLECTADO");
                miDiamante.enabled = false;
            }
        }
    }
}
