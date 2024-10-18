using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Coleccionar : MonoBehaviour
{
    [SerializeField] private GameObject bolsa;
    [SerializeField] private AudioClip colectarSFX;             // para asociar el clip del sonido de levantar el coleccionable
    [SerializeField] private AudioClip usarColeccionableSFX;    // para asociar el clip del sonido de usar el coleccionable

    private Dictionary<String, GameObject> inventario;
    private GameObject bumper = null;
    private GameObject nuevoColeccionable = null;
    private bool bumperActive = false;
    [SerializeField] private float bumperConteo = 0f;

    private AudioSource audioColeccionable;
    private SpriteRenderer miColeccionable;              // se va a acceder al spriteRenderer del coleccionable 

    void Awake()
    {
        inventario = new Dictionary<String, GameObject>();
        audioColeccionable = bolsa.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Coleccionable")) {  return; }

        nuevoColeccionable = collision.gameObject;
        if (inventario.ContainsKey(nuevoColeccionable.name)) {  return; }
        miColeccionable = nuevoColeccionable.GetComponent<SpriteRenderer>();
        audioColeccionable.PlayOneShot(colectarSFX);         // se ejecuta el sonido de recolección
        miColeccionable.enabled = false;                           // se desactiva el spriteRenderer del coleccionable (sin borrarlo)
        inventario.Add(nuevoColeccionable.name, nuevoColeccionable);
        nuevoColeccionable.transform.SetParent(bolsa.transform);
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
