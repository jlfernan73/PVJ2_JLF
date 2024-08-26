using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class MoverJugador : MonoBehaviour
{
    // Variables a configurar desde el editor
    [Header("Configuracion")]
    [SerializeField] float aceleracion = 30f;
    [SerializeField] float freno = 3f;
    [SerializeField] float maxAngulo = 0.3f;
    [SerializeField] float maxRapidez = 20f;

    // Variables de uso interno en el script
    private float girar;
    private float mover;
    private Vector2 direccion = new Vector2(0, 1);
    private float rapidez = 0;
    private float angulo = 0;
    private float deltaAngulo = 0.05f;
    private float rotacionZ = 0;
    private bool frenando = false;
    private bool girando = false;

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
        girar = Input.GetAxis("Horizontal");
        mover = Input.GetAxis("Vertical");
        rapidez = miRigidbody2D.velocity.magnitude;
        if (girar != 0)
        {
            girando = true;
            if (girar > 0 && angulo >= -1 * maxAngulo)
            {
                angulo -= deltaAngulo;
            }
            if (girar < 0 && angulo <= maxAngulo)
            {
                angulo += deltaAngulo;
            }
        }
        if (girar == 0 && girando)
        {
            if (angulo > 0)
            {
                angulo -= deltaAngulo;
            }
            if (angulo < 0)
            {
                angulo += deltaAngulo;
            }
            if (angulo < deltaAngulo && angulo > -1 * deltaAngulo)
            {
                girando = false;
                angulo = 0;
            }
        }
        if (girando)
        {
            if (rapidez < 3.0)
            {
                transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + angulo * 2f);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + angulo * rapidez * 0.2f);
            }
        }
        rotacionZ = transform.eulerAngles.z * Mathf.Deg2Rad;
        miRigidbody2D.velocity = new Vector2(-1 * rapidez * Mathf.Sin(rotacionZ), rapidez * Mathf.Cos(rotacionZ));
        direccion = new Vector2(-1 * Mathf.Sin(rotacionZ), Mathf.Cos(rotacionZ));
        if (mover < 0)
        {
            miRigidbody2D.drag = freno;
            frenando = true;
        }
        else
        {
            if (frenando)
            {
                miRigidbody2D.drag = 1f;
                frenando = false;
            }
        }
    }
    private void FixedUpdate()
    {
        if (mover > 0 && rapidez < maxRapidez)
        {
            miRigidbody2D.AddForce(direccion * aceleracion);
        }
    }
}