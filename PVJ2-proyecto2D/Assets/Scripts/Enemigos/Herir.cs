using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// En caso de colisión (OnCollision) del auto del jugador con otro objeto colisionador
// recibe daño (resta energía)

public class Herir : MonoBehaviour
{
    // Variables a configurar desde el editor
    [Header("Configuracion")]
    [SerializeField] float puntos = 5f;             // puntos a restar (configurable desde el inspector, según con qué se choque)
    [SerializeField] private AudioClip choqueSFX;   // para el clip del choque, que será distinto según dónde se use el script
    private AudioSource audioColision;

    private void OnEnable()
    {
        audioColision = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Jugador jugador = collision.gameObject.GetComponent<Jugador>(); 

            if (jugador != null)            
            {
                if (audioColision.isPlaying) { return; }    // si el sonido de choque aun está activo, el auto no sentirá la colisión 
                audioColision.PlayOneShot(choqueSFX);       // se ejecuta el sonido de la colisión
                jugador.Colision();                         // se llama al método público de la clase Jugador que ejecuta acciones específicas de la colisión (partículas y sonido)
                jugador.ModificarEnergia(-puntos);          // resta puntos a la energía
            }
        }
        if (CompareTag("Enemigo"))
        {
            AutoEnemigo enemigo = GetComponent<AutoEnemigo>();
            enemigo.Colision();                         
            enemigo.ModificarEnergia(-1f);          // resta 1 punto a la energía

        }
    }
}
