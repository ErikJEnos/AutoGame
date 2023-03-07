using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;
public class Card : MonoBehaviour
{
    public GameObject order;
    public GameObject foreground;
    public GameObject background;
    public bool interactable = true;

    public Text textinfo;
    public TMP_Text damageInfoText;
    public TMP_Text cardTitle;
    public TMP_Text attackText;
    public TMP_Text defenceText;
    public TMP_Text infoText;
    public TMP_Text levelText;

    public int cardID = 0;
    public int attack = 0;
    public int defence = 0;
    public int cardLevel = 0;
    public bool isMonster = false;
    public bool canPoison = false;
    public bool canTarget = false;
    public string text;
    public int playerID = 0;
    public bool inScene = false;
    public bool chosen = false;

    public bool isPoisoned = false;

    GameObject player;
    GameObject gameManager;
    System.Random random = new System.Random();

    private GameObject gameLogic;

    private void Awake()
    {
        attack = 0;
        defence = 0;

        cardID = Random.RandomRange(0, 8);
        CheckCardID();

        attackText.text = attack.ToString();
        defenceText.text = defence.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreground = GameObject.Find("ForeGround");
        background = GameObject.Find("CardSpawn");
        player = GameObject.Find("Player1");
        gameManager = GameObject.Find("GameManager");
        gameLogic = GameObject.Find("gameLogic");
    }

    // Update is called once per frame
    void Update()
    {
        attackText.text = attack.ToString();
        defenceText.text = defence.ToString();
        levelText.text = cardLevel.ToString();

        //if(defence <= 0)
        //{
        //    gameManager.GetComponent<GameLoop>().DestroyMonster(gameObject);
        //}
    }

    public void SelectCard()
    {

        if (gameManager.GetComponent<GameState>().gameState == 0) //picking cards to add to the player pool
        {
            gameObject.transform.parent = gameManager.GetComponent<PickingCards>().deckSpawnLocation.transform.parent;
            player.GetComponent<Player>().deck.Insert(0, gameObject);
            gameManager.GetComponent<PickingCards>().actions--;
            gameObject.SetActive(false);
            chosen = true;
           
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
            gameManager.GetComponent<PickingCards>().ReOrderCards();
        }
    }

    public void CheckCardID()
    {
        if (cardID == 0)
        {
            cardTitle.text = "Zombie";
            infoText.text = "On death spawn 1-1 crawler";
            isMonster = true;
            canPoison = false;
            canTarget = true;
            attack = 1;
            defence = 2;
            attackText.text = attack.ToString();
            defenceText.text = defence.ToString();
        }
        else if(cardID == 1)
        {
            cardTitle.text = "Skeleton";
            infoText.text = "On death deal 1 damge to the back row enemy";
            isMonster = true;
            canPoison = false;
            canTarget = true;
            attack = 2;
            defence = 1;
            attackText.text = attack.ToString();
            defenceText.text = defence.ToString();
        }
        else if (cardID == 2)
        {
            cardTitle.text = "Giant Rat";
            infoText.text = "Posions enemy on hit. poison lasts forever";
            isMonster = true;
            canPoison = true;
            canTarget = true;
            attack = 1;
            defence = 3;
            attackText.text = attack.ToString();
            defenceText.text = defence.ToString();
        }
        else if (cardID == 3)
        {
            cardTitle.text = "Goblin";
            infoText.text = "Get +0+1 for each friendly Goblin";
            isMonster = true;
            canPoison = false;
            canTarget = true;
            attack = 2;
            defence = 1;
            attackText.text = attack.ToString();
            defenceText.text = defence.ToString();
        }
        else if (cardID == 4)
        {
            cardTitle.text = "Giant Bat";
            infoText.text = "Bleed stacking";
            isMonster = true;
            canPoison = false;
            canTarget = true;
            attack = 2;
            defence = 2;
            attackText.text = attack.ToString();
            defenceText.text = defence.ToString();
        }
        else if (cardID == 5)
        {
            cardTitle.text = "Human";
            infoText.text = "Get +0+1 for each friendly Human";
            isMonster = true;
            canPoison = false;
            canTarget = true;
            attack = 1;
            defence = 1;
            attackText.text = attack.ToString();
            defenceText.text = defence.ToString();
        }
        else if (cardID == 6)
        {
            cardTitle.text = "Slime";
            infoText.text = "";
            isMonster = true;
            canPoison = false;
            canTarget = true;
            attack = 1;
            defence = 1;
            attackText.text = attack.ToString();
            defenceText.text = defence.ToString();
        }
        else if (cardID == 7)
        {
            cardTitle.text = "Wolves";
            infoText.text = "Get +0+1 for each kill this monster does";
            isMonster = true;
            canPoison = false;
            canTarget = true;
            attack = 2;
            defence = 2;
            attackText.text = attack.ToString();
            defenceText.text = defence.ToString();
        }
        else if (cardID == 8)
        {
            cardTitle.text = "Gnomes";
            infoText.text = "Can't be targeted by spells";
            isMonster = true;
            canPoison = false;
            canTarget = false;
            attack = 1;
            defence = 1;
            attackText.text = attack.ToString();
            defenceText.text = defence.ToString();
        }
        else if (cardID == 9)
        {
            cardTitle.text = "Spell";
            isMonster = false;
            canPoison = false;
            attack = 0;
            defence = 0;
            attackText.text = "";
            defenceText.text = "";
            infoText.text = "Give '1' defence to monster in front if possible else deal '1' damage to monster on enemy side";
        }
        else if (cardID == 10)
        {
            cardTitle.text = "Spell";
            isMonster = false;
            canPoison = false;
            attack = 0;
            defence = 0;
            attackText.text = "";
            defenceText.text = "";
            infoText.text = "If you control a monster deal '2' damage to front enemy monster, else spawn a 0-1 monster ";
        }
        else if(cardID == 99)
        {
            cardTitle.text = "Monster";
            isMonster = true;
            canPoison = false;
            canTarget = false;
            attack = 0;
            defence = 1;
            attackText.text = attack.ToString();
            defenceText.text = defence.ToString();
            infoText.text = "Slime";

        }
        else if(cardID == 98)
        {
            cardTitle.text = "Crawler";
            infoText.text = "";
            isMonster = true;
            canPoison = false;
            canTarget = false;
            attack = 1;
            defence = 1;
            attackText.text = attack.ToString();
            defenceText.text = defence.ToString();
        }

    }

    public void setCardId(int id)
    {
        cardID = id;
        CheckCardID();
    }

    public void CheckCardLevel()
    {
        Debug.Log("Card level: "+ cardLevel);

        if(cardLevel == 0)
        {
            attack += cardLevel;
            defence += cardLevel;
        } 
        else if (cardLevel == 1)
        {
            attack += cardLevel;
            defence += cardLevel;
        }
        else if (cardLevel == 2)
        {
            attack += cardLevel;
            defence += cardLevel;
        }
        else if (cardLevel == 3)
        {
            attack += cardLevel;
            defence += cardLevel;
        }
        else if (cardLevel == 4)
        {
            attack += cardLevel;
            defence += cardLevel;
        }
        else if (cardLevel == 5)
        {
            attack += cardLevel;
            defence += cardLevel;
        }
    }

}
