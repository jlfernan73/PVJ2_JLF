using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Progresión del jugador con variables definidas via Scriptable Object


public class Progresion : MonoBehaviour
{
    private PerfilJugador perfilJugador;
    public PerfilJugador PerfilJugador { get => perfilJugador; }

    [SerializeField] private Transform barrera;                         //para alivianar la barrera que impide llegar a la meta

    void Start()
    {
        perfilJugador = GetComponent<Jugador>().PerfilJugador;
        PerfilJugador.Nivel = 1;
        PerfilJugador.Experiencia = 0;
    }

    public void GanarExperiencia(int nuevaExperiencia)
    {
        PerfilJugador.Experiencia += nuevaExperiencia;
        if (PerfilJugador.Experiencia >= PerfilJugador.ExperienciaProximoNivel)
        {
            Debug.Log("EXPERIENCIA DEL NIVEL COMPLETA");
            barrera.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;      //se hace la valla dinámica poder moverla
            barrera.GetComponent<Rigidbody2D>().mass = 1.0f;                            //se aliviana la valla para poder pasar a la meta
        }
    }

    public void SubirNivel()
    {
        Coleccionar cofreDiamantes = GetComponent<Coleccionar>();
        cofreDiamantes.EntregarDiamantes(PerfilJugador.ExperienciaProximoNivel);
        PerfilJugador.Nivel++;
        PerfilJugador.Experiencia -= PerfilJugador.ExperienciaProximoNivel;
        PerfilJugador.ExperienciaProximoNivel += PerfilJugador.EscalarExperiencia;
        Debug.Log("QUEDARON EN TU COFRE " + PerfilJugador.Experiencia + " DIAMANTES");
    }
}
