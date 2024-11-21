using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disparar : MonoBehaviour
{
    private PerfilJugador perfilJugador;
    public PerfilJugador PerfilJugador { get => perfilJugador; }

    [SerializeField] private GameObject objectPrefab;
    [SerializeField] protected GameObject particlesChispas;
    [SerializeField]
    [Range(0.1f, 1.0f)]
    private float tiempoIntervalo;
    bool disparando = false;
    bool recargando = false;
    float tiempoRecarga;

    private void Awake()
    {
        perfilJugador = GameObject.FindWithTag("Player").GetComponent<Jugador>().PerfilJugador;
    }

    private void Update()
    {
        if (!GetComponent<Collider2D>().enabled)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                if (disparando) { disparando = false; } else { disparando = true; }
            }
            if (disparando && !recargando)
            {
                GenerarObjeto();
                PerfilJugador.CannonConteo -= 1;
                recargando = true;
                tiempoRecarga = 0f;
            }
            if (recargando)
            {
                tiempoRecarga += Time.deltaTime;
                if (tiempoRecarga > tiempoIntervalo)
                {
                    recargando = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.Plus)|| Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                tiempoIntervalo -= 0.05f;
                if (tiempoIntervalo < 0.1f) { tiempoIntervalo = 0.1f; }
            }
            if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                tiempoIntervalo += 0.05f;
                if (tiempoIntervalo > 1.0f) { tiempoIntervalo = 1.0f; }
            }
        }
    }
    void GenerarObjeto()
    {
        GameObject obj = Instantiate(objectPrefab);
        GameObject chispas = Instantiate(particlesChispas);
        obj.SetActive(false);
        chispas.SetActive(false);

        Fireball fireball;
        if (obj != null)
        {
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
            obj.SetActive(true);

            fireball = obj.GetComponent<Fireball>();
            if (chispas != null)
            {
                chispas.transform.position = transform.position;
                chispas.transform.rotation = transform.rotation;
                chispas.SetActive(true);
                fireball.AsignarChispas(chispas);
            }
        }
    }

}
