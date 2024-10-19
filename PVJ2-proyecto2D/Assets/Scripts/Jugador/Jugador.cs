using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;

// clase Jugador con atributos modificables (energ�a, �tems) y m�todos respectivos

public class Jugador : MonoBehaviour
{
    [SerializeField]
    private PerfilJugador perfilJugador;
    public PerfilJugador PerfilJugador { get => perfilJugador; }
    
    [Header("Monitoreo")]
    [SerializeField] private float energia = 100f;      //energ�a inicial del jugador
    [SerializeField] private float combustible = 100f;      //combustible inicial del jugador

    // se incorporan sistemas de part�culas para animar distintas situaciones
    [SerializeField] private ParticleSystem particleSystemCrash;        //choque al colisionar
    [SerializeField] private ParticleSystem particleSystemHumo;         //humo que empieza a salir cuando la energ�a es muy baja
    [SerializeField] private ParticleSystem particleSystemExplosion;    //explosi�n que ocurre al tener energ�a nula
    [SerializeField] private ParticleSystem particleSystemStars;        //fuegos artificiales de estrellas al llegar a la meta

    [SerializeField] private CinemachineVirtualCamera virtualCamera;    //se accede a la c�mara para detener el seguimiento en la meta

    // se accede a estos transform para activar/desactivar seg�n se cumplan condiciones
    [SerializeField] private Transform musicaFondo;                     //para poder apagar la m�sica de fondo
    [SerializeField] private Transform musicaMeta;                      //y poner una m�sica de llegada

    private Progresion progresionJugador;

    // banderas para monitorear situaciones
    bool humeando = false;
    bool vive = true;
    bool meta = false;

    void Awake()
    {
        progresionJugador = GetComponent<Progresion>();
    }

    private void Update()
    {
        particleSystemHumo.transform.position = gameObject.transform.position;          // la posici�n del sist. de part�culas de humo sigue la del auto
        particleSystemExplosion.transform.position = gameObject.transform.position;     // idem con la posici�n del sistema de part�culas de la explosi�n
    }


    public void ModificarEnergia(float puntos)      // m�todo p�blico para modificar la energ�a
    {                                               // sin superar 100 ni bajar de 0
        energia += puntos;
        if (energia > 100)
        {
            energia = 100;
        }
        if (energia < 0)
        {
            energia = 0;                        // con energ�a nula el jugador aun vivir� hasta explotar
        }
        if (energia < 25 && !humeando)          // ac� se activa el sistema de part�culas del humo
        {
            humeando = true;
            particleSystemHumo.Play();
        }
        if ((energia >= 25 || energia <=0) && humeando) // y ac� se lo desactiva
        {
            humeando = false;
            particleSystemHumo.Stop();
        }
    }

    public void modificarCombustible(float cantidad)
    {
        combustible += cantidad;
        if (combustible > 100) { combustible = 100; }
        if (combustible < 0) {  
            combustible = 0;
            energia = 0;
        }
    }

    /*public void AgregarDiamantes()                   // m�todo p�blico para agregar un �tem (usado para los diamantes)
    {
        diamantes++;
        if (diamantes == diamantesRequeridos) 
        {
            barrera.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;      //se hace la valla din�mica poder moverla
            barrera.GetComponent<Rigidbody2D>().mass = 1.0f;      //se aliviana la valla para poder pasar a la meta
        }
    }*/

    public float GetEnergia()           // m�todo p�blico para monitorear la energ�a del jugador desde otra clase
    {
        return energia;
    }
    public bool EstaVivo()              // m�todo p�blico para monitorear desde otra clase si el jugador est� vivo
    {
        return vive;
    }
    public bool AlcanzoMeta()           // m�todo p�blico para monitorear desde otra clase si el jugador alcanz� la meta
    {
        return meta;
    }
    public void JugadorExplota()          // m�todo p�blico para hacer que el jugador explote
    {
        vive = false;
    }

    public void Colision()                                      // m�todo p�blico que acciona los efectos de la colisi�n
    {
        Vector3 posicion = gameObject.transform.position;        
        particleSystemCrash.transform.position = posicion;      // se posiciona el sistema de part�culas donde est� el jugador
        particleSystemCrash.Play();                             // se activa el sistema de part�culas del choque
    }
    public void OnExplosion()                                   // m�todo p�blico que acciona los efectos de la explosi�n
    {                                                           // usado como evento al inicio de la animaci�n de la explosi�n
        particleSystemHumo.Stop();                              // se detiene el humo
        particleSystemExplosion.Play();                         // se ejecuta la proyecci�n de part�culas incendiadas
        Collider2D collider2D = GetComponent<Collider2D>();
        collider2D.enabled = false;                             // se desactiva el collider del auto
    }

    private void OnTriggerEnter2D(Collider2D collision)             //M�todo para chequear la llegada a Meta
    {
        if (!collision.gameObject.CompareTag("Meta")){return;}      //si el choque fue con otra cosa, sale del m�todo
        meta = true;                                                // se indica que se lleg� a la meta
        progresionJugador.SubirNivel();
        Debug.Log("LLEGASTE A LA META!! NIVEL " + progresionJugador.PerfilJugador.Nivel + " COMPLETO");
        if (virtualCamera.Follow)
        {
            virtualCamera.Follow = null;                            // se deja de seguir al auto (ya que el auto avanzar� hacia afuera)
        }
        particleSystemHumo.Stop();                                  // se detiene el humeo por si estaba ocurriendo
        particleSystemStars.Play();                                 // se activan los fuegos artificiales 
        musicaFondo.GetComponent<AudioSource>().Stop();             // se detiene la m�sica de fondo
        musicaMeta.GetComponent<AudioSource>().Play();              // y se pone la m�sica de llegada
    }

}
