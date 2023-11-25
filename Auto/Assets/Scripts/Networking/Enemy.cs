using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public List<GameObject> deck;
    public List<GameObject> cardPool;
    private int deckSize; 
    GameObject gameManager;
    bool spawned = false;

    void SpawnEnemyDeckToScene()
    {
        if (gameManager.GetComponent<GameState>().gameState == 2 && spawned == false)
        {

            foreach (GameObject card in deck)
            {
                card.GetComponent<Card>().playerID = 2;
                GameObject temp = Instantiate(card, new Vector3(gameManager.GetComponent<GameLoop>().enemyDeckPos.transform.position.x, gameManager.GetComponent<GameLoop>().enemyDeckPos.transform.position.y, gameManager.GetComponent<GameLoop>().enemyDeckPos.transform.position.z), transform.rotation);
                temp.transform.parent = gameManager.GetComponent<GameLoop>().enemyDeckPos.transform;
                temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                temp.GetComponent<Card>().playerID = 2; 
                gameManager.GetComponent<GameLoop>().enemyDeckOrder.Add(temp);
            }
            spawned = true;
        }

    }

    public void OnEnemyStart()
    {
        spawned = false;

        if (deck.Count > 0)
        {
            for(int x = 0; x < deckSize; x++)
            {
                deck.RemoveAt(0);
            }
        }

        deckSize = 5;
        gameManager = GameObject.Find("GameManager");
        GameObject temp;

        for (int x = 0; x < deckSize; x++)
        {
            int rand = Random.RandomRange(1, 4);
            temp = cardPool[rand];
            temp.GetComponent<Card>().playerID = 2;
            deck.Add(temp);
        }

        SpawnEnemyDeckToScene();
    }
}
