using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NuevoPerfilJugador", menuName = "SO/Perfil Jugador")]
public class PerfilJugador : ScriptableObject
{
    private int nivel;
    public int Nivel { get => nivel; set => nivel = value; }

    private int experiencia;
    public int Experiencia { get => experiencia; set => experiencia = value; }

    [Header("Configuracion de experiencia")]
    [SerializeField]
    [Tooltip("Experiencia necesaria para pasar al proximo nivel")]
    [Range(50, 500)]
    private int experienciaProximoNivel;
    public int ExperienciaProximoNivel { get => experienciaProximoNivel; set => experienciaProximoNivel = value; }

    [SerializeField]
    [Tooltip("Aumento de la experiencia necesaria de nivel a nivel")]
    [Range(50, 200)]
    private int escalarExperiencia;
    public int EscalarExperiencia { get => escalarExperiencia; set => escalarExperiencia = value; }

    /*
    [Header("Configuracion de atributos de movimiento")]
    [SerializeField] private float aceleracion = 30f;       // modulo de la fuerza de arranque
    public float Aceleracion { get => aceleracion; set => aceleracion = value; }

    [SerializeField] private float maxAngulo = 60.0f;        // máximo angulo de giro del auto al doblar
    public float MaxAngulo { get => maxAngulo; set => maxAngulo = value; }

    [SerializeField] private float maxRapidez = 17f;        // velocidad máxima que alcanza el auto al acelerar varias veces
    public float MaxRapidez { get => maxRapidez; set => maxRapidez = value; }
    */


}
