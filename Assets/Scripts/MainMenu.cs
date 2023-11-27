using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void playButton()
    {
        SceneManager.LoadScene(2);
    }

    public void selectScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void quitButton()
    {
        Application.Quit();
    }
}
