using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase madre abstracta de la que heredan los autos enemigos
public abstract class AutoEnemigo : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] protected float aceleracion = 40f;       // m�dulo de la fuerza de aceleraci�n
    [SerializeField] protected float minRapidez = 20;         // valor al que baja la velocidad para volver a acelerar
    [SerializeField] protected float energiaEnemigo = 100;
    public GameObject particlesCrashEnemigo;        //crash al colisionar
    protected ParticleSystem particleSystemCrashEnemigo;       

    public GameObject particlesHumo;        //humo
    protected ParticleSystem particleSystemHumo;       
    
    protected Vector2 direccion;                      // direcci�n de avance del auto
    protected float rapidez;                          // m�dulo de la velocidad
    protected bool acelerar = true;                   // bandera para activar el impulso
    protected bool humeando = false;

    protected Rigidbody2D miRigidbody2D;
    protected Animator miAnimator;
    protected SpriteRenderer miSprite;

    protected void Start()
    {
        if (particlesCrashEnemigo != null)
        {
            particleSystemCrashEnemigo = particlesCrashEnemigo.GetComponent<ParticleSystem>();
        }
        if (particlesHumo != null)
        {
            particleSystemHumo = particlesHumo.GetComponent<ParticleSystem>();
        }
    }

    protected void OnEnable()
    {
        miRigidbody2D = GetComponent<Rigidbody2D>();
        miAnimator = GetComponent<Animator>();
        miSprite = GetComponent<SpriteRenderer>();
        Inicializar();                                  //inicializa los valores de las variables propias de cada tipo de auto
    }

    private void Update()
    {
        Mover();
        if (particleSystemHumo != null)
        {
            particleSystemHumo.transform.position = transform.position;          // la posici�n del sist. de part�culas de humo sigue la del auto
        }
    }

    private void FixedUpdate()
    {
        Acelerar();
    }

    protected abstract void Inicializar();
    protected abstract void Mover();
    protected abstract void Acelerar();

    public void Colision()                                      // m�todo p�blico que acciona los efectos de la colisi�n
    {
        if (particleSystemCrashEnemigo != null)
        {
            particleSystemCrashEnemigo.transform.position = transform.position;      // se posiciona el sistema de part�culas donde est� el jugador
            particleSystemCrashEnemigo.Play();                             // se activa el sistema de part�culas del choque
        }
    }

    public void ModificarEnergia(float puntos)      // m�todo p�blico para modificar la energ�a
    {                                               // sin bajar de 0
        energiaEnemigo += puntos;
        if (energiaEnemigo < 0)
        {
            energiaEnemigo = 0;                        // con energ�a nula el jugador aun vivir� hasta explotar
        }
        if (energiaEnemigo < 25 && !humeando)          // ac� se activa el sistema de part�culas del humo
        {
            humeando = true;
            if(particleSystemHumo != null)
            {
                particleSystemHumo.Play();
            }
        }
        if ((energiaEnemigo >= 25 || energiaEnemigo <= 0) && humeando) // y ac� se lo desactiva
        {
            humeando = false;
            if (particleSystemHumo != null)
            {
                particleSystemHumo.Stop();
            }
        }
    }

    public void AsignarCrash(GameObject particulas)
    {
        particlesCrashEnemigo = particulas;
    }
    public void AsignarHumo(GameObject particulas)
    {
        particlesHumo = particulas;
    }
}
