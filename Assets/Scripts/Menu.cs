using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    public ScrollSnapping biomeScroller;

    public int selectedBiomeIndex = 0;

    public void Play()
    {
        SceneManager.LoadScene(biomeScroller.closestButtonIndex + 1);
    }

    public void Home()
    {
        Unpause();
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Start()
    {
        //PlayerPrefs.SetString("UnlockedCharacters", "01");
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Unpause()
    {
        Time.timeScale = 1;
    }
}