using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameRoom : MonoBehaviour
{
    public GameObject waitingForPlayerText;
    public TMP_InputField gameRoomName;
    public TMP_InputField joinGameRoomName;
    public GameObject mainMenuPanel;
    public GameObject lobbyPanel;
    public GameObject privateLobbyPanel;
    public GameObject joinPrivateLobbyPanel;

    public GameObject netWorkedClient;

    public bool createRoom = false;
    public bool back = false;

    private void Start()
    {
        netWorkedClient = GameObject.Find("NetworkClient");
    }

    public void createRoomButtonPressed()
    {
        createRoom = true;
    }

    public void createRoomBackButtonPressed()
    {
        back = true;
    }

    public void VersusButton() 
    {
        mainMenuPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public void BackButton()
    {
        mainMenuPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        privateLobbyPanel.SetActive(false);
        joinPrivateLobbyPanel.SetActive(false);

    }

    public void PrivateLobbyButton()
    {
        mainMenuPanel.SetActive(false);
        lobbyPanel.SetActive(false);
        privateLobbyPanel.SetActive(true);

    }

    public void JoinPrivateLobbyButton()
    {
        mainMenuPanel.SetActive(false);
        lobbyPanel.SetActive(false);
        privateLobbyPanel.SetActive(false);
        joinPrivateLobbyPanel.SetActive(true);

    }

    public void CreatePrivateLobbyButton()
    {
        if (gameRoomName.text != "")
        {

            mainMenuPanel.SetActive(true);
            lobbyPanel.SetActive(false);
            privateLobbyPanel.SetActive(false);
            waitingForPlayerText.SetActive(true);
            netWorkedClient.GetComponent<NetworkedClient>().SendMessageToServer(ClientToServerSignifiers.CreatePrivateLobby + "," + gameRoomName.text + ",");

            Debug.Log("Waiting for players");
        }
    }

    public void JoinPrivateGameButton()
    {
        if (joinGameRoomName.text != "")
        {
            mainMenuPanel.SetActive(true);
            lobbyPanel.SetActive(false);
            privateLobbyPanel.SetActive(false);
            netWorkedClient.GetComponent<NetworkedClient>().SendMessageToServer(ClientToServerSignifiers.JoinPrivateRoom + "," + joinGameRoomName.text + ",");
        }
    }
}
