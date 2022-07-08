using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public void LoadSelectedLevel(string index)
    {
        FindObjectOfType<AudioManager>().Play("Theme", 0.5f);
        SceneManager.LoadScene(index);
    }
}
