using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// En caso de colisi�n (OnTrigger) del auto del jugador con alguna de las herramientas
// suma energ�a

public class Arreglar : MonoBehaviour
{
    private PerfilJugador perfilJugador;
    public PerfilJugador PerfilJugador { get => perfilJugador; }

    //[SerializeField] private AudioClip toolSFX;     // para asociar el clip del sonido de levantar una herramienta
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
            if (jugador != null || jugador.PerfilJugador.Energia > 0)         // verifica si el componente Jugador no es null y si no explot�
            {
                audioTool.Stop();                   // se detiene el sonido anterior (para que no se ejecute junto al siguiente)
                audioTool.PlayOneShot(jugador.PerfilJugador.ToolSFX);     // se ejecuta el sonido de levantar herramienta
                jugador.ModificarEnergia(jugador.PerfilJugador.Reparacion);    // agrega los puntos a energ�a
                Debug.Log("ENERG�A GANADA: " + jugador.PerfilJugador.Reparacion);
            }
        }
    }
}