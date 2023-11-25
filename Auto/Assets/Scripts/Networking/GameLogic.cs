using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;
public class GameLogic : MonoBehaviour
{
    LinkedList<int> connectedClientIDs;

    public int player1ID;
    public int player2ID;

    public GameObject gameManager;
    public GameObject menuManager;
    public GameObject networkManager;
    public GameObject LevelLoader;

    public GameObject player1;
    public GameObject player2;

    public GameObject cardPref;

    public GameObject enemyDeckPos;

    public List<int> players;

    public int playerTurn = 1;
    public int playerWins = 0;
    public int playerloses = 10;
    public int actions = 10;

    public TMP_Text playerTurnText;
    public TMP_Text playerWinsText;
    public TMP_Text playerlosesText;
    public TMP_Text actionsText;


    public List<string> clientChatlog;

    int offset = 0;

    void Start()
    {

        NetworkedClientProcessing.SetGameLogic(this);
        gameManager = GameObject.Find("MenuManager");
        LevelLoader = GameObject.Find("LevelLoader");
        networkManager = gameObject;

        connectedClientIDs = new LinkedList<int>();

    }


    public void PlayAsGuestF()
    {
        gameManager.GetComponent<AccountLogin>().GoToMainMenu();
    }

    public void AccountCompleteF()
    {
        Debug.Log("AccountComplete");
    }

    public void AccountFailedF()
    {
        Debug.Log("AccountFailed");
    }

    public void LoginSuccessfullF()
    {
        menuManager.GetComponent<AccountLogin>().GoToMainMenu();
        Debug.Log("LoginSuccessfull");
    }

    public void LoginFailedF()
    {
        Debug.Log("LoginFailed");

    }


    public void EnterVersusGameModeF(string[] player, int id)
    {

        player1ID = int.Parse(player[2]);
        player2ID = int.Parse(player[1]);

        LevelLoader.GetComponent<LevelLoader>().LoadNextLevel();
    }

    public void SetPlayerData(string[] cards, int id)
    {
        enemyDeckPos = GameObject.Find("EnemyDeck");
        player2 = GameObject.Find("Player2");

        if(player2.GetComponent<Player>().deckOrdered.Count < 5)
        {
            GameObject temp = Instantiate(cardPref, new Vector3(enemyDeckPos.transform.position.x + offset, enemyDeckPos.transform.position.y, enemyDeckPos.transform.position.z), transform.rotation);
            temp.transform.parent = enemyDeckPos.transform;
            temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            temp.GetComponent<Card>().playerID = player2ID;
            temp.GetComponent<Card>().setCardId(int.Parse(cards[1]));
            temp.GetComponent<Card>().cardLevel = int.Parse(cards[2]);
            temp.GetComponent<Card>().CheckCardLevel();
            player2.GetComponent<Player>().deckOrdered.Add(temp);
            player2.GetComponent<Player>().isReady = true;

         }
       
    }

    public void PlayerReady(int id)
    {
        Debug.Log("Adding player: " + id);
        players.Add(id);
    }

    public void SendingPlayerToMenu()
    {
        LevelLoader.GetComponent<LevelLoader>().LoadMainMenuLevel();
        //SceneManager.LoadScene("MainMenu");
    }

}
