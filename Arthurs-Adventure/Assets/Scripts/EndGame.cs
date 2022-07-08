using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndGame : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    
    private void Awake() 
    {
        DisplayScore();
    }
    public void RestartGame()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numGameSessions < 1) { return; }
        FindObjectOfType<GameSession>().ResetGameSession();
    }

    public void DisplayScore()
    {
        scoreText.text = FindObjectOfType<GameSession>().GetScore();
        Debug.Log(scoreText);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
