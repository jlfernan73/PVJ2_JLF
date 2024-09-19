using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;

// clase Jugador con atributos modificables (energía, ítems) y métodos respectivos

public class Jugador : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private float energia = 100f;      //energía del jugador
    [SerializeField] private int items = 0;             //ítems colectados
    [SerializeField] private ParticleSystem particleSystemCrash;
    [SerializeField] private ParticleSystem particleSystemHumo;
    [SerializeField] private ParticleSystem particleSystemExplosion;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform barrera;
    [SerializeField] private Transform musicaFondo;
    [SerializeField] private Transform musicaMeta;
    //private Transform player; 
    bool humeando = false;
    bool vive = true;
    bool meta = false;

    private void OnEnable()
    {
    }

    private void Update()
    {
        particleSystemHumo.transform.position = gameObject.transform.position;
        particleSystemExplosion.transform.position = gameObject.transform.position;
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
        if (energia < 25 && !humeando)
        {
            humeando = true;
            particleSystemHumo.Play();
        }
        if ((energia >= 25 || energia <=0) && humeando)
        {
            humeando = false;
            particleSystemHumo.Stop();
        }
    }

    public void AgregarItem()                   // método público para agregar un ítem (usado para los diamantes)
    {
        items++;
        if (items == 200) 
        {
            barrera.GetComponent<Rigidbody2D>().mass = 1f;
        }
    }

    public float GetEnergia()
    {
        return energia;
    }
    public bool EstaVivo()
    {
        return vive;
    }
    public void JugadorMuere()
    {
        vive = false;
    }
    public bool AlcanzoMeta()
    {
        return meta;
    }

    public void Colision()
    {
        Vector3 posicion = gameObject.transform.position;
        particleSystemCrash.transform.position = posicion;
        particleSystemCrash.Play();
    }
    public void OnExplosion()
    {
        particleSystemHumo.Stop();
        particleSystemExplosion.Play();
        Collider2D collider2D = GetComponent<Collider2D>();
        collider2D.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)             //Método para chequear la llegada a Meta
    {
        if (!collision.gameObject.CompareTag("Meta")){return;}
        Debug.Log("LLEGASTE A LA META, GANASTE!!");
        meta = true;
        if (virtualCamera.Follow)
        {
            virtualCamera.Follow = null;
        }
        particleSystemHumo.Stop();
        musicaFondo.GetComponent<AudioSource>().Stop();
        musicaMeta.GetComponent<AudioSource>().Play();
    }

}
