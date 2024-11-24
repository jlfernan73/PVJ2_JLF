using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// clase para el jefe final del nivel 1 que desactiva la barrera para pasar 
public class JefeNivel1 : MonoBehaviour
{
    [SerializeField] Transform barrera;                         //se lo incluye para alivianar la barrera que impide llegar a la meta
    [SerializeField] UnityEvent<string, float> OnJefeKilled;
    [SerializeField] private AudioClip achievementSFX;
    private AudioSource audioAchievement;
    private float tiempoMuerto = 0f;
    private void Awake()
    {
        audioAchievement = barrera.GetComponent<AudioSource>();
    }

    protected void Update()
    {
        if (!gameObject.GetComponent<MoverEnemigo_persigue>().GetVive())
        {
            tiempoMuerto += Time.deltaTime;
            if (tiempoMuerto > 2f)
            {
                barrera.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;      //se hace la valla dinámica poder moverla
                barrera.GetComponent<Rigidbody2D>().mass = 1.0f;                            //se aliviana la valla para poder pasar a la meta
                string mensaje = "Vehículo jefe derrotado\nSegunda barrera desbloqueada";
                OnJefeKilled.Invoke(mensaje, 3f);
                if (!audioAchievement.isPlaying)
                {
                    audioAchievement.PlayOneShot(achievementSFX);
                }
            }
        }
    }
}
