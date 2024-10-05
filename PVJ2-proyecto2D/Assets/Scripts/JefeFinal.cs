using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JefeFinal : MonoBehaviour
{
    [SerializeField] float tiempoEntreEmbestidas;
    [SerializeField] float tiempoEntreRotaciones;
    [SerializeField] float tiempoEntreDisparos;

    [SerializeField] Transform bolaGrua;
    [SerializeField] Transform baseGrua;

    [SerializeField] GameObject prefabProyectil;

    [SerializeField] Transform jugador;

    private Renderer grua;
    private float tiempoActualEspera;
    private int estadoActual;

    private Vector3 direccionGrua;
    private float deltaPosBase = 0.0f;
    private float deltaPosBola = 7.0f;

    // Estados del jefe
    private const int Embestir = 0;
    private const int Rotar = 1;
    private const int DispararProyectil = 2;

    void Start()
    {
        baseGrua.position = new Vector2(transform.position.x + deltaPosBase, transform.position.y + deltaPosBase);
        bolaGrua.position = transform.position + transform.up.normalized * deltaPosBola;
        grua = GetComponent<Renderer>();
        estadoActual = Embestir;
        StartCoroutine(ComportamientoJefe());
    }

    private IEnumerator ComportamientoJefe()
    {
        while (true)
        {
            switch (estadoActual)
            {
                case Embestir:
                    StartCoroutine(Embestida());
                    tiempoActualEspera = tiempoEntreEmbestidas;
                    break;
                case Rotar:
                    StartCoroutine(Rotacion());
                    tiempoActualEspera = tiempoEntreRotaciones;
                    break;
                case DispararProyectil:
                    StartCoroutine(Disparar());
                    tiempoActualEspera = tiempoEntreDisparos;
                    break;
            }
            Debug.Log(estadoActual);
            baseGrua.up = transform.up;
            yield return new WaitForSeconds(tiempoActualEspera);
            bolaGrua.GetComponent<SpriteRenderer>().enabled = true;
            bolaGrua.GetComponent<Collider2D>().enabled = true;
            ActualizarEstado();
        }
    }

    private IEnumerator Disparar()
    {
        //para apuntar hacia el jugador
        Vector3 dirGrua = transform.up.normalized;
        Vector3 dirJugador = (jugador.position - transform.position).normalized;
        float anguloMax = Vector2.Angle(dirGrua, dirJugador);
        float angulo = Mathf.Sign(Vector3.Cross(dirGrua, dirJugador).z);
        int direccionRotacion = (int)(angulo / Mathf.Abs(angulo));
        float tiempoMovimiento = 2f * anguloMax / 180;
        float velocidadAngular = anguloMax / (tiempoMovimiento / 2);

        // Rotar hacia la posición objetivo
        float tiempoInicio = Time.time;
        while (Time.time < tiempoInicio + tiempoMovimiento / 2)
        {
            transform.Rotate(0, 0, direccionRotacion * velocidadAngular * Time.deltaTime);
            bolaGrua.position = transform.position + transform.up.normalized * deltaPosBola;
            yield return null;
        }

        // Dispara
        yield return new WaitForSeconds(0.5f);
        bolaGrua.GetComponent<SpriteRenderer>().enabled = false;
        bolaGrua.GetComponent<Collider2D>().enabled = false;
        Instantiate(prefabProyectil, bolaGrua.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);

        // volver hacia la posición inicial
        tiempoInicio = Time.time;
        direccionRotacion *= -1;
        while (Time.time <= tiempoInicio + tiempoMovimiento/2)
        {
            transform.Rotate(0, 0, direccionRotacion * velocidadAngular * Time.deltaTime);
            bolaGrua.position = transform.position + transform.up.normalized * deltaPosBola;
            yield return null;
        }
        baseGrua.up = transform.up;
    }

    private IEnumerator Embestida()
    {
        float tiempoEmbestida = 1.2f;       // Asociado a la velocidad de la embestida
        float tiempoInicio = Time.time;
        float distanciaEmbestida = 15f;    // Ajusta la distancia de la embestida

        Vector2 posicionInicial = transform.position;
        Vector2 posicionObjetivo = transform.position + distanciaEmbestida * transform.up.normalized; 

        // Mover hacia adelante
        while (Time.time < tiempoInicio + tiempoEmbestida / 2)
        {
            transform.position = Vector2.Lerp(posicionInicial, posicionObjetivo, (Time.time - tiempoInicio) / (tiempoEmbestida / 2));
            baseGrua.position = transform.position + transform.up.normalized * deltaPosBase;
            bolaGrua.position = transform.position + transform.up.normalized * deltaPosBola;
            yield return null;
        }
        // Mover hacia atrás (retroceso)
        tiempoInicio = Time.time;
        while (Time.time < tiempoInicio + tiempoEmbestida / 2)
        {
            transform.position = Vector2.Lerp(posicionObjetivo, posicionInicial, (Time.time - tiempoInicio) / (tiempoEmbestida / 2));
            baseGrua.position = transform.position + transform.up.normalized * deltaPosBase;
            bolaGrua.position = transform.position + transform.up.normalized * deltaPosBola;
            yield return null;
        }
    }

    private IEnumerator Rotacion()
    {
        float tiempoMovimiento = 1.5f; // Ajusta la duración del movimiento (en s)
        float tiempoInicio = Time.time;
        float anguloMaximo = 180.0f;
        float velocidadAngular = anguloMaximo / (tiempoMovimiento / 2);

        //para determinar la direccion de rotación busca al auto del jugador
        Vector3 dirGrua = transform.up.normalized;
        Vector3 dirJugador = (jugador.position - transform.position).normalized;
        float angulo = Mathf.Sign(Vector3.Cross(dirGrua, dirJugador).z);
        int direccionRotacion = (int)(angulo / Mathf.Abs(angulo));

        // Rotar hacia la posición objetivo
        while (Time.time < tiempoInicio + tiempoMovimiento / 2)
        {
            transform.Rotate(0, 0, direccionRotacion*velocidadAngular*Time.deltaTime);
            bolaGrua.position = transform.position + transform.up.normalized * deltaPosBola;
            yield return null;
        }

        // volver hacia la posición inicial
        //tiempoInicio = Time.time;
        direccionRotacion *= -1;
        while (Time.time <= tiempoInicio + tiempoMovimiento)
        {
            transform.Rotate(0, 0, direccionRotacion * velocidadAngular * Time.deltaTime);
            bolaGrua.position = transform.position + transform.up.normalized * deltaPosBola;
            yield return null;
        }
        baseGrua.up = transform.up;
    }

    private void ActualizarEstado()
    {
        // Actualiza el estado actual según las probabilidades y condiciones que desees
        // Puedes usar Random.Range para generar números aleatorios y decidir el siguiente estado
        if (grua.isVisible)
        {
            estadoActual = Random.Range(0, 3);
        }
        else
        {
            estadoActual = Random.Range(0, 2);
        }
    }
}