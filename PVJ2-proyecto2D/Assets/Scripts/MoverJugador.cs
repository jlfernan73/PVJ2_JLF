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
    [SerializeField] float maxAngulo = 5.0f;        // máximo angulo de giro del auto al doblar
    [SerializeField] float maxRapidez = 20f;        // velocidad máxima que alcanza el auto al acelerar varias veces
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
    private float deltaAngulo = 2f;                 // agregado de ángulo de las ruedas que se suma al angulo de giro
    private bool girando = false;                   // bandera para activacion del giro
    private float volMotor = 0.1f;                  // se controla el volumen del sonido del motor
    private float pitchMotor = 1f;                  // se controla la frecuencia de reproducción (que aumentará cuando acelere)
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
        SonidoMotor();                              // se ejecuta el método que controla el sonido del motor
        audioSource.Play();                         // se ejecuta el sonido
        maxRapidezInicial = maxRapidez;
        aceleracionInicial = aceleracion;
    }

    private void Update()
    {
        Jugador jugador = gameObject.GetComponent<Jugador>();
        //si el jugador está vivo y aun no alcanzó la meta se habilita el control del auto
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
            rapidez = miRigidbody2D.velocity.magnitude;     // se obtiene el módulo del vector velocidad, para mantenerlo en el vector al girar
            direccion = transform.up.normalized;            // se lee la dirección (normalizada) del auto
            if (girar != 0)                                 // si se presiona las teclas para girar
            {
                girando = true;                             // depende de la dirección de giro, se le agrega un delta al ángulo
                if (girar > 0 && angulo >= -1 * maxAngulo)  // siempre que no se haya superado el máximo angulo de giro posible
                {
                    angulo -= deltaAngulo;
                }
                if (girar < 0 && angulo <= maxAngulo)
                {
                    angulo += deltaAngulo;
                }
            }

            if (girar == 0 && girando)                      // si no se está girando con las flechas, pero las ruedas aun están giradas
            {
                girando = false;                           // se lleva el angulo a cero
                angulo = 0;
            }

            if (girando)                                // en cualquier caso en que se encuentre girando
            {
                float radioGiro = Mathf.Cos(angulo * Mathf.Deg2Rad) * (1 + rapidez) / 60f;     //se calcula un radio de giro en base al ángulo y a la rapidez
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
                direccion = transform.up.normalized;                                        // se lee la nueva dirección (normalizada) del auto
                miRigidbody2D.velocity = sentido * direccion * rapidez;                     // se recalcula el vector velocidad con la nueva dirección
            }
            // definición de las condiciones para las transiciones de las animaciones
            miAnimator.SetFloat("Giro", girar);
            miAnimator.SetBool("Avanza", (rapidez > 2f && sentido > 0));
            miAnimator.SetBool("Retrocede", (rapidez > 2f && sentido < 0));

            SonidoMotor();
            float combustibleGastado = -0.0001f * (1 + 5 * rapidez);
            jugador.modificarCombustible(combustibleGastado);
        }

        // definición de la condición de transición hacia la explosión
        miAnimator.SetBool("Explota", (jugador.GetEnergia() <= 0 && jugador.EstaVivo()));

        // si se quedó sin energía pero aun no explotó, se lo hace explotar
        if (jugador.GetEnergia() <= 0 && jugador.EstaVivo())
        {
            audioSource.Stop();
            audioSource.volume = 1;
            audioSource.PlayOneShot(explosionSFX);
            jugador.JugadorExplota();                       
        }

        // si ya explotó y se terminó la explosión, se borra el jugador
        if (!audioSource.isPlaying && !jugador.EstaVivo())
        {
            gameObject.SetActive(false);
        }

        // si se alcanzó la meta y el auto aun no se borró
        if (jugador.AlcanzoMeta() && transform.gameObject.activeSelf)
        {
            RetirarAuto();
        }
    }

    private void FixedUpdate()
    {
        Jugador jugador = gameObject.GetComponent<Jugador>();
        //aplicación de fuerza al acelerar sólo cuando el jugador está vivo y aun no alcanzó la meta
        if (jugador.EstaVivo() && !jugador.AlcanzoMeta())
        {
            if (mover > 0 && rapidez < maxRapidez)                  // si se presiona la tecla para acelerar 
            {
                sentido = 1;
                miRigidbody2D.AddForce(direccion * aceleracion);    // aplica fuerza en la dirección del auto
            }
            if (mover < 0 && rapidez < maxRapidez / 2)                // para dar marcha atrás
            {
                sentido = -1;
                miRigidbody2D.AddForce(-direccion * 200f);          // aplica fuerza en la dirección reversa al auto
                audioSource.pitch = 1;                              // aplica condiciones de volumen y pitch normales
                audioSource.volume = 1;
                audioSource.PlayOneShot(reversaSFX);
            }
        }
    }

    private void SonidoMotor()                              // este método acondiciona el sonido del motor según la rapidez
    {
        Jugador jugador = gameObject.GetComponent<Jugador>();
        if (jugador.EstaVivo())
        {
            volMotor = 0.1f + rapidez / maxRapidez * 0.9f;      // el volumen aumentará a mayor rapidez (entre 0.1 y 1)
            audioSource.volume = volMotor;
            float rapidezUp = Mathf.Abs(Vector2.Dot(miRigidbody2D.velocity, transform.up) / transform.up.magnitude);
            if (rapidezUp > maxRapidez) { rapidezUp = maxRapidez; }
            pitchMotor = 0.5f + rapidezUp / maxRapidez * 0.8f;    // el pitch (frecuencia) aumentará a mayor rapidez (hasta 1.3)
            if (sentido > 0)                                    // aunque sólo si no está retrocediendo
            {
                audioSource.pitch = pitchMotor;
            }
        }
    }

    private void RetirarAuto()                          // este método se ejecuta cuando se llega a la meta, y saca el auto de pantalla
    {
        direccion = new Vector3(0,1,0);                 // apunta el auto hacia arriba
        transform.up = direccion;                       
        if (GetComponent<Renderer>().isVisible)                 // si aun no salió de la pantalla lo lleva a su máxima velocidad
        {
            miRigidbody2D.velocity = direccion * maxRapidez;
        }
        else
        {                                                       // si ya salió de la pantalla desactiva el objeto y detiene todo 
            audioSource.Stop();
            gameObject.SetActive(false);
        }
    }

    public void OnAnimationEnd()                                // método para desactivar el spriteRenderer al final de una animación
    {                                                           // usado como evento al final de la animación de la explosión
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