using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// En caso de colisión (OnTrigger) del auto del jugador con alguna de las herramientas
// suma energía

public class Arreglar : MonoBehaviour
{
    // Variables a configurar desde el editor
    [Header("Configuracion")]
    [SerializeField] float puntos = 1f;            // puntos a adicionar a la energía del jugador
    [SerializeField] private AudioClip toolSFX;
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
            if (jugador != null)                    // verifica si el componente Jugador no es null
            {
                audioTool.Stop();
                audioTool.PlayOneShot(toolSFX);
                jugador.ModificarEnergia(puntos);    // agrega los puntos a energía
                Debug.Log("REPARACIONES REALIZADAS AL JUGADOR: " + puntos);
            }
        }
    }
}