using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private AudioSource music;
    private bool pause = false;
    public void OnPlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnMusicButton()
    {
        music = gameObject.GetComponent<AudioSource>();
        if (!pause)
        {
            music.Pause();
        }
        else
        {
            music.UnPause();
        }
        pause = !pause;
    }
}
