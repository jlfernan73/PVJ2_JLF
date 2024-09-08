using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// clase Jugador con atributos modificables (energ�a, �tems) y m�todos respectivos

public class Jugador : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private float energia = 100f;      //energ�a del jugador
    [SerializeField] private int items = 0;             //�tems colectados

    public void ModificarEnergia(float puntos)      // m�todo p�blico para modificar la energ�a
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

    public void AgregarItem()                   // m�todo p�blico para agregar un �tem (usado para los diamantes)
    {
        items++;
    }

    private void OnTriggerEnter2D(Collider2D collision)             //M�todo para chequear la llegada a Meta
    {
        if (!collision.gameObject.CompareTag("Meta")){return;}
        Debug.Log("LLEGASTE A LA META, GANASTE!!");
    }
}
