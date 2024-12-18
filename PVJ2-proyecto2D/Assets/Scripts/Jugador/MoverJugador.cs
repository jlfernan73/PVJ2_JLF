using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using UnityEngine.Rendering;
using static UnityEngine.RuleTile.TilingRuleOutput;

// clase para mover el automovil del jugador al presionar las cuatro teclas de desplazamiento
// el auto acelera hacia adelante/atras con las flechas up/down (o W/S), y gira con las flechas laterales (o A/D)
// el control es complejo para una sola mano, por eso conviene combinar (por ej. flechas laterales y W/S)
// Contiene variables definidas via Scriptable Object

public class MoverJugador : MonoBehaviour
{
    private PerfilJugador perfilJugador;
    public PerfilJugador PerfilJugador { get => perfilJugador; }

    [SerializeField] UnityEvent<string> OnNivelEnd;

    // Variables de uso interno
    private float girar;                            // adquiere valores al presionar las flechas laterals (o A/D) para frenar
    private float mover;                            // adquiere valores al presionar las flecha up/down (o W/S) para acelerar
    private Vector2 direccion = new Vector2(0, 1);  // vector 2D con la direccion del automovil
    private float sentido = 1f;                     //sentido de avance (1: arriba, -1: abajo)
    private float rapidez = 0;                      // modulo del vector velocidad
    private float angulo = 0;                       // angulo de giro de las ruedas del automovil (respecto al eje del auto)
    private float deltaAngulo = 4f;                 // agregado de �ngulo de las ruedas que se suma al angulo de giro
    private bool girando = false;                   // bandera para activacion del giro
    private float volMotor = 0.1f;                  // se controla el volumen del sonido del motor
    private float pitchMotor = 1f;                  // se controla la frecuencia de reproducci�n (que aumentar� cuando acelere)
    //dado que Nitro modifica algunas variables, es necesario saber a qu� valores volver
    private float maxRapidezInicial;                
    private float maxAnguloInicial;
    private float aceleracionInicial;
    private bool nitro = false;

    // Variables para referenciar distintos componentes del objeto
    private Rigidbody2D miRigidbody2D;
    private SpriteRenderer miSprite;
    private Animator miAnimator;
    private AudioSource audioSource;

    private void OnEnable()
    {
        perfilJugador = GetComponent<Jugador>().PerfilJugador;      // para acceder a los datos del SO
        miRigidbody2D = GetComponent<Rigidbody2D>();
        miAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = PerfilJugador.MotorSFX;                // se carga el sonido del motor 
        SonidoMotor();                              // se ejecuta el m�todo que controla el sonido del motor
        audioSource.Play();                         // se ejecuta el sonido
        maxAnguloInicial = PerfilJugador.MaxAngulo;
        maxRapidezInicial = PerfilJugador.MaxRapidez;
        aceleracionInicial = PerfilJugador.Aceleracion;
        miAnimator.SetBool("Reinicia", false);
    }

