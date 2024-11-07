using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// clase que mueve de manera recta la fireball al ser instanciada, por aplicaci�n de una fuerza sobre esta
// en la direcci�n de un �ngulo de rotaci�n aleatorio
// (hereda de Fireball)

public class DisparoOblicuo : Fireball
{
    protected override void Disparar()
    {
        transform.Rotate(new Vector3(0, 0, angulo));                       //rota el firewall un angulo aleatorio
        miRigidbody2D.AddForce(transform.up.normalized * aceleracion);    // aplica fuerza en la direcci�n aleatoria resultante
    }
}
