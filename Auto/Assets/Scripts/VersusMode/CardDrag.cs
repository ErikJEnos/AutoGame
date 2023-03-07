using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDrag : MonoBehaviour
{
    private GameObject gameManager;
    private GameObject player;
    private Vector3 mousePos;
    Vector2 screenPosition;
    Vector2 worldPosition;
    private bool released = false;
    private bool hovering = false;

    // Start is called before the first frame update
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
        if(gameManager.GetComponent<GameState>().gameState == 1)
        {
            released = false;
            mousePos = Input.mousePosition;
            gameObject.transform.position = worldPosition;
        }
        
    }
    private void OnMouseUp()
    {
        released = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Card>().cardID == gameObject.GetComponent<Card>().cardID && gameManager.GetComponent<GameState>().gameState == 1)
        {
            if (!released)
            {
                collision.gameObject.GetComponent<Card>().cardLevel += 1;
                collision.gameObject.GetComponent<Card>().CheckCardID();
                collision.gameObject.GetComponent<Card>().CheckCardLevel();
                player.GetComponent<Player>().deck.Remove(this.gameObject);
                Destroy(this.gameObject);
            }    
        }
        hovering = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        hovering = false;
    }


}
