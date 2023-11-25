using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDrag : MonoBehaviour
{
    private GameObject gameManager;
    private GameObject player;
    
    private Vector3 mousePos;
    private Vector2 screenPosition;
    private Vector2 worldPosition;
    private Vector3 anchor;

    public bool released = false;
    private bool hovering = false;

    private GameObject hoveredObject;
    public GameObject orderPostion;
    public bool canGrab = true;
    public bool inArray = false;
    public int cardPos = 0;

    public List<GameObject> objectsEntered;


    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        player = GameObject.Find("Player1");
    }

    // Update is called once per frame
    void Update()
    {
        screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
    }

    private void OnMouseDrag()
    {
        if(gameManager.GetComponent<GameState>().gameState == 1 && canGrab)
        {
            released = false;
            mousePos = Input.mousePosition;
            gameObject.transform.position = new Vector3(worldPosition.x, worldPosition.y, 0.0f);
        }  
    }

    private void OnMouseUp()
    {
        released = true;
    }

    private void OnMouseDown()
    {
        gameManager.GetComponent<PickingCards>().sort = true;

        if (player.GetComponent<Player>().deckOrdered.Contains(gameObject))
        {
            player.GetComponent<Player>().deckOrdered.Remove(gameObject);
            gameObject.GetComponent<CardDrag>().cardPos = 0;
            inArray = false;
            Debug.Log("removed");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Card>() != null)
        {
            objectsEntered.Add(collision.gameObject);
        }
      
        foreach (GameObject cards in objectsEntered)
        {
            if (cards.gameObject.GetComponent<Card>() != null)
            {
                if (cards.gameObject.GetComponent<Card>().cardID == gameObject.GetComponent<Card>().cardID && gameManager.GetComponent<GameState>().gameState == 1 && released == false && cards.gameObject.GetComponent<CardDrag>().inArray == false)
                {
                    hovering = true;

                    gameObject.GetComponent<Image>().color = Color.green;
                    hoveredObject = cards;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (released && collision.gameObject.GetComponent<DragPos>() != null && collision.gameObject.GetComponent<DragPos>().card == null)
        {

            if (orderPostion != null)
            {
                orderPostion.GetComponent<DragPos>().card = null;
            }

            if (collision.GetComponent<DragPos>().card == null)
            {
                if (!inArray)
                {
                    player.GetComponent<Player>().deckOrdered.Add(gameObject);
                    gameObject.transform.position = gameManager.GetComponent<PickingCards>().handOrder[player.GetComponent<Player>().deckOrdered.Count - 1].transform.position;
                    inArray = true;

                    gameManager.GetComponent<PickingCards>().handOrder[player.GetComponent<Player>().deckOrdered.Count - 1].GetComponent<DragPos>().card = gameObject;
                    
                }
            }
        }
        

        if (hovering && released)
        {
            Debug.Log("Level Up");
            LevelUp();
            hovering = false;
        }

    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Card>() != null)
        {
            if (collision.gameObject.GetComponent<Card>().cardID == gameObject.GetComponent<Card>().cardID)
            {
                gameObject.GetComponent<Image>().color = Color.white;

                hovering = false;
            }

            objectsEntered.Remove(collision.gameObject);
        }
        
    }

    public void LevelUp()
    { 
        hoveredObject.GetComponent<Card>().cardLevel += gameObject.GetComponent<Card>().cardLevel + 1;
        hoveredObject.GetComponent<Card>().CheckCardID();
        player.GetComponent<Player>().deck.Remove(this.gameObject);
        Destroy(this.gameObject);  
    }

}
