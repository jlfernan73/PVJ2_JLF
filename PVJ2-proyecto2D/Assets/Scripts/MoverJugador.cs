using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.RuleTile.TilingRuleOutput;

// clase para mover el automovil del jugador al presionar las cuatro teclas de desplazamiento
// el auto acelera y frena con las flechas up/down (o W/S), y gira con las flechas laterales (o A/D)
// el control es complejo para una sola mano, por eso conviene combinar (por ej. flechas laterales y W/S)

public class MoverJugador : MonoBehaviour
{
    // Variables a configurar desde el editor
    [Header("Configuracion")]
    [SerializeField] float aceleracion = 10f;       // modulo de la fuerza de arranque
    [SerializeField] float freno = 3f;              // drag lineal que se aplica al frenar
    [SerializeField] float maxAngulo = 5.0f;        // máximo angulo de giro del auto al doblar
    [SerializeField] float maxRapidez = 20f;        // velocidad máxima que alcanza el auto al acelerar varias veces

    // Variables de uso interno en el script
    private float girar;                            // adquiere valores al presionar las flechas laterals (o A/D) para frenar
    private float mover;                            // adquiere valores al presionar las flecha up/down (o W/S) para acelerar
    private Vector2 direccion = new Vector2(0, 1);  // vector 2D con la direccion del automovil
    private float rapidez = 0;                      // modulo del vector velocidad
    private float angulo = 0;                       // angulo de giro de las ruedas del automovil (respecto al eje del auto)
    private float deltaAngulo = 0.05f;               // agregado de ángulo de las ruedas que se suma al angulo de giro
    private float dragInicial;                      // valor del drag lineal en condiciones normales
    private bool frenando = false;                  // bandera para activación del frenado
    private bool girando = false;                   // bandera para activacion del giro

    // Variable para referenciar otro componente del objeto
    private Rigidbody2D miRigidbody2D;

    // Codigo ejecutado cuando el objeto se activa en el nivel
    private void OnEnable()
    {
        miRigidbody2D = GetComponent<Rigidbody2D>();
        dragInicial = miRigidbody2D.drag;           // el drag inicial toma el valor del seteo original
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
            transform.Rotate(0, 0, angulo);                                             // se rota el auto
            direccion = transform.up.normalized;                                        // se lee la nueva dirección (normalizada) del auto
            miRigidbody2D.velocity = direccion * rapidez;                               // se recalcula el vector velocidad con la nueva dirección
        }

        if (mover < 0)                              // si frena, se incrementará momentaneamente el drag
        {
            miRigidbody2D.drag = freno;
            frenando = true;
        }
        else
        {
            if (frenando)                           // si estaba frenando pero se soltó la tecla
            {
                miRigidbody2D.drag = dragInicial;   // se vuelve a las condiciones iniciales de avance
                frenando = false;
            }
        }
    }
    private void FixedUpdate()
    {
        if (mover > 0 && rapidez < maxRapidez)                  // si se presiona la tecla para acelerar 
        {
            miRigidbody2D.AddForce(direccion * aceleracion);    // aplica fuerza en la dirección del auto
        }
    }
}