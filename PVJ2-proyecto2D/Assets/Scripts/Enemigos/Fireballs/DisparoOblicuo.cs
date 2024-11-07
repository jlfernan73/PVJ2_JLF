using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// clase que mueve de manera recta la fireball al ser instanciada, por aplicación de una fuerza sobre esta
// en la dirección de un ángulo de rotación aleatorio
// (hereda de Fireball)

public class DisparoOblicuo : Fireball
{
    protected override void Disparar()
    {
        transform.Rotate(new Vector3(0, 0, angulo));                       //rota el firewall un angulo aleatorio
        miRigidbody2D.AddForce(transform.up.normalized * aceleracion);    // aplica fuerza en la dirección aleatoria resultante
    }
}
