using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Coleccionar : MonoBehaviour
{
    //[SerializeField] private List<GameObject> coleccionables;
    [SerializeField] private GameObject bolsa;

    private Dictionary<String, GameObject> inventario;
    private GameObject bumper = null;
    private bool bumperActive = false;

    void Awake()
    {
        //coleccionables = new List<GameObject>();
        inventario = new Dictionary<String, GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Coleccionable")) {  return; }

        GameObject nuevoColeccionable = collision.gameObject;
        if (inventario.ContainsKey(nuevoColeccionable.name)) {  return; }
        nuevoColeccionable.SetActive(false);
        //coleccionables.Add(nuevoColeccionable);
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

        if (item.name == "Bumper") {
            item.GetComponent<CapsuleCollider2D>().isTrigger = false;
            item.GetComponent<CapsuleCollider2D>().excludeLayers = LayerMask.GetMask("Default");
            item.transform.localScale = new Vector3(1.4f,1.4f,1f);
            item.transform.position = transform.position;
            //jugador.GetComponent<CapsuleCollider2D>().excludeLayers = LayerMask.GetMask("Enemigos");
            bumper = item;
            bumper.SetActive(true);
            bumperActive = true;
        }
    }

}
