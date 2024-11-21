using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// clase para colectar ítems en Diccionario (objetos) y en una Cola (diamantes)
// Diccionario: Inventario con 3 objetos (sólo puede colectar uno de cada uno en la bolsa) que se activan con los números
//              1: Recarga de combustible; 2: Potenciador de Nitro;
//              3: Bumper (protege de choques con otros autos); 4: Cañón (dispara fireballs con LeftCtrl)
// Cola: Diamantes necesarios para pasar de nivel (se almacenan en el cofre y se extraen de la cola para destruirlos al pasar de nivel)

public class Coleccionar : MonoBehaviour
{
    private PerfilJugador perfilJugador;
    public PerfilJugador PerfilJugador { get => perfilJugador; }

    [SerializeField] private GameObject bolsa;
    [SerializeField] private GameObject cofre;

    private Dictionary<String, GameObject> inventario;  // se define el inventario que contendrá los objetos
    private Queue<GameObject> diamantes;                // se define la cola que contendrá los diamantes colectados

    private GameObject nuevoColeccionable = null;
    private GameObject nuevoDiamante = null;

    private bool bumperActive = false;                  // el funcionamiento del bumper se gestionará desde esta clase
    private GameObject bumper = null;                   // gameObject del bumper activado por el jugador

    private bool cannonActive = false;                  // la presencia del cañón se gestionará desde esta clase
    private GameObject cannon = null;                   // gameObject del cañón activado por el jugador

    private AudioSource audioColeccionable;             
    private AudioSource audioDiamante;

    private Progresion progresionJugador;

    void Awake()
    {
        perfilJugador = GetComponent<Jugador>().PerfilJugador;      //el SO está asignado desde el Inspector en el script Jugador
        inventario = new Dictionary<String, GameObject>();          //se instancia el inventario y la cola de diamantes
        diamantes = new Queue<GameObject>();
        audioColeccionable = bolsa.GetComponent<AudioSource>(); // se usan AudioSources independientes (definidos en la bolsa y en el cofre)
        audioDiamante = cofre.GetComponent<AudioSource>();
        progresionJugador = GetComponent<Progresion>();
        PerfilJugador.BumperConteo = 0;                         //se inicializa en 0 el conteo del bumper activo (ya que no está activo)
        PerfilJugador.CannonConteo = 0;                         //se inicializa en 0 el conteo del cañón activo (ya que no está activo)
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Jugador jugador = gameObject.GetComponent<Jugador>();       //para acceder a los métodos de Jugador 
        if (collision.gameObject.CompareTag("Coleccionable"))                   // recolección del objeto
        {
            nuevoColeccionable = collision.gameObject;
            if (inventario.ContainsKey(nuevoColeccionable.name)) { return; }    // si ya está el objeto en el inventario no lo colecta
            audioColeccionable.PlayOneShot(PerfilJugador.ColectarSFX);         // se ejecuta el sonido de recolección
            inventario.Add(nuevoColeccionable.name, nuevoColeccionable);        //se agrega el objeto al coleccionable
            nuevoColeccionable.transform.SetParent(bolsa.transform);            //se lo manda a la bolsa
            nuevoColeccionable.SetActive(false);                                //se lo desactiva
            if (nuevoColeccionable.name == "Gasolina") { jugador.ModificarItem(1, true); }
            if (nuevoColeccionable.name == "Nitro") { jugador.ModificarItem(2, true); }
            if (nuevoColeccionable.name == "Bumper") { jugador.ModificarItem(3, true); }
            if (nuevoColeccionable.name == "Cannon") { jugador.ModificarItem(4, true); }
        }
        if (collision.gameObject.CompareTag("Diamante"))                        //recolección del diamante
        {
            nuevoDiamante = collision.gameObject;
            audioDiamante.PlayOneShot(PerfilJugador.DiamanteSFX);         // se ejecuta el sonido de recolección de diamante
            progresionJugador.GanarExperiencia(1);                     // se suma experiencia (asociada a la cantidad de diamantes colectados)
            jugador.ReportarDiamantes();                                //actualiza la experiencia (diamantes) en el panel 
            Debug.Log("ITEM COLECTADO - EXPERIENCIA: " + progresionJugador.PerfilJugador.Experiencia);
            diamantes.Enqueue(nuevoDiamante);                           // se agrega el diamante a la cola con diamantes
            nuevoDiamante.transform.SetParent(cofre.transform);         //se guarda el diamante en el cofre
            nuevoDiamante.SetActive(false);                             // se desactiva el diamante
            GameManager.Instance.AdPuntaje(10);
        }
    }

    private void Update()
    {
        //acciones asociadas a la activación de un objeto
        if ((Input.GetKeyDown(KeyCode.Alpha1)) && inventario.ContainsKey("Gasolina")) UsarInventario(inventario["Gasolina"]);
        if ((Input.GetKeyDown(KeyCode.Alpha2)) && inventario.ContainsKey("Nitro")) UsarInventario(inventario["Nitro"]);
        if ((Input.GetKeyDown(KeyCode.Alpha3)) && inventario.ContainsKey("Bumper")) UsarInventario(inventario["Bumper"]);
        if ((Input.GetKeyDown(KeyCode.Alpha4)) && inventario.ContainsKey("Cannon")) UsarInventario(inventario["Cannon"]);

        //acciones asociadas al uso del bumper
        if (bumperActive)
        {
            bumper.transform.position = transform.position;       
            bumper.transform.rotation = transform.rotation;     
            PerfilJugador.BumperConteo -= PerfilJugador.ConsumoObj * Time.deltaTime;    //consumo de uso del bumper
            if (PerfilJugador.BumperConteo < 0 || !GetComponent<Jugador>().EstaVivo()) DesactivarBumper();                     //pasado el tiempo, se desactiva el bumper
        }
        //acciones asociadas a la presencia del cañón
        if (cannonActive)
        {
            cannon.transform.position = transform.position;
            cannon.transform.rotation = transform.rotation;
            if (PerfilJugador.CannonConteo == 0 || !GetComponent<Jugador>().EstaVivo()) DesactivarCannon();                     //pasado el tiempo, se desactiva el cañón
        }
    }

