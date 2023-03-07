using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickingCards : MonoBehaviour
{
    public GameObject cardSpawnLocation; 
    public GameObject deckSpawnLocation;
    public GameObject gameDeckLocation;

    private GameObject player1;
    public GameObject cardPref;
    public GameObject gameManager;

    public GameObject Scene1;
    public GameObject Scene2;
    public GameObject Scene3;

    public List<GameObject> pickingCardList;

   // public int cardPoolSize;

    public int maxActions = 0;
    public int actions = 5;

    bool called = false;

    // Start is called before the first frame update
    void Start()
    {
        maxActions = 0;
        actions = 5;
        player1 = GameObject.Find("Player1");
        gameManager = GameObject.Find("GameManager");

        OnSceneLoad();
    }

    // Update is called once per frame
    void Update()
    {
        if (actions <= maxActions && called == false)
        {
            gameManager.GetComponent<GameState>().gameState = 1;
            ChangeScene(1);
            ReOrderCards();

            RemoveLeftOverCards();


            called = true;
            actions = 3;
        }
    }

    public void OnSceneLoad()
    {
        called = false;
        
        foreach(GameObject card in player1.GetComponent<Player>().deck)
        {
            card.GetComponent<Card>().isPoisoned = false;
        }

        DisplayCardsToPick();

    }

    public void ReOrderCards()
    {
        float offset = 2.0f;
        for (int x = 0; x < player1.GetComponent<Player>().deck.Count; x++)
        {
            player1.GetComponent<Player>().deck[x].transform.position = new Vector3(deckSpawnLocation.transform.position.x + offset, deckSpawnLocation.transform.position.y, deckSpawnLocation.transform.position.z);
            player1.GetComponent<Player>().deck[x].GetComponent<Card>().interactable = true;
            player1.GetComponent<Player>().deck[x].SetActive(true);
            player1.GetComponent<Player>().deck[x].transform.parent = deckSpawnLocation.transform;

            offset += 1.5f;
        }

        int temp = player1.GetComponent<Player>().deckOrdered.Count;
        for (int x = 0; x < temp; x++)
        {
            player1.GetComponent<Player>().deckOrdered.RemoveAt(0);
        }
    }

    public void DisplayCardsToPick()
    {
        GameObject temp;
        float offset = 2.0f;
        

        for (int x = 0; x < 5; x++)
        {
            if (cardPref.GetComponent<Card>().inScene == false)
            {
                temp = Instantiate(cardPref, new Vector3(cardSpawnLocation.transform.position.x + offset, cardSpawnLocation.transform.position.y, cardSpawnLocation.transform.position.z), transform.rotation);
                temp.transform.parent = cardSpawnLocation.transform;
                temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                temp.GetComponent<Card>().inScene = true;
                pickingCardList.Add(temp);
                offset += 2.0f;
            }
        }
    }

    public void ReRoll()
    {
        actions--;

        for (int x = 0; x < 5; x++)
        {
            GameObject temp = pickingCardList[0];

            if (temp.GetComponent<Card>().chosen == false)
            {
                Destroy(temp);
            }

            pickingCardList.RemoveAt(0);
        }

        DisplayCardsToPick();
    }

    public void ChangeScene(int state)
    {
        int temp = state;

        if(temp == 1)
        {
            Scene1.SetActive(false);
            Scene2.SetActive(true);
        }
        else if (temp == 2)
        {
            Scene2.SetActive(false);
            Scene3.SetActive(true);
        }
        
    } 


    private void RemoveLeftOverCards()
    {
        int tempCount = pickingCardList.Count;
        for (int x = 0; x < player1.GetComponent<Player>().deck.Count;  x++)
        {
            for(int y = 0; y < tempCount; y++)
            {
                if(player1.GetComponent<Player>().deck[x] == pickingCardList[y])
                {
                    pickingCardList.RemoveAt(y);
                }
            }
        }

        for (int f = 0; f < tempCount; f++)
        {
            Destroy(pickingCardList[0]);
            pickingCardList.RemoveAt(0);
        }
    }

   
}
