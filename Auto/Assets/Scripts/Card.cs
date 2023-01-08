using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;
public class Card : MonoBehaviour
{
    public GameObject order;
    public GameObject foreground;
    public GameObject background;
    public bool interactable = true;

    public TMP_Text slotNumberl;
    public TMP_Text cardTitle;
    public TMP_Text attackText;
    public TMP_Text defenceText;
    public TMP_Text infoText;

    public int cardID = 0;
    public int attack = 0;
    public int defence = 0;
    public bool isMonster = false;
    public string text;
    public int playerID = 0;
    public bool inScene = false;

    public bool chosen = false;

    Vector3 oriPos;
    GameObject player;
    GameObject gameManager;
    System.Random random = new System.Random();

    private void Awake()
    {
        attack = 0;
        defence = 0;

        cardID = Random.RandomRange(0, 5);
        CheckCardID();

        attackText.text = attack.ToString();
        defenceText.text = defence.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreground = GameObject.Find("ForeGround");
        background = GameObject.Find("CardSpawn");
        player = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        attackText.text = attack.ToString();
        defenceText.text = defence.ToString();
    }

    public void SelectCard()
    {
        //   gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //  gameObject.transform.parent = background.transform;

        if (gameManager.GetComponent<GameState>().gameState == 0) //picking cards to add to the player pool
        {
            gameObject.transform.parent = null;
            player.GetComponent<Player>().deck.Insert(0, gameObject);
            gameObject.SetActive(false);
            chosen = true;
            gameManager.GetComponent<PickACard>().pickLeftInt--;
        }

        else if (gameManager.GetComponent<GameState>().gameState == 1 && interactable == true && player.GetComponent<Player>().deckOrdered.Count < 5) //picking cards to add to the player deck order
        {
            
            interactable = false;
            order = GameObject.Find("CardOrder");
            gameObject.transform.position = order.transform.position;
            player.GetComponent<Player>().deckOrdered.Insert(0, gameObject);

            float offset = 2.0f;
            float index = 0;
            foreach (GameObject card in player.GetComponent<Player>().deckOrdered)
            {
                index += 1.0f;
                card.transform.position = new Vector3(order.transform.position.x + offset, order.transform.position.y, order.transform.position.z);
                offset += 2.0f;


            }

        }
        else if (gameManager.GetComponent<GameState>().gameState == 1 && interactable == false) //picking cards to add to the player deck order
        {
            gameManager.GetComponent<PickACard>().ReOrderCards();
        }
    }

    public void CheckCardID()
    {
        Debug.Log("Card ID "  + cardID);
        if (cardID == 0)
        {
            isMonster = true;
            attack = 1;
            defence = 2;
            attackText.text = attack.ToString();
            defenceText.text = defence.ToString();
        }
        else if(cardID == 1)
        {
            isMonster = true;
            attack = 2;
            defence = 1;
            attackText.text = attack.ToString();
            defenceText.text = defence.ToString();
        }
        else if (cardID == 2)
        {
            isMonster = true;
            attack = 1;
            defence = 4;
            attackText.text = attack.ToString();
            defenceText.text = defence.ToString();
        }
        else if (cardID == 3)
        {
            cardTitle.text = "Spell";
            isMonster = false;
            infoText.text = "Give '1' defence to monster in front if possible else deal '1' damage to monster on enemy side";
        }
        else if (cardID == 4)
        {
            cardTitle.text = "Spell";
            isMonster = false;
            infoText.text = "If you control a monster deal '2' damage to front enemy monster, else spawn a 0-1 monster ";
        }
        else if(cardID == 99)
        {
            cardTitle.text = "Monster";
            isMonster = true;
            attack = 0;
            defence = 1;
            attackText.text = attack.ToString();
            defenceText.text = defence.ToString();
            infoText.text = "Slime";

        }
     
    }

    public void setCardId(int id)
    {
        cardID = id;
        CheckCardID();
    }

    bool callOnce = false;

    void OnMouseOver()
    {
        //if (!callOnce)
        //{
        //    callOnce = true;
        //    oriPos = gameObject.transform.position;

        //    if (gameManager.GetComponent<GameState>().gameState == 0)
        //    {

        //        gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        //        gameObject.transform.position += new Vector3(1.0f, 1.0f, 0.0f);
        //        gameObject.transform.parent = foreground.transform;
        //        background = GameObject.Find("CardSpawn");
        //    }
        //    else if (gameManager.GetComponent<GameState>().gameState == 1)
        //    {
        //        background = GameObject.Find("DeckSpawn");
        //        foreground = GameObject.Find("ForeGround2");
        //        gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        //        //gameObject.transform.position -= new Vector3(1.0f, 1.0f, 0.0f);
        //        gameObject.transform.parent = foreground.transform;
        //    }
        //}

    }
    
    void OnMouseExit()
    {
        //if(callOnce)
        //{
        //    callOnce = false;

        //    if (gameManager.GetComponent<GameState>().gameState == 0)
        //    {
        //        gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //        gameObject.transform.position = oriPos;
        //        gameObject.transform.parent = background.transform;
        //        background = GameObject.Find("CardSpawn");
        //    }
        //    else if (gameManager.GetComponent<GameState>().gameState == 1)
        //    {
        //        background = GameObject.Find("DeckSpawn");
        //        gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //        //gameObject.transform.position = oriPos;
        //        gameObject.transform.parent = background.transform;
        //    }
        //}
       
    }
}
