using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Coleccionar : MonoBehaviour
{
    [SerializeField] private GameObject bolsa;
    [SerializeField] private GameObject cofre;
    [SerializeField] private AudioClip diamanteSFX;             // para asociar el clip del sonido de levantar diamante
    [SerializeField] private AudioClip colectarSFX;             // para asociar el clip del sonido de levantar el coleccionable
    [SerializeField] private AudioClip usarColeccionableSFX;    // para asociar el clip del sonido de usar el coleccionable

    private Dictionary<String, GameObject> inventario;
    private List<GameObject> diamantes;
    private GameObject bumper = null;
    private GameObject nuevoColeccionable = null;
    private GameObject nuevoDiamante = null;
    private SpriteRenderer miColeccionable;             // se va a acceder al spriteRenderer del coleccionable 
    private SpriteRenderer miDiamante;                  // se va a acceder al spriteRenderer del diamante 
    private bool bumperActive = false;
    [SerializeField] private float bumperConteo = 0f;

    private AudioSource audioColeccionable;
    private AudioSource audioDiamante;

    private Progresion progresionJugador;

    void Awake()
    {
        inventario = new Dictionary<String, GameObject>();
        diamantes = new List<GameObject>();
        audioColeccionable = bolsa.GetComponent<AudioSource>();
        audioDiamante = cofre.GetComponent<AudioSource>();
    
        progresionJugador = GetComponent<Progresion>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coleccionable"))
        {
            nuevoColeccionable = collision.gameObject;
            if (inventario.ContainsKey(nuevoColeccionable.name)) { return; }
            miColeccionable = nuevoColeccionable.GetComponent<SpriteRenderer>();
            audioColeccionable.PlayOneShot(colectarSFX);         // se ejecuta el sonido de recolección
            miColeccionable.enabled = false;                           // se desactiva el spriteRenderer del coleccionable (sin borrarlo)
            inventario.Add(nuevoColeccionable.name, nuevoColeccionable);
            nuevoColeccionable.transform.SetParent(bolsa.transform);
        }
        if (collision.gameObject.CompareTag("Diamante"))
        {
            if(miDiamante != null)  // como los diamantes están muy cerca, puede no haberse desactivado el anterior aun
            {
                nuevoDiamante.SetActive(false);                        // se borra el diamante
                miDiamante = null;
            }
            //Jugador jugador = gameObject.GetComponent<Jugador>();
            nuevoDiamante = collision.gameObject;
            miDiamante = nuevoDiamante.GetComponent<SpriteRenderer>();
            audioDiamante.PlayOneShot(diamanteSFX);         // se ejecuta el sonido de recolección de diamante
            //jugador.AgregarDiamantes();                          // suma uno a los ítems recolectados
            progresionJugador.GanarExperiencia(1);
            Debug.Log("ITEM COLECTADO - EXPERIENCIA: " + progresionJugador.PerfilJugador.Experiencia);
            miDiamante.enabled = false;                     // se desactiva el spriteRenderer del diamante (sin borrarlo)
            diamantes.Add(nuevoDiamante);                   // se agrega el diamante a la lista de diamantes
            nuevoDiamante.transform.SetParent(cofre.transform);     //se guarda el diamante en el cofre
        }

    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Alpha1)) && inventario.ContainsKey("Gasolina")) UsarInventario(inventario["Gasolina"]);
        if ((Input.GetKeyDown(KeyCode.Alpha2)) && inventario.ContainsKey("Nitro")) UsarInventario(inventario["Nitro"]);
        if ((Input.GetKeyDown(KeyCode.Alpha3)) && inventario.ContainsKey("Bumper")) UsarInventario(inventario["Bumper"]);
        if (bumperActive)
        {
            bumper.transform.position = transform.position;
            bumper.transform.rotation = transform.rotation;
            bumperConteo -= 0.04f;
            if (bumperConteo < 0) DesactivarBumper();
        }
        if (miColeccionable != null && !miColeccionable.enabled && !audioColeccionable.isPlaying)     //si se ha desactivado el spriteRenderer y la música ya terminó
        {
            nuevoColeccionable.SetActive(false);                        // se borra el coleccionable
            miColeccionable = null;
        }
        if (miDiamante != null && !miDiamante.enabled && !audioDiamante.isPlaying)     //si se ha desactivado el spriteRenderer del diamante y la música ya terminó
        {
            nuevoDiamante.SetActive(false);                        // se borra el diamante
            miDiamante = null;
        }
    }

    private void UsarInventario(GameObject item)
    {
        Jugador jugador = gameObject.GetComponent<Jugador>();
        MoverJugador movimientoJugador = gameObject.GetComponent<MoverJugador>();
        inventario.Remove(item.name);
        item.transform.SetParent(null);
        if (item.name == "Gasolina") { jugador.modificarCombustible(50.0f); }

        if (item.name == "Nitro") { movimientoJugador.activarNitro(); }

        if (item.name == "Bumper") { ActivarBumper(item);}

        audioColeccionable.PlayOneShot(usarColeccionableSFX);         // se ejecuta el sonido de uso del coleccionable
    }

    private void ActivarBumper(GameObject item)
    {
        item.GetComponent<CapsuleCollider2D>().isTrigger = false;
        item.GetComponent<CapsuleCollider2D>().excludeLayers = LayerMask.GetMask("Default");
        item.transform.localScale = new Vector3(1.4f, 1.4f, 1f);
        item.transform.position = transform.position;
        bumper = item;
        item.GetComponent<SpriteRenderer>().enabled = true;
        bumper.SetActive(true);
        bumperActive = true;
        bumperConteo = 100;
    }

    private void DesactivarBumper()
    {
        bumperActive = false;
        bumper.SetActive(false);
        bumperConteo = 0;
    }

}
