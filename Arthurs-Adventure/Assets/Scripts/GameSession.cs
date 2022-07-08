using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 100;

    [SerializeField] int playerDeathCoinsLost = 500;

    [SerializeField] float loadSceneDelay = 2f;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;

    void Awake() 
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numGameSessions > 1) 
        {
            Destroy(gameObject);
        }  
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start() 
    {
        livesText.text = playerLives.ToString(); 
        scoreText.text = score.ToString();     
    }
    public void ProcessPlayerDeath()
    {
        if(playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            playerLives--;
            FindObjectOfType<AudioManager>().Stop("Theme");
            FindObjectOfType<AudioManager>().Play("Game Over", 0.75f);
            LoadEndScreen();
        }
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = score.ToString();
    }

    public void ReduceScoreOnFire(int pointsToSub)
    {
        score -= pointsToSub;
        scoreText.text = score.ToString();
    }

    public string GetScore()
    {
        return scoreText.text;
    }

    public int GetCoins()
    {
        return score;
    }

    void TakeLife()
    {
        playerLives--;
        if(score >= 500)
        {
            score -= playerDeathCoinsLost;
        }
        else
        {
            score = 0;
        }        
        scoreText.text = score.ToString();
        
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadSceneDelay());
    }

    IEnumerator LoadSceneDelay()
    {
        yield return new WaitForSecondsRealtime(loadSceneDelay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playerLives.ToString();
    }
    public void ResetGameSession()
    {
        SceneManager.LoadScene("1");
        FindObjectOfType<AudioManager>().Play("Theme", 0f);
        Destroy(gameObject);
    }

    public void ResetGameSessionWithPause()
    {
        Destroy(gameObject);
    }

    void LoadEndScreen()
    {
        livesText.text = playerLives.ToString();
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene("End Screen");
    }

    void OnPause()
    {
        FindObjectOfType<PauseMenu>().DisplayPauseMenu();
    }
}
