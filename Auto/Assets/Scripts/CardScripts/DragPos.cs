using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragPos : MonoBehaviour
{

    public GameObject card;
    public GameObject gameManager;
    public GameObject player;
    public bool hovered = false;

    public int pos = 0;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager");
        player = GameObject.Find("Player1");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player.GetComponent<Player>().deckOrdered.Count < 5)
        {
            if (!collision.gameObject.GetComponent<CardDrag>().inArray && card == null)
            {
                Debug.Log(collision.gameObject + "not hovering over card in Array");

            }
            else if (!collision.gameObject.GetComponent<CardDrag>().inArray && card != null)
            {
                Debug.Log(collision.gameObject + "hovering over card in Array");
                player.GetComponent<Player>().deckOrdered.Insert(pos, collision.gameObject);
            }
        }
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<CardDrag>().released == false)
        {
            Debug.Log("asssdadsadsasdasdasda");
            player.GetComponent<Player>().deckOrdered.Remove(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        card = collision.gameObject;

        if (collision.GetComponent<CardDrag>().released == true && player.GetComponent<Player>().deckOrdered.Count <= 5)
        {
            collision.GetComponent<CardDrag>().inArray = true;
            collision.gameObject.transform.position = gameObject.transform.position;
            Debug.Log("cjecl me");
        }
    }

}
