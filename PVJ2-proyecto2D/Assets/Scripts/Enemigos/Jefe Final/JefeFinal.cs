using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase que controla el comportamiento del jefe final mediante tres corrutinas ejecutadas eleatoriamente 
// Los tres posibles comportamientos son: 1- Embestida, 2- Avance y rotación, 3- Avanzar, apuntar y disparar

public class JefeFinal : MonoBehaviour
{
    [SerializeField] float tiempoEntreEmbestidas;
    [SerializeField] float tiempoEntreRotaciones;
    [SerializeField] float tiempoEntreDisparos;
    
    //dado que el script funciona sobre la grua, la misma debe tener acceso a la base y a la bola
    [SerializeField] Transform bolaGrua;
    [SerializeField] Transform baseGrua;

    //la bola se que cuelga de la grua se transforma en un proyectil (prefab) que es disparado
    //el prefab tiene un script que controla las características del disparo
    [SerializeField] GameObject prefabProyectil;

    //dado que debe apuntar al jugador, la grua debe tener acceso al transform del jugador
    [SerializeField] GameObject jugador;

    // se accede al renderer de la grua, para identificar cuando esta sea visible en la pantalla
    // ya que sólo efectuará disparos si la misma está en pantalla
    private Renderer grua;          
    
    private float tiempoActualEspera;
    private int estadoActual;

    //posiciones de la base y de la bola respecto al centro de la grua
    private float deltaPosBase = 0.0f;
    private float deltaPosBola = 7.0f;

    // Estados del jefe
    private const int Embestir = 0;
    private const int Rotar = 1;
    private const int DispararProyectil = 2;

