using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDcontroller : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI miTexto;
    [SerializeField] Transform barraEnergia;
    [SerializeField] Transform barraCombustible;
    [SerializeField] GameObject GasolinaItem;
    [SerializeField] GameObject NitroItem;
    [SerializeField] GameObject BumperItem;

    public void ActualizarTextoHUD(string nuevoTexto)
    {
        miTexto.text = nuevoTexto;
    }

    public void ActualizarBarraEnergia(float energia)
    {
        barraEnergia.localScale = new Vector3(1, energia / 100f, 1);
    }

    public void ActualizarBarraCombustible(float combustible)
    {
        barraCombustible.localScale = new Vector3(1, combustible / 100f, 1);
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

}
