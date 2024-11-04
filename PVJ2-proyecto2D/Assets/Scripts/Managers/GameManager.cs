using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int puntaje;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            puntaje = 0;
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

}