    void Start()
    {
        //posiciona las otras dos partes de la grua
        baseGrua.position = new Vector2(transform.position.x + deltaPosBase, transform.position.y + deltaPosBase);
        bolaGrua.position = transform.position + transform.up.normalized * deltaPosBola;
        //asigna el renderer de la grua
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
            yield return new WaitForSeconds(tiempoActualEspera);
            ActualizarEstado();
        }
    }

    private IEnumerator Embestida()
    {
        float tiempoEmbestida = 1.2f;       // Asociado a la velocidad de la embestida
        float distanciaEmbestida = 15f;    // Ajusta la distancia de la embestida

        // condidiones iniciales (para reestablecerlas al final, por cualquier inexactitud en el movimiento)
        Quaternion rotacionInicial = transform.rotation;
        Vector2 posicionInicial = transform.position;
        Vector2 posicionObjetivo = transform.position + distanciaEmbestida * transform.up.normalized; // calcula la posición final

        // Mover hacia adelante
        float tiempoInicio = Time.time;
        while (Time.time < tiempoInicio + tiempoEmbestida / 2)
        {
            transform.position = Vector2.Lerp(posicionInicial, posicionObjetivo, (Time.time - tiempoInicio) / (tiempoEmbestida / 2));
            //ubica las otras dos partes de la grua
            baseGrua.position = transform.position + transform.up.normalized * deltaPosBase;
            bolaGrua.position = transform.position + transform.up.normalized * deltaPosBola;
            yield return null;
        }
        // Mover hacia atrás (retroceso)
        tiempoInicio = Time.time;
        while (Time.time < tiempoInicio + tiempoEmbestida / 2)
        {
            transform.position = Vector2.Lerp(posicionObjetivo, posicionInicial, (Time.time - tiempoInicio) / (tiempoEmbestida / 2));
            //ubica las otras dos partes de la grua
            baseGrua.position = transform.position + transform.up.normalized * deltaPosBase;
            bolaGrua.position = transform.position + transform.up.normalized * deltaPosBola;
            yield return null;
        }

        //se reestablece toda la grua tal como estaba al inicio, por cualquier inexactitud en el movimiento
        transform.position = posicionInicial;
        transform.rotation = rotacionInicial;
        baseGrua.up = transform.up;
        bolaGrua.position = transform.position + transform.up.normalized * deltaPosBola;
    }

    private IEnumerator Rotacion()
    {
        float tiempoMovimiento = 1.5f;                                  // Ajusta la duración del movimiento (en s)
        float anguloMaximo = 180.0f;                                    // rota 180 grados
        float velocidadAngular = anguloMaximo / (tiempoMovimiento / 2);

        //para determinar la direccion de rotación busca al auto del jugador
        Vector3 dirGrua = transform.up.normalized;
        Vector3 dirJugador = (jugador.transform.position - transform.position).normalized;
        float angulo = Mathf.Sign(Vector3.Cross(dirGrua, dirJugador).z);        // obtiene el signo del producto cruzado de las direcciones
        int direccionRotacion = (int)(angulo / Mathf.Abs(angulo));              // -1 o 1 según el sentido horario o antihorario

        //condiciones del avance simultaneo a la rotacion
        float distanciaEmbestida = 12f;    // Ajusta la distancia de la embestida
        // condidiones iniciales
        Quaternion rotacionInicial = transform.rotation;
        Vector2 posicionInicial = transform.position;
        Vector3 direccionInicial = transform.up.normalized;
        Vector2 posicionObjetivo = transform.position + distanciaEmbestida * transform.up.normalized; // calcula la posición final

        // Avanzar y rotar hacia la posición objetivo
        float tiempoInicio = Time.time;
        while (Time.time < tiempoInicio + tiempoMovimiento / 2)
        {
            transform.position = Vector2.Lerp(posicionInicial, posicionObjetivo, (Time.time - tiempoInicio) / (tiempoMovimiento / 2)); //mueve la grua
            transform.Rotate(0, 0, direccionRotacion*velocidadAngular*Time.deltaTime);          // rota la grua en la dirección obtenida
            //ubica las otras dos partes de la grua
            baseGrua.position = transform.position + direccionInicial * deltaPosBase;
            bolaGrua.position = transform.position + transform.up.normalized * deltaPosBola;
            yield return null;
        }

        // volver hacia la posición inicial
        direccionRotacion *= -1;                // cambia el sentido de rotación
        tiempoInicio = Time.time;
        while (Time.time <= tiempoInicio + tiempoMovimiento/2)
        {
            transform.position = Vector2.Lerp(posicionObjetivo, posicionInicial, (Time.time - tiempoInicio) / (tiempoMovimiento / 2));
            transform.Rotate(0, 0, direccionRotacion * velocidadAngular * Time.deltaTime);
            baseGrua.position = transform.position + direccionInicial * deltaPosBase;
            bolaGrua.position = transform.position + transform.up.normalized * deltaPosBola;
            yield return null;
        }

        //se reestablece toda la grua tal como estaba al inicio, por cualquier inexactitud en el movimiento
        transform.position = posicionInicial;
        transform.rotation = rotacionInicial;
        baseGrua.up = transform.up;
        bolaGrua.position = transform.position + transform.up.normalized * deltaPosBola;
    }

    private IEnumerator Disparar()
    {
        // registra los valores iniciales de posición, para aplicarlos al final del movimiento
        Quaternion rotacionInicial = transform.rotation;
        Vector3 posicionInicial = transform.position;

        //condiciones del avance previo al disparo
        float distanciaEmbestida = 8f;      // Ajusta la distancia de la embestida
        float tiempoEmbestida = 0.9f;       // Asociado a la velocidad de la embestida
        Vector2 posicionObjetivo = transform.position + distanciaEmbestida * transform.up.normalized; //calcula la posicion objetivo

        // Mover hacia adelante, similar a Embestida() 
        float tiempoInicio = Time.time;
        while (Time.time < tiempoInicio + tiempoEmbestida / 2)
        {
            transform.position = Vector2.Lerp(posicionInicial, posicionObjetivo, (Time.time - tiempoInicio) / (tiempoEmbestida / 2));
            baseGrua.position = transform.position + transform.up.normalized * deltaPosBase;
            bolaGrua.position = transform.position + transform.up.normalized * deltaPosBola;
            yield return null;
        }

        //Calculos para apuntar hacia el jugador
        Vector3 dirGrua = transform.up.normalized;
        Vector3 dirJugador = (jugador.transform.position - transform.position).normalized;
        float anguloMax = Vector2.Angle(dirGrua, dirJugador);               // calcula el ángulo entre la direccion de la grua y el jugador
        float angulo = Mathf.Sign(Vector3.Cross(dirGrua, dirJugador).z);    // obtiene el signo del producto cruzado de las direcciones
        int direccionRotacion = (int)(angulo / Mathf.Abs(angulo));          // -1 sentido horario, 1 sentido antihorario
        float velocidadAngular = 360;
        float tiempoMovimiento = anguloMax / 180;                           // calcula el tiempo, tomando 360 grados/seg

        // Rotar hacia la posición objetivo, similar a Rotacion()
        tiempoInicio = Time.time;
        while (Time.time < tiempoInicio + tiempoMovimiento / 2)
        {
            transform.Rotate(0, 0, direccionRotacion * velocidadAngular * Time.deltaTime);
            bolaGrua.position = transform.position + transform.up.normalized * deltaPosBola;
            yield return null;
        }

        // Dispara (antes de volver)
        yield return new WaitForSeconds(0.1f);                          //espera un instante
        bolaGrua.GetComponent<SpriteRenderer>().enabled = false;        //desactiva la bola de la grua
        bolaGrua.GetComponent<Collider2D>().enabled = false;
        Instantiate(prefabProyectil, bolaGrua.position, Quaternion.identity);       //activa la bola proyectil
        yield return new WaitForSeconds(0.2f);

        AutoEnemigo enemigo = baseGrua.GetComponent<AutoEnemigo>();
        enemigo.ModificarEnergia(-20.0f);

        // volver hacia la posición inicial, similar a Rotacion()
        tiempoInicio = Time.time;
        direccionRotacion *= -1;
        while (Time.time <= tiempoInicio + tiempoMovimiento / 2)
        {
            transform.Rotate(0, 0, direccionRotacion * velocidadAngular * Time.deltaTime);
            bolaGrua.position = transform.position + transform.up.normalized * deltaPosBola;
            yield return null;
        }

        // Mover hacia atrás (retroceso), similar a Embestida()
        tiempoInicio = Time.time;
        while (Time.time < tiempoInicio + tiempoEmbestida / 2)
        {
            transform.position = Vector2.Lerp(posicionObjetivo, posicionInicial, (Time.time - tiempoInicio) / (tiempoEmbestida / 2));
            baseGrua.position = transform.position + transform.up.normalized * deltaPosBase;
            bolaGrua.position = transform.position + transform.up.normalized * deltaPosBola;
            yield return null;
        }

        //se reestablece toda la grua tal como estaba al inicio, por cualquier inexactitud en el movimiento
        transform.position = posicionInicial;
        transform.rotation = rotacionInicial;
        baseGrua.up = transform.up;
        bolaGrua.position = transform.position + transform.up.normalized * deltaPosBola;
        bolaGrua.GetComponent<SpriteRenderer>().enabled = true;
        bolaGrua.GetComponent<Collider2D>().enabled = true;
    }

    private void ActualizarEstado()
    {
        //si la grua aparece en pantalla y el auto existe, tendrá en consideración la posibilidad de disparar, sino no
        if (grua.isVisible && jugador.GetComponent<Collider2D>().enabled)
        {
            estadoActual = Random.Range(0, 3);
        }
        else
        {
            estadoActual = 3;
        }
    }
}