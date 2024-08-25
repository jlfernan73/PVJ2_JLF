using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverEnemigo_lineal : MonoBehaviour
{
    // Variables a configurar desde el editor
    [Header("Configuracion")]
    [SerializeField] float aceleracion = 40f;
    [SerializeField] float angulo;
    [SerializeField] float minRapidez = 2;

    // Variables de uso interno en el script
    private Vector2 direccion;
    private float rapidez = 0;
    private float deltaAngulo = 0;
    private int impulso = 0;
    private bool acelerar = true;
    private bool girar = false;

    // Variable para referenciar otro componente del objeto
    private Rigidbody2D miRigidbody2D;

    // Codigo ejecutado cuando el objeto se activa en el nivel
    private void OnEnable()
    {
        miRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Codigo ejecutado en cada frame del juego (Intervalo variable)
    private void Update()
    {
        rapidez = miRigidbody2D.velocity.magnitude;
        angulo = transform.eulerAngles.z * Mathf.Deg2Rad;
        if (rapidez < minRapidez && !girar)
        {
            acelerar = true;
        }
        if(impulso > 3)
        {
            impulso = 0;
            girar = true;
            deltaAngulo = 0;
        }
        if (girar)
        {
            if(deltaAngulo < 90)
            {
                transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + 1);
                deltaAngulo++;
            }
            else
            {
                girar = false;
            }
        }
        direccion = new Vector2(-1 * Mathf.Sin(angulo), Mathf.Cos(angulo));
        miRigidbody2D.velocity = new Vector2(-1 * rapidez * Mathf.Sin(angulo), rapidez * Mathf.Cos(angulo));
    }
    private void FixedUpdate()
    {
        if (acelerar && !girar)
        {
            impulso++;
            miRigidbody2D.AddForce(direccion * aceleracion);
            acelerar = false;
        }
    }
}
