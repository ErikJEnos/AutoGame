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

    public int playerID = 0; 

    public GameObject gameManager;
    public GameObject menuManager;

    public GameObject player1;
    public GameObject player2;

    public GameObject cardPref;

    public GameObject enemyDeckPos;

    public List<int> players;

    public int playerTurn = 1;
    public int playerWins = 0;
    public int playerloses = 10;
    public int actions = 5;

    public TMP_Text playerTurnText;
    public TMP_Text playerWinsText;
    public TMP_Text playerlosesText;
    public TMP_Text actionsText;

    int offset = 0;
    int count = 0;

    public List<string> clientChatlog;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        playerloses = 10;
        NetworkedClientProcessing.SetGameLogic(this);
        gameManager = GameObject.Find("GameManager");
        menuManager = GameObject.Find("MenuManager");

        connectedClientIDs = new LinkedList<int>();
    }

    private void Update()
    {
        if(gameManager != null)
        {
            actions = gameManager.GetComponent<PickingCards>().actions;

            playerTurnText.text = playerTurn.ToString();
            playerWinsText.text = playerWins.ToString();
            playerlosesText.text = playerloses.ToString();
            actionsText.text = actions.ToString();
        }
      
    }

    public void PlayAsGuestF()
    {
        menuManager.GetComponent<AccountLogin>().GoToMainMenu();
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

    public void WaitingForAnotherPlayerF()
    {
        //gameManager.GetComponent<GameRoom>().waitingForPlayerText.SetActive(true);
       // gameManager.GetComponent<GameRoom>().buttonBack.SetActive(true);
    }

    public void EnterPlayStateF()
    {
       // gameManager.GetComponent<GameRoom>().GamePanel.SetActive(true);
       // gameManager.GetComponent<GameRoom>().RoomPanel.SetActive(false);
        // gameManager.GetComponent<StateMachineManger>().roomSystemState.ButtonPress(gameManager.GetComponent<StateMachineManger>());

    }

    public void EnterVersusGameModeF()
    {
        SceneManager.LoadScene("VersusMode");
    }

    public void SetPlayerData(string[] cards, int id)
    {
        Debug.Log("Called: " + count);
        enemyDeckPos = GameObject.Find("EnemyDeck");
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");

        if(player2.GetComponent<Player>().deckOrdered.Count < 5)
        {
            GameObject temp = Instantiate(cardPref, new Vector3(enemyDeckPos.transform.position.x + offset, enemyDeckPos.transform.position.y, enemyDeckPos.transform.position.z), transform.rotation);
            temp.transform.parent = enemyDeckPos.transform;
            temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            temp.GetComponent<Card>().playerID = int.Parse(cards[4]);
            temp.GetComponent<Card>().setCardId(int.Parse(cards[1]));
            temp.GetComponent<Card>().cardLevel = int.Parse(cards[2]);
            temp.GetComponent<Card>().CheckCardLevel();
            player2.GetComponent<Player>().deckOrdered.Add(temp);
            player2.GetComponent<Player>().id = int.Parse(cards[4]);

            if (player2.GetComponent<Player>().deckOrdered.Count == 5)
            {
                player2.GetComponent<Player>().isReady = true;
            }
        }
       
    }

    public void PlayerReady(int id)
    {
        Debug.Log("Adding player: " + id);
        players.Add(id);
    }

    public void SetPlayerID(string[] otherPlayerId, int playerId)
    {
       player1.GetComponent<Player>().id = int.Parse(otherPlayerId[3]);
       player2.GetComponent<Player>().id = int.Parse(otherPlayerId[4]);

       gameManager.GetComponent<GameLoop>().SetPlayerDeckIDs(int.Parse(otherPlayerId[3]));

    }

    public void SendingPlayerToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
