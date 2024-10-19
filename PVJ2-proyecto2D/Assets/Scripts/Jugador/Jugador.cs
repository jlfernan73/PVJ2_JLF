using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;

// clase Jugador con variables definidas via Scriptable Object


public class Jugador : MonoBehaviour
{
    [SerializeField]
    private PerfilJugador perfilJugador;
    public PerfilJugador PerfilJugador { get => perfilJugador; }

    // se incorporan sistemas de partículas para animar distintas situaciones
    [Header("Efectos visuales")]
    [SerializeField] private ParticleSystem particleSystemCrash;        //choque al colisionar
    [SerializeField] private ParticleSystem particleSystemHumo;         //humo que empieza a salir cuando la energía es muy baja
    [SerializeField] private ParticleSystem particleSystemExplosion;    //explosión que ocurre al tener energía nula
    [SerializeField] private ParticleSystem particleSystemStars;        //fuegos artificiales de estrellas al llegar a la meta

    [Header("Gestion de escena y sonido de fondo")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;    //se accede a la cámara para detener el seguimiento en la meta

    // se accede a estos transform para activar/desactivar según se cumplan condiciones
    [SerializeField] private Transform musicaFondo;                     //para poder apagar la música de fondo
    [SerializeField] private Transform musicaMeta;                      //y poner una música de llegada

    private Progresion progresionJugador;

    // banderas para monitorear situaciones
    bool humeando = false;
    bool vive = true;
    bool meta = false;

    void Awake()
    {
        progresionJugador = GetComponent<Progresion>();
        PerfilJugador.Energia = 100f;
        PerfilJugador.Combustible = 100f;
        PerfilJugador.NitroTank = 0;
    }

    private void Update()
    {
        particleSystemHumo.transform.position = gameObject.transform.position;          // la posición del sist. de partículas de humo sigue la del auto
        particleSystemExplosion.transform.position = gameObject.transform.position;     // idem con la posición del sistema de partículas de la explosión
    }

    public void ModificarEnergia(float puntos)      // método público para modificar la energía
    {                                               // sin superar 100 ni bajar de 0
        PerfilJugador.Energia += puntos;
        if (PerfilJugador.Energia > 100)
        {
            PerfilJugador.Energia = 100;
        }
        if (PerfilJugador.Energia < 0)
        {
            PerfilJugador.Energia = 0;                        // con energía nula el jugador aun vivirá hasta explotar
        }
        if (PerfilJugador.Energia < 25 && !humeando)          // acá se activa el sistema de partículas del humo
        {
            humeando = true;
            particleSystemHumo.Play();
        }
        if ((PerfilJugador.Energia >= 25 || PerfilJugador.Energia <=0) && humeando) // y acá se lo desactiva
        {
            humeando = false;
            particleSystemHumo.Stop();
        }
    }

    public void modificarCombustible(float cantidad)
    {
        PerfilJugador.Combustible += cantidad;
        if (PerfilJugador.Combustible > 100) { PerfilJugador.Combustible = 100; }
        if (PerfilJugador.Combustible < 0) {
            PerfilJugador.Combustible = 0;
            PerfilJugador.Energia = 0;
        }
    }

    public float GetEnergia()           // método público para monitorear la energía del jugador desde otra clase
    {
        return PerfilJugador.Energia;
    }
    public bool EstaVivo()              // método público para monitorear desde otra clase si el jugador está vivo
    {
        return vive;
    }
    public bool AlcanzoMeta()           // método público para monitorear desde otra clase si el jugador alcanzó la meta
    {
        return meta;
    }
    public void JugadorExplota()          // método público para hacer que el jugador explote
    {
        vive = false;
    }

    public void Colision()                                      // método público que acciona los efectos de la colisión
    {
        Vector3 posicion = gameObject.transform.position;
        particleSystemCrash.transform.position = posicion;      // se posiciona el sistema de partículas donde está el jugador
        particleSystemCrash.Play();                             // se activa el sistema de partículas del choque
    }
    public void OnExplosion()                                   // método público que acciona los efectos de la explosión
    {                                                           // usado como evento al inicio de la animación de la explosión
        particleSystemHumo.Stop();                              // se detiene el humo
        particleSystemExplosion.Play();                         // se ejecuta la proyección de partículas incendiadas
        Collider2D collider2D = GetComponent<Collider2D>();
        collider2D.enabled = false;                             // se desactiva el collider del auto
    }

    private void OnTriggerEnter2D(Collider2D collision)             //Método para chequear la llegada a Meta
    {
        if (!collision.gameObject.CompareTag("Meta")){return;}      //si el choque fue con otra cosa, sale del método
        meta = true;                                                // se indica que se llegó a la meta
        Debug.Log("LLEGASTE A LA META!! NIVEL " + progresionJugador.PerfilJugador.Nivel + " COMPLETO");
        progresionJugador.SubirNivel();
        if (virtualCamera.Follow)
        {
            virtualCamera.Follow = null;                            // se deja de seguir al auto (ya que el auto avanzará hacia afuera)
        }
        particleSystemHumo.Stop();                                  // se detiene el humeo por si estaba ocurriendo
        PerfilJugador.BumperConteo = 0;                             // en caso que esté el bumper, se lo desactiva
        particleSystemStars.Play();                                 // se activan los fuegos artificiales 
        musicaFondo.GetComponent<AudioSource>().Stop();             // se detiene la música de fondo
        musicaMeta.GetComponent<AudioSource>().Play();              // y se pone la música de llegada
    }

}
