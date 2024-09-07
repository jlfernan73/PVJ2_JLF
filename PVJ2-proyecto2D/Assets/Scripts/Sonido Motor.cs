using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonidoMotor : MonoBehaviour
{
    private AudioSource motorAudioSource;

    public void setSonidoMotor(float volumen)      
    {
        motorAudioSource = GetComponent<AudioSource>();
        motorAudioSource.volume = volumen;
    }
}
