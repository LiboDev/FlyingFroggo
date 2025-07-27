using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioMixer audioMixer;

    public Sound[] musicSounds;
    public AudioSource musicSource;

    public bool musicToggled = false;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        musicToggled = PlayerPrefs.GetInt("musicToggled", 1) == 1;
    }

    public void ToggleMusic()
    {
        musicToggled = !musicToggled;
        PlayerPrefs.SetInt("musicToggled", musicToggled ? 1 : 0);

        if(musicToggled )
        {
            audioMixer.SetFloat("MusicVolume", 0);    
        }
        else
        {
            audioMixer.SetFloat("MusicVolume", -80);
        }
    }
}