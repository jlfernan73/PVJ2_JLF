using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase madre abstracta de la que heredan las fireballs

public abstract class Fireball : MonoBehaviour
{
    [SerializeField] protected float aceleracion = 30000;       //fuerza a aplicar
    protected ParticleSystem particleSystemChispas;        //chispazos al colisionar
    protected Rigidbody2D miRigidbody2D;              //rigidBody del objeto donde se aplicará la fuerza
    protected bool colision;
    protected AudioSource audioColision;
    protected float angulo;
    protected float lifetime;

    protected void OnEnable()
    {
        colision = false;
        lifetime = 0;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        audioColision = GetComponent<AudioSource>();
        miRigidbody2D = GetComponent<Rigidbody2D>();
        GameObject particleObject = GameObject.Find("CrashFireball");
        particleSystemChispas = particleObject.GetComponent<ParticleSystem>();
        angulo = Random.Range(-45f, 45f); 
        Disparar();
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        colision = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        particleSystemChispas.transform.position = gameObject.transform.position;      // se posiciona el sistema de partículas donde está el fireball
        particleSystemChispas.Play();                             // se activa el sistema de partículas de chispas
    }

    protected void Update()
    {
        Actualizar();
    }
    protected void Actualizar()
    {
        lifetime+=Time.deltaTime;
        if ((colision && !audioColision.isPlaying)||lifetime > 10f)
        {
            gameObject.SetActive(false);
        }
    }
    protected abstract void Disparar();
}
