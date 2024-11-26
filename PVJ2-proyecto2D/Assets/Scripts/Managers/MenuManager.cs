using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Slider mySlider;
    [SerializeField] private Toggle myToggle;

    // Start is called before the first frame update
    void Start()
    {
        mySlider.value = PersistenceManager.Instance.GetFloat("MusicVolume");
        myToggle.isOn = PersistenceManager.Instance.GetBool("Music");
    }

    public void SaveMusicConfig(bool status)
    {
        PersistenceManager.Instance.SetBool("Music", status);
    }
    public void SaveVolumeConfig(float volume)
    {
        PersistenceManager.Instance.SetFloat("MusicVolume", volume);
    }
    public void Save()
    {
        PersistenceManager.Instance.Save();
    }


}
