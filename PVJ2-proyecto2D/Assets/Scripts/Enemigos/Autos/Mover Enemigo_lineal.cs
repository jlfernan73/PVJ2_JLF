using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// clase para que un tipo de enemigo se mueva linealmente un cierto tramo, y luego gire
// para ello acelera, y espera hasta que la velocidad baje a un cierto valor, y vuelve a acelerar
// (hereda de AutoEnemigo)

public class MoverEnemigo_lineal : AutoEnemigo
{
    [Header("Configuracion")]
    [SerializeField] float maxImpulso = 3;          // número de veces que repite la aceleración antes de girar 90 grados

    private float deltaAngulo;                  // usado a modo de contador del angulo de giro
    private int impulso;                        // contador del número de impulsos
    private bool girar;                     // bandera para activar el giro

    protected override void Inicializar()
    {
        deltaAngulo = 0;
        impulso = 0;
        girar = false;
    }

    protected override void Mover()
    {
        rapidez = miRigidbody2D.velocity.magnitude;         // se obtiene el módulo de la velocidad
        if (rapidez < minRapidez && !girar)                 // si la rapidez llegó al mínimo y no está girando
        {
            acelerar = true;                                // activa el impulso
        }
        if (impulso > maxImpulso)                            // pero si ya se superó el número de máximos impulsos
        {
            impulso = 0;                                    // se activa el giro y se reinicia
            girar = true;
            deltaAngulo = 0;
        }
        if (girar)                                          // si el giro está activado
        {
            if (deltaAngulo < 90)                            // si el ángulo de giro aun no alcanzó los 90 grados
            {
                transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + 2);     //se suman 2 grados al ángulo del auto
                deltaAngulo += 2;                                                             // y también al ángulo de giro
            }
            else
            {
                girar = false;                              // si alcanzó los 90 grados, se desactiva el giro
            }
        }
        direccion = transform.up.normalized;                // se lee la dirección en que quedó el auto
        miRigidbody2D.velocity = new Vector2(rapidez * direccion.x, rapidez * direccion.y); // se recalcula el vector velocidad

        // definición de las condiciones para las transiciones de las animaciones
        miAnimator.SetFloat("Rapidez", rapidez);
        miAnimator.SetBool("Girar", girar);
    }
    protected override void Acelerar()
    {
        if (acelerar && !girar)                             // si tiene que impulsarse
        {
            impulso++;                                          //aplica la fuerza en la dirección del auto
            miRigidbody2D.AddForce(direccion * aceleracion);
            acelerar = false;                                   //desactiva el impulso
        }
    }
}
