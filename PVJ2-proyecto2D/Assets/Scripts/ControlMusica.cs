using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMusica : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private AudioClip musicaJuego;
    [SerializeField] private AudioClip musicaMeta;
    private AudioSource sonidoFondo;

    // Start is called before the first frame update
    void Start()
    {
        sonidoFondo = GetComponent<AudioSource>();
    }

    public void playFondo()
    {
        sonidoFondo.loop = true;
        sonidoFondo.clip = musicaJuego;
        sonidoFondo.Play();
    }

    public void playMeta()
    {
        sonidoFondo.Stop();
        sonidoFondo.loop = false;
        sonidoFondo.PlayOneShot(musicaMeta);
    }
}
