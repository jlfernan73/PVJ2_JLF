using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
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
/*
    private void OnEnable()
    {
        GameEvents.OnPause += Pausar;
        GameEvents.OnResume += Reanudar;
    }
    private void OnDisable()
    {
        GameEvents.OnPause -= Pausar;
        GameEvents.OnResume -= Reanudar;
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
    }
    private void Pausar()
    {
        Time.timeScale = 0;

    }
    private void Reanudar()
    {
        Time.timeScale = 1;
    }
*/
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
