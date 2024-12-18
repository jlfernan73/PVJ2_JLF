using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// clase para que un tipo de enemigo busque al auto del jugador y acelere hacia �l
// si el auto choca, esperar� un per�odo de tiempo antes de volver a acelerar
// (hereda de AutoEnemigo)

public class MoverEnemigo_persigue : AutoEnemigo
{
    [Header("Configuracion")]
    [SerializeField] float espera = 500;                    // ciclos que el auto esperar� antes de volver a acelerar
    // Referencia al transform del jugador serializada
    [SerializeField] Transform jugador;                     // toma la referencia del jugador

    private float rapidezMinima;                       // variable usada para cambiar la rapidez m�nima          
    private float angulo;                           // angulo usado para corregir la velocidad al girar
    private float dragInicial;                      // valor del drag lineal en condiciones normales
    private int contador;                       // contador que empieza a sumar cuando colisiona, para comparar con la espera 

    protected override void Inicializar()
    {
        rapidezMinima = minRapidez;                 // se transfiere el valor seteado de minRapidez a la variables usada
        dragInicial = miRigidbody2D.drag;           // el drag inicial toma el valor del seteo original
        contador = 0;

        if (jugador == null)                                                //en caso que no se haya asignado desde el inspector un transform jugador al prefab
        {
            GameObject jugadorObject = GameObject.FindWithTag("Player");    //toma el gameObject de Player, para obtener su transform
            if (jugadorObject != null)
            {
                jugador = jugadorObject.transform;
            }
        }
    }

    protected override void Mover()
    {
        rapidez = miRigidbody2D.velocity.magnitude;

        if (contador != 0)                           // si ocurri� una colisi�n (contador sum� 1, ver abajo)
        {
            contador++;                             // empieza a correr la espera
            if (contador > espera)                  // si supera la espera, vuelve a su situaci�n original
            {
                miRigidbody2D.drag = dragInicial;       //drag recupera su valor original
                rapidezMinima = minRapidez;             //igual que la rapidez m�nima
                contador = 0;                           //se reinicializa el contador hasta una nueva colisi�n
            }
        }

        // en caso que el jugador no haya explotado y aun est� activo, lo buscar�
        if (jugador.GetComponent<Collider2D>().enabled)
        {
            // en cualquier caso se posiciona buscando al auto del jugador
            direccion = (jugador.position - new Vector3(-0.5f, 0, 0) - transform.position).normalized;  // busca al auto del jugador, ligeramente corrido en x
            angulo = Mathf.Atan2(-1 * direccion.x, direccion.y);          // calcula el �ngulo con la arcotangente, para girar el auto
            transform.eulerAngles = new Vector3(0, 0, angulo / Mathf.Deg2Rad);   //gira el auto ese �ngulo
            miRigidbody2D.velocity = new Vector2(-1 * rapidez * Mathf.Sin(angulo), rapidez * Mathf.Cos(angulo)); // recalcula el vector velocidad

            if (rapidez < rapidezMinima)                       // s�lo si la rapidez lleg� al m�nimo activa la aceleraci�n
            {
                acelerar = true;
            }
        }

        // definici�n de la condici�n para la �nica transici�n de las animaciones
        miAnimator.SetFloat("Rapidez", rapidez);

    }
    protected override void Acelerar()
    {
        if (acelerar)
        {
            miRigidbody2D.AddForce(direccion * aceleracion);       // aplica fuerza
            acelerar = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // si hay colisi�n con cualquier objeto
            contador++;                                     // inicia el contador de espera
            miRigidbody2D.drag = 5f;                        // frena el auto
            rapidezMinima = 1f;                                // baja la rapidez m�nima, para que no acelere
    }                                           
}
