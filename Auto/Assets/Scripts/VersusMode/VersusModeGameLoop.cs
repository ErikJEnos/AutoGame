using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersusModeGameLoop : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    private GameObject gameManager;
    private GameObject netWorkedClient;

    private GameObject gameLogic;


    public GameObject enemyDeckPos;

    public bool done = false;

    public bool checkedPl = false;

    private void Awake()
    {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        gameManager = GameObject.Find("GameManager");
        netWorkedClient = GameObject.Find("NetworkClient");
        gameLogic = GameObject.Find("GameLogic");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameLogic.GetComponent<GameLogic>().players.Count >= 1 && !done)
        {
            SetUpPlayerDecks();

            gameManager.GetComponent<GameLoop>().hasWon = false;
            gameManager.GetComponent<GameLoop>().OnStart();

            done = true;

            gameLogic.GetComponent<GameLogic>().players.RemoveAt(0);
        }

    }

    public void SetUpPlayerDecks()
    {
        for(int x = 0; x < player1.GetComponent<Player>().deckOrdered.Count; x++)
        {
            netWorkedClient.GetComponent<NetworkedClient>().SendMessageToServer(ClientToServerSignifiers.SendPlayerData + "," + player1.GetComponent<Player>().deckOrdered[x].GetComponent<Card>().cardID + "," + player1.GetComponent<Player>().deckOrdered[x].GetComponent<Card>().cardLevel + ",");
        }
    }

    public void StartBattleButton()
    {
        if (!checkedPl)
        {
            netWorkedClient.GetComponent<NetworkedClient>().SendMessageToServer(ClientToServerSignifiers.CheckIfPlayerIsReady + ",");
            checkedPl = true;

            
        }
    }

    public void SetUpPlayer1Deck()
    {
        player1 = GameObject.Find("Player1");
        GameObject DeckPos = GameObject.Find("GameDeck");
        int offset = 0;
        foreach (GameObject card in player1.GetComponent<Player>().deckOrdered)
        {
            card.GetComponent<Card>().playerID = player1.GetComponent<Player>().id;
            card.transform.parent = DeckPos.transform;
            card.transform.position = new Vector3(DeckPos.transform.position.x + offset, DeckPos.transform.position.y, DeckPos.transform.position.z);
        }
    }
}
