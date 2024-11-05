using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase madre abstracta de la que heredan los autos enemigos
public abstract class AutoEnemigo : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] protected float aceleracion = 40f;       // m�dulo de la fuerza de aceleraci�n
    [SerializeField] protected float minRapidez = 20;         // valor al que baja la velocidad para volver a acelerar

    protected Vector2 direccion;                      // direcci�n de avance del auto
    protected float rapidez;                          // m�dulo de la velocidad
    protected bool acelerar = true;                   // bandera para activar el impulso

    protected Rigidbody2D miRigidbody2D;
    protected Animator miAnimator;
    protected SpriteRenderer miSprite;

    protected void OnEnable()
    {
        miRigidbody2D = GetComponent<Rigidbody2D>();
        miAnimator = GetComponent<Animator>();
        miSprite = GetComponent<SpriteRenderer>();
        Inicializar();                                  //inicializa los valores de las variables propias de cada tipo de auto
    }

    private void Update()
    {
        Mover();
    }

    private void FixedUpdate()
    {
        Acelerar();
    }

    protected abstract void Inicializar();
    protected abstract void Mover();
    protected abstract void Acelerar();
}
