using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDcontroller : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textoDiamantes;
    [SerializeField] TextMeshProUGUI textoPuntaje;
    [SerializeField] GameObject iconoVida;
    [SerializeField] GameObject contenedorIconosVida;
    [SerializeField] GameObject barraEnergia;
    [SerializeField] GameObject barraCombustible;
    [SerializeField] GameObject GasolinaItem;
    [SerializeField] GameObject NitroItem;
    [SerializeField] GameObject BumperItem;
    [SerializeField] GameObject MenuPausa;
    [SerializeField] GameObject MenuGameOver;
    [SerializeField] GameObject MenuVictoria;
    private Animator barraEnergiaAnimator;
    private Animator barraCombustibleAnimator;

    public void Awake()
    {
        barraEnergiaAnimator = barraEnergia.GetComponent<Animator>();
        barraCombustibleAnimator = barraCombustible.GetComponent<Animator>();
        for (int i=1;i<4;i++)
        {
            ActualizarEstadoObjeto(i, false);
        }
        ActualizarMenuPausa(false);
        ActualizarMenuGameOver(false);
        ActualizarMenuVictoria(false);
    }

    private void OnEnable()
    {
       GameEvents.OnPause += Pausa;
       GameEvents.OnResume += Reanuda;
       GameEvents.OnGameOver += GameOver;
       GameEvents.OnVictory += Victoria;
    }
    private void OnDisable()
    {
        GameEvents.OnPause -= Pausa;
        GameEvents.OnResume -= Reanuda;
        GameEvents.OnGameOver -= GameOver;
        GameEvents.OnVictory -= Victoria;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale != 0)
            {
                GameEvents.TriggerPause();
            }
            else
            {
                GameEvents.TriggerResume();
            }
        }
        if (GameManager.Instance.GetGameOver())
        {
            GameEvents.TriggerGameOver();
        }
        if (GameManager.Instance.GetVictoria())
        {
            GameEvents.TriggerVictory();
        }
    }
    private void Pausa()
    {
        ActualizarMenuPausa(true);
        Time.timeScale = 0;

    }
    public void Reanuda()
    {
        ActualizarMenuPausa(false);
        Time.timeScale = 1;
    }
    public void GameOver()
    {
        ActualizarMenuGameOver(true);
    }
    public void Victoria()
    {
        ActualizarMenuVictoria(true);
    }

    public void ActualizarVidasHUD(int vidas)
    {
        if (EstaVacioContenedor())
        {
            CargarContenedor(vidas);
            return;
        }
        if (CantidadIconosVida() > vidas)
        {
            EliminarUltimoIcono();
        }
        else
        {
            CrearIcono();
        }
    }

    private bool EstaVacioContenedor()
    {
        return contenedorIconosVida.transform.childCount == 0;
    }
    private int CantidadIconosVida()
    {
        return contenedorIconosVida.transform.childCount;
    }
    private void EliminarUltimoIcono()
    {
        Transform contenedor = contenedorIconosVida.transform;
        GameObject.Destroy(contenedor.GetChild(contenedor.childCount - 1).gameObject);
    }
    private void CargarContenedor(int cantidadIconos)
    {
        for(int i=0; i<cantidadIconos; i++)
        {
            CrearIcono();
        }
    }
    private void CrearIcono()
    {
        Instantiate(iconoVida, contenedorIconosVida.transform);
    }

    public void ActualizarTextoDiamantes(string nuevoTexto)
    {
        textoDiamantes.text = nuevoTexto;
    }

    public void ActualizarBarraEnergia(float energia)
    {
        barraEnergia.transform.localScale = new Vector3(1, energia / 100f, 1);
        barraEnergiaAnimator.SetFloat("Energia", energia);
    }

    public void ActualizarBarraCombustible(float combustible)
    {
        barraCombustible.transform.localScale = new Vector3(1, combustible / 100f, 1);
        barraCombustibleAnimator.SetFloat("Combustible", combustible);
    }

    public void ActualizarEstadoObjeto(int objeto, bool estado)
    {
        switch (objeto)
        {
            case 1:
                GasolinaItem.SetActive(estado); break;
            case 2:
                NitroItem.SetActive(estado); break;
            case 3:
                BumperItem.SetActive(estado); break;
        }
    }
    public void ActualizarTextoPuntaje(string nuevoTexto)
    {
        textoPuntaje.text = nuevoTexto;
    }

    public void ActualizarMenuPausa(bool estado)
    {
        MenuPausa.SetActive(estado);
    }
    public void ActualizarMenuGameOver(bool estado)
    {
        MenuGameOver.SetActive(estado);
    }
    public void ActualizarMenuVictoria(bool estado)
    {
        MenuVictoria.SetActive(estado);
    }

}
