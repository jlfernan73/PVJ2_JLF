using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// clase que mueve la fireball al ser instanciada, por aplicaci�n de una fuerza sobre esta

public class DisparoFireball : MonoBehaviour
{
    [SerializeField] float aceleracion = 30000;       //fuerza a aplicar
    private ParticleSystem particleSystemChispas;        //chispazos al colisionar
    private Rigidbody2D miRigidbody2D;              //rigidBody del objeto donde se aplicar� la fuerza
    bool colision = false;
    private AudioSource audioColision;

    private void Awake()
    {
    }

    void OnEnable()
    {
        colision = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        audioColision = GetComponent<AudioSource>();
        miRigidbody2D = GetComponent<Rigidbody2D>();
        miRigidbody2D.AddForce(transform.up.normalized * aceleracion);    // aplica fuerza en la direcci�n en que fue instanciado
        GameObject particleObject = GameObject.Find("CrashFireball");
        particleSystemChispas = particleObject.GetComponent<ParticleSystem>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        colision = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        particleSystemChispas.transform.position = gameObject.transform.position;      // se posiciona el sistema de part�culas donde est� el fireball
        particleSystemChispas.Play();                             // se activa el sistema de part�culas de chispas
    }

    private void Update()
    {
        if (colision && !audioColision.isPlaying)
        {
            gameObject.SetActive(false);
        }
    }

}
