using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// En caso de colisi�n (OnTrigger) del auto del jugador con la caja de herramientas
// activa el sistema de part�culas

public class AbrirCaja : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private ParticleSystem particleSystemTools;        // para asociar el sistema de part�culas de cada caja en particular
    [SerializeField] private AudioClip cajaSFX;                         // para asociar el clip de apertura de la caja
    private AudioSource audioCaja;
    private SpriteRenderer miCaja;                                      // se va a acceder al spriteRenderer de la caja
    private void OnEnable()
    {
        audioCaja = GetComponent<AudioSource>();
        particleSystemTools.transform.position = gameObject.transform.position;  // se hace coincidir ambos centros (part�culas y caja)
    }
    private void Update()
    {
        if (miCaja != null)
        {
            if (!miCaja.enabled)                    // si el spriteRenderer de la caja fue desactivado por la colisi�n (ver abajo)
            {
                particleSystemTools.Play();         // activa el sistema de part�culas con las herramientas
            }
            if (!miCaja.enabled && !audioCaja.isPlaying)        // si adem�s ya termin� de ejecutar el sonido de apertura de la caja
            {
                gameObject.SetActive(false);                    // se borra la caja
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))          // en caso de colisi�n con el jugador
        {
            Jugador jugador = collision.gameObject.GetComponent<Jugador>();
            miCaja = gameObject.GetComponent<SpriteRenderer>(); // se asigna el spriteRenderer del gameObject (la caja) a miCaja
            if (jugador != null)                                // verifica si el componente Jugador no es null
            {
                audioCaja.PlayOneShot(cajaSFX);                 // se activa el sonido de apertura de la caja
                miCaja.enabled = false;                         // se desactiva el spriteRenderer de la caja (no se la borra todav�a)
            }
        }
    }
}
