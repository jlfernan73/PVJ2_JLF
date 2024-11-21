using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaludJefe : AutoEnemigo
{
    [SerializeField] GameObject bolaGrua;
    [SerializeField] GameObject cabinaGrua;
    [SerializeField] Transform barrera;                         //se lo incluye para alivianar la barrera que impide llegar a la meta


    private void Update()
    {
        if (!vive)
        {
            if (bolaGrua.activeSelf)
            {
                bolaGrua.SetActive(false);
                cabinaGrua.SetActive(false);
            }
            tiempoMuerto += Time.deltaTime;
            if (tiempoMuerto > 3f)
            {
                GetComponent<Collider2D>().enabled = true;
                gameObject.SetActive(false);
                particlesCrashEnemigo.SetActive(false);
                particlesHumo.SetActive(false);

                barrera.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;      //se hace la valla dinámica poder moverla
                barrera.GetComponent<Rigidbody2D>().mass = 1.0f;                            //se aliviana la valla para poder pasar a la meta
            }
        }
    }

    protected override void Inicializar()
    {
    }

    protected override void Mover()
    {
    }
    protected override void Acelerar()
    {
    }
}
