using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// En caso de colisión (OnTrigger) del auto del jugador con la caja de herramientas
// activa el sistema de partículas

public class AbrirCaja : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private new ParticleSystem particleSystem;
    [SerializeField] private AudioClip cajaSFX;
    private AudioSource audioCaja;
    private SpriteRenderer miCaja;
    private void OnEnable()
    {
        audioCaja = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (miCaja != null && !audioCaja.isPlaying)
        {
            gameObject.SetActive(false);
            miCaja = null;
            particleSystem.Play();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Jugador jugador = collision.gameObject.GetComponent<Jugador>();
            miCaja = gameObject.GetComponent<SpriteRenderer>();
            if (jugador != null)                    // verifica si el componente Jugador no es null
            {
                audioCaja.PlayOneShot(cajaSFX);
                miCaja.enabled = false;
            }
        }
    }
}
