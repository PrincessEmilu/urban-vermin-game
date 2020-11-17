using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private float sliderLevel;

    public void NextLevel ()
    {
        if(SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCount)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            LoadLevel(0);
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
