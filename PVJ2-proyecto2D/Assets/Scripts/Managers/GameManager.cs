using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField]
    private PerfilJugador perfilJugador;
    public PerfilJugador PerfilJugador { get => perfilJugador; }

    private int vidasIniciales;
    private int vidas;
    private int puntaje;
    private int experienciaNecesaria;
    private bool gameOver;
    private bool victoria;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            vidas = PerfilJugador.VidasIniciales;
            vidasIniciales = PerfilJugador.VidasIniciales;
            experienciaNecesaria = PerfilJugador.ExperienciaProximoNivel;
            ResetExperiencia();
            puntaje = 0;
            gameOver = false;
            victoria = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AdPuntaje(int puntos)
    {
        puntaje += puntos;
        if (puntaje < 0) { puntaje = 0; }
    }
    public void ResetPuntaje()
    {
        puntaje = 0;
    }
    public int GetPuntaje()
    {
        return puntaje;
    }
    public void SetVidasIniciales(int cantidad)
    {
        vidas = cantidad;
    }
    public void ModificarVida(int cantidad)
    {
        vidas+=cantidad;
        if (vidas > 10) { vidas = 10; }
    }
    public void ResetVidas()
    {
        vidas = vidasIniciales;
    }
    public int GetVidas()
    {
        return vidas;
    }
    public bool GetGameOver()
    {
        return gameOver;
    }
    public void SetGameOver(bool estado)
    {
        gameOver = estado;
    }
    public bool GetVictoria()
    {
        return victoria;
    }
    public void SetVictoria(bool estado)
    {
        victoria = estado;
    }
    public void ResetExperiencia()
    {
        PerfilJugador.Experiencia = 0;
    }
    public int GetExperiencia()
    {
        return PerfilJugador.Experiencia;
    }
    public void ActualizarExperiencia()
    {
        PerfilJugador.Experiencia -= PerfilJugador.EscalarExperiencia;
    }
    public void EscalarExperiencia()
    {
        PerfilJugador.ExperienciaProximoNivel += PerfilJugador.EscalarExperiencia;
    }
    public void ResetExperienciaNivel()
    {
        PerfilJugador.ExperienciaProximoNivel = experienciaNecesaria;
    }
    public int GetNivel()
    {
        return PerfilJugador.Nivel;
    }
    public void SubirNivel()
    {
        PerfilJugador.Nivel ++;
    }
    public void ResetNivel()
    {
        PerfilJugador.Nivel = 1;
    }
}
