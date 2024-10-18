using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Rendering;
using static UnityEngine.RuleTile.TilingRuleOutput;

// clase para mover el automovil del jugador al presionar las cuatro teclas de desplazamiento
// el auto acelera hacia adelante/atras con las flechas up/down (o W/S), y gira con las flechas laterales (o A/D)
// el control es complejo para una sola mano, por eso conviene combinar (por ej. flechas laterales y W/S)

public class MoverJugador : MonoBehaviour
{
    // Variables a configurar desde el editor
    [Header("Configuracion")]
    [SerializeField] float aceleracion = 10f;       // modulo de la fuerza de arranque
    [SerializeField] float maxAngulo = 5.0f;        // m�ximo angulo de giro del auto al doblar
    [SerializeField] float maxRapidez = 20f;        // velocidad m�xima que alcanza el auto al acelerar varias veces
    [SerializeField] private float nitroTank = 100f;
    //clips de los sonidos de distintas situaciones del auto
    [SerializeField] private AudioClip reversaSFX;
    [SerializeField] private AudioClip motorSFX;
    [SerializeField] private AudioClip explosionSFX;
    
    // Variables de uso interno en el script
    private float girar;                            // adquiere valores al presionar las flechas laterals (o A/D) para frenar
    private float mover;                            // adquiere valores al presionar las flecha up/down (o W/S) para acelerar
    private Vector2 direccion = new Vector2(0, 1);  // vector 2D con la direccion del automovil
    private float sentido = 1f;                     //sentido de avance (1: arriba, -1: abajo)
    private float rapidez = 0;                      // modulo del vector velocidad
    private float angulo = 0;                       // angulo de giro de las ruedas del automovil (respecto al eje del auto)
    private float deltaAngulo = 2f;                 // agregado de �ngulo de las ruedas que se suma al angulo de giro
    private bool girando = false;                   // bandera para activacion del giro
    private float volMotor = 0.1f;                  // se controla el volumen del sonido del motor
    private float pitchMotor = 1f;                  // se controla la frecuencia de reproducci�n (que aumentar� cuando acelere)
    private float maxRapidezInicial;
    private float aceleracionInicial;
    private bool nitro = false;

    // Variables para referenciar distintos componentes del objeto
    private Rigidbody2D miRigidbody2D;
    private SpriteRenderer miSprite;
    private Animator miAnimator;
    private AudioSource audioSource;

    private void OnEnable()
    {
        miRigidbody2D = GetComponent<Rigidbody2D>();
        miAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = motorSFX;                // se carga el sonido del motor 
        SonidoMotor();                              // se ejecuta el m�todo que controla el sonido del motor
        audioSource.Play();                         // se ejecuta el sonido
        maxRapidezInicial = maxRapidez;
        aceleracionInicial = aceleracion;
    }

    private void Update()
    {
        Jugador jugador = gameObject.GetComponent<Jugador>();
        //si el jugador est� vivo y aun no alcanz� la meta se habilita el control del auto
        if (jugador.EstaVivo() && !jugador.AlcanzoMeta())       
        {
            girar = Input.GetAxis("Horizontal");
            mover = Input.GetAxis("Vertical");
            if (nitro)
            {
                if (mover == 0) { mover = 1; }
                nitroTank -= 0.1f;
                if(nitroTank < 0) { 
                    nitro = false;
                    maxRapidez = maxRapidezInicial;
                    aceleracion = aceleracionInicial;
                }
            }
            rapidez = miRigidbody2D.velocity.magnitude;     // se obtiene el m�dulo del vector velocidad, para mantenerlo en el vector al girar
            direccion = transform.up.normalized;            // se lee la direcci�n (normalizada) del auto
            if (girar != 0)                                 // si se presiona las teclas para girar
            {
                girando = true;                             // depende de la direcci�n de giro, se le agrega un delta al �ngulo
                if (girar > 0 && angulo >= -1 * maxAngulo)  // siempre que no se haya superado el m�ximo angulo de giro posible
                {
                    angulo -= deltaAngulo;
                }
                if (girar < 0 && angulo <= maxAngulo)
                {
                    angulo += deltaAngulo;
                }
            }

            if (girar == 0 && girando)                      // si no se est� girando con las flechas, pero las ruedas aun est�n giradas
            {
                girando = false;                           // se lleva el angulo a cero
                angulo = 0;
            }

            if (girando)                                // en cualquier caso en que se encuentre girando
            {
                float radioGiro = Mathf.Cos(angulo * Mathf.Deg2Rad) * (1 + rapidez) / 60f;     //se calcula un radio de giro en base al �ngulo y a la rapidez
                float velocidadAngular;
                if (angulo > 0)
                {
                    velocidadAngular = sentido * (rapidez / radioGiro);           //se calcula la velocidad angular en base a la rapidez y el radio de giro
                }
                else
                {
                    velocidadAngular = -1 * sentido * (rapidez / radioGiro);
                }
                transform.Rotate(0, 0, velocidadAngular * Time.deltaTime);                  // se rota el auto
                direccion = transform.up.normalized;                                        // se lee la nueva direcci�n (normalizada) del auto
                miRigidbody2D.velocity = sentido * direccion * rapidez;                     // se recalcula el vector velocidad con la nueva direcci�n
            }
            // definici�n de las condiciones para las transiciones de las animaciones
            miAnimator.SetFloat("Giro", girar);
            miAnimator.SetBool("Avanza", (rapidez > 2f && sentido > 0));
            miAnimator.SetBool("Retrocede", (rapidez > 2f && sentido < 0));

            SonidoMotor();
            float combustibleGastado = -0.0001f * (1 + 5 * rapidez);
            jugador.modificarCombustible(combustibleGastado);
        }

        // definici�n de la condici�n de transici�n hacia la explosi�n
        miAnimator.SetBool("Explota", (jugador.GetEnergia() <= 0 && jugador.EstaVivo()));

        // si se qued� sin energ�a pero aun no explot�, se lo hace explotar
        if (jugador.GetEnergia() <= 0 && jugador.EstaVivo())
        {
            audioSource.Stop();
            audioSource.volume = 1;
            audioSource.PlayOneShot(explosionSFX);
            jugador.JugadorExplota();                       
        }

        // si ya explot� y se termin� la explosi�n, se borra el jugador
        if (!audioSource.isPlaying && !jugador.EstaVivo())
        {
            gameObject.SetActive(false);
        }

        // si se alcanz� la meta y el auto aun no se borr�
        if (jugador.AlcanzoMeta() && transform.gameObject.activeSelf)
        {
            RetirarAuto();
        }
    }

