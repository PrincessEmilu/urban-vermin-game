using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioManager audioManager;

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
