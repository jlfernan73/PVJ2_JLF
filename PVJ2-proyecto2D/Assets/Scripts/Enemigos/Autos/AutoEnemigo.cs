using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase madre abstracta de la que heredan los autos enemigos
public abstract class AutoEnemigo : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] protected float aceleracion = 40f;       // módulo de la fuerza de aceleración
    [SerializeField] protected float minRapidez = 20;         // valor al que baja la velocidad para volver a acelerar
    [SerializeField] protected float energiaInicial = 100;
    [SerializeField] protected AudioClip explosionSFX;
    protected AudioSource audioExplosion;
    public GameObject particlesCrashEnemigo;        //crash al colisionar
    protected ParticleSystem particleSystemCrashEnemigo;       

    public GameObject particlesHumo;        //humo
    protected ParticleSystem particleSystemHumo;       
    
    protected float energiaEnemigo;
    protected Vector2 direccion;                      // dirección de avance del auto
    protected float rapidez;                          // módulo de la velocidad
    protected bool acelerar = true;                   // bandera para activar el impulso
    protected bool humeando = false;
    protected bool vive = true;
    protected float tiempoMuerto = 0f;

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
        audioExplosion = GetComponent<AudioSource>();
        vive = true;
        tiempoMuerto = 0f;
        energiaEnemigo = energiaInicial;
        miAnimator.SetBool("Vive", true);
        Inicializar();                                  //inicializa los valores de las variables propias de cada tipo de auto
    }

    private void Update()
    {
        if (vive)
        {
            Mover();
            if (particleSystemHumo != null)
            {
                particleSystemHumo.transform.position = transform.position;          // la posición del sist. de partículas de humo sigue la del auto
            }
        }
        else
        {
            tiempoMuerto += Time.deltaTime;
            if (tiempoMuerto > 3f)
            {
                GetComponent<Collider2D>().enabled = true;
                gameObject.SetActive(false);
                particlesCrashEnemigo.SetActive(false);
                particlesHumo.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (vive)
        {
            Acelerar();
        }
    }

    protected abstract void Inicializar();
    protected abstract void Mover();
    protected abstract void Acelerar();

    public void Colision()                                      // método público que acciona los efectos de la colisión
    {
        if (particleSystemCrashEnemigo != null)
        {
            particleSystemCrashEnemigo.transform.position = transform.position;      // se posiciona el sistema de partículas donde está el jugador
            particleSystemCrashEnemigo.Play();                             // se activa el sistema de partículas del choque
        }
    }

    public void ModificarEnergia(float puntos)      // método público para modificar la energía
    {                                               // sin bajar de 0
        if (vive)
        {
            energiaEnemigo += puntos;
            if (energiaEnemigo <= 0)
            {
                energiaEnemigo = 0;
                Explota();
            }
            if (energiaEnemigo < 25 && !humeando)          // acá se activa el sistema de partículas del humo
            {
                humeando = true;
                if (particleSystemHumo != null)
                {
                    particleSystemHumo.Play();
                }
            }
            if ((energiaEnemigo >= 25 || energiaEnemigo <= 0) && humeando) // y acá se lo desactiva
            {
                humeando = false;
                if (particleSystemHumo != null)
                {
                    particleSystemHumo.Stop();
                }
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
    protected void Explota()
    {
        vive = false;
        GetComponent<Collider2D>().enabled = false;
        if (GetComponent<Renderer>().isVisible)
        {
            audioExplosion.PlayOneShot(explosionSFX);       // se ejecuta el sonido de la explosion sólo si está visible
        }
        miAnimator.SetBool("Vive", false);
    }
    public bool GetVive()
    {
        return vive;
    }

}
