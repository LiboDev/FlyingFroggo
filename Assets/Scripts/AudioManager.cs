using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Sound[] musicSounds;
    [SerializeField] private AudioSource musicSource;

    [SerializeField] private bool musicToggled = false;
    [SerializeField] private bool sfxToggled = false;

    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle sfxToggle;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        if(musicToggled = PlayerPrefs.GetInt("musicToggled", 1) == 1)
        {
            audioMixer.SetFloat("MusicVolume", 0);
            musicToggle.isOn = true;
        }
        else
        {
            audioMixer.SetFloat("MusicVolume", -80);
            musicToggle.isOn = false;
        }


        if(sfxToggled = PlayerPrefs.GetInt("sfxToggled", 1) == 1)
        {
            audioMixer.SetFloat("SFXVolume", 0);
            sfxToggle.isOn = true;
        }
        else
        {
            audioMixer.SetFloat("SFXVolume", -80);
            sfxToggle.isOn = false;
        }
            
    }

    public void ToggleMusic()
    {
        musicToggled = !musicToggled;
        PlayerPrefs.SetInt("musicToggled", musicToggled ? 1 : 0);

        if(musicToggled)
        {
            audioMixer.SetFloat("MusicVolume", 0);    
        }
        else
        {
            audioMixer.SetFloat("MusicVolume", -80);
        }
    }

    public void ToggleSFX()
    {
        sfxToggled = !sfxToggled;
        PlayerPrefs.SetInt("sfxToggled", sfxToggled ? 1 : 0);

        if (sfxToggled)
        {
            audioMixer.SetFloat("SFXVolume", 0);
        }
        else
        {
            audioMixer.SetFloat("SFXVolume", -80);
        }
    }
}