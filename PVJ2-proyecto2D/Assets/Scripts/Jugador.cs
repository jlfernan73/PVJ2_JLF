using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private float energia = 100f;
    [SerializeField] private int items = 0;

    public void ModificarEnergia(float puntos)
    {
        energia += puntos;
        if (energia > 100)
        {
            energia = 100;
        }
        if (energia < 0)
        {
            energia = 0;
        }
        Debug.Log(EstasVivo());
    }

    public void AgregarItem()
    {
        items++;
    }

    private bool EstasVivo()
    {
        return energia > 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Meta")) { return; }

        Debug.Log("GANASTE");
    }
}
