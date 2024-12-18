using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// clase para que un tipo de enemigo se mueva linealmente un cierto tramo, y luego gire
// para ello acelera, y espera hasta que la velocidad baje a un cierto valor, y vuelve a acelerar
// (hereda de AutoEnemigo)

public class MoverEnemigo_lineal : AutoEnemigo
{
    [Header("Configuracion")]
    [SerializeField] float maxImpulso = 3;          // n�mero de veces que repite la aceleraci�n antes de girar 90 grados

    private float deltaAngulo;                  // usado a modo de contador del angulo de giro
    private int impulso;                        // contador del n�mero de impulsos
    private bool girar;                     // bandera para activar el giro

    protected override void Inicializar()
    {
        deltaAngulo = 0;
        impulso = 0;
        girar = false;
    }

    protected override void Mover()
    {
        rapidez = miRigidbody2D.velocity.magnitude;         // se obtiene el m�dulo de la velocidad
        if (rapidez < minRapidez && !girar)                 // si la rapidez lleg� al m�nimo y no est� girando
        {
            acelerar = true;                                // activa el impulso
        }
        if (impulso > maxImpulso)                            // pero si ya se super� el n�mero de m�ximos impulsos
        {
            impulso = 0;                                    // se activa el giro y se reinicia
            girar = true;
            deltaAngulo = 0;
        }
        if (girar)                                          // si el giro est� activado
        {
            if (deltaAngulo < 90)                            // si el �ngulo de giro aun no alcanz� los 90 grados
            {
                transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + 2);     //se suman 2 grados al �ngulo del auto
                deltaAngulo += 2;                                                             // y tambi�n al �ngulo de giro
            }
            else
            {
                girar = false;                              // si alcanz� los 90 grados, se desactiva el giro
            }
        }
        direccion = transform.up.normalized;                // se lee la direcci�n en que qued� el auto
        miRigidbody2D.velocity = new Vector2(rapidez * direccion.x, rapidez * direccion.y); // se recalcula el vector velocidad

        // definici�n de las condiciones para las transiciones de las animaciones
        miAnimator.SetFloat("Rapidez", rapidez);
        miAnimator.SetBool("Girar", girar);
    }
    protected override void Acelerar()
    {
        if (acelerar && !girar)                             // si tiene que impulsarse
        {
            impulso++;                                          //aplica la fuerza en la direcci�n del auto
            miRigidbody2D.AddForce(direccion * aceleracion);
            acelerar = false;                                   //desactiva el impulso
        }
    }
}
