using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused;
    public static bool isMute = false;

    public static PauseMenu instance;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        GetComponentInChildren<Canvas>().enabled = isPaused;
    }
    public void DisplayPauseMenu()
    {
        isPaused = !isPaused;
        if(isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        GetComponentInChildren<Canvas>().enabled = isPaused;
    }

    public void MainMenu()
    {
        isPaused = false;
        GetComponentInChildren<Canvas>().enabled = isPaused;
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numGameSessions < 1) { return; }
        FindObjectOfType<GameSession>().ResetGameSessionWithPause();
        SceneManager.LoadScene("Splash Screen");
        FindObjectOfType<AudioManager>().Stop("Theme");
        FindObjectOfType<AudioManager>().Play("Game Load", 0.2f);
        Time.timeScale = 1;
    }

    public void RestartGameFromPause()
    {
        isPaused = false;
        GetComponentInChildren<Canvas>().enabled = isPaused;
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numGameSessions < 1) { return; }
        FindObjectOfType<GameSession>().ResetGameSessionWithPause();
        SceneManager.LoadScene("1");
        Time.timeScale = 1;
    }

    public void AdjustAudio()
    {
        // isMute = !isMute;
        // if (isMute)
        // {
        //     AudioListener.volume = 0;
        // }
        // else
        // {
        //     AudioListener.volume = 1;
        // }

        Slider volume = FindObjectOfType<Slider>();
        AudioListener.volume = volume.normalizedValue;
    }
}
