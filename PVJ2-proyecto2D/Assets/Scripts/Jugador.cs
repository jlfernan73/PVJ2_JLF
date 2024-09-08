using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// clase Jugador con atributos modificables (energía, ítems) y métodos respectivos

public class Jugador : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private float energia = 100f;      //energía del jugador
    [SerializeField] private int items = 0;             //ítems colectados

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
    }

    public void AgregarItem()                   // método público para agregar un ítem (usado para los diamantes)
    {
        items++;
    }

    private void OnTriggerEnter2D(Collider2D collision)             //Método para chequear la llegada a Meta
    {
        if (!collision.gameObject.CompareTag("Meta")){return;}
        Debug.Log("LLEGASTE A LA META, GANASTE!!");
    }
}
