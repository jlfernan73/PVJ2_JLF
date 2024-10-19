using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

// clase para colectar ítems en Diccionario (objetos) y en una Cola (diamantes)
// Diccionario: Inventario con 3 objetos (sólo puede colectar uno de cada uno en la bolsa) que se activan con los números
//              1: Recarga de combustible; 2: Potenciador de Nitro; 3: Bumper (protege de choques con otros autos)
// Cola: Diamantes necesarios para pasar de nivel (se almacenan en el cofre y se extraen de la cola para destruirlos al pasar de nivel)

public class Coleccionar : MonoBehaviour
{
    private PerfilJugador perfilJugador;
    public PerfilJugador PerfilJugador { get => perfilJugador; }

    [SerializeField] private GameObject bolsa;
    [SerializeField] private GameObject cofre;

    private Dictionary<String, GameObject> inventario;
    private Queue<GameObject> diamantes;

    private GameObject bumper = null;
    private GameObject nuevoColeccionable = null;
    private GameObject nuevoDiamante = null;
    private SpriteRenderer miColeccionable;             // se va a acceder al spriteRenderer del coleccionable 
    private SpriteRenderer miDiamante;                  // se va a acceder al spriteRenderer del diamante 
    private bool bumperActive = false;

    private AudioSource audioColeccionable;
    private AudioSource audioDiamante;

    private Progresion progresionJugador;

    void Awake()
    {
        perfilJugador = GetComponent<Jugador>().PerfilJugador;
        inventario = new Dictionary<String, GameObject>();
        diamantes = new Queue<GameObject>();
        audioColeccionable = bolsa.GetComponent<AudioSource>();
        audioDiamante = cofre.GetComponent<AudioSource>();
        progresionJugador = GetComponent<Progresion>();
        PerfilJugador.BumperConteo = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coleccionable"))
        {
            nuevoColeccionable = collision.gameObject;
            if (inventario.ContainsKey(nuevoColeccionable.name)) { return; }
            miColeccionable = nuevoColeccionable.GetComponent<SpriteRenderer>();
            audioColeccionable.PlayOneShot(PerfilJugador.ColectarSFX);         // se ejecuta el sonido de recolección
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
            nuevoDiamante = collision.gameObject;
            miDiamante = nuevoDiamante.GetComponent<SpriteRenderer>();
            audioDiamante.PlayOneShot(PerfilJugador.DiamanteSFX);         // se ejecuta el sonido de recolección de diamante
            progresionJugador.GanarExperiencia(1);
            Debug.Log("ITEM COLECTADO - EXPERIENCIA: " + progresionJugador.PerfilJugador.Experiencia);
            miDiamante.enabled = false;                     // se desactiva el spriteRenderer del diamante (sin borrarlo)
            diamantes.Enqueue(nuevoDiamante);                   // se agrega el diamante a la lista de diamantes
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
            PerfilJugador.BumperConteo -= PerfilJugador.ConsumoObj * Time.deltaTime;
            if (PerfilJugador.BumperConteo < 0) DesactivarBumper();
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

        audioColeccionable.PlayOneShot(PerfilJugador.UsarColeccionableSFX);         // se ejecuta el sonido de uso del coleccionable
    }

    public void EntregarDiamantes(int cantidad)
    {
        for(int i=0;i<cantidad; i++)
        {
            GameObject diamanteEntregado = diamantes.Dequeue();
            Destroy(diamanteEntregado);
        }
        Debug.Log("SE ENTREGARON " + cantidad + " DIAMANTES");
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
        PerfilJugador.BumperConteo = 100;
    }

    private void DesactivarBumper()
    {
        bumperActive = false;
        bumper.SetActive(false);
        PerfilJugador.BumperConteo = 0;
    }

}
