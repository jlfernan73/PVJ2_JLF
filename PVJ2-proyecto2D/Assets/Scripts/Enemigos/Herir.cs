using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// En caso de colisi�n (OnCollision) del auto del jugador con otro objeto colisionador
// recibe da�o (resta energ�a)

public class Herir : MonoBehaviour
{
    // Variables a configurar desde el editor
    [Header("Configuracion")]
    [SerializeField] float puntos = 5f;             // puntos a restar (configurable desde el inspector, seg�n con qu� se choque)
    [SerializeField] private AudioClip choqueSFX;   // para el clip del choque, que ser� distinto seg�n d�nde se use el script
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
                if (audioColision.isPlaying) { return; }    // si el sonido de choque aun est� activo, el auto no sentir� la colisi�n 
                audioColision.PlayOneShot(choqueSFX);       // se ejecuta el sonido de la colisi�n
                jugador.Colision();                         // se llama al m�todo p�blico de la clase Jugador que ejecuta acciones espec�ficas de la colisi�n (part�culas y sonido)
                jugador.ModificarEnergia(-puntos);          // resta puntos a la energ�a
            }
        }
        if (CompareTag("Enemigo"))
        {
            AutoEnemigo enemigo = GetComponent<AutoEnemigo>();
            enemigo.Colision();                         
            enemigo.ModificarEnergia(-1f);          // resta 1 punto a la energ�a

        }
    }
}
