using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int vidasIniciales;
    private int vidas;
    private int puntaje;
    private bool gameOver;
    private bool victoria;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            vidas = vidasIniciales;
            puntaje = 0;
            gameOver = false;
            victoria = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
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
    public void ModificarVida(int cantidad)
    {
        vidas+=cantidad;
        if (vidas > 5) { vidas = 5; }
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

}
