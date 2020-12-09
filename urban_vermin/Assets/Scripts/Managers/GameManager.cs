using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public AudioManager audioManager;

    public AudioClip titleMusic;
    public AudioClip levelMusic;
    public AudioClip bossMusic;
    public AudioClip creditsMusic;

    // Start is called before the first frame update
    void Start()
    {
        if (audioManager == null)
            Debug.LogError("Audio Manager is null!");

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            PlayMusic(titleMusic);
        else if (SceneManager.GetActiveScene().buildIndex == 1)
            PlayMusic(levelMusic);
        else if (SceneManager.GetActiveScene().buildIndex == 5)
            PlayMusic(bossMusic);
        else if (SceneManager.GetActiveScene().buildIndex == 6)
            PlayMusic(creditsMusic);
    }

    public void PlaySound(AudioClip clip)
    {
        audioManager.PlaySFX(clip);
    }

    public void PlayMusic(AudioClip music)
    {
        audioManager.PlayMusic(music);
    }
}
