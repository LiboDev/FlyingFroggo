using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEffects : MonoBehaviour
{
    private Animation animation;
    private ParticleSystem particleSystem;

    [SerializeField] private string audio;

    public void Start()
    {
        animation = GetComponent<Animation>();
        particleSystem = GetComponent<ParticleSystem>();
    }

    public void Play()
    {
        if (animation != null)
        {
            animation.Play();
        }
        if (particleSystem != null)
        {
            particleSystem.Play();
        }

        if(audio != null)
        {
            PlaySound.instance.PlaySFX(audio, 1f, 0.01f);
        }
    }
}