    private void FixedUpdate()
    {
        Jugador jugador = gameObject.GetComponent<Jugador>();
        //aplicaci�n de fuerza al acelerar s�lo cuando el jugador est� vivo y aun no alcanz� la meta
        if (jugador.EstaVivo() && !jugador.AlcanzoMeta())
        {
            if (mover > 0 && rapidez < maxRapidez)                  // si se presiona la tecla para acelerar 
            {
                sentido = 1;
                miRigidbody2D.AddForce(direccion * aceleracion);    // aplica fuerza en la direcci�n del auto
            }
            if (mover < 0 && rapidez < maxRapidez / 2)                // para dar marcha atr�s
            {
                sentido = -1;
                miRigidbody2D.AddForce(-direccion * 200f);          // aplica fuerza en la direcci�n reversa al auto
                audioSource.pitch = 1;                              // aplica condiciones de volumen y pitch normales
                audioSource.volume = 1;
                audioSource.PlayOneShot(reversaSFX);
            }
        }
    }

    private void SonidoMotor()                              // este m�todo acondiciona el sonido del motor seg�n la rapidez
    {
        Jugador jugador = gameObject.GetComponent<Jugador>();
        if (jugador.EstaVivo())
        {
            volMotor = 0.1f + rapidez / maxRapidez * 0.9f;      // el volumen aumentar� a mayor rapidez (entre 0.1 y 1)
            audioSource.volume = volMotor;
            float rapidezUp = Mathf.Abs(Vector2.Dot(miRigidbody2D.velocity, transform.up) / transform.up.magnitude);
            if (rapidezUp > maxRapidez) { rapidezUp = maxRapidez; }
            pitchMotor = 0.5f + rapidezUp / maxRapidez * 0.8f;    // el pitch (frecuencia) aumentar� a mayor rapidez (hasta 1.3)
            if (sentido > 0)                                    // aunque s�lo si no est� retrocediendo
            {
                audioSource.pitch = pitchMotor;
            }
        }
    }

    private void RetirarAuto()                          // este m�todo se ejecuta cuando se llega a la meta, y saca el auto de pantalla
    {
        direccion = new Vector3(0,1,0);                 // apunta el auto hacia arriba
        transform.up = direccion;                       
        if (GetComponent<Renderer>().isVisible)                 // si aun no sali� de la pantalla lo lleva a su m�xima velocidad
        {
            miRigidbody2D.velocity = direccion * maxRapidez;
        }
        else
        {                                                       // si ya sali� de la pantalla desactiva el objeto y detiene todo 
            audioSource.Stop();
            gameObject.SetActive(false);
        }
    }

    public void OnAnimationEnd()                                // m�todo para desactivar el spriteRenderer al final de una animaci�n
    {                                                           // usado como evento al final de la animaci�n de la explosi�n
        miSprite = gameObject.GetComponent<SpriteRenderer>();
        miSprite.enabled = false;
    }

    public void activarNitro()
    {
        nitro = true;
        maxRapidez = maxRapidezInicial*1.5f;
        aceleracion = aceleracionInicial * 3;
        nitroTank = 100f;
    }
}