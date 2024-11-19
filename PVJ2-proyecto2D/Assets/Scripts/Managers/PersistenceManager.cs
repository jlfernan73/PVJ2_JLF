using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistenceManager : MonoBehaviour
{
    public static PersistenceManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }
    public int GetInt(string key, int defaultValue = 0) 
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }
    public void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }
    public float GetFloat(string key, float defaultValue = 0.0f)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }
    public void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }
    public string GetString(string key, string defaultValue = "")
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }
    public void SetBool(string key, bool state)
    {
        PlayerPrefs.SetInt(key, state ? 1 : 0);
    }
    public bool GetBool(string key, bool defaultValue = false)
    {
        int value = PlayerPrefs.GetInt(key, defaultValue ? 1 : 0);
        return value == 1;
    }
    public void Save()
    {
        PlayerPrefs.Save();
    }
    public void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }
    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
    public void SaveMusicConfig(bool status)
    {
        SetBool("Music", status);
    }
    public void SaveVolumeConfig(float volume)
    {
        SetFloat("MusicVolume", volume);
    }

}
