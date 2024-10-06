using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// clase para que un tipo de enemigo busque al auto del jugador y acelere hacia él
// si el auto choca, esperará un período de tiempo antes de volver a acelerar


public class MoverEnemigo_persigue : MonoBehaviour
{
    // Variables a configurar desde el editor
    [Header("Configuracion")]
    [SerializeField] float aceleracion = 60f;               // módulo de la fuerza de aceleración
    [SerializeField] float rapidezMinima;                   // valor al que baja la velocidad para volver a acelerar mientra avanza
    [SerializeField] float espera = 500;                    // ciclos que el auto esperará antes de volver a acelerar

    // Referencia al transform del jugador serializada
    [SerializeField] Transform jugador;                     // toma la referencia del jugador

    // Variables de uso interno en el script
    private Vector2 direccion;                      // dirección del auto
    private float rapidez = 0;                      // módulo de la velocidad
    private float minRapidez;                       // variable usada para cambiar la rapidez mínima          
    private float angulo;                           // angulo usado para corregir la velocidad al girar
    private float dragInicial;                      // valor del drag lineal en condiciones normales
    private int contador = 0;                       // contador que empieza a sumar cuando colisiona, para comparar con la espera 
    private bool acelerar = true;                   // bandera para activar la aceleración

    // Variable para referenciar otro componente del objeto
    private Rigidbody2D miRigidbody2D;
    private Animator miAnimator;
    private SpriteRenderer miSprite;

    // Codigo ejecutado cuando el objeto se activa en el nivel
    private void Awake()
    {
        miRigidbody2D = GetComponent<Rigidbody2D>();
        miAnimator = GetComponent<Animator>();
        miSprite = GetComponent<SpriteRenderer>();

        minRapidez = rapidezMinima;                 // se transfiere el valor seteado de rapidez minima a la variables usada
        dragInicial = miRigidbody2D.drag;           // el drag inicial toma el valor del seteo original

        if (jugador == null)                                                //en caso que no se haya asignado desde el inspector un transform jugador al prefab
        {
            GameObject jugadorObject = GameObject.FindWithTag("Player");    //toma el gameObject de Player, para obtener su transform
            if (jugadorObject != null)
            {
                jugador = jugadorObject.transform;
            }
        }
    }

    private void Update()
    {
        rapidez = miRigidbody2D.velocity.magnitude;
        
        if(contador != 0)                           // si ocurrió una colisión (contador sumó 1, ver abajo)
        {
            contador++;                             // empieza a correr la espera
            if (contador > espera)                  // si supera la espera, vuelve a su situación original
            {
                miRigidbody2D.drag = dragInicial;       //drag recupera su valor original
                minRapidez = rapidezMinima;             //igual que la rapidez mínima
                contador = 0;                           //se reinicializa el contador hasta una nueva colisión
            }
        }

        // en caso que el jugador no haya explotado y aun esté activo, lo buscará
        if (jugador.GetComponent<SpriteRenderer>().enabled)
        {
            // en cualquier caso se posiciona buscando al auto del jugador
            direccion = (jugador.position - new Vector3(-0.5f, 0, 0) - transform.position).normalized;  // busca al auto del jugador, ligeramente corrido en x
            angulo = Mathf.Atan2(-1 * direccion.x, direccion.y);          // calcula el ángulo con la arcotangente, para girar el auto
            transform.eulerAngles = new Vector3(0, 0, angulo / Mathf.Deg2Rad);   //gira el auto ese ángulo
            miRigidbody2D.velocity = new Vector2(-1 * rapidez * Mathf.Sin(angulo), rapidez * Mathf.Cos(angulo)); // recalcula el vector velocidad

            if (rapidez < minRapidez)                       // sólo si la rapidez llegó al mínimo activa la aceleración
            {
                acelerar = true;
            }
        }

        // definición de la condición para la única transición de las animaciones
        miAnimator.SetFloat("Rapidez", rapidez);
    }

    private void FixedUpdate()
    {
        if (acelerar)
        {
            miRigidbody2D.AddForce(direccion * aceleracion);       // aplica fuerza
            acelerar = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // si hay colisión con cualquier objeto
            contador++;                                     // inicia el contador de espera
            miRigidbody2D.drag = 5f;                        // frena el auto
            minRapidez = 1f;                                // baja la rapidez mínima, para que no acelere
    }                                           
}
