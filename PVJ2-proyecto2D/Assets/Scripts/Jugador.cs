using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// clase Jugador con atributos modificables (energía, ítems) y métodos respectivos

public class Jugador : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private float energia = 100f;      //energía del jugador
    [SerializeField] private int items = 0;             //ítems colectados
    [SerializeField] private ParticleSystem particleSystemCrash;
    [SerializeField] private ParticleSystem particleSystemHumo;
    bool humeando = false;

    private void Update()
    {
        particleSystemHumo.transform.position = gameObject.transform.position;
    }


    public void ModificarEnergia(float puntos)      // método público para modificar la energía
    {                                               // sin superar 100 ni bajar de 0
        energia += puntos;
        if (energia > 100)
        {
            energia = 100;
        }
        if (energia < 0)
        {
            energia = 0;
        }
        if (energia < 20 && !humeando)
        {
            humeando = true;
            particleSystemHumo.Play();
        }
        if (energia >= 20 && humeando)
        {
            humeando = false;
            particleSystemHumo.Stop();
        }
    }

    public void AgregarItem()                   // método público para agregar un ítem (usado para los diamantes)
    {
        items++;
    }

    public void Colision()
    {
        Vector3 posicion = gameObject.transform.position;
        particleSystemCrash.transform.position = posicion;
        particleSystemCrash.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)             //Método para chequear la llegada a Meta
    {
        if (!collision.gameObject.CompareTag("Meta")){return;}
        Debug.Log("LLEGASTE A LA META, GANASTE!!");
    }
}
