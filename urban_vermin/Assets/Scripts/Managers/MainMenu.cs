using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private AudioClip titleMusic;
    private AudioClip levelMusic;

    private float sliderLevel;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayMusic(titleMusic);
    }

    public void NextLevel ()
    {
        if(SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCount)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            //LoadLevel(0);
        }
    }

    public void LoadLevel (int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }

    public void OpenCloseMenu (GameObject menu)
    {
        menu.SetActive(!menu.activeSelf);
    }

    public void ChangeAudio (GameObject slider)
    {
        sliderLevel = slider.GetComponent<Slider>().value;
        GameObject[] allObjects = FindObjectsOfType<GameObject>(); foreach (Object o in allObjects)
        foreach(GameObject oB in allObjects)
        {
            if(oB.GetComponent<AudioSource>() != null)
            {
                oB.GetComponent<AudioSource>().volume = sliderLevel;
            }
        }
    }

    public void LeaveGame ()
    {
        Application.Quit();
    }
}
