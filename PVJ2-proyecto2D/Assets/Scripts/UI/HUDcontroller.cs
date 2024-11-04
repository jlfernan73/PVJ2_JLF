using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDcontroller : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textoDiamantes;
    [SerializeField] TextMeshProUGUI textoPuntaje;
    [SerializeField] GameObject barraEnergia;
    [SerializeField] GameObject barraCombustible;
    [SerializeField] GameObject GasolinaItem;
    [SerializeField] GameObject NitroItem;
    [SerializeField] GameObject BumperItem;
    private Animator barraEnergiaAnimator;
    private Animator barraCombustibleAnimator;

    public void Start()
    {
        barraEnergiaAnimator = barraEnergia.GetComponent<Animator>();
        barraCombustibleAnimator = barraCombustible.GetComponent<Animator>();
        for (int i=1;i<4;i++)
        {
            ActualizarEstadoObjeto(i, false);
        }
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

}
