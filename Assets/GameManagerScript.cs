using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{

    public GameObject gameOverUI, timer, player, boss, controlUI;

    public void GameOver()
    {
        gameOverUI.SetActive(true);
        timer.SetActive(false);
        player.SetActive(false);
        boss.SetActive(false);
    }

    public void OnRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }


}
