using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkedClientProcessing
{
    #region Send and Receive Data Functions
    static public void ReceivedMessageFromServer(string msg, int id)
    {

        Debug.Log("msg received = " + msg + ".");

        string[] csv = msg.Split(',');
        int signifier = int.Parse(csv[0]);

        string[] temp = msg.Split(',');
        int signifierID = int.Parse(temp[0]);
        
        if (signifierID == ServerToClientSignifiers.PlayAsGuestAccount)
        {
            gameLogic.playerID = id;
            gameLogic.PlayAsGuestF();
        }

        if (signifierID == ServerToClientSignifiers.LoginSuccessfull)
        {
            gameLogic.playerID = id;
            gameLogic.LoginSuccessfullF();
        }

        if (signifierID == ServerToClientSignifiers.LoginFailed)
        {

            gameLogic.LoginFailedF();
        }

        if (signifierID == ServerToClientSignifiers.EnterVersusGameMode)
        {

            gameLogic.EnterVersusGameModeF();
        }

        if (signifierID == ServerToClientSignifiers.SendPlayerData)
        {
            gameLogic.SetPlayerData(temp, id);
            gameLogic.SetPlayerID(temp, id);
        }

        if (signifierID == ServerToClientSignifiers.CheckIfPlayerIsReady)
        {
            gameLogic.PlayerReady(id);
        }

        if (signifierID == ServerToClientSignifiers.SendPlayerToMainMenu)
        {
            gameLogic.SendingPlayerToMenu();
        }

    }

    static public void SendMessageToServer(string msg)
    {
        networkedClient.SendMessageToServer(msg);
    }

    #endregion

    #region Connection Related Functions and Events
    static public void ConnectionEvent()
    {
        Debug.Log("Network Connection Event!");
    }
    static public void DisconnectionEvent()
    {
        Debug.Log("Network Disconnection Event!");
    }
    static public bool IsConnectedToServer()
    {
        return networkedClient.IsConnected();
    }
    static public void ConnectToServer()
    {

        networkedClient.Connect();
    }
    static public void DisconnectFromServer()
    {
        networkedClient.Disconnect();
    }

    #endregion

    #region Setup
    static NetworkedClient networkedClient;
    static GameLogic gameLogic;

    static public void SetNetworkedClient(NetworkedClient NetworkedClient)
    {
        networkedClient = NetworkedClient;
    }
    static public NetworkedClient GetNetworkedClient()
    {
        return networkedClient;
    }
    static public void SetGameLogic(GameLogic GameLogic1)
    {
        gameLogic = GameLogic1;
    }

    #endregion

}

#region Protocol Signifiers
static public class ClientToServerSignifiers
{
    public const int PlayAsGuestAccount = 0;
    public const int CreatePlayerAccount = 1;
    public const int Login = 2;
    public const int LoginFailed = 3;
    public const int LoginSuccessfull = 4;
    public const int CreatePrivateLobby = 5;
    public const int EnterVersusGameMode = 6;
    public const int JoinPrivateRoom = 7;

    public const int SendPlayerData = 8;
    public const int CheckIfPlayerIsReady = 9;
    public const int SendPlayerToMainMenu = 10;

}

public static class ServerToClientSignifiers
{
    public const int PlayAsGuestAccount = 0;
    public const int CreatePlayerAccount = 1;
    public const int Login = 2;
    public const int LoginFailed = 3;
    public const int LoginSuccessfull = 4;
    public const int CreatePrivateLobby = 5;
    public const int EnterVersusGameMode = 6;
    public const int JoinPrivateRoom = 7;

    public const int SendPlayerData = 8;
    public const int CheckIfPlayerIsReady = 9;
    public const int SendPlayerToMainMenu = 10;
}


#endregion

