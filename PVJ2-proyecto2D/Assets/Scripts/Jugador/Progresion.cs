using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Progresión del jugador con variables definidas via Scriptable Object


public class Progresion : MonoBehaviour
{
    private PerfilJugador perfilJugador;
    public PerfilJugador PerfilJugador { get => perfilJugador; }

    [SerializeField] private Transform barrera;                         //se lo incluye para alivianar la barrera que impide llegar a la meta

    void Start()
    {
        perfilJugador = GetComponent<Jugador>().PerfilJugador;
        PerfilJugador.Nivel = 1;
        PerfilJugador.Experiencia = 0;
    }

    public void GanarExperiencia(int nuevaExperiencia)
    {
        PerfilJugador.Experiencia += nuevaExperiencia;                                  // suma la experiencia
        if (PerfilJugador.Experiencia >= PerfilJugador.ExperienciaProximoNivel)         //se alcanza el objetivo del nivel
        {
            Debug.Log("EXPERIENCIA DEL NIVEL COMPLETA");
            barrera.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;      //se hace la valla dinámica poder moverla
            barrera.GetComponent<Rigidbody2D>().mass = 1.0f;                            //se aliviana la valla para poder pasar a la meta
        }
    }

    public void SubirNivel()                                                        //acciones de paso de Nivel
    {
        Coleccionar cofreDiamantes = GetComponent<Coleccionar>();                   //se accede al cofre con diamantes
        cofreDiamantes.EntregarDiamantes(PerfilJugador.ExperienciaProximoNivel);    //se entregan los diamantes requeridos del nivel
        PerfilJugador.Nivel++;                                                      //se adiciona 1 al nivel
        PerfilJugador.Experiencia -= PerfilJugador.ExperienciaProximoNivel;         //se actualiza la experiencia (cantidad de diamantes) 
        PerfilJugador.ExperienciaProximoNivel += PerfilJugador.EscalarExperiencia;  //se actualiza la próxima experiencia requerida
        Debug.Log("QUEDARON EN TU COFRE " + PerfilJugador.Experiencia + " DIAMANTES");
    }
}
