using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// En caso de colisi�n (OnCollision) del auto del jugador con otro objeto colisionador
// recibe da�o (resta energ�a)

public class Herir : MonoBehaviour
{
    // Variables a configurar desde el editor
    [Header("Configuracion")]
    [SerializeField] float puntos = 5f;
    [SerializeField] private AudioClip choqueSFX;
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
                jugador.ModificarEnergia(-puntos);      // resta puntos a la energ�a
                Debug.Log("PUNTOS DE DA�O REALIZADOS AL JUGADOR " + puntos);
                if (audioColision.isPlaying) { return; }
                audioColision.PlayOneShot(choqueSFX);
            }
        }
    }


}
