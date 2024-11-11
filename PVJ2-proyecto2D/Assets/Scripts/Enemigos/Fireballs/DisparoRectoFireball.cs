using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// clase que mueve de manera recta la fireball al ser instanciada, por aplicaci�n de una fuerza sobre esta
// (hereda de Fireball)

public class DisparoFireball : Fireball
{
    protected override void Disparar()
    {
        miRigidbody2D.AddForce(transform.up.normalized * aceleracion);    // aplica fuerza en la direcci�n en que fue instanciado
    }
}
