using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;
using UnityEngine.Events;

// clase Jugador con variables definidas via Scriptable Object


public class Jugador : MonoBehaviour
{
    [SerializeField]
    private PerfilJugador perfilJugador;
    public PerfilJugador PerfilJugador { get => perfilJugador; }

    // se incorporan sistemas de part�culas para animar distintas situaciones
    [Header("Efectos visuales")]
    [SerializeField] private ParticleSystem particleSystemCrash;        //choque al colisionar
    [SerializeField] private ParticleSystem particleSystemHumo;         //humo que empieza a salir cuando la energ�a es muy baja
    [SerializeField] private ParticleSystem particleSystemExplosion;    //explosi�n que ocurre al tener energ�a nula
    [SerializeField] private ParticleSystem particleSystemStars;        //fuegos artificiales de estrellas al llegar a la meta

    [Header("Gestion de escena y sonido de fondo")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;    //se accede a la c�mara para detener el seguimiento en la meta

    // se accede a estos transform para activar/desactivar seg�n se cumplan condiciones
    [SerializeField] private Transform musicaFondo;                     //para poder apagar la m�sica de fondo
    [SerializeField] private Transform musicaMeta;                      //y poner una m�sica de llegada
    [SerializeField] private Transform protector;

    private Progresion progresionJugador;

    // banderas para monitorear situaciones
    bool humeando = false;
    bool vive = true;
    bool meta = false;
    bool estatico = false;
    bool protegido = false;

    float tiempoInicial;        // para chequear periodos de tiempo

    //----Eventos del jugador----
    [SerializeField] UnityEvent<float> OnEnergyChanged;
    [SerializeField] UnityEvent<float> OnFuelChanged;
    [SerializeField] UnityEvent<string> OnDiamantesChanged;
    [SerializeField] UnityEvent<string> OnPuntajeChanged;
    [SerializeField] UnityEvent<int,bool> OnItemChanged;
    [SerializeField] UnityEvent<int> OnVidasChanged;

    void Start()
    {
        progresionJugador = GetComponent<Progresion>();
        //inicializaci�n de atributos
        PerfilJugador.Energia = 100f;
        PerfilJugador.Combustible = 100f;
        PerfilJugador.NitroTank = 0;

        OnEnergyChanged.Invoke(perfilJugador.Energia);
        OnFuelChanged.Invoke(perfilJugador.Combustible);
        OnDiamantesChanged.Invoke(GameManager.Instance.GetExperiencia().ToString());
        OnPuntajeChanged.Invoke(GameManager.Instance.GetPuntaje().ToString());
        OnVidasChanged.Invoke(GameManager.Instance.GetVidas() - 1);
        for (int i = 1; i < 5; i++)
        {
            OnItemChanged.Invoke(i, false);
        }
        tiempoInicial = Time.time;
    }

    private void Update()
    {
        particleSystemHumo.transform.position = gameObject.transform.position;          // la posici�n del sist. de part�culas de humo sigue la del auto
        // condiciones iniciales luego de explosion
        if ((Time.time - tiempoInicial) <= 5 && protegido)
        {
            protector.transform.position = transform.position;
            protector.transform.rotation = transform.rotation;
        }
        if ((Time.time - tiempoInicial) > 1 && estatico)
        {
            //GetComponent<Collider2D>().excludeLayers = LayerMask.GetMask("Nothing");    //reactiva la colisi�n con Enemigos por un tiempo
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            estatico = false;
        }
        if ((Time.time - tiempoInicial) > 5 && protegido)
        {
            protector.GetComponent<Collider2D>().enabled = false;
            protegido = false;
        }
    }

    public void ModificarEnergia(float puntos)      // m�todo p�blico para modificar la energ�a
    {                                               // sin superar 100 ni bajar de 0
        PerfilJugador.Energia += puntos;
        GameManager.Instance.AdPuntaje((int)puntos);
        if (PerfilJugador.Energia > 100)
        {
            PerfilJugador.Energia = 100;
        }
        if (PerfilJugador.Energia < 0)
        {
            PerfilJugador.Energia = 0;                        // con energ�a nula el jugador aun vivir� hasta explotar
        }
        if (PerfilJugador.Energia < 25 && !humeando)          // ac� se activa el sistema de part�culas del humo
        {
            humeando = true;
            particleSystemHumo.Play();
        }
        if ((PerfilJugador.Energia >= 25 || PerfilJugador.Energia <=0) && humeando) // y ac� se lo desactiva
        {
            humeando = false;
            particleSystemHumo.Stop();
        }
        OnEnergyChanged.Invoke(perfilJugador.Energia);
        OnPuntajeChanged.Invoke(GameManager.Instance.GetPuntaje().ToString());
    }

    public void modificarCombustible(float cantidad)    //m�todo p�blico para modificar el combustible desde MoverJugador y desde Coleccionar
    {
        PerfilJugador.Combustible += cantidad;
        if (PerfilJugador.Combustible > 100) { PerfilJugador.Combustible = 100; }
        if (PerfilJugador.Combustible < 0) {            //si se queda sin combustible, tambi�n se queda sin energ�a
            PerfilJugador.Combustible = 0;
            ModificarEnergia(-PerfilJugador.Energia);
        }
        OnFuelChanged.Invoke(perfilJugador.Combustible);
    }

    public void ReportarDiamantes()
    {
        OnDiamantesChanged.Invoke(perfilJugador.Experiencia.ToString());
        OnPuntajeChanged.Invoke(GameManager.Instance.GetPuntaje().ToString());
    }

    public void ModificarItem(int objeto, bool estado)
    {
        OnItemChanged.Invoke(objeto,estado);
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
        GameManager.Instance.ModificarVida(-1);
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
        particleSystemExplosion.transform.position = gameObject.transform.position;     // posici�n del sistema de part�culas de la explosi�n
        particleSystemExplosion.Play();                         // se ejecuta la proyecci�n de part�culas incendiadas
        Collider2D collider2D = GetComponent<Collider2D>();
        collider2D.enabled = false;                             // se desactiva el collider del auto
    }


    public void ResetJugador()
    {
        OnVidasChanged.Invoke(GameManager.Instance.GetVidas()-1);
        vive = true;
        gameObject.transform.position = particleSystemExplosion.transform.position;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = true;                             // se reactiva el collider del auto
        protector.GetComponent<Collider2D>().enabled = true;
        GetComponent<Coleccionar>().VaciarInventario();
        PerfilJugador.Energia = 100f;
        PerfilJugador.Combustible = 100f;
        PerfilJugador.NitroTank = 0;
        OnEnergyChanged.Invoke(perfilJugador.Energia);
        OnFuelChanged.Invoke(perfilJugador.Combustible);
        tiempoInicial = Time.time;
        estatico = true;
        protegido = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)             //M�todo para chequear la llegada a Meta
    {
        if (!collision.gameObject.CompareTag("Meta")){return;}      //si el choque fue con otra cosa, sale del m�todo
        meta = true;                                                // se indica que se lleg� a la meta
        Debug.Log("LLEGASTE A LA META!! NIVEL " + progresionJugador.PerfilJugador.Nivel + " COMPLETO");
        //progresionJugador.SubirNivel();
        ReportarDiamantes();
        if (virtualCamera.Follow)
        {
            virtualCamera.Follow = null;                            // se deja de seguir al auto (ya que el auto avanzar� hacia afuera)
        }
        particleSystemHumo.Stop();                                  // se detiene el humeo por si estaba ocurriendo
        PerfilJugador.BumperConteo = 0;                             // en caso que est� el bumper, se lo desactiva
        PerfilJugador.CannonConteo = 0;                             // en caso que est� el ca�on, se lo desactiva
        particleSystemStars.Play();                                 // se activan los fuegos artificiales 
        musicaFondo.GetComponent<AudioSource>().Stop();             // se detiene la m�sica de fondo
        musicaMeta.GetComponent<AudioSource>().Play();              // y se pone la m�sica de llegada
    }
}
