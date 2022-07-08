using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] Slider loadingBar;
    [SerializeField] float loadTime = 3f;
    float currentTime = 0f;

    float progressValue;
    void Awake()
    {
        StartCoroutine(LoadSceneDelay());
    }
    void Update() 
    {   
        currentTime +=  Time.deltaTime;
        progressValue = currentTime / (loadTime - 1);
        loadingBar.value = progressValue;
    }
    IEnumerator LoadSceneDelay()
    {
        yield return new WaitForSecondsRealtime(loadTime);
        LoadMainMenu();
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("Game Start");
    }
}
