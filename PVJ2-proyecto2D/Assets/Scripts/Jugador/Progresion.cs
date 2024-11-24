using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Progresión del jugador con variables definidas via Scriptable Object


public class Progresion : MonoBehaviour
{
    private PerfilJugador perfilJugador;
    public PerfilJugador PerfilJugador { get => perfilJugador; }

    [SerializeField] private Transform barrera;                         //se lo incluye para alivianar la barrera que impide llegar a la meta
    [SerializeField] UnityEvent<string, float> OnNivelBegin;
    [SerializeField] UnityEvent<string, float> OnExperienciaCompleted;

    void Start()
    {
        perfilJugador = GetComponent<Jugador>().PerfilJugador;
        string mensaje = "Comienza nivel " + PerfilJugador.Nivel.ToString() + "\nColecta " + PerfilJugador.ExperienciaProximoNivel.ToString() + " diamantes";
        OnNivelBegin.Invoke(mensaje, 6f);
    }

    public void GanarExperiencia(int nuevaExperiencia)
    {
        PerfilJugador.Experiencia += nuevaExperiencia;                                  // suma la experiencia
        if (PerfilJugador.Experiencia == PerfilJugador.ExperienciaProximoNivel)         //se alcanza el objetivo del nivel
        {
            Debug.Log("EXPERIENCIA DEL NIVEL COMPLETA");
            barrera.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;      //se hace la valla dinámica poder moverla
            barrera.GetComponent<Rigidbody2D>().mass = 1.0f;                            //se aliviana la valla para poder pasar a la meta
            string mensaje = PerfilJugador.ExperienciaProximoNivel.ToString() + " diamantes colectados\nPrimer barrera desbloqueada";
            OnExperienciaCompleted.Invoke(mensaje, 3f);
        }
    }

}
