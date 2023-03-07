using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AccountLogin : MonoBehaviour
{
    public TMP_InputField username_Input;
    public TMP_InputField password_Input;

    public TMP_InputField signUpUsername_Input;
    public TMP_InputField signUpEmail_Input;
    public TMP_InputField signUpEmailAgain_Input;
    public TMP_InputField signUpPassword_Input;

    public GameObject failed;
    public GameObject loginPanel;
    public GameObject signUpPanel;
    public GameObject mainMenuPanel;

    public GameObject netWorkedClient;



    public void Start()
    {
        netWorkedClient = GameObject.Find("NetworkClient");
    }

    public void SaveButtonPressed()
    {
     
    }

    public void LoginButtonPressed()
    {
        netWorkedClient.GetComponent<NetworkedClient>().SendMessageToServer(2 + "," + username_Input.text + "," + password_Input.text + ",");// 2 = login signifier
    }

    public void SubmitButtonPressed()
    {
        if (signUpEmail_Input.text == signUpEmailAgain_Input.text && signUpEmail_Input.text != "" && signUpEmailAgain_Input.text != "")
        {
            netWorkedClient.GetComponent<NetworkedClient>().SendMessageToServer(1 + "," + signUpUsername_Input.text + "," + signUpEmail_Input.text + "," + signUpEmailAgain_Input.text + "," + signUpPassword_Input.text + ",");// 1 = Submits sign up signifier
            CancelButtonPressed();
        }
        else
        {
            Debug.Log("Emails don't match");
        }

    }

    public void PlayAsGuestButtonPressed()
    {
        netWorkedClient.GetComponent<NetworkedClient>().SendMessageToServer(0 + ",");// 0 = Guest login signifier
    }

    public void ForgotPasswordButtonPressed()
    {

    }

    public void SignUpButtonPressed()
    {
        loginPanel.SetActive(false);
        signUpPanel.SetActive(true);
    }

    public void CancelButtonPressed()
    {
        loginPanel.SetActive(true);
        signUpPanel.SetActive(false);
    }

    public void GoToMainMenu()
    {
        loginPanel.SetActive(false);
        signUpPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }


}
