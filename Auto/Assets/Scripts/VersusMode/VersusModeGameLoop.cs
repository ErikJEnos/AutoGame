using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VersusModeGameLoop : MonoBehaviour
{
    public GameObject player;
    public GameObject hand;


    private GameObject gameManager;
    private GameObject netWorkedClient;

    private GameObject gameLogic;
    public GameObject enemyDeckPos;

    public GameObject lockScreen;

    public bool done = false;

    public bool checkedPl = false;




    private void Awake()
    {
        CanvasObject.logined += 1;
        player = GameObject.Find("Player1");
        gameManager = GameObject.Find("GameManager");
        netWorkedClient = GameObject.Find("NetworkClient");
        gameLogic = GameObject.Find("NetworkClient");
    }

    // Update is called once per frame
    void Update()
    {
        if (gameLogic.GetComponent<GameLogic>().players.Count >= 1 && !done)
        {
            lockScreen.SetActive(false);

            player.GetComponent<Player>().deckOrdered.Reverse();

            SetUpPlayerDecks();

            gameManager.GetComponent<GameLoop>().hasWon = false;
            gameManager.GetComponent<GameLoop>().OnStart();

            done = true;

            gameLogic.GetComponent<GameLogic>().players.RemoveAt(0);
        }

    }

    public void SetUpPlayerDecks()
    {
        for(int x = 0; x < player.GetComponent<Player>().deckOrdered.Count; x++)
        {
            netWorkedClient.GetComponent<NetworkedClient>().SendMessageToServer(ClientToServerSignifiers.SendPlayerData + "," + player.GetComponent<Player>().deckOrdered[x].GetComponent<Card>().cardID + "," + player.GetComponent<Player>().deckOrdered[x].GetComponent<Card>().cardLevel + ",");
        }
    }

    public void StartBattleButton()
    {
        hand.GetComponent<DragToHand>().AddToPlayerDeck();



        if (!checkedPl && player.GetComponent<Player>().deckOrdered.Count > 0)
        {
            lockScreen.SetActive(true);
            netWorkedClient.GetComponent<NetworkedClient>().SendMessageToServer(ClientToServerSignifiers.CheckIfPlayerIsReady + ",");
            checkedPl = true;
            
        }
    }

    public void SetUpPlayer1Deck()
    {
        player = GameObject.Find("Player1");
        GameObject DeckPos = GameObject.Find("GameDeck");
        int offset = 0;
        

        foreach (GameObject card in player.GetComponent<Player>().deckOrdered)
        {
            card.GetComponent<Card>().playerID = player.GetComponent<Player>().id; 
            card.transform.parent = DeckPos.transform;
            card.transform.position = DeckPos.transform.position;
            //card.transform.position = new Vector3(DeckPos.transform.position.x + offset, DeckPos.transform.position.y, DeckPos.transform.position.z);
        }
    }
}
