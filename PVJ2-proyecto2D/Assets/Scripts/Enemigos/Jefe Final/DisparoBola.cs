using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// clase que mueve el proyectil al ser instanciado, por aplicación de una fuerza sobre este

public class Disparo : MonoBehaviour
{
    [SerializeField] float aceleracion = 100;       //fuerza a aplicar
    private Rigidbody2D miRigidbody2D;              //rigidBody del objeto donde se aplicará la fuerza

    void Start()
    {
        miRigidbody2D = GetComponent<Rigidbody2D>();
        GameObject gruaObject = GameObject.FindWithTag("Grua");     //se lee el gameObject de la grua, para obtener su dirección  
        if (gruaObject != null)
        {
            miRigidbody2D.AddForce(gruaObject.transform.up.normalized * aceleracion);    // aplica fuerza en la dirección de la grua
        }
    }
}
