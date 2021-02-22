using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    private RhythmGamePlayerActions controls;
    public GameObject pauseMenuUI;

    private void Start()
    {
        controls.UI.Pause.performed += _ => pause();
    }

    private void OnEnable()
    {
        controls = new RhythmGamePlayerActions();
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    
    void pause()
    {
        if (GameIsPaused)
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1;
            FMODVibeTracker.musicInstance.setPaused(false);
            GameIsPaused = false;
        }
        else
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0;
            FMODVibeTracker.musicInstance.setPaused(true);
            GameIsPaused = true;
        }
    }

    public void Resume()
    {
        if (GameIsPaused)
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1;
            FMODVibeTracker.musicInstance.setPaused(false);
            GameIsPaused = false;
        }
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        GameIsPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
