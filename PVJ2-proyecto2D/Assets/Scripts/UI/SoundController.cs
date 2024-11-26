using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
    }

    void Update()
    {
        audioSource = GetComponent<AudioSource>();
        if (PersistenceManager.Instance != null )
        {
            audioSource.mute = !PersistenceManager.Instance.GetBool("Music");
            audioSource.volume = PersistenceManager.Instance.GetFloat("MusicVolume");
        }
    }
}
