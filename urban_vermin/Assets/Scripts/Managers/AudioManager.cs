using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private float musicVolume;
    private float sfxVolume;

    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;

    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private AudioSource musicSource;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        // Sets the playerprefs for the first time loaded. Otherwise, read in saved values
        if (PlayerPrefs.GetString("PrefsSavedPreviously") == "")
        {
            musicVolume = 1.0f;
            sfxVolume = 1.0f;

            PlayerPrefs.SetFloat("musicVolume", musicVolume);
            PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
            PlayerPrefs.SetString("PrefsSavedPreviously", "true");
        }
        else
        {
            musicVolume = PlayerPrefs.GetFloat("musicVolume");
            sfxVolume = PlayerPrefs.GetFloat("sfxVolume");

            musicSlider.SetValueWithoutNotify(musicVolume);
            sfxSlider.SetValueWithoutNotify(sfxVolume);

            musicSource.volume = musicVolume;
        }

        // Add listeners to the sliders and invokes a method when the value changes.
        musicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { SetSFXVolume(); });
    }

    public void SetSFXVolume()
    {
        Debug.Log("SFX volume changed");
        sfxVolume = sfxSlider.value;

        if (sfxVolume > 1.0f)
            sfxVolume = 1.0f;
        else if (sfxVolume < 0.0f)
            sfxVolume = 0.0f;
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
    }

    public void SetMusicVolume()
    {

        Debug.Log("Music volume changed");
        musicVolume = musicSlider.value;

        if (musicVolume > 1.0f)
            musicVolume = 1.0f;
        else if (musicVolume < 0.0f)
            musicVolume = 0.0f;

        musicSource.volume = musicVolume;
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
    }

    public void PlaySFX(AudioClip audioClip)
    {
        sfxSource.PlayOneShot(audioClip, sfxVolume);
    }

    public void PlayMusic(AudioClip music)
    {
        if (music != musicSource.clip)
        {
            musicSource.Stop();
            musicSource.clip = music;
            musicSource.Play();
        }
    }
}
