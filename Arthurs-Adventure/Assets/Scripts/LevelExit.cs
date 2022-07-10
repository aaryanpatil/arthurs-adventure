using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 4f;
    private bool sfxPlayed = false;

    ParticleSystem myParticleSystem;
    void Start() 
    {
        myParticleSystem = GetComponent<ParticleSystem>();
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {

        if(other.tag == "Player" && !sfxPlayed)
        {
            FindObjectOfType<AudioManager>().Play("Level Complete", 0f);
            sfxPlayed = true;
            FindObjectOfType<Rigidbody2D>().velocity = new Vector2(0f,0f);
            myParticleSystem.Play();
            StartCoroutine(LoadNextLevel());
            FindObjectOfType<SceneTransition>().MakeTransition();
        }      
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        sfxPlayed = false;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene("End Screen");
        }

        FindObjectOfType<ScenePersist>().ResetScenePersist();
        
        SceneManager.LoadScene(nextSceneIndex);      
        FindObjectOfType<PlayerMovement>().enabled = true;
    }
}
