using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;

    public static bool paused;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.P))
        {
            if (paused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        PausePanel.SetActive(true);
        AudioManager.Instance.PauseMusic();
        Time.timeScale = 0;
        paused = true;
    }

    public void Resume()
    {
        PausePanel.SetActive(false); 
        AudioManager.Instance.ResumeMusic();
        Time.timeScale = 1;
        paused = false;
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        paused = false;
        SceneManager.LoadScene("TitleScreen");
    }
}
