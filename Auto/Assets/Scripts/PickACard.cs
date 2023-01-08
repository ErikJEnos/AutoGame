using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickACard : MonoBehaviour
{
    public List<GameObject> cards;
    public GameObject CardSpawnLocation;
    public GameObject deckSpawnLocation;
    public GameObject gameDeckLocation;
    
    public GameObject Player;
    public GameObject Scene1;
    public GameObject Scene2;
    public GameObject Scene3;
    public GameObject enemy;
    public GameObject card;
    public bool called = false;

    public int cardPoolSize = 5;
    public int deckSize = 0;

    public TMP_Text picksLeftTxt;
    public int pickLeftInt = 5;


    GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        OnSceneLoad();
    }

    // Update is called once per frame
    void Update()
    {
        picksLeftTxt.text = pickLeftInt.ToString();

        if (Player.GetComponent<Player>().deck.Count >= deckSize && called == false)
        {
            int c = cards.Count;

            for (int x = 0; x < c; x++)
            {
                cards[x].transform.parent = null;
            }

            Scene1.SetActive(false);
            Scene2.SetActive(true);
            
            float offset = 2.0f;
            gameManager.GetComponent<GameState>().gameState = 1;

            foreach (GameObject card in Player.GetComponent<Player>().deck)
            {
                card.SetActive(true);
                card.GetComponent<Card>().interactable = true;
                card.transform.position = new Vector3(deckSpawnLocation.transform.position.x + offset, deckSpawnLocation.transform.position.y, deckSpawnLocation.transform.position.z);
                card.transform.parent = deckSpawnLocation.transform;
                offset += 1.5f;
            }
            
            called = true;

            for(int x = 0; x < 5; x++)
            {
                cards.RemoveAt(0);
            }
        }
    }

    public void StartBattleButton()
    {
        if(Player.GetComponent<Player>().deckOrdered.Count >= 5)
        {
            pickLeftInt = 5;
            Scene1.SetActive(false);
            Scene2.SetActive(false);
            Scene3.SetActive(true);
            gameManager.GetComponent<GameState>().gameState = 2;
            gameManager.GetComponent<GameLoop>().hasWon = false;
            gameManager.GetComponent<GameLoop>().OnStart();
            enemy.GetComponent<Enemy>().OnEnemyStart();
        
            foreach (GameObject card in Player.GetComponent<Player>().deckOrdered)
            {
                card.SetActive(true);
                card.GetComponent<Card>().playerID = 1;
                card.transform.position = new Vector3(gameDeckLocation.transform.position.x, gameDeckLocation.transform.position.y, gameDeckLocation.transform.position.z);
                card.transform.parent = gameDeckLocation.transform;
            }
        }
    }

    public void OnSceneLoad()
    {
        deckSize = deckSize + 5;
        called = false;

        DisplayCardsToPick();

    }

    public void DisplayCardsToPick()
    {
        GameObject temp;
        float offset = 2.0f;


        for (int x = 0; x < 5; x++)
        {
            if (card.GetComponent<Card>().inScene == false)
            {
                Debug.Log("cards");
                temp = Instantiate(card, new Vector3(CardSpawnLocation.transform.position.x + offset, CardSpawnLocation.transform.position.y, CardSpawnLocation.transform.position.z), transform.rotation);
                temp.transform.parent = CardSpawnLocation.transform;
                temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                temp.GetComponent<Card>().inScene = true;
                cards.Add(temp);
                offset += 2.0f;
            }
        }
    }

    public void ReOrderCards()
    {
        Debug.Log(cards.Count);
        float offset = 2.0f;
        for (int x = 0; x < Player.GetComponent<Player>().deck.Count; x++)
        {
            Player.GetComponent<Player>().deck[x].transform.position = new Vector3(deckSpawnLocation.transform.position.x + offset, deckSpawnLocation.transform.position.y, deckSpawnLocation.transform.position.z);
            Player.GetComponent<Player>().deck[x].GetComponent<Card>().interactable = true;
            
            offset += 1.5f;
        }

        int temp = Player.GetComponent<Player>().deckOrdered.Count;
        for (int x = 0; x < temp; x++)
        {
            Player.GetComponent<Player>().deckOrdered.RemoveAt(0);
        }
    }

    public void ReRoll()
    {
        for (int x = 0; x < 5; x++)
        {
            GameObject temp = cards[0];
            
            if (temp.GetComponent<Card>().chosen == false)
            {
                Destroy(temp);
            }
            
            cards.RemoveAt(0);
        }

        DisplayCardsToPick();
    }
}
