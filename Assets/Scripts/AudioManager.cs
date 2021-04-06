using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum SoundEffects
    {
        PillbugStep,
        ButtonClick,
        Pickup,
        Win
    }

    private static AudioManager instance;

    [Header("Sound effects")]
    [SerializeField] private AudioClip pillbugStep;
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip pickup;
    [SerializeField] private AudioClip win;

    private AudioSource audioSrc;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(transform.gameObject);
            audioSrc = GetComponent<AudioSource>();
            instance = this;
        }
        else if (this != instance)
        {
            Destroy(gameObject);
        }
    }

    public static void PlaySoundEffect(SoundEffects type)
    {
        switch (type)
        {
            case SoundEffects.PillbugStep:
                if (instance.pillbugStep != null)
                {
                    instance.audioSrc.PlayOneShot(instance.pillbugStep, 0.1f);
                }
                break;
            case SoundEffects.ButtonClick:
                if (instance.buttonClick != null)
                {
                    instance.audioSrc.PlayOneShot(instance.buttonClick);
                }
                break;
            case SoundEffects.Pickup:
                if (instance.pickup != null)
                {
                    instance.audioSrc.PlayOneShot(instance.pickup);
                }
                break;
            case SoundEffects.Win:
                if (instance.win != null)
                {
                    instance.audioSrc.PlayOneShot(instance.win, 0.5f);
                }
                break;
        }
    }
}
