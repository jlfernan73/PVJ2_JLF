using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scriptable Object con el perfil normal del jugador

[CreateAssetMenu(fileName = "NuevoPerfilJugador", menuName = "SO/Perfil Jugador")]
public class PerfilJugador : ScriptableObject
{
    private int nivel;
    public int Nivel { get => nivel; set => nivel = value; }

    private int experiencia;                                        // corresponde al número de diamantes colectados
    public int Experiencia { get => experiencia; set => experiencia = value; }

    [Header("Configuraciones de juego")]
    [SerializeField]
    [Tooltip("Vidas iniciales")]
    [Range(1, 5)]
    private int vidasIniciales;
    public int VidasIniciales { get => vidasIniciales;}

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

    
    [Header("Configuraciones de atributos")]
    [SerializeField] private float aceleracion = 30f;       // modulo de la fuerza de arranque
    public float Aceleracion { get => aceleracion; set => aceleracion = value; }

    [SerializeField] private float maxAngulo = 60.0f;        // máximo angulo de giro del auto al doblar
    public float MaxAngulo { get => maxAngulo; set => maxAngulo = value; }

    [SerializeField] private float maxRapidez = 17f;        // velocidad máxima que alcanza el auto al acelerar varias veces
    public float MaxRapidez { get => maxRapidez; set => maxRapidez = value; }

    [Tooltip("Velocidad de consumo de combustible (en reposo)")]
    [SerializeField] private float consumoCombustible = 0.005f;        // velocidad de consumo de combustible (en reposo)
    public float ConsumoComb { get => consumoCombustible; set => consumoCombustible = value; }

    [Tooltip("Velocidad de desgaste del objeto activado")]
    [SerializeField] private float consumoObjeto = 2.5f;        // velocidad de consumo del objeto activado
    public float ConsumoObj { get => consumoObjeto; set => consumoObjeto = value; }

    [Tooltip("Puntos de energía adicionada por herramienta colectada")]
    [SerializeField] float reparacion = 5f;             // puntos a adicionar a la energía del jugador al colectar herramienta
    public float Reparacion { get => reparacion; set => reparacion = value; }


    [Header("Monitoreo de atributos")]
    [SerializeField] private float energia;      //energía inicial del jugador
    public float Energia { get => energia; set => energia = value; }

    [SerializeField] private float combustible;      //combustible inicial del jugador
    public float Combustible { get => combustible; set => combustible = value; }

    [Tooltip("Carga del tanque de nitro (si fue activado)")]
    [SerializeField] private float nitroTank;
    public float NitroTank { get => nitroTank; set => nitroTank = value; }

    [Tooltip("Porcentaje de tiempo remanente del bumper activado")]
    [SerializeField] private float bumperConteo = 0f;
    public float BumperConteo { get => bumperConteo; set => bumperConteo = value; }

    [Tooltip("Proyectiles remanentes del cañón activado")]
    [SerializeField] private int cannonConteo = 0;
    public int CannonConteo { get => cannonConteo; set => cannonConteo = value; }


    [Header("Configuraciones de SFX")]
    [SerializeField] private AudioClip reversaSFX;
    public AudioClip ReversaSFX { get => reversaSFX; set => reversaSFX = value; }

    [SerializeField] private AudioClip motorSFX;
    public AudioClip MotorSFX { get => motorSFX; set => motorSFX = value; }

    [SerializeField] private AudioClip explosionSFX;
    public AudioClip ExplosionSFX { get => explosionSFX; set => explosionSFX = value; }

    [SerializeField] private AudioClip diamanteSFX;             // para asociar el clip del sonido de levantar diamante
    public AudioClip DiamanteSFX { get => diamanteSFX; set => diamanteSFX = value; }

    [SerializeField] private AudioClip colectarSFX;             // para asociar el clip del sonido de levantar el coleccionable
    public AudioClip ColectarSFX { get => colectarSFX; set => colectarSFX = value; }

    [SerializeField] private AudioClip usarColeccionableSFX;    // para asociar el clip del sonido de usar el coleccionable
    public AudioClip UsarColeccionableSFX { get => usarColeccionableSFX; set => usarColeccionableSFX = value; }

    [SerializeField] private AudioClip toolSFX;     // para asociar el clip del sonido de levantar una herramienta
    public AudioClip ToolSFX { get => toolSFX; set => toolSFX = value; }
}
