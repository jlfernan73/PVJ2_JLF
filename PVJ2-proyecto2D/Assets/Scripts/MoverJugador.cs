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
    [SerializeField] private AudioClip reversaSFX;
    [SerializeField] private AudioClip motorSFX;

    // Variables de uso interno en el script
    private float girar;                            // adquiere valores al presionar las flechas laterals (o A/D) para frenar
    private float mover;                            // adquiere valores al presionar las flecha up/down (o W/S) para acelerar
    private Vector2 direccion = new Vector2(0, 1);  // vector 2D con la direccion del automovil
    private float sentido = 1f;                     //sentido de avance (1: arriba, -1: abajo)
    private float rapidez = 0;                      // modulo del vector velocidad
    private float angulo = 0;                       // angulo de giro de las ruedas del automovil (respecto al eje del auto)
    private float deltaAngulo = 2f;                 // agregado de ángulo de las ruedas que se suma al angulo de giro
    private bool girando = false;                   // bandera para activacion del giro
    private float volMotor = 0.1f;
    private float pitchMotor = 1f;

    // Variable para referenciar otro componente del objeto
    private Rigidbody2D miRigidbody2D;
    private Animator miAnimator;
    private AudioSource audioSource;

    // Codigo ejecutado cuando el objeto se activa en el nivel
    private void OnEnable()
    {
        miRigidbody2D = GetComponent<Rigidbody2D>();
        miAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = motorSFX;
        SonidoMotor();
        audioSource.Play();
    }

    // Codigo ejecutado en cada frame del juego (Intervalo variable)
    private void Update()
    {
        girar = Input.GetAxis("Horizontal");
        mover = Input.GetAxis("Vertical");
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
            float radioGiro = Mathf.Cos(angulo * Mathf.Deg2Rad) * (1+rapidez)/ 60f;     //se calcula un radio de giro en base al ángulo y a la rapidez
            float velocidadAngular;
            if (angulo > 0)
            {
                velocidadAngular = sentido*(rapidez / radioGiro);           //se calcula la velocidad angular en base a la rapidez y el radio de giro
            }
            else
            {
                velocidadAngular = -1*sentido*(rapidez / radioGiro);
            }
            transform.Rotate(0, 0, velocidadAngular * Time.deltaTime);                  // se rota el auto
            direccion = transform.up.normalized;                                        // se lee la nueva dirección (normalizada) del auto
            miRigidbody2D.velocity = sentido * direccion * rapidez;                     // se recalcula el vector velocidad con la nueva dirección
        }
        miAnimator.SetFloat("Giro", girar);
        miAnimator.SetBool("Avanza", (rapidez>2f && sentido > 0));
        miAnimator.SetBool("Retrocede", (rapidez > 2f && sentido < 0));

        SonidoMotor();
    }

    private void FixedUpdate()
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
            audioSource.pitch = 1;
            audioSource.volume = 1;
            audioSource.PlayOneShot(reversaSFX);
        }
    }

    private void SonidoMotor()
    {
        volMotor = 0.1f + rapidez / maxRapidez * 0.9f;
        pitchMotor = 0.5f + rapidez / maxRapidez * 0.8f;
        audioSource.volume = volMotor;
        if (sentido > 0)
        {
            audioSource.pitch = pitchMotor;
        }
    }

}