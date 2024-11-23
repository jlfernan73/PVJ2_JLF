using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// clase para el jefe final del nivel 1 que desactiva la barrera para pasar 
public class JefeNivel1 : MonoBehaviour
{
    [SerializeField] Transform barrera;                         //se lo incluye para alivianar la barrera que impide llegar a la meta

    protected void Update()
    {
        if (!gameObject.GetComponent<MoverEnemigo_persigue>().GetVive())
        {
            barrera.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;      //se hace la valla dinámica poder moverla
            barrera.GetComponent<Rigidbody2D>().mass = 1.0f;                            //se aliviana la valla para poder pasar a la meta
        }
    }
}
