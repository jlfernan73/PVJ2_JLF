using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disparo : MonoBehaviour
{
    [SerializeField] float aceleracion = 100;
    Transform grua;
    private Rigidbody2D miRigidbody2D;

    void Start()
    {
        miRigidbody2D = GetComponent<Rigidbody2D>();
        GameObject gruaObject = GameObject.FindWithTag("Grua");
        if (gruaObject != null)
        {
            grua = gruaObject.transform;
        }
        miRigidbody2D.AddForce(grua.up.normalized * aceleracion);    // aplica fuerza en la dirección de la grua
    }

    void Update()
    {
        
    }
}
