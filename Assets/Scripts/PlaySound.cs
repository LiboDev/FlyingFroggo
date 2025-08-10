using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public static PlaySound instance;

    private AudioSource audioSource;
    [SerializeField] private Sound[] sounds;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        audioSource= GetComponent<AudioSource>();

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(string name, float volume, float variation)
    {
        Sound s = null;

        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
            {
                s = sounds[i];
            }
        }

        if (s == null)
        {
            Debug.LogError("Sound: (" + name + ") Not Found");
        }
        else
        {
            audioSource.pitch = 1 + Random.Range(-variation, variation);
            audioSource.PlayOneShot(s.clip, volume);
        }
    }

    public void PlaySFX(string name, float volume)
    {
        Sound s = null;

        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
            {
                s = sounds[i];
            }
        }

        if (s == null)
        {
            Debug.LogError("Sound: (" + name + ") Not Found");
        }
        else
        {
            audioSource.PlayOneShot(s.clip, volume);
        }
    }
}
