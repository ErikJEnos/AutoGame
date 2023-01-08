using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject optionsPanel;

    public GameObject OptSubPanel;
    public GameObject OptSubPanel1;
    public GameObject OptSubPanel2;
    public GameObject OptSubPanel3;
    public GameObject OptSubPanel4;
    public Slider soundSlider;
    public TMP_Text soundTxt;
    public Slider musicSlider;
    public TMP_Text musicTxt;

    private int speedState = 0;

    // Update is called once per frame
    void Update()
    {
        AudioSliders();
    }

    public void AudioSliders()
    {
        soundTxt.text = soundSlider.value.ToString("0") + "%";
        musicTxt.text = musicSlider.value.ToString("0") + "%";
    }

    public void OptionsButton()
    {
        pausePanel.SetActive(true);
        optionsPanel.SetActive(false);
        
    }

    public void BackButton()
    {
        pausePanel.SetActive(true);
        optionsPanel.SetActive(false);

    }

    public void settingButton()
    {
        pausePanel.SetActive(false);
        optionsPanel.SetActive(true);


        OptSubPanel.SetActive(true);
        OptSubPanel1.SetActive(false);
        OptSubPanel2.SetActive(false);
        OptSubPanel3.SetActive(false);
        OptSubPanel4.SetActive(false);
    }

    public void GeneralButton()
    {
        OptSubPanel.SetActive(true);
        OptSubPanel1.SetActive(false);
        OptSubPanel2.SetActive(false);
        OptSubPanel3.SetActive(false);
        OptSubPanel4.SetActive(false);
    }
    public void GameplayButton()
    {
        OptSubPanel.SetActive(false);
        OptSubPanel1.SetActive(true);
        OptSubPanel2.SetActive(false);
        OptSubPanel3.SetActive(false);
        OptSubPanel4.SetActive(false);
    }
    public void AudioButton()
    {
        OptSubPanel.SetActive(false);
        OptSubPanel1.SetActive(false);
        OptSubPanel2.SetActive(true);
        OptSubPanel3.SetActive(false);
        OptSubPanel4.SetActive(false);
    }

    public void DisplayButton()
    {
        OptSubPanel.SetActive(false);
        OptSubPanel1.SetActive(false);
        OptSubPanel2.SetActive(false);
        OptSubPanel3.SetActive(true);
        OptSubPanel4.SetActive(false);
    }
    public void PrivacyButton()
    {
        OptSubPanel.SetActive(false);
        OptSubPanel1.SetActive(false);
        OptSubPanel2.SetActive(false);
        OptSubPanel3.SetActive(false);
        OptSubPanel4.SetActive(true);
    }
    public void MiscButton()
    {
        OptSubPanel.SetActive(false);
        OptSubPanel1.SetActive(false);
        OptSubPanel2.SetActive(false);
        OptSubPanel3.SetActive(false);
        OptSubPanel4.SetActive(false);
    }

    public void resume()
    {
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
    }


    public void BackToMainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }


    public void playButton()
    {
        if (gameObject.GetComponent<GameLoop>().isPaused)
        {
            gameObject.GetComponent<GameLoop>().isPaused = false;
        }
        else
        {
            gameObject.GetComponent<GameLoop>().isPaused = true;
        }
       
    }

    public void AutoPlayButton()
    {
        if (gameObject.GetComponent<GameLoop>().isAutoPlay)
        {
            gameObject.GetComponent<GameLoop>().isAutoPlay = false;
        }
        else
        {
            gameObject.GetComponent<GameLoop>().isAutoPlay = true;
        }
    }

    public void SpeedUPplayButton()
    {
        if(speedState == 0) 
        {
            speedState = 1;
            gameObject.GetComponent<GameLoop>().gameSpeed = 0.3f;
        }
        else if(speedState == 1)
        {
            speedState = 0;
            gameObject.GetComponent<GameLoop>().gameSpeed = 1.0f;
        }
        //else if (speedState == 2)
        //{
        //    speedState = 0;
        //    gameObject.GetComponent<GameLoop>().gameSpeed = 0.2f;
        //}

    }
}
