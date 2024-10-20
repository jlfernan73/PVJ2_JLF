using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// En caso de colisión (OnTrigger) del auto del jugador con alguna de las herramientas
// suma energía

public class Arreglar : MonoBehaviour
{
    //sonido y valor de energía agregada se controlan desde el SO PerfilJugador
    private PerfilJugador perfilJugador;
    public PerfilJugador PerfilJugador { get => perfilJugador; }

    private AudioSource audioTool;
    private void OnEnable()
    {
        audioTool = GetComponent<AudioSource>();
    }

    private void OnParticleCollision(GameObject tool)
    {
        if (tool.CompareTag("Player"))             
        {
            Jugador jugador = tool.GetComponent<Jugador>();
            if (jugador != null || jugador.PerfilJugador.Energia > 0)         // verifica si el componente Jugador no es null y si no explotó
            {
                audioTool.Stop();                   // se detiene el sonido anterior (para que no se ejecute junto al siguiente)
                audioTool.PlayOneShot(jugador.PerfilJugador.ToolSFX);     // se ejecuta el sonido de levantar herramienta
                jugador.ModificarEnergia(jugador.PerfilJugador.Reparacion);    // agrega los puntos a energía
                Debug.Log("ENERGÍA GANADA: " + jugador.PerfilJugador.Reparacion);
            }
        }
    }
}