    private void UsarInventario(GameObject item)
    {
        Jugador jugador = gameObject.GetComponent<Jugador>();       //para acceder a los métodos de Jugador 
        inventario.Remove(item.name);                                               //se quita el objeto del inventario
        item.transform.SetParent(null);                                             //y de la bolsa
        audioColeccionable.PlayOneShot(PerfilJugador.UsarColeccionableSFX);         // se ejecuta el sonido de uso del coleccionable
        if (item.name == "Gasolina") {
            jugador.modificarCombustible(50.0f);                        //adiciona combustible
            jugador.ModificarItem(1, false);
        }
        if (item.name == "Nitro") {
            MoverJugador movimientoJugador = gameObject.GetComponent<MoverJugador>();   //para acceder a los métodos de MoverJugador
            movimientoJugador.activarNitro();                                           //activa Nitro (modifica aceleracion y rapidezMax)
            jugador.ModificarItem(2, false);
        }
        if (item.name == "Bumper") { 
            ActivarBumper(item);
            jugador.ModificarItem(3, false);
        }
        if (item.name == "Cannon")
        {
            ActivarCannon(item);
            jugador.ModificarItem(4, false);
        }
    }

    public void VaciarInventario()
    {
        Jugador jugador = gameObject.GetComponent<Jugador>();       //para acceder a los métodos de Jugador
        if (inventario.ContainsKey("Gasolina")) { inventario["Gasolina"].transform.SetParent(null); }
        if (inventario.ContainsKey("Nitro")) { inventario["Nitro"].transform.SetParent(null); }
        if (inventario.ContainsKey("Bumper")) { inventario["Bumper"].transform.SetParent(null); }
        if (inventario.ContainsKey("Cannon")) { inventario["Cannon"].transform.SetParent(null); }
        inventario.Clear();                                               //se quitan los objetos del inventario
        for (int i = 1; i<5; i++)
        {
            jugador.ModificarItem(i, false);
        }
    }

    public void EntregarDiamantes(int cantidad)             // método usado al final del nivel, para entregar los diamantes requeridos
    {
        for(int i=0;i<cantidad; i++)                        
        {
            GameObject diamanteEntregado = diamantes.Dequeue();     //se sacan los diamantes de la cola 
            Destroy(diamanteEntregado);                             //y se los destruye
        }
        Debug.Log("SE ENTREGARON " + cantidad + " DIAMANTES");
    }

    private void ActivarBumper(GameObject item)                         //activación del bumper (el item es el bumper)
    {
        item.GetComponent<CapsuleCollider2D>().isTrigger = false;       //deja de colisionar tipo trigger
        item.GetComponent<CapsuleCollider2D>().excludeLayers = LayerMask.GetMask("Default");    //no colisionará con Default (donde está el auto)
        item.transform.localScale = new Vector3(1.7f, 1.7f, 1f);        //se lo agranda un poco para que rodee al auto
        item.transform.position = transform.position;                   //se lo ubica en el centro del auto
        item.transform.rotation = transform.rotation;
        bumper = item;                                                  //se asigna el item al objeto bumper (para seguir moviendolo) 
        bumper.SetActive(true);                                         //se lo activa
        bumperActive = true;                                            //se avisa que está activo
        GetComponent<FixedJoint2D>().connectedBody = bumper.GetComponent<Rigidbody2D>();
        GetComponent<FixedJoint2D>().enabled = true;
        PerfilJugador.BumperConteo = 100;                               //se inicia en 100 el contador
    }
    private void ActivarCannon(GameObject item)                         //activación del cañón (el item es el cañón)
    {
        item.GetComponent<SpriteRenderer>().sortingLayerName = "Frente";
        item.GetComponent<SpriteRenderer>().sortingOrder = 2;
        item.GetComponent<CapsuleCollider2D>().enabled = false;       //no colisiona
        item.transform.position = transform.position;                   //se lo ubica en el centro del auto
        item.transform.rotation = transform.rotation;
        cannon = item;                                                  //se asigna el item al objeto cañon (para seguir moviendolo) 
        cannon.SetActive(true);                                         //se lo activa
        cannonActive = true;                                            //se avisa que está activo
        PerfilJugador.CannonConteo = 75;                               //se inicia en 50 el contador de disparos
    }

    private void DesactivarBumper()                                     //desactivación del bumper
    {
        bumperActive = false;                                           //se avisa que está inactivo
        GetComponent<FixedJoint2D>().enabled = false;
        bumper.SetActive(false);                                        //se desactiva el gameObject
        PerfilJugador.BumperConteo = 0;                                 //se pone en 0 el contador
    }
    private void DesactivarCannon()                                     //desactivación del cañón
    {
        cannonActive = false;                                           //se avisa que está inactivo
        cannon.SetActive(false);                                        //se desactiva el gameObject
        PerfilJugador.CannonConteo = 0;                                 //se pone en 0 el contador
    }

}