    private void Update()
    {
        Jugador jugador = gameObject.GetComponent<Jugador>();
        //si el jugador est� vivo y aun no alcanz� la meta se habilita el control del auto
        if (jugador.EstaVivo() && !jugador.AlcanzoMeta())       
        {
            girar = Input.GetAxis("Horizontal");
            mover = Input.GetAxis("Vertical");
            if (nitro)                                  //si est� activado el Nitro
            {
                PerfilJugador.NitroTank -= PerfilJugador.ConsumoObj * Time.deltaTime * 1.5f;   //consumo del nitro
                if(PerfilJugador.NitroTank < 0) {           //acciones al terminarse
                    nitro = false;
                    PerfilJugador.NitroTank = 0;
                    PerfilJugador.MaxRapidez = maxRapidezInicial;
                    PerfilJugador.MaxAngulo = maxAnguloInicial;
                    PerfilJugador.Aceleracion = aceleracionInicial;
                }
            }
            rapidez = miRigidbody2D.velocity.magnitude;     // se obtiene el m�dulo del vector velocidad, para mantenerlo en el vector al girar
            direccion = transform.up.normalized;            // se lee la direcci�n (normalizada) del auto
            if (girar != 0)                                 // si se presiona las teclas para girar
            {
                girando = true;                             // depende de la direcci�n de giro, se le agrega un delta al �ngulo
                if (girar > 0 && angulo >= -1 * PerfilJugador.MaxAngulo)  // siempre que no se haya superado el m�ximo angulo de giro posible
                {
                    angulo -= deltaAngulo;
                }
                if (girar < 0 && angulo <= PerfilJugador.MaxAngulo)
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
                float radioGiro = Mathf.Cos(angulo * Mathf.Deg2Rad) * (1f + 0.75f*rapidez) / 60f;     //se calcula un radio de giro en base al �ngulo y a la rapidez
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
                miRigidbody2D.velocity = (miRigidbody2D.velocity.normalized + sentido*(new Vector2 (transform.up.normalized.x, transform.up.normalized.y) - direccion.normalized)) * rapidez;                     // se recalcula el vector velocidad con la nueva direcci�n
                direccion = transform.up.normalized;                                        // se lee la nueva direcci�n (normalizada) del auto
            }
            // definici�n de las condiciones para las transiciones de las animaciones
            miAnimator.SetFloat("Giro", girar);
            miAnimator.SetBool("Avanza", (rapidez > 2f && sentido > 0));
            miAnimator.SetBool("Retrocede", (rapidez > 2f && sentido < 0));

            SonidoMotor();
            float combustibleGastado = -PerfilJugador.ConsumoComb * (1 + 5 * rapidez) * Time.deltaTime; //consumo de combustible
            jugador.modificarCombustible(combustibleGastado);                       
        }

        // definici�n de la condici�n de transici�n hacia la explosi�n
        miAnimator.SetBool("Explota", (PerfilJugador.Energia <= 0 && jugador.EstaVivo()));

        // si se qued� sin energ�a pero aun no explot�, se lo hace explotar
        if (PerfilJugador.Energia <= 0 && jugador.EstaVivo())
        {
            audioSource.Stop();
            audioSource.volume = 1;
            audioSource.PlayOneShot(PerfilJugador.ExplosionSFX);
            Reinicio(false);
            jugador.JugadorExplota();                       
        }

        // si ya explot� y se termin� la explosi�n, se borra el jugador
        if (!audioSource.isPlaying && !jugador.EstaVivo())
        {
            if(GameManager.Instance.GetVidas() < 1)
            {
                gameObject.SetActive(false);
                Reinicio(false);
                GameManager.Instance.SetGameOver(true);
            }
            else
            {
                Reinicio(true);
                GetComponent<Jugador>().ResetJugador();
                audioSource.Play();
            }
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
            if (mover > 0 && rapidez < PerfilJugador.MaxRapidez)                  // si se presiona la tecla para acelerar 
            {
                sentido = 1;
                miRigidbody2D.AddForce(direccion * PerfilJugador.Aceleracion);    // aplica fuerza en la direcci�n del auto
            }
            if (mover < 0 && rapidez < PerfilJugador.MaxRapidez / 2)                // para dar marcha atr�s
            {
                sentido = -1;
                miRigidbody2D.AddForce(-direccion * 200f);          // aplica fuerza en la direcci�n reversa al auto
                audioSource.pitch = 1;                              // aplica condiciones de volumen y pitch normales
                audioSource.volume = 1;
                audioSource.PlayOneShot(PerfilJugador.ReversaSFX);
            }
        }
    }

    private void SonidoMotor()                              // este m�todo acondiciona el sonido del motor seg�n la rapidez
    {
        Jugador jugador = gameObject.GetComponent<Jugador>();
        if (jugador.EstaVivo())
        {
            volMotor = 0.1f + rapidez / PerfilJugador.MaxRapidez * 0.9f;      // el volumen aumentar� a mayor rapidez (entre 0.1 y 1)
            audioSource.volume = volMotor;
            float rapidezUp = Mathf.Abs(Vector2.Dot(miRigidbody2D.velocity, transform.up) / transform.up.magnitude);
            if (rapidezUp > PerfilJugador.MaxRapidez) { rapidezUp = PerfilJugador.MaxRapidez; }
            pitchMotor = 0.5f + rapidezUp / PerfilJugador.MaxRapidez * 0.8f;    // el pitch (frecuencia) aumentar� a mayor rapidez (hasta 1.3)
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
            miRigidbody2D.velocity = direccion * PerfilJugador.MaxRapidez;
        }
        else
        {                                                       // si ya sali� de la pantalla desactiva el objeto y detiene todo 
            audioSource.Stop();
            gameObject.SetActive(false);
            string mensaje = "Nivel " + PerfilJugador.Nivel.ToString() + " completo!!";
            OnNivelEnd.Invoke(mensaje);
            GameManager.Instance.SetVictoria(true);
        }
    }

    public void activarNitro()                              // m�todo para activar el objeto Nitro
    {
        nitro = true;
        PerfilJugador.MaxRapidez = maxRapidezInicial*1.25f;
        PerfilJugador.MaxAngulo = maxAnguloInicial + 5;
        PerfilJugador.Aceleracion = aceleracionInicial * 2;
        PerfilJugador.NitroTank = 100f;
    }
    public void Reinicio(bool valor)
    {
        miAnimator.SetBool("Reinicia", valor);

    }
}