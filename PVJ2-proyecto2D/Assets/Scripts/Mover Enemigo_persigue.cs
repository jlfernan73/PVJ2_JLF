using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverEnemigo_persigue : MonoBehaviour
{
    // Variables a configurar desde el editor
    [Header("Configuracion")]
    [SerializeField] float aceleracion = 60f;
    [SerializeField] float rapidezMinima;
    [SerializeField] float espera = 500;

    // Referencia al transform del jugador serializada
    [SerializeField] Transform jugador;

    // Variables de uso interno en el script
    private Vector2 direccion;
    private float rapidez = 0;
    private float minRapidez;
    private float angulo;
    private bool acelerar = true;
    private int contador = 0;

    // Variable para referenciar otro componente del objeto
    private Rigidbody2D miRigidbody2D;

    // Codigo ejecutado cuando el objeto se activa en el nivel
    private void Awake()
    {
        miRigidbody2D = GetComponent<Rigidbody2D>();
        minRapidez = rapidezMinima;
    }

    // Codigo ejecutado en cada frame del juego (Intervalo variable)
    private void Update()
    {
        rapidez = miRigidbody2D.velocity.magnitude;
        if(contador != 0)
        {
            contador++;
            if (contador > espera)
            {
                miRigidbody2D.drag = 1f;
                minRapidez = rapidezMinima;
                contador = 0;
            }
        }
        direccion = (jugador.position - new Vector3(-1f, 0, 0) - transform.position).normalized;
        angulo = Mathf.Atan2(-1*direccion.x, direccion.y);
        transform.eulerAngles = new Vector3(0, 0, angulo/ Mathf.Deg2Rad);
        if (rapidez < minRapidez)
        {
            acelerar = true;
        }
        //direccion = new Vector2(-1 * Mathf.Sin(angulo), Mathf.Cos(angulo));
        miRigidbody2D.velocity = new Vector2(-1 * rapidez * Mathf.Sin(angulo), rapidez * Mathf.Cos(angulo));
    }
    private void FixedUpdate()
    {
        if (acelerar)
        {
            miRigidbody2D.AddForce(direccion * aceleracion);
            acelerar = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            contador++;
            miRigidbody2D.drag = 5f;
            minRapidez = 1f;
        }
    }
}
