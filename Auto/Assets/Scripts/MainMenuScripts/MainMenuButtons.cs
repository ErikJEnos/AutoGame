using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using TMPro;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public GameObject howToPlayPanel;
    public GameObject customizePanel;
    public GameObject creditsPanel;
    public GameObject achievementsPanel;


    public GameObject OptSubPanel;
    public GameObject OptSubPanel1;
    public GameObject OptSubPanel2;
    public GameObject OptSubPanel3;
    public GameObject OptSubPanel4;
    public Slider soundSlider;
    public TMP_Text soundTxt;
    public Slider musicSlider;
    public TMP_Text musicTxt;

    public GameObject Canvas;

    private void Update()
    {
        AudioSliders();
    }

    public void CloseMenu()
    {
       CanvasObject.logined = 0;
    }

    public void BackToMenuButton()
    {

        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        customizePanel.SetActive(false);
        creditsPanel.SetActive(false);
        achievementsPanel.SetActive(false);

        OptSubPanel.SetActive(false);
        OptSubPanel1.SetActive(false);
        OptSubPanel2.SetActive(false);
        OptSubPanel3.SetActive(false);
        OptSubPanel4.SetActive(false);
    }

    public void HowToPlayButton()
    {

        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(false);
        howToPlayPanel.SetActive(true);
        customizePanel.SetActive(false);
        creditsPanel.SetActive(false);
        achievementsPanel.SetActive(false);
    }

    public void CustomizeButton()
    {

        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        customizePanel.SetActive(true);
        creditsPanel.SetActive(false);
        achievementsPanel.SetActive(false);
    }

    public void CreditsButton()
    {

        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        customizePanel.SetActive(false);
        creditsPanel.SetActive(true);
        achievementsPanel.SetActive(false);
    }

    public void AchievementsButton()
    {

        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        customizePanel.SetActive(false);
        creditsPanel.SetActive(false);
        achievementsPanel.SetActive(true);
    }

    public void OptionsButton()
    {

        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
        howToPlayPanel.SetActive(false);
        customizePanel.SetActive(false);
        creditsPanel.SetActive(false);
        achievementsPanel.SetActive(false);

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

    public void AudioSliders()
    {
        soundTxt.text = soundSlider.value.ToString("0") + "%";
        musicTxt.text = musicSlider.value.ToString("0") + "%";
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
    
    public void QuickPlay()
    {
        
    }


    public void QuitButton()
    {
        Application.Quit();
    }
}
